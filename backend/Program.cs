using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.ComponentModel;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Used Swagger over OpenAPI interface
builder.Services.AddControllers();
builder.Services.AddScoped<ICRMService, MockCRMService>();

//The below block is for allowing React to read from the database 
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


var app = builder.Build();

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
    public string DonorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime? Date { get; set; }
}

public class DonationContext : DbContext
{
    public DonationContext(DbContextOptions<DonationContext> options)
        : base(options) { }

    public DbSet<Donation> Donations { get; set; }
}

//Using Endpoints to 
[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly DonationContext _context;
    private readonly ICRMService _crm;

    public DonationsController(DonationContext context, ICRMService crm)
    {
        _context = context;
        _crm = crm;
    }

    [HttpPost]
    public async Task<IActionResult> PostDonation(Donation donation)
    {
        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();
        await _crm.SendToCRM(donation);
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
        var response = new { status = "success", message = $"Donation from {donation.DonorName} sent to CRM." };
        Console.WriteLine(JsonSerializer.Serialize(response));
        return Task.FromResult(JsonSerializer.Serialize(response));
    }
}

