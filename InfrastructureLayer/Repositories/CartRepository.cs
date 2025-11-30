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
        var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
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
        var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            var newCart = new Cart
            {
                UserId = userId,
                ArticlesJson = "[]"
            };

            _databaseContext.Carts.Add(newCart);
            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task<CartResponse> AddOrUpdateCartForUserAsync(int userId, List<ArticleDto> articles)
    {

        var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        foreach (var item in articles)
        {
            if (item.Id < 1)
                throw new ValidationException("Article Id must be ≥ 1");

            if (item.Quantity < 1)
                throw new ValidationException("Article quantity must be ≥ 1");

            if (item.Price < 1)
                throw new ValidationException("Article price must be ≥ 1");
        }
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };
            _databaseContext.Carts.Add(cart);
            cart.ArticlesJson = JsonSerializer.Serialize(articles);
        }
        else
        {
            List<ArticleDto> listOfArtiicles = new List<ArticleDto>();
            var currentArticles = string.IsNullOrWhiteSpace(cart.ArticlesJson) ? new List<ArticleDto>() : JsonSerializer.Deserialize<List<ArticleDto>>(cart.ArticlesJson)!;
            var articlesToAdd = articles;

            foreach (var item in currentArticles)
            {
                listOfArtiicles.Add(item);
            }
            foreach (var item in articlesToAdd)
            {
                var exists = listOfArtiicles.FirstOrDefault(x => x.Id == item.Id);
                if (exists != null)
                {
                    exists.Quantity += item.Quantity;
                }
                else
                {
                    listOfArtiicles.Add(item);
                }
            }
            cart.ArticlesJson = JsonSerializer.Serialize(listOfArtiicles);
        }
        await _databaseContext.SaveChangesAsync();

        var resultArticles = string.IsNullOrWhiteSpace(cart.ArticlesJson) ? new List<ArticleDto>() : JsonSerializer.Deserialize<List<ArticleDto>>(cart.ArticlesJson)!;
        return new CartResponse
        {
            Id = cart.Id,
            Articles = resultArticles
        };
    }
    public async Task<bool> RemoveArticleAsync(int userId, int articleId)
    {
        var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return false;

        var items = JsonSerializer.Deserialize<List<ArticleDto>>(cart.ArticlesJson)!;
        var item = items.FirstOrDefault(x => x.Id == articleId);

        if (item == null)
            return false;

        items.Remove(item);

        cart.ArticlesJson = JsonSerializer.Serialize(items);

        await _databaseContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        var cart = await _databaseContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;

        cart.ArticlesJson = "[]";
        await _databaseContext.SaveChangesAsync();
        return true;
    }
}