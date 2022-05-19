using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Newtonsoft.Json;

namespace Application.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public async Task<HttpResponseMessage> DoUpdateSystems(object obj, string url, string httpMethod)
        {
            Console.WriteLine("Updating Systems");
            HttpClient client=new HttpClient();
            string str = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            switch (httpMethod)
            {
                case "POST":
                    response = await client.PostAsync(url, content);
                    break;
                case "PUT":
                    response = await client.PutAsync(url, content);
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(url);
                    break;
                case "GET":
                    response = await client.GetAsync(url);
                    break;
            }
            return response;


        }
    }
}