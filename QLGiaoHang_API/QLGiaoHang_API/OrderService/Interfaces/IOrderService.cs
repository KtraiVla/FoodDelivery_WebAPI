using OrderService.DTOs;


namespace OrderService.Interfaces
{
    public interface IOrderService
    {
        // KHÁCH 
        (int ResultCode, string Message, int OrderId) CreateOrder(int userId, CreateOrderRequest request);    // Tạo đơn hàng mới
        List<OrderResponseDto> GetMyOrders(int userId, string? statusFilter);    // xem lịch sử đặt hàng
        (bool Success, string Message) CancelOrder(int userId, int orderId);   // hủy đơn hàng

        // NHÀ HÀNG & ADMIN
        List<OrderResponseDto> GetMerchantOrders(int userId); // userId của nhân viên -> tìm ra quán
        List<OrderResponseDto> GetAllOrders(); // Admin
        (bool Success, string Message) UpdateOrderStatus(int orderId, string newStatus);
        OrderStatsDto GetStats();

        // SHIPPER
        List<OrderResponseDto> GetPendingOrdersForShipper();
        (bool Success, string Message) ShipperReceiveOrder(int userId, int orderId);
        List<OrderResponseDto> GetShipperTasks(int userId);
        (bool Success, string Message) ShipperCompleteOrder(int userId, int orderId);

    }
}
   
