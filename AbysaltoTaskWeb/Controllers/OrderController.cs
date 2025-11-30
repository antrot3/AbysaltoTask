using AplicationLayer.DTOs;
using AplicationLayer.Entities;
using AplicationLayer.Interfaces;
using DomainLayer.Entites;
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
    public async Task<ActionResult<List<Order>>> GetMyOrders()
    {
        var userId = GetUserId();
        var userOrders = await _orderRepository.GetAllOrdersForUser(userId);
        return Ok(userOrders);
    }

    [HttpPost]
    [Route("UpdateMyOrderDetails")]
    public async Task<ActionResult<OrderDetailsDTO>> CreateOrderFromCart([FromBody] OrderDetailsDTO userDetails)
    {
        var userId = GetUserId();
        var userOrders = await _orderRepository.CreateOrderFromCart(userId, userDetails);
        return Ok(userOrders);
    }

    [HttpPost]
    [Route("ExecuteOrder/{orderId}")]
    public async Task<IActionResult> ExecuteOrder(int orderId)
    {
        var success = await _orderRepository.ExecuteOrderByIdAsync(orderId);
        if (!success) return NotFound("Order not found or already executed");
        return Ok("Order marked as delivered");
    }

}