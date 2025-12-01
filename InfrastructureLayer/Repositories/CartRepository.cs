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

    public async Task<CartDto?> GetCartForUserAsync(int userId)
    {
        var cart = await _databaseContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        return new CartDto
        {
            Articles = cart.Items.Select(i => new ArticleDto
            {
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

    public async Task<CartDto> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles)
    {
        if (articles == null || !articles.Any())
            throw new ValidationException("No articles provided.");

        var cart = await _databaseContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _databaseContext.Carts.Add(cart);
        }

        var itemsDict = cart.Items.ToDictionary(x => x.Name);

        foreach (var article in articles)
        {
            if (article.Quantity < 1 || article.Price < 0)
                throw new ValidationException("Invalid article data.");

            if (itemsDict.TryGetValue(article.Name, out var existingItem))
                existingItem.Quantity += article.Quantity;
            else
                cart.Items.Add(new CartItem
                {
                    Name = article.Name,
                    Price = article.Price,
                    Quantity = article.Quantity
                });
        }

        await _databaseContext.SaveChangesAsync();

        return await GetCartForUserAsync(userId) ?? new CartDto();
    }

    public async Task<bool> RemoveArticleAsync(int userId, string name)
    {
        var cartItem = await _databaseContext.CartItems.Include(i => i.Cart).FirstOrDefaultAsync(i => i.Cart.UserId == userId && i.Name == name);

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
