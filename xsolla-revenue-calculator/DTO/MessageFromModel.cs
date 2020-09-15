namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for receiving messages from model via RabbitMQ
    /// </summary>
    public class MessageFromModel
    {
        public string RevenueForecastId { get; set; }
        public string Result { get; set; }

        public override string ToString()
        {
            return $"RevenueForecastID: {RevenueForecastId}, Result: {Result}";
        }
    }
}