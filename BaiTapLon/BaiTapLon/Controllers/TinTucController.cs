using BaiTapLon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Controllers
{
	public class TinTucController : Controller
	{
		private readonly BanHangDbContext _context;

		public TinTucController(BanHangDbContext context)
		{
			_context = context;
		}
		// GET: TinTucController
		public ActionResult Index()
		{
			if (HttpContext.Session.GetInt32("ID") != null)
			{
				var NguoiDungID = HttpContext.Session.GetInt32("ID");
				//Lay list gio hàng của người dùng đẩy về layout
				var gioHang = _context.GioHangs.Include(t => t.SanPham).Where(m => m.NguoiDung.ID == NguoiDungID).ToList();
				ViewData["GioHang"] = gioHang;
			}

			var tinTuc = _context.TinTucs.ToList();
			return View(tinTuc);
		}

		// GET: TinTucController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}


		// POST: TinTucController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind("TenTinTuc,NoiDung")] TinTuc tinTuc, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				tinTuc.NgayDang = DateTime.Now;
				_context.Add(tinTuc);
				 _context.SaveChanges();
				var tinTucMoi = _context.TinTucs.OrderByDescending(m => m.ID).FirstOrDefault();

				tinTucMoi.HinhAnh = Upload.UploadFile.UploadAnh(tinTucMoi.ID, "TinTuc", file);
				_context.TinTucs.Update(tinTucMoi);
				_context.SaveChanges();
				return RedirectToAction("QuanLyTinTuc", "NguoiDung");
			}
			return View();
		}


		// GET: TinTucController/Delete/5
		public ActionResult Delete(int id)
		{
			var tinTuc = _context.TinTucs.Find(id);
			_context.TinTucs.Remove(tinTuc);
			_context.SaveChanges();
			return RedirectToAction("QuanLyTinTuc", "NguoiDung");
		}

		// POST: TinTucController/Delete/5
	}
}
