using System;
using System.Collections.Generic;
using System.IO;
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
            var message = GetHtmlMessage(await chartUrls, requestBody);
            await _mailingService.SendMessageAsync(await message, requestBody.Email);
        }

        private async Task<string> GetHtmlMessage(List<Tuple<string, string>> chartUrls, ExportRequestBody requestBody)
        {
            using var reader = new StreamReader(@"Properties/Assets/Message.html");
            var html = await reader.ReadToEndAsync();
            foreach (var chartUrl in chartUrls)
            {
                html = html.Replace('{'+chartUrl.Item1+'}', chartUrl.Item2);
            }

            html = FillPlaceHolders(html, requestBody);
            return html;
        }

        private string FillPlaceHolders(string html, ExportRequestBody requestBody)
        {
            html = FillPlaceholder(html, "{monetization}", requestBody.Monetization);
            html = FillPlaceholder(html, "{revenue}", requestBody.Revenue);
            html = FillPlaceholder(html, "{topMarket}", requestBody.TopMarket);
            return html;
        }
        
        private string FillPlaceholder(string destination, string placeholder, string value)
        {
            return destination.Replace( placeholder, value);
        }
    }
    

    public interface IForecastExportService
    {
        public Task ExportForecast(ExportRequestBody requestBody);
    }
}