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

            if (orderDetails == null)
                throw new ValidationException("Order details must be provided.");
            if (string.IsNullOrWhiteSpace(orderDetails.DeliveryAdress))
                throw new ValidationException("Delivery address is required.");
            if (string.IsNullOrWhiteSpace(orderDetails.DeliveryCountry))
                throw new ValidationException("Delivery country is required.");
            if (string.IsNullOrWhiteSpace(orderDetails.DeliveryName))
                throw new ValidationException("Delivery name is required.");
            if (string.IsNullOrWhiteSpace(orderDetails.DeliveryCardNumber))
                throw new ValidationException("Delivery card number is required.");

            Order order = new Order()
            {
                UserId = userId,
                CartId = cart.Id,
                DeliveryAdress = orderDetails.DeliveryAdress,
                DeliveryCardNumber = orderDetails.DeliveryCardNumber,
                DeliveryCountry = orderDetails.DeliveryCountry,
                DeliveryName = orderDetails.DeliveryName
            };

            _databaseContext.Orders.Add(order);
            await _databaseContext.SaveChangesAsync();
            return orderDetails;
        }
        public async Task<bool> ExecuteOrderByIdAsync(int orderId)
        {
            var order = await _databaseContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return false; 

            if (order.IsDelivered)
                return false;

            order.IsDelivered = true;

            _databaseContext.Orders.Update(order);
            await _databaseContext.SaveChangesAsync();

            return true;
        }
    }
}
