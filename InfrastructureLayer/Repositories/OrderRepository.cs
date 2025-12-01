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

        public async Task<List<OrderDto>> GetAllOrdersForUser(int userId)
        {
            var response = await _db.Orders
                .Include(o => o.DeliveryCart)
                    .ThenInclude(c => c.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var result = new List<OrderDto>();

            foreach (var item in response)
            {
                var order = new OrderDto
                {
                    CreatedAt = item.CreatedAt,
                    DeliveryAddress = item.DeliveryAddress,
                    DeliveryCountry = item.DeliveryCountry,
                    DeliveryName = item.DeliveryName,
                    DeliveryCardNumberEncrypted = item.DeliveryCardNumberEncrypted,
                    IsDelivered = item.IsDelivered,
                    CartResponse = new CartDto
                    {
                        Articles = item.DeliveryCart.Items.Select(i => new ArticleDto
                        {
                            Quantity= i.Quantity,
                            Name = i.Name,
                            Price = i.Price,

                        }).ToList()
                    }
                };

                result.Add(order);
            }

            return result;
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
    }
}