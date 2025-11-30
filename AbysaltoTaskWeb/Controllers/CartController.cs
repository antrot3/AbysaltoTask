using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetMyCart()
    {
        var userId = GetUserId();
        var cart = await _cartRepository.GetCartForUserAsync(userId);
        if (cart == null) return NotFound();

        return Ok(cart);
    }

    [HttpPost]
    public async Task<ActionResult<CartResponse>> UpdateMyCart([FromBody] List<ArticleDto> articles)
    {
        var userId = GetUserId();
        var cart = await _cartRepository.AddOrUpdateCartForUserAsync(userId, articles);
        return Ok(cart);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearMyCart()
    {
        var userId = GetUserId();
        var ok = await _cartRepository.ClearCartAsync(userId);
        return ok ? NoContent() : NotFound();
    }
}