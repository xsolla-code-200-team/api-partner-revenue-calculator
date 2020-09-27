using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

namespace xsolla_revenue_calculator.Exceptions
{
    public class ExceptionDetails
    {
        public ExceptionDetails(Exception exception)
        {
            Message = exception.Message;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public class ExceptionDetailsExceptionExample : IExamplesProvider<ExceptionDetails>
        {
            public ExceptionDetails GetExamples()
            {
                return new ExceptionDetails(new Exception("Something bad happened")){StatusCode = 500};
            }
        }
    }
}