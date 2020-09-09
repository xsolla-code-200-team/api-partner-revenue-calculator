namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for sending via RabbitMQ
    /// </summary>
    public class MessageToModel
    {
        public string Message { get; set; }
    }
}