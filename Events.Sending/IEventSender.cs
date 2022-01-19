using System.Threading.Tasks;
using Events.Base;

namespace Events.Sending
{
    public interface IEventSender
    {
        Task Send(params Event[] events);
    }
}