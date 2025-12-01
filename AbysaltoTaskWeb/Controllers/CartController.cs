using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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

    [HttpGet("mycart")]
    public async Task<ActionResult<CartDto>> GetMyCart()
    {
        var cart = await _cartRepository.GetCartForUserAsync(GetUserId());
        return cart == null ? NotFound() : Ok(cart);
    }

    [HttpPost("update")]
    public async Task<ActionResult<CartDto>> UpdateMyCart([FromBody] List<ArticleDto> articles)
    {
        var cart = await _cartRepository.AddOrUpdateCartForUserAsync(GetUserId(), articles);
        return Ok(cart);
    }

    [HttpPost("remove")]
    public async Task<ActionResult<CartDto>> RemoveArticle([FromBody] int articleId)
    {
        var success = await _cartRepository.RemoveArticleAsync(GetUserId(), articleId);
        return success ? Ok(await _cartRepository.GetCartForUserAsync(GetUserId())) : NotFound();
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var success = await _cartRepository.ClearCartAsync(GetUserId());
        return success ? NoContent() : NotFound();
    }
}
