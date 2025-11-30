using AplicationLayer.Entities;
using AplicationLayer.DTOs;
using InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AplicationLayer.Interfaces;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _db;

    public CartRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CartResponse?> GetCartForUserAsync(int userId)
    {
        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return null;

        var articles = JsonSerializer.Deserialize<List<ArticleDto>>(cart.ArticlesJson)
                       ?? new List<ArticleDto>();

        return new CartResponse
        {
            Id = cart.Id,
            Articles = articles
        };
    }

    public async Task EnsureCartForUserAsync(int userId)
    {
        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            var newCart = new Cart
            {
                UserId = userId,
                ArticlesJson = "[]"
            };

            _db.Carts.Add(newCart);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<CartResponse> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles)
    {
        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };
            _db.Carts.Add(cart);
        }

        cart.ArticlesJson = JsonSerializer.Serialize(articles);

        await _db.SaveChangesAsync();

        return new CartResponse
        {
            Id = cart.Id,
            Articles = articles
        };
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;

        cart.ArticlesJson = "[]";
        await _db.SaveChangesAsync();
        return true;
    }
}