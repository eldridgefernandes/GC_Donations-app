using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Used Swagger over OpenAPI interface
builder.Services.AddControllers();

//The below block is for allowing React to read from the database because of the port difference
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//Using Sqlite database 
builder.Services.AddDbContext<DonationContext>(options =>
    options.UseSqlite("Data Source=donations.db"));

builder.Services.AddScoped<ICRMService, MockCRMService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

//If any columns in the database is added, we need to delete and create a new database
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<DonationContext>();
//     db.Database.EnsureDeleted();   // optional: drops old db if exists
//     db.Database.EnsureCreated();   // creates tables from your model
// }

app.UseCors("AllowReactApp");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/ping", () =>
{
    return "pong";
})
.WithName("PingCheck");


app.Run();

//Class for defining and initialising inputs needed
public class Donation
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public string donorfirstname { get; set; } = string.Empty;
    public string donorlastname { get; set; } = string.Empty;
    public string elmail { get; set; } = string.Empty;
    public string phonenumber { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public string projectpreference { get; set; } = string.Empty;
    public string donationmethod { get; set; } = string.Empty;
    public int frequency { get; set; } = 0;
    public string address { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string province { get; set; } = string.Empty;
    public string postalcode { get; set; } = string.Empty;
}

public class DonationContext : DbContext
{
    public DonationContext(DbContextOptions<DonationContext> options)
        : base(options) { }

    public DbSet<Donation> Donations { get; set; }
}


public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}


//Send an email appreciation to the donor 
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Validate required SMTP settings
        var smtpSettings = _config.GetSection("Smtp");
        var host = _config["Smtp:Host"];
        var portString = _config["Smtp:Port"];
        var username = _config["Smtp:Username"];
        var password = _config["Smtp:Password"];
        var fromAddress = _config["Smtp:From"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(portString) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(fromAddress))
        {
            throw new InvalidOperationException(
                "SMTP configuration is incomplete. Please check appsettings.json.");
        }

        if (!int.TryParse(portString, out int port))
            throw new InvalidOperationException("SMTP port is invalid.");

        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipient email address is required.", nameof(to));

        try
        {
            using var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
                // DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await smtpClient.SendMailAsync(message);
            Console.WriteLine($" Email successfully sent to {to}");
        }
        catch (SmtpException ex)
        {
            // Clear and meaningful error
            Console.WriteLine($"❌ Email sending failed: {ex.Message}");
            throw new InvalidOperationException(
                $"Failed to send email. Check SMTP credentials and server. Details: {ex.Message}", ex);
        }
    }
}


    
//Using Endpoints to communicate with the database
[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly DonationContext _context;
    private readonly ICRMService _crm;
    private readonly IEmailService _emailService;

    public DonationsController(DonationContext context, ICRMService crm, IEmailService emailService)
    {
        _context = context;
        _crm = crm;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> PostDonation(Donation donation)
    {

        donation.Date ??= DateTime.UtcNow;
        donation.donorfirstname = string.IsNullOrWhiteSpace(donation.donorfirstname) ? "Anonymous" : donation.donorfirstname.Trim();
        donation.donorlastname = string.IsNullOrWhiteSpace(donation.donorlastname) ? "Anonymous" : donation.donorlastname.Trim();
        donation.elmail = string.IsNullOrWhiteSpace(donation.elmail) ? "Anonymous" : donation.elmail.Trim();
        donation.phonenumber = string.IsNullOrWhiteSpace(donation.phonenumber) ? "Anonymous" : donation.phonenumber.Trim();
        donation.address = string.IsNullOrWhiteSpace(donation.address) ? "Anonymous" : donation.address.Trim();
        donation.city = string.IsNullOrWhiteSpace(donation.city) ? "Anonymous" : donation.city.Trim();
        donation.province = string.IsNullOrWhiteSpace(donation.province) ? "Anonymous" : donation.province.Trim();
        donation.postalcode = string.IsNullOrWhiteSpace(donation.postalcode)? "Anonymous": donation.postalcode.Trim();
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();
        await _crm.SendToCRM(donation);

        var emailBody = $@"
        <h3>Thank you for your donation, {donation.donorfirstname}!</h3>
        <p>We received your contribution of <strong>${donation.amount}</strong> 
        towards <strong>{donation.projectpreference}</strong> on {donation.Date?.ToShortDateString()}.</p>
        <p>We sincerely appreciate your support!</p>
         ";

        if (!string.IsNullOrWhiteSpace(donation.elmail) && donation.elmail.Contains("@"))
        {
            try
            {
                await _emailService.SendEmailAsync(
                donation.elmail,
                "Donation Confirmation",
                emailBody
                );
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No email sent; skipping email sending.");
        }

        return Ok(donation);
    }

    [HttpGet]
    public async Task<IActionResult> GetDonations() =>
        Ok(await _context.Donations.ToListAsync());
}

// Mocked a call to external API 
public interface ICRMService
{
    Task<string> SendToCRM(Donation donation);
}

public class MockCRMService : ICRMService
{
    public Task<string> SendToCRM(Donation donation)
    {
        var response = new {status = "success", message = $"Donation from {donation.donorlastname} sent to CRM." };
        Console.WriteLine(JsonSerializer.Serialize(response));
        return Task.FromResult(JsonSerializer.Serialize(response));
    }
}


