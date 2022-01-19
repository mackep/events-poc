using System.Net.Http;

namespace ParallelChangeTrigger
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new();
    }
}