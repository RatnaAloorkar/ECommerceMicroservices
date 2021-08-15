using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;

        private readonly ILogger<OrdersProvider> logger;

        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Total = 100,
                    Items = new List<Db.OrderItem>() { new Db.OrderItem() { Id = 1, ProductId = 1, Quantity = 10, UnitPrice = 10 } }
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Total = 200,
                    Items = new List<Db.OrderItem>() { new Db.OrderItem() { Id = 2, ProductId = 2, Quantity = 20, UnitPrice = 10 } }
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 3,
                    OrderDate = DateTime.Now,
                    Total = 300,
                    Items = new List<Db.OrderItem>() { new Db.OrderItem() { Id = 3, ProductId = 3, Quantity = 10, UnitPrice = 10 } }
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 4,
                    CustomerId = 4,
                    OrderDate = DateTime.Now,
                    Total = 400,
                    Items = new List<Db.OrderItem>() { new Db.OrderItem() { Id = 4, ProductId = 4, Quantity = 10, UnitPrice = 10 } }
                });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(c => c.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
