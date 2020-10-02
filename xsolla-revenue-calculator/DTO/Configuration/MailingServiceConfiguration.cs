using xsolla_revenue_calculator.Services.ForecastExportService;

namespace xsolla_revenue_calculator.DTO.Configuration
{
    public class MailingServiceConfiguration : IMailingServiceConfiguration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        public string Sign { get; set; }
        
        public string MessageSubject { get; set; }
    }

    public interface IMailingServiceConfiguration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        public string Sign { get; set; }
        
        public string MessageSubject { get; set; }
    }
}