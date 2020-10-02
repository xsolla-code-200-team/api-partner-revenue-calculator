using System;
using System.Collections.Generic;
using System.Linq;
using xsolla_revenue_calculator.Models.ForecastModels;

namespace xsolla_revenue_calculator.Services.ForecastExportService
{
    public class ChartService : IChartService
    {
        public List<Tuple<string, string>> GetChartUrl(RevenueForecasts forecasts)
        {
          var tendencyChartConfig = GetTendencyChartConfig(forecasts);
          var cumulativeChartConfig = GetCumulativeChartConfig(forecasts);
          string baseChartUrl = "https://quickchart.io/chart?width=500&height=350&chart=";
          string tendencyChartUrl = baseChartUrl + Uri.EscapeDataString(tendencyChartConfig);
          string cumulativeChartUrl = baseChartUrl + Uri.EscapeDataString(cumulativeChartConfig);
          return new List<Tuple<string, string>>
          {
            new Tuple<string, string>
            (
              "Tendency", tendencyChartUrl
            ),
            new Tuple<string, string>
            (
              "Cumulative", cumulativeChartUrl
            )
          };
        }

        private string GetTendencyChartConfig(RevenueForecasts forecasts)
        {
          return GetBaseChartConfig(
            "Revenue (tendency)",
            forecasts.ChosenForecast.Monetization,
            forecasts.ChosenForecast.TendencyForecast,
            forecasts.OtherForecasts.Single().Monetization,
            forecasts.OtherForecasts.Single().TendencyForecast
          );
        }
        
        private string GetCumulativeChartConfig(RevenueForecasts forecasts)
        {
          return GetBaseChartConfig(
            "Revenue (cumulative)",
            forecasts.ChosenForecast.Monetization,
            forecasts.ChosenForecast.CumulativeForecast,
            forecasts.OtherForecasts.Single().Monetization,
            forecasts.OtherForecasts.Single().CumulativeForecast
          );
        }

        private string GetBaseChartConfig(string title, string chosenForecastLabel, List<double> chosenForecast, string anotherForecastLabel, List<double> anotherForecast)
        {
          int monthCount = chosenForecast.Count;
          string yAxisLabels = $"[{String.Join(',', Enumerable.Range(1, monthCount))}]";
          string chosenForecastString = $"[{String.Join(',', chosenForecast)}]";
          string anotherForecastString = $"[{String.Join(',', anotherForecast)}]";
          string config = $@"{{
  type: 'line',
  data: {{
    labels: {yAxisLabels},
    datasets: [
      {{
        label: '{chosenForecastLabel}',
        backgroundColor: 'rgb(255, 99, 132)',
        borderColor: 'rgb(255, 99, 132)',
        data: {chosenForecastString},
        fill: false,
      }},
      {{
        label: '{anotherForecastLabel}',
        fill: false,
        backgroundColor: 'rgb(54, 162, 235)',
        borderColor: 'rgb(54, 162, 235)',
        data: {anotherForecastString},
      }},
    ],
  }},
  options: {{
    title: {{
      display: true,
      text: '{title}',
    }},
      scales: {{
        yAxes: [{{
          ticks: {{
            suggestedMin: 0
          }},
          scaleLabel: {{
            display: true,
            labelString: 'Revenue'
          }}
        }}],
          xAxes: [{{
            scaleLabel: {{
              display: true,
              labelString: 'Month'
            }}
          }}],
      }}
  }},
}}
";
          return config;
        }
    }

    public interface IChartService
    {
      List<Tuple<string, string>> GetChartUrl(RevenueForecasts forecasts);
    }
}