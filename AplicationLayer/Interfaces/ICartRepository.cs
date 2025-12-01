using AplicationLayer.DTOs;

namespace AplicationLayer.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto?> GetCartForUserAsync(int userId);
        Task EnsureCartForUserAsync(int userId);
        Task<CartDto> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles);
        Task<bool> RemoveArticleAsync(int userId, string name);
        Task<bool> ClearCartAsync(int userId);
    }
}
