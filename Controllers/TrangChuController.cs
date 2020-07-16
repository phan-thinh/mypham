using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMyPham.Models;

namespace ShopMyPham.Controllers 
{
    public class TrangChuController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public ActionResult ViewTrangChu()
        {
            Session["TongTien1"] = null;
            Session["TongTien"] = null;
            return View(db.NuocHoas.OrderByDescending(n => n.MaNuocHoa).ToList());
        }
        public ActionResult ViewPage1()
        {
            return View();
        }
        public ActionResult TimKiem(string name)
        {
            List<NuocHoa> sp = db.NuocHoas.Where(n => n.TenNuocHoa.Contains(name)).ToList();
            Session["nametimkiem"] = name;
            return View(sp);
        }
    }
}