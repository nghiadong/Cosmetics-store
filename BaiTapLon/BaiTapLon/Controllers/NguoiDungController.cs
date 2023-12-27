using BaiTapLon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace BaiTapLon.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly BanHangDbContext _context;

        public NguoiDungController(BanHangDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult DangNhap(IFormCollection Connection)
        {
            string TaiKhoan = Connection["TaiKhoan"].ToString();
            string MatKhau = Connection["MatKhau"].ToString();
            var login = _context.NguoiDungs.FirstOrDefault(t => t.TaiKhoan == TaiKhoan);
            if (login == null)
            {
                return View();
            }
            else
            {
                SHA256 hashMethod = SHA256.Create();
                if (Md5.Mahoa.VerifyHash(hashMethod, MatKhau, login.MatKhau))
                    {
                    HttpContext.Session.SetInt32("ID",login.ID);
                    HttpContext.Session.SetString("HoTen",login.HoTen);
                    HttpContext.Session.SetInt32("Quyen", login.Quyen);
                if (login.Quyen == 0)
                    {
                        return RedirectToAction("Index", "SanPhams");
                    }
                    else 
                    {
                        return RedirectToAction("QuanLySanPham");
                    }
                }


            }
            return View();

        }

        [HttpPost]
        public ActionResult DangKy([Bind("HoTen","DiaChi","SDT","Email","TaiKhoan","MatKhau")] NguoiDung Account)
        {
            SHA256 hashMethod = SHA256.Create();
            Account.MatKhau = Md5.Mahoa.GetHash(hashMethod, Account.MatKhau);
            _context.NguoiDungs.Add(Account);
            _context.SaveChanges();
            return RedirectToAction("Index", "SanPhams");
        }

        public ActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "SanPhams");
        }

        public IActionResult QuanLySanPham()
        {
            //lấy list danh mục
            var listDanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = listDanhMuc;

            var listSanPham = _context.SanPhams.Include(m => m.DanhMuc).ToList();
            return View(listSanPham);
        }

		public IActionResult QuanLyDonHang()
		{
			var listDonHang = _context.DonHangChiTiets.Include(m => m.DonHang).Include(m => m.SanPham).ToList();
			return View(listDonHang);
		}

        public IActionResult QuanLyTinTuc()
        {
            var listTinTuc = _context.TinTucs.ToList();
			return View(listTinTuc);
		}

	}

}
