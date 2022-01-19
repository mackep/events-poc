using System;
using System.Net.Http;
using System.Threading.Tasks;
using Model;

namespace Client
{
    public class XApiClient : Client
    {
        private readonly string _uri;

        public XApiClient(IHttpClientFactory factory) : base(factory)
        {
            _uri = "http://localhost:2001";
        }

        public Task<Character> GetX(Guid id)
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