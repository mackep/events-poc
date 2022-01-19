using System;
using System.Net.Http;
using System.Threading.Tasks;
using Model;

namespace Client
{
    public class ZApiClient : Client
    {
        private readonly string _uri;

        public ZApiClient(IHttpClientFactory factory) : base(factory)
        {
            _uri = "http://localhost:2003";
        }

        public Task<Character> GetZ(Guid id)
        {
            return Get($"{_uri}/{id}");
        }

        public Task AddRandomCharacter()
        {
            return Put(_uri);
        }

        public Task UpdateRandomCharacter()
        {
            return Post(_uri);
        }
    }
}