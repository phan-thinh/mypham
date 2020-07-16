using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopMyPham.Models
{
    public class SanPhamGH
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public int? MaSP { get; set; }
        public string TenSP { get; set; }
        public double GiaSP { get; set; }
        public string MoTaSP { get; set; }
        public string AnhSP { get; set; }
        public DateTime Ngay { get; set; }
        public int SoLuongTonSP { get; set; }
        public int SoLuong { get; set; }
        public int MaNSX { get; set; }
        public double TongTien
        {
            get { return SoLuong * GiaSP; }
        }
        public int MaLoaiSP { get; set; }
        public string NoiDung { get; set; }
    }
}