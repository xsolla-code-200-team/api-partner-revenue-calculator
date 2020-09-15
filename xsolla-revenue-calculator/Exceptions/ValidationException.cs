using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace xsolla_revenue_calculator.Exceptions
{
    public class ValidationException : Exception
    {
        private readonly IEnumerable<KeyValuePair<string, ModelStateEntry>> _modelState;
        public ValidationException(IEnumerable<KeyValuePair<string,ModelStateEntry>> modelState)
        {
            _modelState = modelState;
        }

        public override string Message
        {
            get
            {
                var message = "Validation exception: ";
                foreach (var entry in _modelState)
                {
                    message += $"{entry.Key}: ";
                    foreach (var error in entry.Value.Errors)
                    {
                        message += $"{error.ErrorMessage} ";
                    }
                }
                return message;
            }
        }
    }
}