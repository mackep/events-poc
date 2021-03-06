using System.Threading.Tasks;
using Client;
using Events.Contracts;
using Events.Handling;
using Repository;

namespace Handlers
{
    public class XUpdatedEventHandler : DomainEventHandler<XUpdated>
    {
        private readonly XApiClient _apiClient;
        private readonly ICharacterRepository _repository;

        public XUpdatedEventHandler(XApiClient apiClient, ICharacterRepository repository)
        {
            _apiClient = apiClient;
            _repository = repository;
        }

        public override async Task Handle(XUpdated evnt)
        {
            var updated = await _apiClient.GetX(evnt.Id);

            await _repository.Upsert(updated);
        }
    }
}