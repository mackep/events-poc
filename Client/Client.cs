using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Model;

namespace Client
{
    public abstract class Client
    {
        private readonly IHttpClientFactory _factory;

        protected Client(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        protected async Task<Character> Get(string uri)
        {
            var client = _factory.CreateClient();

            var result = await client.GetAsync(uri);

            result.EnsureSuccessStatusCode();

            if (result.StatusCode == HttpStatusCode.NoContent)
                return null;

            var response = await JsonSerializer.DeserializeAsync<Character>(
                await result.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }

        protected async Task Put(string uri)
        {
            var client = _factory.CreateClient();

            var result = await client.PutAsync(uri, new StringContent(string.Empty, Encoding.UTF8));

            result.EnsureSuccessStatusCode();
        }

        protected async Task Post(string uri)
        {
            var client = _factory.CreateClient();

            var result = await client.PostAsync(uri, new StringContent(string.Empty, Encoding.UTF8));

            result.EnsureSuccessStatusCode();
        }
    }
}
