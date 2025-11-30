using AplicationLayer.Entities;
using AplicationLayer.DTOs;
using InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AplicationLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _databaseContext;

    public CartRepository(AppDbContext database)
    {
        _databaseContext = database;
    }

    public async Task<CartResponse?> GetCartForUserAsync(int userId)
    {
        var cart = await _databaseContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        return new CartResponse
        {
            Id = cart.Id,
            Articles = cart.Items.Select(i => new ArticleDto
            {
                Id = i.ArticleId,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task EnsureCartForUserAsync(int userId)
    {
        if (!await _databaseContext.Carts.AnyAsync(c => c.UserId == userId))
        {
            _databaseContext.Carts.Add(new Cart { UserId = userId });
            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task<CartResponse> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles)
    {
        if (articles == null || !articles.Any())
            throw new ValidationException("No articles provided.");

        var cart = await _databaseContext.Carts.Include(c => c.Items)
                                  .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _databaseContext.Carts.Add(cart);
        }

        var itemsDict = cart.Items.ToDictionary(x => x.ArticleId);

        foreach (var article in articles)
        {
            if (article.Id < 1 || article.Quantity < 1 || article.Price < 0)
                throw new ValidationException("Invalid article data.");

            if (itemsDict.TryGetValue(article.Id, out var existingItem))
                existingItem.Quantity += article.Quantity;
            else
                cart.Items.Add(new CartItem
                {
                    ArticleId = article.Id,
                    Name = article.Name,
                    Price = article.Price,
                    Quantity = article.Quantity
                });
        }

        await _databaseContext.SaveChangesAsync();

        return await GetCartForUserAsync(userId) ?? new CartResponse();
    }

    public async Task<bool> RemoveArticleAsync(int userId, int articleId)
    {
        var cartItem = await _databaseContext.CartItems.Include(i => i.Cart).FirstOrDefaultAsync(i => i.Cart.UserId == userId && i.ArticleId == articleId);

        if (cartItem == null) 
            return false;

        _databaseContext.CartItems.Remove(cartItem);
        await _databaseContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        var cart = await _databaseContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
            return false;

        _databaseContext.CartItems.RemoveRange(cart.Items);
        await _databaseContext.SaveChangesAsync();
        return true;
    }
}
