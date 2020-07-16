using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMyPham.Models;
using System.Data;
using System.Data.Sql;
using System.Data.Entity;
using System.Net.Mail;

namespace ShopMyPham.Controllers
{
    public class GioHangController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public List<SanPhamGH> LayGioHang()
        {
            Session["NgayNhanHang"] = DateTime.Parse(DateTime.Now.AddDays(+5).ToString("dd-MM-yyyy"));
            List<SanPhamGH> lstSP = Session["GioHang"] as List<SanPhamGH>;
            if (lstSP == null)
            {
                lstSP = new List<SanPhamGH>();
                Session["GioHang"] = lstSP;
            }
            return lstSP;
        }
        public ActionResult GioHang()
        {
            List<SanPhamGH> listSP = LayGioHang();
            int TongSL = 0;
            double TongTien = 0;
            //int x = int.Parse(Session["Madh"].ToString());
            foreach (var item in listSP)
            {
                TongSL += item.SoLuong;
                TongTien += item.TongTien;
            }
            Session["TongSL"] = TongSL.ToString();
            Session["TongTien"] = String.Format("{0:C}", TongTien.ToString());
            ////List<DonHang> dh = db.DonHangs.Where(n => n.MaDonHang == n).ToList();
            //ViewData["dondathang"] = dh;
            ViewData["spdamua"] = listSP;
            return View(listSP);
        }
        [HttpPost]
        public ActionResult ThemGioHang(int iMaSP, int? SL)
        {
            List<SanPhamGH> lstSP = LayGioHang();
            SanPhamGH SP = lstSP.Find(n => n.MaSP == iMaSP);
            if (SP == null)
            {
                SP = new SanPhamGH();
                NuocHoa nuocHoa = db.NuocHoas.Single(n => n.MaNuocHoa == iMaSP);
                SP.MaSP = iMaSP;
                SP.TenSP = nuocHoa.TenNuocHoa;
                SP.AnhSP = nuocHoa.AnhBia;
                SP.GiaSP = double.Parse(nuocHoa.GiaBan.ToString());
                if (SL>0)
                {
                    SP.SoLuong = 1;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                lstSP.Add(SP);

                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SL == null)
                {
                    SP.SoLuong++;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult XoaSP(int iMaSP)
        {
            List<SanPhamGH> lstSP = LayGioHang();
            SanPhamGH SP = lstSP.Find(n => n.MaSP == iMaSP);
            lstSP.Remove(SP);
            Session["GioHang"] = lstSP;
            GioHang();
            return Json(lstSP, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GiamGia(FormCollection frm,PhieuGiamGia phieuGiamGia)
        {
            Session["loi"] = null;
            string MaGiamGia1 = frm["txtmagiamgia"];
                var MaGiamGia = db.PhieuGiamGias.Where(x => x.GiamGia == MaGiamGia1).SingleOrDefault();
                if (db.PhieuGiamGias.Where(x => x.GiamGia == MaGiamGia1).SingleOrDefault()!=null/*MaGiamGia.SoLuong > 0/* && MaGiamGia.MucGiamGia !=null*/ /*&& MaGiamGia.ThoiGianKetThucGiamGia<DateTime.Now*/)
                {
                    Session["giamgia"] = MaGiamGia.MucGiamGia;
                    int a = Convert.ToInt32(MaGiamGia.MucGiamGia);
                    int b = (int.Parse(Session["TongTien"].ToString()));
                    int c = b - ((a * b) / 100);
                    Session["TongTien1"] = c.ToString();
                    Session["TongTien"] = c.ToString();
                    MaGiamGia.SoLuong--;
                    db.Entry(MaGiamGia).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GioHang");
                }
                else
                {
                Session["giamgia"] = null;
                Session["loi"] = "Code hết hạn sử dụng hoặc đã hết lượt!";
                return RedirectToAction("GioHang");
                }

            }
        [HttpPost]
        public ActionResult GioHang(FormCollection frm, DonHang donhang, ShopMyPham.Models.MailModel model)
        {
            Session["NgayGiaoHang"] = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("DangNhap", "DKDN");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    donhang.MaKH = int.Parse(Session["MaKH"].ToString());
                    donhang.NgayDat = DateTime.Parse(DateTime.Now.ToString());
                    donhang.NgayGiao = DateTime.Parse(DateTime.Now.AddDays(+5).ToString("dd-MM-yyyy"));
                    donhang.TriGia = decimal.Parse(Session["TongTien1"].ToString());
                    bool a = false;
                    donhang.DaThanhToan = a;
                    donhang.TinhTrangGiaoHang = a;
                    donhang.TenNguoiNhan = frm["tennguoinhan"];
                    donhang.DienThoaiNhan = frm["dienthoainhanhang"];
                    donhang.DiaChiGH = frm["diachinhanhang"];
                    //donhang.Email = frm["email"];
                    db.DonHangs.Add(donhang);
                    db.SaveChanges();
                    Session["Madh"] = donhang.MaDonHang;
                    List<SanPhamGH> listSP = LayGioHang();
                    foreach (var item in listSP)
                    {
                        ChiTietDonHang ctdh = new ChiTietDonHang();
                        ctdh.MaDonHang = donhang.MaDonHang;
                        ctdh.MaNuocHoa = Convert.ToInt32(item.MaSP);
                        ctdh.SoLuong = item.SoLuong;
                        ctdh.DonGia = (decimal)item.GiaSP;
                        db.ChiTietDonHangs.Add(ctdh);
                        db.SaveChanges();
                    }
                   
                    Session["GioHang"] = null;
                    return RedirectToAction("ThanhToanThanhCong", "GioHang");
                }
                else
                {
                    return RedirectToAction("DangkY", "DKDN");
                }
            }
        }
        public ActionResult ThanhToanThanhCong()
        {
            Session["GioHang"] = null;
            Session["giamgia"] = null;
            int makh = int.Parse(Session["MaKH"].ToString());
            int sodh = int.Parse(Session["Madh"].ToString());
            List<SanPhamGH> listSP = LayGioHang();
            List<DonHang> dh = db.DonHangs.Where(n => n.MaDonHang == sodh).ToList();
            ViewData["dondathang"] = dh;
            ViewData["spdamua"] = listSP;
            List<SanPhamGH> tenspp = LayGioHang();
            List<ChiTietDonHang> ten = db.ChiTietDonHangs.Where(n => n.MaDonHang == sodh).ToList();
            List<KhachHang> kh = db.KhachHangs.Where(n=>n.MaKH == makh).ToList();
            ViewData["khachhang"] = kh;
            ViewData["tensp"] = ten;
            return View(listSP);
        }
        [HttpPost]
        public ActionResult ThanhToanThanhCong(ShopMyPham.Models.MailModel model)
        {
            if (ModelState.IsValid)
            {
                string to = model.To;
                string subject = model.Subject;
                string body = model.Body;
                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new MailAddress("phanvanthinh601@gmail.com");
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("phanvanthinh601@gmail.com", "Thinh@@01636142697");
                smtp.Send(mail);
                ViewBag.message = "giui r";
                Session["GioHang"] = null;
                return RedirectToAction("ViewTrangChu", "TrangChu");
            }
            else
            {
                return View();
            }
        }
    }
}