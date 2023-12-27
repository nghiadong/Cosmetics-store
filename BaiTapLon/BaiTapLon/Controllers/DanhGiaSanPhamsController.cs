using BaiTapLon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapLon.Controllers
{
    public class DanhGiaSanPhamsController : Controller
    {
        private readonly BanHangDbContext _context;
        public DanhGiaSanPhamsController(BanHangDbContext context)
        {
            _context = context;
        }
        // GET: DanhGiaSanPhams

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("SanPhamID", "NoiDung")] DanhGiaSanPham danhGiaSanPham)
        {
            if (HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Details", "SanPhams", new { id = danhGiaSanPham.SanPhamID });
            }
            else
            {
                var NguoiDungID = HttpContext.Session.GetInt32("ID");
                danhGiaSanPham.NgayDanhGia = DateTime.Now;
                danhGiaSanPham.NguoiDungID = (int)NguoiDungID;
                _context.DanhGiaSanPhams.Add(danhGiaSanPham);
                _context.SaveChanges();
                return RedirectToAction("Details", "SanPhams", new { id = danhGiaSanPham.SanPhamID });
            }
            return View();
            
        }

      
    }
}
