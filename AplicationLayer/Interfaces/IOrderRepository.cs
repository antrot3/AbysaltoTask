using AplicationLayer.DTOs;
using AplicationLayer.Entities;
using DomainLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersForUser(int userId);
        Task<OrderDetailsDTO> CreateOrderFromCart(int userId, OrderDetailsDTO orderDetails);
    }
}
