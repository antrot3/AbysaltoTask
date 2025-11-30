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
        private readonly AppDbContext _db;
        private readonly IEncryptionService _encryptionService;

        public OrderRepository(AppDbContext db, IEncryptionService encryptionService)
        {
            _db = db;
            _encryptionService = encryptionService;
        }

        public async Task<List<Order>> GetAllOrdersForUser(int userId)
        {
            return await _db.Orders.Include(o => o.DeliveryCart)
                                   .ThenInclude(c => c.Items)
                                   .Where(o => o.UserId == userId)
                                   .ToListAsync();
        }

        public async Task<OrderDetailsDTO> CreateOrderFromCart(int userId, OrderDetailsDTO details)
        {
            if (details == null) 
                throw new ValidationException("Order details are required.");

            var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.Items.Any())
                throw new ValidationException("Cart must have at least one item.");

            var order = new Order
            {
                UserId = userId,
                CartId = cart.Id,
                DeliveryAddress = details.DeliveryAddress,
                DeliveryCountry = details.DeliveryCountry,
                DeliveryName = details.DeliveryName,
                DeliveryCardNumberEncrypted = _encryptionService.Encrypt(details.DeliveryCardNumber)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return details;
        }

        public async Task<bool> ExecuteOrderByIdAsync(int orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null || order.IsDelivered) 
                return false;

            order.IsDelivered = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}