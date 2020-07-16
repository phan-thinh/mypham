using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMyPham.Models;

namespace ShopMyPham.Controllers
{
    public class PartialViewController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        // GET: PartialView
        public ActionResult TopicPartial()
        {
            return PartialView(db.LoaiNuocHoas.OrderBy(n => n.STT).ToList());
        }
        public ActionResult PublishPartialRespon()
        {
            return PartialView(db.NhaSanXuats.ToList());
        }
        public ActionResult LocTheoGia()
        {
            var sampleList = new List<Tuple<string,string>>()
        {
            new Tuple<string, string>("1","Dưới 1 triệu"),
            new Tuple<string, string>("2","Từ 1 - 3 triệu"),
            new Tuple<string, string>("3","Từ 3 - 4 triệu"),
            new Tuple<string, string>("4","Từ 4 - 5 triệu"),
            new Tuple<string, string>("5","Từ 5 - 6 triệu"),
            new Tuple<string, string>("6"," Trên 6 triệu"),
        };
        return View(sampleList);
        }
        public ActionResult SanPhamMoi()
        {
            return PartialView(db.NuocHoas.OrderByDescending(n=>n.MaNuocHoa).Take(6).ToList());
        }
        public ActionResult SanPhamBanChay()
        {
            return PartialView(db.ChiTietDonHangs.OrderByDescending(n => n.MaNuocHoa).Take(6).ToList().Count());
        }
    }
}