using AplicationLayer.DTOs;

namespace AplicationLayer.Interfaces
{
    public interface ICartRepository
    {
        Task<CartResponse?> GetCartForUserAsync(int userId);
        Task EnsureCartForUserAsync(int userId);
        Task<CartResponse> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles);
        Task<bool> RemoveArticleAsync(int userId, int articleId);
        Task<bool> ClearCartAsync(int userId);
    }
}
