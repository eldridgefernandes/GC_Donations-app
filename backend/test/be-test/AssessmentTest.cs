using System.Threading.Tasks;
using NUnit.Framework.Legacy;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BackendTesting
{
    public class AssessmentTest
    {
        const string BASE_URL = "http://localhost:5050";
        const string DONATE_ENDPOINT = "/donate";
        const string DONATIONS_ENDPOINT = "/donations";

        const string DONOR_NAME = "Jane Smith";
        const double AMOUNT = 200.00;
        const string DATE = "2025-09-10";

        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BASE_URL);
        }

        [Test, Order(1)]
        [Ignore("Ignore for now")]
        public async Task Test_POST_donations()
        {
            var jsonString = $$"""
                {
                    "DonorName": "{{DONOR_NAME}}",
                    "Amount": {{AMOUNT}},
                    "Date": "{{DATE}}"
                }
            """;

            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            ClassicAssert.NotNull(await ApiHelper.SendPostRequest(_httpClient, "/donations", httpContent));
        }

        [Test, Order(2)]
        [Ignore("Ignore for now")]
        public async Task Test_GET_Donations()
        {
            var result = await ApiHelper.SendGetRequest(_httpClient, "/donations");

            var jsonResult = JsonNode.Parse(result);
            Assert.That(jsonResult["DonorName"].ToString() == DONOR_NAME);
            Assert.That(jsonResult["Amount"].GetValue<double>() == AMOUNT);
            Assert.That(jsonResult["Date"].ToString() == DATE);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }
    }
}