using FoodService.DTOs;
using FoodService.Interfaces;
using Shared.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FoodService.Services
{
    public class FoodService : IFoodService
    {
        private readonly SqlHelper _helper;

        public FoodService(SqlHelper helper)
        {
            _helper = helper;
        }

        // ===================== NHÀ HÀNG =====================

        public Task<List<NhaHang>> GetAllNhaHang()
        {
            List<NhaHang> list = new();

            DataTable dt = _helper.ExecuteQuery("sp_GetAllNhaHang");

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new NhaHang
                {
                    MaNhaHang = Convert.ToInt32(row["MaNhaHang"]),
                    TenNhaHang = row["TenNhaHang"]?.ToString(),
                    DiaChi = row["DiaChi"]?.ToString(),
                    SoDienThoai = row["SoDienThoai"]?.ToString(),
                    HinhAnh = row["HinhAnh"]?.ToString(),
                    MinOrder = Convert.ToInt32(Convert.ToDecimal(row["MinOrder"])),
                    Macode = row["MaCode"]?.ToString()
                });
            }

            return Task.FromResult(list);
        }

        //public Task<NhaHang?> GetNhaHangById(int maNhaHang)
        //{
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@MaNhaHang", maNhaHang)
        //    };

        //    DataTable dt = _helper.ExecuteQuery("sp_GetNhaHangById", parameters);

        //    if (dt.Rows.Count == 0)
        //        return Task.FromResult<NhaHang?>(null);

        //    DataRow row = dt.Rows[0];

        //    return Task.FromResult(new NhaHang
        //    {
        //        MaNhaHang = Convert.ToInt32(row["MaNhaHang"]),
        //        TenNhaHang = row["TenNhaHang"].ToString(),
        //        DiaChi = row["DiaChi"].ToString(),
        //        SoDienThoai = row["SoDienThoai"].ToString(),
        //        HinhAnh = row["HinhAnh"].ToString(),
        //        MinOrder = Convert.ToInt32(row["MinOrder"]),
        //        Macode = row["MaCode"].ToString()
        //    });
        //}

        // tìm nhà hàng
        public Task<NhaHang?> GetNhaHangById(int maNhaHang)
        {
            SqlParameter[] parameters =
            {
        new SqlParameter("@MaNhaHang", maNhaHang)
    };

            DataTable dt = _helper.ExecuteQuery("dbo.sp_GetNhaHangById", parameters);

            if (dt.Rows.Count == 0)
                return Task.FromResult<NhaHang?>(null);

            DataRow row = dt.Rows[0];

            var nhaHang = new NhaHang
            {
                MaNhaHang = Convert.ToInt32(row["MaNhaHang"]),
                TenNhaHang = row["TenNhaHang"]?.ToString(),
                DiaChi = row["DiaChi"]?.ToString(),
                SoDienThoai = row["SoDienThoai"]?.ToString(),
                HinhAnh = row["HinhAnh"]?.ToString(),
                MinOrder = Convert.ToInt32(Convert.ToDecimal(row["MinOrder"])),
                Macode = row["MaCode"]?.ToString()
            };

            return Task.FromResult(nhaHang);
        }

        // ===================== DANH MỤC =====================

        public Task<List<DanhMuc>> GetAllDanhMuc()
        {
            List<DanhMuc> list = new();

            DataTable dt = _helper.ExecuteQuery("sp_GetAllDanhMuc");

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DanhMuc
                {
                    MaDanhMuc = Convert.ToInt32(row["MaDanhMuc"]),
                    TenDanhMuc = row["TenDanhMuc"].ToString()
                });
            }

            return Task.FromResult(list);
        }

        public Task<List<DanhMuc>> GetDanhMucByNhaHang(int maNhaHang)
        {
            List<DanhMuc> list = new();

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaNhaHang", maNhaHang)
            };

            DataTable dt = _helper.ExecuteQuery("sp_GetDanhMucByNhaHang", parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DanhMuc
                {
                    MaDanhMuc = Convert.ToInt32(row["MaDanhMuc"]),
                    TenDanhMuc = row["TenDanhMuc"].ToString()
                });
            }

            return Task.FromResult(list);
        }

        // ===================== MÓN ĂN =====================

        public Task<List<MonAn>> GetMonAnByDanhMuc(int maDanhMuc, int maNhaHang)
        {
            List<MonAn> list = new();

            SqlParameter[] parameters =
            {
        new SqlParameter("@MaDanhMuc", maDanhMuc),
        new SqlParameter("@MaNhaHang", maNhaHang)
    };

            DataTable dt = _helper.ExecuteQuery("sp_GetMonAnByDanhMuc", parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MonAn
                {
                    MaMonAn = Convert.ToInt32(row["MaMonAn"]),
                    TenMon = row["TenMon"].ToString(),
                    Gia = Convert.ToDouble(row["Gia"]),
                    MoTa = row["MoTa"].ToString(),
                    HinhAnh = row["HinhAnh"].ToString(),
                    MaDanhMuc = Convert.ToInt32(row["MaDanhMuc"]),
                    MaNhaHang = Convert.ToInt32(row["MaNhaHang"])
                });
            }

            return Task.FromResult(list);
        }

        public Task<List<MonAn>> GetMonAnByNhaHang(int maNhaHang)
        {
            List<MonAn> list = new();

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaNhaHang", maNhaHang)
            };

            DataTable dt = _helper.ExecuteQuery("sp_GetMonAnByNhaHang", parameters);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MonAn
                {
                    MaMonAn = Convert.ToInt32(row["MaMonAn"]),
                    TenMon = row["TenMon"].ToString(),
                    Gia = Convert.ToDouble(row["Gia"]),
                    MoTa = row["MoTa"].ToString(),
                    HinhAnh = row["HinhAnh"].ToString(),
                    MaDanhMuc = Convert.ToInt32(row["MaDanhMuc"]),
                    MaNhaHang = Convert.ToInt32(row["MaNhaHang"])
                });
            }

            return Task.FromResult(list);
        }

        public Task<MonAn?> GetMonAnById(int maMonAn)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaMonAn", maMonAn)
            };

            DataTable dt = _helper.ExecuteQuery("sp_GetMonAnById", parameters);

            if (dt.Rows.Count == 0)
                return Task.FromResult<MonAn?>(null);

            DataRow row = dt.Rows[0];

            return Task.FromResult(new MonAn
            {
                MaMonAn = Convert.ToInt32(row["MaMonAn"]),
                TenMon = row["TenMon"].ToString(),
                Gia = Convert.ToDouble(row["Gia"]),
                MoTa = row["MoTa"].ToString(),
                HinhAnh = row["HinhAnh"].ToString(),
                MaDanhMuc = Convert.ToInt32(row["MaDanhMuc"]),
                MaNhaHang = Convert.ToInt32(row["MaNhaHang"])
            });
        }
    }
}
