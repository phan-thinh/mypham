using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using ShopMyPham.Models;

namespace ShopMyPham.Controllers
{
    public class ThongKeController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        // GET: ThongKe
        public ActionResult Edita()
        {
            return View();
        }

        private List<DonHang> ddh;
        public DonHang donHang1;
        public ActionResult ThongKe(int? iyear, int? imonth)
        {
            List<int> ItemMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 2019; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            //int month = int.Parse(frmTao["thang"]);
            //int year = int.Parse(frmTao["nam"]);

            ddh = db.DonHangs.Where(n => n.NgayDat.Value.Month == imonth && n.NgayDat.Value.Year == iyear).ToList();
            var x = db.DonHangs.Where(n => n.NgayDat.Value.Month == imonth && n.NgayDat.Value.Year == iyear && n.TinhTrangGiaoHang == true).Sum(n => n.TriGia);

         
            Session["TongDG"] = x;
            Session["month"] = imonth;
            Session["year"] = iyear;
            Session["data"] = ddh;
            donHang1 = db.DonHangs.SingleOrDefault(n => n.NgayDat.Value.Month == imonth && n.NgayDat.Value.Year == iyear);
            return View(ddh);
        }
        public ActionResult HoaDonThanhToan(int? year, int? month, int? day)
        {
            List<int> ItemDay = new List<int>();
            for (int i = 1; i < 31; i++)
            {
                ItemDay.Add(i);
            }
            List<int> ItemMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 2019; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemday = new SelectList(ItemDay);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            ViewBag.ItemDay = itemday;
            List<DonHang> donhang1 = db.DonHangs.ToList();
            List<DonHang> donhang = db.DonHangs.Where(n => n.NgayDat.Value.Month == month && n.NgayDat.Value.Year == year && n.NgayDat.Value.Day == day).ToList();

            Session["data"] = donhang;
            Session["year"] = year;
            Session["month"] = month;

            return View(donhang1);
        }
        public ActionResult Hoadonngaythanhtoan(int? day, int? year, int? month)
        {
            List<int> ItemDay = new List<int>();
            for (int i = 1; i < 31; i++)
            {
                ItemDay.Add(i);
            }
            List<int> ItemMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 2019; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemday = new SelectList(ItemDay);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            ViewBag.ItemDay = itemday;
            Session["year"] = year;
            Session["month"] = month;
            Session["day"] = day;
            List<DonHang> donhang = db.DonHangs.Where(n => n.NgayDat.Value.Day == day && n.NgayDat.Value.Month == month && n.NgayDat.Value.Year == year).ToList();
            return View(donhang);
        }
        public ActionResult Hoadonthangthanhtoan(int? year, int? month)
        {
            List<int> ItemDay = new List<int>();
            for (int i = 1; i < 31; i++)
            {
                ItemDay.Add(i);
            }
            List<int> ItemMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 2019; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemday = new SelectList(ItemDay);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            ViewBag.ItemDay = itemday;
            Session["year"] = year;
            Session["month"] = month;
            List<DonHang> ddh = db.DonHangs.Where(n => n.NgayDat.Value.Month == month && n.NgayDat.Value.Year == year && n.TinhTrangGiaoHang == true ).ToList();
            var x = db.DonHangs.Where(n => n.NgayDat.Value.Month == month && n.NgayDat.Value.Year == year && n.TinhTrangGiaoHang == true).Sum(n => n.TriGia);
            Session["tongtien"] = x;
            return View(ddh);
        }
        public ActionResult Hoadonnamthanhtoan(int? year)
        {
            List<int> ItemDay = new List<int>();
            for (int i = 1; i < 31; i++)
            {
                ItemDay.Add(i);
            }
            List<int> ItemMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 2019; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemday = new SelectList(ItemDay);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            ViewBag.ItemDay = itemday;
            Session["year"] = year;
            List<DonHang> donhang = db.DonHangs.Where(n => n.NgayDat.Value.Year == year).ToList();
            return View(donhang);
        }
        public ActionResult Edit(int? id)
        {

            //CTDONHANG dh = db.CTDONHANGs.Find(sohd);
            List<DonHang> donhang = db.DonHangs.Where(n => n.MaDonHang == id).ToList();
            //ViewData["datadh"] = db.SANPHAMs.Where(n => n.MaSP == dh.MaSP );
            return View(donhang);
        }


        [HttpPost]
        public ActionResult CapNhatEditThongKe(int MaHoaDon, string TinhTrangGiaoHang, DonHang donHang)
        {
            try
            {
                {
                    donHang = db.DonHangs.Single(n => n.MaDonHang == MaHoaDon);
                    if (TinhTrangGiaoHang.Equals("True"))
                    {
                        donHang.TinhTrangGiaoHang = true;
                    }
                    else
                    {
                        donHang.TinhTrangGiaoHang = false;
                    }

                    db.Entry(donHang);
                    db.SaveChanges();
                   
                }
            }
            catch (Exception ex)
            {
                return Json("false");
            }
            return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Cập nhật thành công!');
                         window.location.href='/ThongKe/Hoadonthangthanhtoan'
                         </script>
                      ");
            try
            {
                {
                    donHang = db.DonHangs.Single(n => n.MaDonHang == MaHoaDon);
                    if (TinhTrangGiaoHang.Equals("false"))
                    {
                        donHang.TinhTrangGiaoHang = false;
                    }
                    else
                    {
                        donHang.TinhTrangGiaoHang = true;
                    }

                    db.Entry(donHang);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return Json("true");
            }
            return Content(@"<script language='javascript' type='text/javascript'>
                         alert('Cập nhật thành công!');
                         window.location.href='/ThongKe/Hoadonthangthanhtoan'
                         </script>
                      ");
        }
        [HttpPost]
        public FileResult Export()
        {
            ShopNuocHoaEntities entities = new ShopNuocHoaEntities();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Mã Đơn Hàng"),
                                        new DataColumn("Tên khách hàng"),
                                        new DataColumn("Ngày đặt hàng"),
                                        new DataColumn("Ngày giao hàng"),
                                        new DataColumn("Trị giá"),
                                        new DataColumn("Tình trạng giao hàng")
                                            });
                int i = int.Parse(Session["month"].ToString());
                int i1 = int.Parse(Session["year"].ToString());
                int tong = int.Parse(Session["tongtien"].ToString());
                var customers = from customer in entities.DonHangs.Where(n => n.NgayDat.Value.Month == i && n.NgayDat.Value.Year == i1 && n.TinhTrangGiaoHang == true)
                                select customer;
                Session["excel"] = customers;
                    foreach (var customer in customers)
                    {
                        if (customer.TinhTrangGiaoHang == true)
                        {
                            dt.Rows.Add(customer.MaDonHang, customer.TenNguoiNhan, customer.NgayGiao, customer.NgayGiao, customer.TriGia, customer.TinhTrangGiaoHang);
                        }
                        else
                        {
                            dt.Rows.Add(customer.MaDonHang, customer.TenNguoiNhan, customer.NgayGiao, customer.NgayGiao, customer.TriGia, "Đang giao");
                        }
                    }
            dt.Rows.Add("Tổng tiền:", tong);
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                        }
                    }
        }
    }
}   