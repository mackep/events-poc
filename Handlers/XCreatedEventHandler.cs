using System.Threading.Tasks;
using Client;
using Events.Contracts;
using Events.Handling;
using Repository;

namespace Handlers
{
    public class XCreatedEventHandler : DomainEventHandler<XCreated>
    {
        private readonly XApiClient _apiClient;
        private readonly ICharacterRepository _repository;

        public XCreatedEventHandler(XApiClient apiClient, ICharacterRepository repository)
        {
            _apiClient = apiClient;
            _repository = repository;
        }

        public override async Task Handle(XCreated evnt)
        {
            var character = await _apiClient.GetX(evnt.Id);

            await _repository.Upsert(character);
        }
    }
}
