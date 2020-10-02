using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using xsolla_revenue_calculator.Controllers.Requests;

namespace xsolla_revenue_calculator.Services.ForecastExportService
{
    public class ForecastExportService : IForecastExportService
    {
        private readonly IChartService _chartService;
        private readonly IDatabaseAccessService _databaseAccessService;
        private readonly IMailingService _mailingService;

        public ForecastExportService(IChartService chartService, IDatabaseAccessService databaseAccessService, IMailingService mailingService)
        {
            _chartService = chartService;
            _databaseAccessService = databaseAccessService;
            _mailingService = mailingService;
        }

        public async Task ExportForecast(ExportRequestBody requestBody)
        {
            var chartUrls = _chartService.GetChartUrl(await _databaseAccessService.GetForecastAsync(requestBody.RevenueForecastId));
            var message = FormatMessage(chartUrls);
            await _mailingService.SendMessageAsync(message, requestBody.Email);
        }

        private string FormatMessage(List<Tuple<string, string>> chartUrls)
        {
            string message = $"<h2>Hello, please see the charts below:</h2><br><br>";
            foreach (var chartUrl in chartUrls)
            {
                message += $"<h3>{chartUrl.Item1}</h3><br><img src=\"{chartUrl.Item2}\">";
            }
            return message;
        }
    }

    public interface IForecastExportService
    {
        public Task ExportForecast(ExportRequestBody requestBody);
    }
}