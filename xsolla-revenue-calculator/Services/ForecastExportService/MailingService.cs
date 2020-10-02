using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO.Configuration;

namespace xsolla_revenue_calculator.Services.ForecastExportService
{
    public class MailingService : IMailingService
    {
        private readonly IMailingServiceConfiguration _mailingServiceConfiguration;
        private SmtpClient _client;

        private string _messageSubject = "Your potential revenue report";
        private string _messageFrom = "Xsolla Partner Calculator";
        
        public MailingService(IMailingServiceConfiguration mailingServiceConfiguration)
        {
            _mailingServiceConfiguration = mailingServiceConfiguration;
            InitializeSmtpClient();
        }

        private void InitializeSmtpClient()
        {
            _client = new SmtpClient();
            _client.UseDefaultCredentials = false;
            _client.Port = 587;
            _client.Host = "smtp.gmail.com";
            _client.EnableSsl = true;
            _client.UseDefaultCredentials = false;
            _client.Credentials = new NetworkCredential(_mailingServiceConfiguration.Username, _mailingServiceConfiguration.Password);
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
        public async Task SendMessageAsync(string messageBody, string email)
        {
            var message = new MailMessage(_messageFrom,
                email, _messageSubject,
                messageBody) {IsBodyHtml = true}; 
            await Task.Run(() => _client.Send(message));;
        }
    }

    public interface IMailingService
    {
        Task SendMessageAsync(string messageBody, string email);
    }
}