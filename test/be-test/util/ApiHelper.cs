namespace BackendTesting
{
    using System.Threading.Tasks;
    using NUnit.Framework.Legacy;
    public class ApiHelper
    {
        public static async Task<string> SendGetRequest(HttpClient httpClient, string endpoint)
        {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> SendPostRequest(HttpClient httpClient, string endpoint, HttpContent content)
        {
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}