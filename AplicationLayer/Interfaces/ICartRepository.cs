using AplicationLayer.DTOs;
using DomainLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.Interfaces
{
    public interface ICartRepository
    {
        Task<CartResponse?> GetCartForUserAsync(int userId);
        Task EnsureCartForUserAsync(int userId);
        Task<CartResponse> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles);
        Task<bool> ClearCartAsync(int userId);
    }
}
