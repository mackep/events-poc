using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class YApiClient : Client
    {
        private readonly string _uri;

        public YApiClient(IHttpClientFactory factory) : base(factory)
        {
            _uri = "http://localhost:2002";
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