using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public OrderController(ICartRepository cartRepository, IOrderRepository orderRepository)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
    }


    [HttpGet]
    [Route("GetMyOrders")]
    public async Task<ActionResult<CartResponse>> GetMyOrders()
    {
        var userId = GetUserId();
        var userOrders = await _orderRepository.GetAllOrdersForUser(userId);
        return Ok(userOrders);
    }

    [HttpPost]
    [Route("UpdateMyOrderDetails")]
    public async Task<ActionResult<CartResponse>> UpdateMyOrderDetails([FromBody] List<ArticleDto> articles)
    {
        return Ok();
    }

}