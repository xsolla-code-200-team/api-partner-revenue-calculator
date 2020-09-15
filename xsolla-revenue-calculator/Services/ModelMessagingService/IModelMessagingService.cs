using System;
using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO;

namespace xsolla_revenue_calculator.Services.ModelMessagingService
{
    public interface IModelMessagingService
    {
        Task SendAsync(MessageToModel message);
        Action<IModelMessagingService, MessageFromModel> ResponseProcessor { get; set; }

        void Dispose();
    }
}