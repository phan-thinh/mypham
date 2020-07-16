using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMyPham.Models;
using System.IO;
using PagedList;
using System.Data;
using System.Data.Sql;

namespace ShopMyPham.Controllers
{
    public class AdminController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();

        public LoaiNuocHoa LoaiNuocHoa { get; private set; }
        public object TenBaiViet { get; private set; }
        public object DanhMuc { get; private set; }

        public ActionResult ThemNuocHoa()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                return View();
            }
        }
        // GET: Admin
        public ActionResult TrangChuAd()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult NuocHoaAD(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                if (page == null)
                    page = 1;
                return View(db.NuocHoas.ToList().ToPagedList(page ?? 1, 8));
            }
        }
        // Thêm Nước Hoa
        [HttpPost]
        public ActionResult ThemNuocHoa(FormCollection frm, NuocHoa nuocHoa, HttpPostedFileBase AnhBia)
        {
            if (AnhBia != null)
            {
                string path = Server.MapPath("~/Content/Images/product/");
                AnhBia.SaveAs(path + Path.GetFileName(AnhBia.FileName));
                nuocHoa.Keywords = frm["Keywords"];
                nuocHoa.AnhBia = AnhBia.FileName;
                nuocHoa.TenNuocHoa = frm["TenNuocHoa"];
                nuocHoa.GiaBan = decimal.Parse(frm["Giaban"]);
                nuocHoa.MoTa = frm["MoTa"];
                nuocHoa.NgayCapNhat = DateTime.Parse(DateTime.Now.ToString());
                nuocHoa.SoLuongTon = int.Parse(frm["SoLuong"]);
                nuocHoa.MaNSX = int.Parse(frm["MaNSX"]);
                nuocHoa.MaLoaiNuocHoa = int.Parse(frm["MaLoaiNuocHoa"]);
                nuocHoa.MaNuocSanXuat = int.Parse(frm["MaLoaiNuocSanXuat"]);
                db.NuocHoas.Add(nuocHoa);
                db.SaveChanges();
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Thêm nước hoa mới thành công!');
                         window.location.href='/Admin/NuocHoaAD'
                         </script>
                      ");
            }
            else
            {
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Thêm nước hoa mới tất bại!');
                         window.location.href='/Admin/ThemNuocHoa'
                         </script>
                      ");
            }
        }
        public ActionResult DangNhapAD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAD(FormCollection frm, Admin ad)
        {
            string sTaiKhoan = frm["TaiKhoanAD"];
            string sMatKhau = frm["MatKhau"];
            ad = db.Admins.SingleOrDefault(n => n.TaiKhoanAd == sTaiKhoan && n.MatKhauAd == sMatKhau);
            if (ad == null)
            {
                ViewBag.Tb = "Tài Khoản Hoặc Mật Khẩu Không Chính Xác!";
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                Session["Admin"] = ad.MaAd;
                Session["TenAd"] = ad.HoTenAd;
                return RedirectToAction("TrangChuAD", "Admin");
            }
        }
        //Danh mục menu
        public ActionResult DanhMucMenu()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                return View(db.DanhMucs.OrderBy(n => n.STT).ToList());
            }
        }
        ////xử lý danh mục menu

        ////Danh mục nước hoa
        public ActionResult DanhMucLoaiNuocHoa()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                return View(db.LoaiNuocHoas.OrderBy(n => n.STT).ToList()); ;
            }
        }
        //Thêm danh mục loại nước hoa
        public ActionResult ThemDanhMucLoaiNuocHoa()
        {
            return View();
        }
        //Xử lý thêm danh mục nước hoa
        [HttpPost]
        public ActionResult ThemDanhMucLoaiNuocHoa(FormCollection frm, LoaiNuocHoa loaiNuocHoa)
        {
            try
            {
                loaiNuocHoa.TenLoaiNuocHoa = frm["TenLoaiNuocHoa"];
                loaiNuocHoa.Keywords = frm["Keywords"];
                loaiNuocHoa.MetaTitle = frm["MetaTitle"];
                loaiNuocHoa.Title = frm["Title"];
                loaiNuocHoa.STT = int.Parse(frm["STT"]);
                loaiNuocHoa.Description = frm["Description"];
                db.LoaiNuocHoas.Add(loaiNuocHoa);
                db.SaveChanges();
                return RedirectToAction("DanhMucLoaiNuocHoa", "Admin");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        //Edit danh mục loại nước hoa
        public ActionResult EditDanhMucLoaiNuocHoa(int? MaLoaiNuocHoa)
        {
            return View(db.LoaiNuocHoas.Where(n => n.MaLoaiNuocHoa == MaLoaiNuocHoa).ToList());
        }

        [HttpPost]
        public ActionResult CapNhatEditDanhMucLoaiNuocHoa(string TenLoaiNuocHoa, string Keywords, string Description, string Title, string MetaTitle, string STT, int MaLoaiNuocHoa, LoaiNuocHoa loaiNuocHoa)
        {
            try
            {
                loaiNuocHoa = db.LoaiNuocHoas.Single(n => n.MaLoaiNuocHoa == MaLoaiNuocHoa);
                loaiNuocHoa.TenLoaiNuocHoa = TenLoaiNuocHoa;
                loaiNuocHoa.Keywords = Keywords;
                loaiNuocHoa.Description = Description;
                loaiNuocHoa.Title = Title;
                loaiNuocHoa.MetaTitle = MetaTitle;
                loaiNuocHoa.STT = int.Parse(STT);
                db.Entry(loaiNuocHoa);
                db.SaveChanges();
                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("false");
            }
        }

        //bài viết
        public ActionResult BaiViet(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                if (page == null)
                    page = 1;
                return View(db.BaiViets.ToList().ToPagedList(page ?? 1, 8));
            }
        }
        //thêm bài viết
        public ActionResult ThemBaiViet()
        {
            ViewBag.ID = new SelectList(db.DanhMucs.Where(n => (n.ID != 1 && n.ID != 2 && n.ID != 1002)).ToList().OrderBy(n => n.TenDanhMuc), "ID", "TenDanhMuc");
            return View();
        }
        //thêm bài viết
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemBaiViet(FormCollection frm, BaiViet baiviet, HttpPostedFileBase AnhDaiDien)
        {
            try
            {
                ViewBag.ID = new SelectList(db.DanhMucs.Where(n => (n.ID != 1 && n.ID != 2 && n.ID != 1002)).ToList().OrderBy(n => n.TenDanhMuc), "ID", "TenDanhMuc");
                string path = Server.MapPath("~/Content/Images/baiviet/");
                AnhDaiDien.SaveAs(path + Path.GetFileName(AnhDaiDien.FileName));
                baiviet.AnhDaiDien = AnhDaiDien.FileName;
                baiviet.DanhMuc = int.Parse(frm["ID"]);
                baiviet.TenBaiViet = frm["TenBaiViet"];
                baiviet.Keywords = frm["MetaKeywords"];
                baiviet.Description = frm["MetaDescription"];
                baiviet.MetaTitle = frm["MetaTitle"];
                baiviet.Title = frm["Title"];
                baiviet.CapNhat = DateTime.Parse(DateTime.Now.ToString());
                baiviet.NoiDung = frm["NoiDung"];
                db.BaiViets.Add(baiviet);
                db.SaveChanges();
                return RedirectToAction("BaiViet", "Admin");
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        //thêm danh mục bài viết
        public ActionResult ThemDanhMucBaiViet()
        {
            return View();
        }

        //Edit danh mục bài viết
        public ActionResult EditDanhMucBaiViet(int? ID)
        {
            return View(db.DanhMucs.Where(n => n.ID == ID).ToList());
        }

        public ActionResult GiamGia(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                if (page == null)
                    page = 1;
                return View(db.PhieuGiamGias.ToList().ToPagedList(page ?? 1, 8));
            }
        }
        public ActionResult ThemMaGiamGia => View();
        // Xóa Sản Phẩm
        public ActionResult DeleteSP(int? id)
        {

            NuocHoa Lnh = db.NuocHoas.Find(id);
            return View(Lnh);
        }
        [HttpPost, ActionName("DeleteSP")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSP(int id, NuocHoa nuocHoa, ChiTietDonHang chiTietDonHang)
        {
            try {

                nuocHoa = db.NuocHoas.Where(x => x.MaNuocHoa == id).SingleOrDefault();
                db.NuocHoas.Remove(nuocHoa);
                db.SaveChanges();
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Sản phẩm đã xóa thành công!');
                         window.location.href='/Admin/NuocHoaAD'
                         </script>
                      ");
            }
            catch (Exception)
            {
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Sản phẩm đang bán không được xóa!');
                         window.location.href='/Admin/NuocHoaAD'
                         </script>
                      ");
            }
        }
        // Xóa Danh Mục Loại Nước Hoa
        public ActionResult DeleteDanhMuc(int? id)
        {

            LoaiNuocHoa Lnh = db.LoaiNuocHoas.Find(id);
            return View(Lnh);
        }

        [HttpPost, ActionName("DeleteDanhMuc")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDanhMuc(int id, LoaiNuocHoa loaiNuocHoa)
        {
            LoaiNuocHoa = db.LoaiNuocHoas.Where(x => x.MaLoaiNuocHoa == id).SingleOrDefault();
            db.LoaiNuocHoas.Remove(LoaiNuocHoa);
            db.SaveChanges();
            return RedirectToAction("DanhMucLoaiNuocHoa", "Admin");
        }
        // Xóa Danh Mục Bài Viết
        public ActionResult DeleteDanhMucBaiViet(int? id)
        {

            DanhMuc bv = db.DanhMucs.Find(id);
            return View(bv);
        }

        [HttpPost, ActionName("DeleteDanhMucBaiViet")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDanhMucBaiViet(int id, DanhMuc danhMuc)
        {
            danhMuc = db.DanhMucs.Where(x => x.ID == id).SingleOrDefault();
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("DanhMucMenu", "Admin");
        }
        // Xóa Bài Viết
        public ActionResult DeleteBaiViet(int? id)
        {

            BaiViet bv = db.BaiViets.Find(id);
            return View(bv);
        }

        [HttpPost, ActionName("DeleteBaiViet")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBaiViet(int id, BaiViet baiViet)
        {
            baiViet = db.BaiViets.Where(x => x.ID == id).SingleOrDefault();
            db.BaiViets.Remove(baiViet);
            db.SaveChanges();
            return RedirectToAction("BaiViet", "Admin");
        }
        // Xóa Mã Giảm Giá
        public ActionResult DeleteMaGiamGia(int? id)
        {

            PhieuGiamGia bv = db.PhieuGiamGias.Find(id);
            return View(bv);
        }

        [HttpPost, ActionName("DeleteMaGiamGia")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMaGiamGia(int id, PhieuGiamGia phieuGiamGia)
        {
            phieuGiamGia = db.PhieuGiamGias.Where(x => x.MaGiamGia == id).SingleOrDefault();
            db.PhieuGiamGias.Remove(phieuGiamGia);
            db.SaveChanges();
            return RedirectToAction("GiamGia", "Admin");
        }
        // Nhà Sản Xuất
        public ActionResult NhaSanXuat(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                if (page == null)
                    page = 1;
                return View(db.NhaSanXuats.ToList().ToPagedList(page ?? 1, 8));
            }
        }
        // Xóa Nhà Sản Xuất
        public ActionResult DeleteNhaSanXuat(int? id)
        {

            NhaSanXuat bv = db.NhaSanXuats.Find(id);
            return View(bv);
        }

        [HttpPost, ActionName("DeleteNhaSanXuat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNhaSanXuat(int id, NhaSanXuat nhaSanXuat)
        {

            nhaSanXuat = db.NhaSanXuats.Where(x => x.MaNSX == id).SingleOrDefault();
            db.NhaSanXuats.Remove(nhaSanXuat);
            db.SaveChanges();
            return RedirectToAction("NhaSanXuat", "Admin");
        }
        [ValidateInput(false)]
        // Thêm Nhà Sản Xuất
        [HttpPost]
        public ActionResult ThemNhaSanXuat(FormCollection frm,NhaSanXuat nhaSanXuat1)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                try
                {
                    nhaSanXuat1.TenNSX = frm["NhaSanXuat"];
                    db.NhaSanXuats.Add(nhaSanXuat1);
                    db.SaveChanges();
                    return RedirectToAction("NhaSanXuat", "Admin");
                }
                catch (Exception)
                {

                }
            }
           
            return View();
        }
        public ActionResult ThemNhaSanXuat()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhapAD", "Admin");
            }
            else
            {
                return View();
            }
        }
        //// Edit Nước Hoa
        public ActionResult EditNuocHoa(int? MaNuocHoa)
        {
            return View(db.NuocHoas.Where(n => n.MaNuocHoa == MaNuocHoa).ToList());
        }

        [HttpPost]
        public ActionResult CapNhatEditNuocHoa(String HinhAnh, string TenNuocHoa, string Keywords, int MaNuocHoa, string MoTa, Decimal GiaBan, NuocHoa nuocHoa)
        {
            try
            {
                nuocHoa = db.NuocHoas.Single(n => n.MaNuocHoa == MaNuocHoa);
                nuocHoa.TenNuocHoa = TenNuocHoa;
                nuocHoa.MoTa = MoTa;
                nuocHoa.GiaBan = GiaBan;
                db.Entry(nuocHoa);
                db.SaveChanges();
                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("false");
            }
        }
        

        // Thêm Mã Giảm Giá
        public ActionResult ThemPhieuGiamGia()

        {
            return View();
        }
        //Xử lý thêm mã giảm giá
        [HttpPost]
        public ActionResult ThemPhieuGiamGia(FormCollection frm, PhieuGiamGia phieuGiamGia)
        {
            try
            {
                phieuGiamGia.GiamGia = frm["GiamGia"];
                phieuGiamGia.ThoiGianBatDauGiamGia = DateTime.Parse(DateTime.Now.ToString());
                phieuGiamGia.ThoiGianKetThucGiamGia = DateTime.Parse(DateTime.Now.ToString());
                phieuGiamGia.MucGiamGia = int.Parse(frm["MucGiamGia"]);
                phieuGiamGia.SoLuong = int.Parse(frm["SoLuong"]);
                phieuGiamGia.TenGiamGia = frm["TenGiamGia"];
                db.PhieuGiamGias.Add(phieuGiamGia);
                db.SaveChanges();
                return RedirectToAction("GiamGia", "Admin");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}
