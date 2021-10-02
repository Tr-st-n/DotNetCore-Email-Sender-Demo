using System;
using System.Threading.Tasks;

namespace EmailCore
{
    public interface ISender
    {
        /// <summary> Asynchronously sends an email using a EmailCore.Message argument. Returns a EmailCore.BaseSenderResult. </summary>
        Task<BaseSenderResult> SendAsync(Message message);
    }
}
