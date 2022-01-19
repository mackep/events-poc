using System.Threading.Tasks;
using Client;
using Events.Contracts;
using Events.Handling;
using Repository;

namespace Handlers
{
    public class ZCreatedEventHandler : DomainEventHandler<ZCreated>
    {
        private readonly ICharacterRepository _repository;
        private readonly ZApiClient _apiClient;

        public ZCreatedEventHandler(ICharacterRepository repository, ZApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }

        public override async Task Handle(ZCreated evnt)
        {
            var character = await _apiClient.GetZ(evnt.Id);

            await _repository.Upsert(character);
        }
    }
}