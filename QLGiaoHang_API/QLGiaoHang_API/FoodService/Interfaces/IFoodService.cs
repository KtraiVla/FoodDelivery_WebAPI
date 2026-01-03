using FoodService.DTOs;
namespace FoodService.Interfaces
{
    public class IFoodService
    {
        // 1. Nhà Hàng
        Task<List<NhaHang>> GetAllNhaHang();

        //2. Danh muc theo nha hang
        Task<List<DanhMuc>> GetDanhMucByNhaHang(int MaNhaHang);

        //3. Món ăn theo danh mục
        Task<List<MonAn>> GetMonAnByDanhMuc(int MaDanhMuc);

        //4. Chi tiết món ăn
        Task<MonAn> GetMonAnById(int MaMonAn);
    }
}
