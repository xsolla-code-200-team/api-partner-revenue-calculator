using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xsolla_revenue_calculator.Models.ForecastModels;

namespace xsolla_revenue_calculator.Services.ForecastExportService
{
    public class ChartService : IChartService
    {
        public async Task<List<Tuple<string, string>>> GetChartUrl(RevenueForecasts forecasts)
        {
          var tendencyChartConfig = GetTendencyChartConfig(forecasts);
          var cumulativeChartConfig = GetCumulativeChartConfig(forecasts);
          string baseChartUrl = "https://quickchart.io/chart?width=500&height=350&chart=";
          string tendencyChartUrl = baseChartUrl + Uri.EscapeDataString(await tendencyChartConfig);
          string cumulativeChartUrl = baseChartUrl + Uri.EscapeDataString(await cumulativeChartConfig);
          return new List<Tuple<string, string>>
          {
            new Tuple<string, string>
            (
              "TendencyChartUrl", tendencyChartUrl
            ),
            new Tuple<string, string>
            (
              "CumulativeChartUrl", cumulativeChartUrl
            )
          };
        }

        private async Task<string> GetTendencyChartConfig(RevenueForecasts forecasts)
        {
          return await GetBaseChartConfig(
            "Revenue (tendency)",
            forecasts.ChosenForecast.Monetization,
            forecasts.ChosenForecast.TendencyForecast,
            forecasts.OtherForecasts.Single().Monetization,
            forecasts.OtherForecasts.Single().TendencyForecast
          );
        }
        
        private async Task<string> GetCumulativeChartConfig(RevenueForecasts forecasts)
        {
          return await GetBaseChartConfig(
            "Revenue (cumulative)",
            forecasts.ChosenForecast.Monetization,
            forecasts.ChosenForecast.CumulativeForecast,
            forecasts.OtherForecasts.Single().Monetization,
            forecasts.OtherForecasts.Single().CumulativeForecast
          );
        }

        private async Task<string> GetBaseChartConfig(string title, string chosenForecastLabel, List<double> chosenForecast, string anotherForecastLabel, List<double> anotherForecast)
        {
          int monthCount = chosenForecast.Count;
          using var reader = new StreamReader(@"Properties/Assets/ChartConfig.json");
          string json = await reader.ReadToEndAsync();
          string yAxisLabels = $"[{String.Join(',', Enumerable.Range(1, monthCount))}]";
          string chosenForecastString = $"[{String.Join(',', chosenForecast)}]";
          string anotherForecastString = $"[{String.Join(',', anotherForecast)}]";
          json = FillPlaceholders(json, title, yAxisLabels, chosenForecastLabel, chosenForecastString, anotherForecastLabel,
            anotherForecastString);
          return json;
        }

        private string FillPlaceholders(string json, string title, string yAxisLabels, string chosenForecastLabel,
          string chosenForecastString, string anotherForecastLabel, string anotherForecastString)
        {
          json = FillPlaceholder(json, "{title}", title);
          json = FillPlaceholder(json, "\"{yAxisLabels}\"", yAxisLabels);
          json = FillPlaceholder(json, "{chosenForecastLabel}", chosenForecastLabel);
          json = FillPlaceholder(json, "\"{chosenForecastString}\"", chosenForecastString);
          json = FillPlaceholder(json, "{anotherForecastLabel}", anotherForecastLabel); 
          json = FillPlaceholder(json, "\"{anotherForecastString}\"", anotherForecastString);
          return json;
        }

        private string FillPlaceholder(string destination, string placeholder, string value)
        {
          return destination.Replace( placeholder, value);
        }
    }

    public interface IChartService
    {
      Task<List<Tuple<string, string>>> GetChartUrl(RevenueForecasts forecasts);
    }
}