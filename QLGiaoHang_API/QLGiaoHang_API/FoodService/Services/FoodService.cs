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
        // Thêm nhà hàng
        public Task<bool> CreateNhaHang(NhaHang nhaHang)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TenNhaHang", nhaHang.TenNhaHang),
                new SqlParameter("@DiaChi", nhaHang.DiaChi),
                new SqlParameter("@SoDienThoai", nhaHang.SoDienThoai),
                new SqlParameter("@HinhAnh", nhaHang.HinhAnh),
                new SqlParameter("@MinOrder", nhaHang.MinOrder),
                new SqlParameter("@MaCode", nhaHang.Macode)
            };

            int result = _helper.ExecuteNonQuery("sp_ThemNhaHang", parameters);

            return Task.FromResult(result > 0);
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

        // ------------------- MÓN ĂN ------------------

        //Thêm món ăn
        public async Task<bool> Create(MonAn monAn)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter("@TenMon", monAn.TenMon),
                new SqlParameter("@Gia", monAn.Gia),
                new SqlParameter("@MoTa", monAn.MoTa),
                new SqlParameter("@HinhAnh", monAn.HinhAnh),
                new SqlParameter("@MaDanhMuc", monAn.MaDanhMuc),
                new SqlParameter("@MaNhaHang", monAn.MaNhaHang)
            };

                await Task.Run(() =>
                {
                    _helper.ExecuteNonQuery("sp_ThemMonAn", parameters);
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sửa món ăn
        public async Task<bool> Update(MonAn monAn)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter("@MaMonAn", monAn.MaMonAn),
                new SqlParameter("@TenMon", monAn.TenMon),
                new SqlParameter("@Gia", monAn.Gia),
                new SqlParameter("@MoTa", monAn.MoTa),
                new SqlParameter("@HinhAnh", monAn.HinhAnh),
                new SqlParameter("@MaDanhMuc", monAn.MaDanhMuc),
                new SqlParameter("@MaNhaHang", monAn.MaNhaHang)
            };

                await Task.Run(() =>
                {
                    _helper.ExecuteNonQuery("sp_SuaMonAn", parameters);
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
        // Xóa món ăn
        public async Task<bool> Delete(int maMonAn)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter("@MaMonAn", maMonAn)
            };

                await Task.Run(() =>
                {
                    _helper.ExecuteNonQuery("sp_XoaMonAn", parameters);
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

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

        public Task<bool> UpdateNhaHang(NhaHang nhaHang)
        {
            throw new NotImplementedException();
        }
    }
}
