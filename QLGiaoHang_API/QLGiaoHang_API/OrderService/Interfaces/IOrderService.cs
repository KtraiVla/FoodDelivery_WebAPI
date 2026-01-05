using OrderService.DTOs;


namespace OrderService.Interfaces
{
    public interface IOrderService
    {
        (int ResultCode, string Message, int OrderId) CreateOrder(int userId, CreateOrderRequest request);
    }
}
