using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace xsolla_revenue_calculator.Models
{
    public class UserInfo
    {
        public ObjectId Id { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
    }
}