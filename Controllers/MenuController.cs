using System;
using System.Collections.Generic;
using System.Linq;
using ShopMyPham.Models;
using System.Web.Mvc;
using System.Web;

namespace ShopMyPham.Controllers
{
    public class MenuController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public ActionResult Menu()
        {
            return View(db.DanhMucs.OrderBy(n => n.STT).ToList());
        }
    }
}