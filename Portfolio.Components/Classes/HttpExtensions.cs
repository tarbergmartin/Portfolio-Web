using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Components.Classes
{
    public static class HttpExtensions
    {
        public async static Task<T> GetJsonAsync<T>(this HttpClient client, string endpoint)
        {
            try
            {
                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
            }

            catch (Exception e)
            {
                // TODO: Log error or whatever
            }

            return default;
        }

        public async static Task<bool> PostJsonAsync<T>(this HttpClient client, string endpoint, T content)
        {
            var json = JsonConvert.SerializeObject(content);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, data);

            return response.IsSuccessStatusCode ? true : false;
        }
    }
}
