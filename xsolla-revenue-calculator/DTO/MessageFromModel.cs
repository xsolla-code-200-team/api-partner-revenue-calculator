using System.Collections.Generic;

namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for receiving messages from model via RabbitMQ
    /// </summary>
    public class MessageFromModel
    {
        public string RevenueForecastId { get; set; }
        
        public List<double> Result { get; set; }

        public override string ToString()
        {
            var result = "";
            foreach (double value in Result)
            {
                result += $"{value} ";
            }
            return $"RevenueForecastID: {RevenueForecastId}, Result: {result}";
        }
    }
}