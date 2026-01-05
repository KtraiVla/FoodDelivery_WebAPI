using FoodService.DTOs;
namespace FoodService.Interfaces
{
    public interface IFoodService
    {
        // ===== NHÀ HÀNG =====
        Task<List<NhaHang>> GetAllNhaHang();
        Task<NhaHang?> GetNhaHangById(int maNhaHang);

        // ===== DANH MỤC =====
        Task<List<DanhMuc>> GetAllDanhMuc();
        Task<List<DanhMuc>> GetDanhMucByNhaHang(int maNhaHang);

        // ===== MÓN ĂN =====
        Task<List<MonAn>> GetMonAnByDanhMuc(int maDanhMuc, int MaNhaHang);
        Task<List<MonAn>> GetMonAnByNhaHang(int maNhaHang);
        Task<MonAn?> GetMonAnById(int maMonAn);

        Task<bool> Create(MonAn monAn);
        Task<bool> Update(MonAn monAn);
    }
}
