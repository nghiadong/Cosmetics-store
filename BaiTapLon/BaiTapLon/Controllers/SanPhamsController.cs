using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Models;

namespace BaiTapLon.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly BanHangDbContext _context;

        public SanPhamsController(BanHangDbContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                var NguoiDungID = HttpContext.Session.GetInt32("ID");
                //Lay list gio hàng của người dùng đẩy về layout
                var gioHang = _context.GioHangs.Include(t => t.SanPham).Where(m => m.NguoiDung.ID == NguoiDungID).ToList();
                ViewData["GioHang"] = gioHang;
            }
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            var banHangDbContext = _context.SanPhams.Include(t => t.DanhMuc);
            
            return View(await banHangDbContext.ToListAsync());
        }

        public async Task<IActionResult> ViewByIDDanhMuc(int? id)
        {
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            var banHangDbContext = _context.SanPhams.Include(s => s.DanhMuc).Where(i=>i.DanhMuc.ID == id);
            return View("Index", await banHangDbContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Lay danh sach gio hang day ve layout
            var NguoiDungID = HttpContext.Session.GetInt32("ID");
            var gioHang = _context.GioHangs.Include(t => t.SanPham).Where(m => m.NguoiDung.ID == NguoiDungID).ToList();
            ViewData["GioHang"] = gioHang;


            //Lay ra cac danh gia cua san pham nay
            var DanhGia = _context.DanhGiaSanPhams.Include(m => m.NguoiDung).Where(m => m.SanPhamID == id).ToList();
            ViewData["DanhGia"] = DanhGia;

            var sanPham = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        //Tìm kiếm
        public async Task<IActionResult> Sreach(string key)
        {
            var sanpham = _context.SanPhams.Where(m=>m.TenSanPham.Contains(key)).ToList();
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            return View("Index",sanpham);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenSanPham,DanhMucID,Gia,MoTa")] SanPham sanPham, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
				_context.Add(sanPham);
				await _context.SaveChangesAsync();
                var sanPhamMoi = _context.SanPhams.OrderByDescending(m => m.ID).FirstOrDefault();

				sanPhamMoi.HinhAnh = Upload.UploadFile.UploadAnh(sanPhamMoi.ID, "SanPham", file);
                _context.SanPhams.Update(sanPhamMoi);
                _context.SaveChanges();
                return RedirectToAction("QuanLySanPham", "NguoiDung");
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.Include(m => m.DanhMuc).FirstOrDefaultAsync(m => m.ID == id);

			//lấy list danh mục
			var listDanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = listDanhMuc;
			return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, HinhAnh,TenSanPham,DanhMucID, MoTa,Gia")] SanPham sanPham, IFormFile? file)
        {
                if(file != null)
                {
                    sanPham.HinhAnh = Upload.UploadFile.UploadAnh(sanPham.ID, "SanPham", file);
                }    
                _context.Update(sanPham);   
                 await _context.SaveChangesAsync();
			return RedirectToAction("QuanLySanPham", "NguoiDung");
		}

        public async Task<IActionResult> Delete(int? id)
        {
            var gioHang = _context.GioHangs.Where(m => m.SanPham.ID == id);
            _context.GioHangs.RemoveRange(gioHang);

            var DonHang = _context.DonHangChiTiets.Where(m => m.SanPham.ID == id);
            _context.DonHangChiTiets.RemoveRange(DonHang);

            var danhGia = _context.DanhGiaSanPhams.Where(m => m.SanPham.ID == id);
            _context.DanhGiaSanPhams.RemoveRange(danhGia);

            var sanPham = _context.SanPhams.Find(id);
            _context.SanPhams.Remove(sanPham);
            _context.SaveChanges();

            string directoryPath1 = "./wwwroot/assets/img/SanPham/" + sanPham.ID;
            Directory.Delete(directoryPath1, true);
            return RedirectToAction("QuanLySanPham", "NguoiDung");
        }

        private bool SanPhamExists(int id)
        {
          return (_context.SanPhams?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
