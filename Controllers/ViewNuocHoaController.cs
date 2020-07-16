using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMyPham.Models;

namespace ShopMyPham.Controllers
{
    public class ViewNuocHoaController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public ActionResult NuocHoaTheoLoai(int? MaLoaiNuocHoa)
        {
            return View(db.NuocHoas.Where(n => n.MaLoaiNuocHoa == MaLoaiNuocHoa).ToList());
        }
        public ActionResult NuocHoaTheoGia(int? Gia)
        {
            if(Gia==1)
            {
                return View(db.NuocHoas.Where(n => n.GiaBan<1000000).OrderBy(n => n.GiaBan).ToList());
            }
            else if(Gia==2)
            {
                return View(db.NuocHoas.Where(n => n.GiaBan<3000000 && n.GiaBan >= 1000000).OrderBy(n => n.GiaBan).ToList());
            }
            else if (Gia == 3)
            {
                return View(db.NuocHoas.Where(n => n.GiaBan < 4000000 && n.GiaBan >= 3000000).OrderBy(n => n.GiaBan).ToList());
            }
            else if (Gia == 4)
            {
                return View(db.NuocHoas.Where(n => n.GiaBan < 5000000 && n.GiaBan >= 4000000).OrderBy(n => n.GiaBan).ToList());
            }
            else if (Gia == 5)
            {
                return View(db.NuocHoas.Where(n => n.GiaBan < 6000000 && n.GiaBan >= 5000000).OrderBy(n => n.GiaBan).ToList());
            }
            else
            {
                return View(db.NuocHoas.Where(n => n.GiaBan >= 6000000 ).OrderBy(n => n.GiaBan).ToList());
            }

        }
        public ActionResult ChiTietNuocHoa(int? MaNuocHoa)
        {
            if (MaNuocHoa == null)
            {
                MaNuocHoa = int.Parse(TempData["MaNuocHoa"].ToString());
                List<DanhGia> DG = db.DanhGias.Where(n => n.MaNuocHoa == MaNuocHoa).OrderByDescending(n => n.MaDanhGia).ToList();
                ViewData["listDG"] = DG;
                List<DanhGia> LDG = db.DanhGias.Where(n => n.MaNuocHoa == MaNuocHoa).ToList();
                double DGTB = 0;
                int DiemDG = 0;
                for (int i = 0; i < LDG.Count(); i++)
                {
                    DiemDG += int.Parse(LDG[i].DiemDG.ToString());
                }
                DGTB = DiemDG / LDG.Count();

                return View(db.NuocHoas.SingleOrDefault(n => n.MaNuocHoa == MaNuocHoa));
            }
            else
            {
                List<DanhGia> DG = db.DanhGias.Where(n => n.MaNuocHoa == MaNuocHoa).OrderByDescending(n => n.MaDanhGia).ToList();
                ViewData["listDG"] = DG;
                return View(db.NuocHoas.SingleOrDefault(n => n.MaNuocHoa == MaNuocHoa));
            }
        }
        //public ActionResult Rating()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult ChiTietNuocHoa(int iMaNuocHoa, string iName, string iComment, string Ngay, int iRating, DanhGia DG)
        {
            DG.MaNuocHoa = iMaNuocHoa;
            DG.TenKhachHang = iName;
            DG.cmtDanhGia = iComment;
            DG.DiemDG = iRating;
            DG.NgayCmt = DateTime.Parse(Ngay);
            TempData["MaNuocHoa"] = iMaNuocHoa;
            db.DanhGias.Add(DG);
            db.SaveChanges();
            List<DanhGia> LDG = db.DanhGias.Where(n => n.MaNuocHoa == iMaNuocHoa).ToList();
            double DGTB = 0;
            int DiemDG = 0;
            for (int i = 0; i < LDG.Count(); i++)
            {
                DiemDG += int.Parse(DG.DiemDG.ToString());
            }
            DGTB = DiemDG / LDG.Count();
            return RedirectToAction("ChiTietNuocHoa", "ViewNuocHoa");
        }
    }
}