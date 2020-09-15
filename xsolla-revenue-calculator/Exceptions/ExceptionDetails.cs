using System;
using Newtonsoft.Json;

namespace xsolla_revenue_calculator.Exceptions
{
    public class ExceptionDetails
    {
        public ExceptionDetails(Exception exception)
        {
            Message = exception.Message;
        }

        public int StatusCode { get; set; } = 500;
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}