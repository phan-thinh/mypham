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
    public class TinTucController : Controller
    {
        ShopNuocHoaEntities db = new ShopNuocHoaEntities();
        public ActionResult TinTuc(int? page)
        {
            if (page == null)
                page = 1;
            return View(db.BaiViets.Where(n => n.DanhMuc == 3).ToList().ToPagedList(page ?? 1, 8));
        }

        //nội dung tin tức
        public ActionResult NoiDungTinTuc(int? IDDanhMuc)
        {
            return View(db.BaiViets.Where(n => n.ID == IDDanhMuc).ToList());
        }
        //giới thiệu
        public ActionResult GioiThieu()
        {
            return View(db.BaiViets.Where(n => n.DanhMuc == 2).ToList());
        }

        //liên hệ
        public ActionResult LienHe()
        {
            return View(db.BaiViets.Where(n => n.DanhMuc == 4).ToList());
        }
    }
}