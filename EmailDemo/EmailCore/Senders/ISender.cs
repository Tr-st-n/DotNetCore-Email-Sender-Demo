using EmailCore.Results;
using System.Threading.Tasks;

namespace EmailCore.Senders
{
    public interface ISender
    {
        /// <summary> Asynchronously sends an email using a EmailCore.Message argument. Returns a EmailCore.BaseSenderResult. </summary>
        Task<ISenderResult> SendAsync(Message message);
    }
}
