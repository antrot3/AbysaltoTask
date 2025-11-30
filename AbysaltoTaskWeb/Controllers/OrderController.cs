using AplicationLayer.DTOs;
using AplicationLayer.Entities;
using AplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public OrderController(ICartRepository cartRepository, IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet("myorders")]
    public async Task<ActionResult<List<Order>>> GetMyOrders()
    {
        var orders = await _orderRepository.GetAllOrdersForUser(GetUserId());
        return Ok(orders);
    }

    [HttpPost("create")]
    public async Task<ActionResult<OrderDetailsDTO>> CreateOrder([FromBody] OrderDetailsDTO orderDetails)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _orderRepository.CreateOrderFromCart(GetUserId(), orderDetails);
        return Ok(created);
    }

    [HttpPost("execute/{orderId}")]
    public async Task<IActionResult> ExecuteOrder(int orderId)
    {
        var success = await _orderRepository.ExecuteOrderByIdAsync(orderId);
        return success ? Ok("Order marked as delivered") : NotFound("Order not found or already executed");
    }
}
