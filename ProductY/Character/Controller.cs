using Controller;
using Events.Sending;
using Repository;

namespace ProductY.Character
{
    public class Controller : CharacterController
    {
        public Controller(ICharacterRepository repository, IEventSender sender) : base(repository, sender)
        {
        }

        protected override char Char => 'Y';
    }
}
