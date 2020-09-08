using MongoDB.Bson;

namespace xsolla_revenue_calculator.Models
{
    public class UserInfo
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
    }
}