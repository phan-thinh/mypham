using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using ShopMyPham.Models;
using Facebook;
using System.Configuration;

namespace ShopMyPham.Controllers
{
    public class DKDNController  : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        // GET: DKDN
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection frmDK, KhachHang KH)
        {
            KH.HoTen = frmDK["HoTen"];
            KH.GioiTinh = frmDK["GioiTinh"];
            KH.Email = frmDK["Email"];
            KH.DiaChi = frmDK["DiaChi"];
            KH.DienThoai = frmDK["SDT"];
            string NgaySinh = frmDK["NgaySinh"];
            DateTime date = DateTime.ParseExact(NgaySinh, "dd/MM/yyyy", null);
            KH.NgaySinh = date;
            KH.TaiKhoan = frmDK["TenDN"];
            KH.MatKhau = GetMD5(frmDK["MatKhau"]);
            db.KhachHangs.Add(KH);
            db.SaveChanges();
            return RedirectToAction("DangNhap", "DKDN");
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection frmDN)
        {
            string sTaiKhoan = frmDN["TaiKhoan"];
            string sMatKhau = GetMD5(frmDN["MK"]);
            KhachHang KH = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (KH != null)
            {
                Session["KhachHang"] = KH.MaKH;
                Session["TenKH"] = KH.HoTen;
                Session["MaKH"] = KH.MaKH;
                Session["Email"] = KH.Email;
                Session["ThongBao"] = "";
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Đăng nhập thành công!');
                         window.location.href='/TrangChu/ViewTrangChu'
                         </script>
                      ");
            }
            else
            {
                return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Sai tài khoản & mật khẩu!');
                         window.location.href='/DKDN/DangNhap'
                         </script>
                      ");
            }
        }
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            Session["KhachHang"] = null;
            Session["TenKH"] = null;
            Session["ThongBao"] = "";
            return RedirectToAction("ViewTrangChu", "TrangChu");
        }

        public ActionResult ThongTinCaNhan()
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "DKDN");
            }
            else
            {
                int MaKH = int.Parse(Session["KhachHang"].ToString());
                KhachHang KH = db.KhachHangs.Single(n => n.MaKH == MaKH);
                return View(KH);
            }

        }
        [HttpPost]
        public ActionResult CapNhatThongTin(string HoTen, string GioiTinh, string Email, string DiaChi, string SDT, string NgaySinh, int MaKH, KhachHang KH)
        {
            KH = db.KhachHangs.Single(n => n.MaKH == MaKH);
            KH.HoTen = HoTen;
            KH.GioiTinh = GioiTinh;
            KH.Email = Email;
            KH.DiaChi = DiaChi;
            KH.DienThoai = SDT;
            KH.NgaySinh = DateTime.Parse(NgaySinh);
            db.Entry(KH);
            db.SaveChanges();
            return Json("Cập Nhật Thành Công!");
        }
        [HttpPost]
        public ActionResult CapNhatMatKhau(int MaKH, string MatKhauCu, string MatKhauMoi)
        {
            KhachHang KH = db.KhachHangs.Single(n => n.MaKH == MaKH);
            string MK = GetMD5(MatKhauCu);
            string MKMoi = GetMD5(MatKhauMoi);
            if (MK == KH.MatKhau)
            {
                KH.MatKhau = MKMoi;
                db.Entry(KH);
                db.SaveChanges();
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }
        public string GetMD5(string MD5)
        {
            string str = "";
            byte[] A = System.Text.Encoding.UTF8.GetBytes(MD5);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            A = md5.ComputeHash(A);
            foreach (Byte b in A)
            {
                str += b.ToString("X2");
            }
            return str;
        }
        ShopNuocHoaEntities dn = new ShopNuocHoaEntities();
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult DNFB()
        {
            var fb = new FacebookClient();
            var DNurl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(DNurl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            Session["MaKH"] = null;
            FacebookClient fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "2331601510468906",
                client_secret = "3aa421c108336f8199f47eb68c95ff8b",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
                var accessToken = result.access_token;
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string first_name = me.first_name;
                string middedlename = me.middle_name;
                string last_name = me.last_name;
                
                KhachHang kh = new KhachHang();
                kh.Email = email;
                kh.TaiKhoan = first_name + " " + middedlename + " " + last_name;
                kh.MatKhau= first_name + " " + middedlename + " " + last_name;
                kh.HoTen = first_name + " "+ middedlename +" "+ last_name;
                KhachHang TK = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == kh.TaiKhoan);
                if (TK == null)
                {
                    db.KhachHangs.Add(kh);
                    db.SaveChanges();
                Session["MaKH"] = kh.MaKH;
                    Session["TenKH"] = kh.HoTen;
                    Session["ThongBao"] = "";
                    return RedirectToAction("ViewTrangChu", "TrangChu");
                }
                else
                {
                Session["MaKH"] = TK.MaKH;
                    Session["TenKH"] = TK.HoTen;
                    Session["ThongBao"] = "";
                    return RedirectToAction("ViewTrangChu", "TrangChu");
                }
        }

    }
}