using AplicationLayer.DTOs;
using AplicationLayer.Entities;
using AplicationLayer.Interfaces;
using DomainLayer.Entites;
using InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _databaseContext;

        public OrderRepository(AppDbContext database)
        {
            _databaseContext = database;
        }

        public async Task<List<Order>> GetAllOrdersForUser(int userId)
        {
            return await _databaseContext.Orders
                .Include(o => o.DeliveryCart)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<OrderDetailsDTO> CreateOrderFromCart(int userId, OrderDetailsDTO orderDetails)
        {
            var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                throw new ValidationException("Cart needs to have some items before creating order");
            }
            Order order = new Order()
            {
                UserId = userId,
                CartId = cart.Id,
                DeliveryAdress =orderDetails.DeliveryAdress,
                DeliveryCardNumber = orderDetails.DeliveryCardNumber,
                DeliveryCountry = orderDetails.DeliveryCountry,
                DeliveryName = orderDetails.DeliveryName
            };

            _databaseContext.Orders.Add(order);
            await _databaseContext.SaveChangesAsync();
            return orderDetails;
        }
    }
}
