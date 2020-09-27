using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        private readonly string _itemName;

        public ItemNotFoundException(string itemName)
        {
            _itemName = itemName;
        }

        public override string Message => $"{_itemName} was not found. " + base.Message;
        
        public class ItemNotFoundExceptionExample : IExamplesProvider<ItemNotFoundException>
        {
            public ItemNotFoundException GetExamples()
            {
                return new ItemNotFoundException("Entity");
            }
        }
    }
}