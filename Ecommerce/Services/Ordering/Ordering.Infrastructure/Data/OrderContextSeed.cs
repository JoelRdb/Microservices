using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed      
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded!");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "tsiry",
                    TotalPrice = 750,

                    FirstName = "Joël",
                    LastName = "RANDRIAMBAO",
                    EmailAddress = "joelrandriambao08@gmail.com",
                    AddressLine = "Ambohijanahary",
                    Country = "Madagascar",
                    State = "MG",
                    ZipCode = "101",

                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifyBy = "Joël",
                    LastModifiedDate = new DateTime(),
                },
            };

        }
    }
}
