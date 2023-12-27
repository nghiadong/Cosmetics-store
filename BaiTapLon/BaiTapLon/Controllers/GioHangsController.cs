using BaiTapLon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace BaiTapLon.Controllers
{
    public class GioHangsController : Controller
    {
        private readonly BanHangDbContext _context;
        public GioHangsController(BanHangDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("ID") != null)
            {
                var NguoiDungID = HttpContext.Session.GetInt32("ID");
                //Lay list gio hàng của người dùng đẩy về layout
                var gioHang = _context.GioHangs.Include(t => t.SanPham).Where(m => m.NguoiDung.ID == NguoiDungID).ToList();
                ViewData["GioHang"] = gioHang;

             
                //tinh tong tat cac cac hang trong gio hang cua nguoi dung va gan vao viewbag
                float TongTien = 0;
                foreach(var i in gioHang)
                {
                    TongTien += (float)i.SanPham.Gia * i.SoLuong;
                }

                ViewBag.TongTien = TongTien;
                return View(gioHang);
            }

            return View();
        }

        // Thêm giỏ hàng
        public IActionResult Create(int id, int SoLuong, string type = "Normal")
        {
           
            if (HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("DangNhap", "NguoiDungs");
            }
            else
            {
                var NguoiDungID = HttpContext.Session.GetInt32("ID");
                var item = _context.GioHangs.FirstOrDefault(t => t.SanPham.ID == id && t.NguoiDung.ID == NguoiDungID);

                if (item == null)
                {
                    GioHang gioHang = new GioHang();
                    gioHang.SanPhamID = id;
                    gioHang.SoLuong = SoLuong;
                    gioHang.NguoiDungID = (int)NguoiDungID;

                    _context.GioHangs.Add(gioHang);
                    _context.SaveChanges();
                }
                else
                {
                    item.SoLuong += 1;
                    _context.GioHangs.Update(item);
                    _context.SaveChanges();
                }

                if (type == "ajax")
                {
                    return Json(new
                    {
                        SoLuong = _context.GioHangs.Sum(m => m.SoLuong)
                    });
                }
                return RedirectToAction("Index", "SanPhams");
            }
        }
        //Thanh toán
        public IActionResult ThanhToan(int id)
        {
            float TongTien = 0;
            //Lay list gio hang đẩy về layout
            var NguoiDungID = HttpContext.Session.GetInt32("ID");
            var gioHang = _context.GioHangs.Include(t => t.SanPham).Where(m => m.NguoiDung.ID == NguoiDungID).ToList();
            ViewData["GioHang"] = gioHang;
            if (id == 0)
            {
                //Mua het
                //Lay list gio hàng của người dùng đẩy về layout
                var listgioHang = _context.GioHangs.Include(t => t.SanPham).ToList();
                
                foreach (var item in listgioHang)
                {
                    TongTien += (float)item.SanPham.Gia * item.SoLuong;
                }
                ViewBag.Tong = TongTien;
                return View(gioHang);
            }
            else
            {
                //Mua 1 cai
                var hang = _context.GioHangs.Include(m => m.SanPham).Where(m => m.ID == id).ToList();
                foreach (var item in hang)
                {
                    TongTien += (float)item.SanPham.Gia * item.SoLuong;
                    ViewBag.SanPhamMua = item.ID;
                }
                ViewBag.Tong = TongTien;
                //Mua rieng le thi `
                return View(hang);
            }    
            
        }
        //Thanh toán xong thêm vào sql
        [HttpPost]
        public IActionResult ThanhToan(int SanPhamMua, [Bind("NguoiDungID, DiaChi, TongTien")] DonHang donHang)
        {
            donHang.NgayDatHang = DateTime.Now;
            donHang.TrangThai = 0;
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();
            var donHangMoi = _context.DonHangs.OrderByDescending(m => m.ID).FirstOrDefault();
            if (SanPhamMua == 0)
            {
                //Mua het
                //list hang
                var listHangMua = _context.GioHangs.Where(m => m.NguoiDung.ID == donHang.NguoiDungID);

                    foreach (var item in listHangMua)
                    {
                        DonHangChiTiet donHangChiTiet = new DonHangChiTiet();
                        donHangChiTiet.DonHangID = donHangMoi.ID;
                        donHangChiTiet.SanPhamID = item.SanPhamID;
                        donHangChiTiet.SoLuong = item.SoLuong;
                        _context.DonHangChiTiets.Add(donHangChiTiet);
                        
                    }

                //Sau khi dat hang thi xoa toan bo trong gio hang
                    _context.GioHangs.RemoveRange(listHangMua);
                _context.SaveChanges();

            }
            else
            {
                //Mua rieng le
                var hangMua = _context.GioHangs.FirstOrDefault(m => m.ID == SanPhamMua);
                DonHangChiTiet donHangChiTiet = new DonHangChiTiet();
                donHangChiTiet.DonHangID = donHangMoi.ID;
                donHangChiTiet.SanPhamID = hangMua.SanPhamID;
                donHangChiTiet.SoLuong = hangMua.SoLuong;
                _context.DonHangChiTiets.Add(donHangChiTiet);
                _context.GioHangs.Remove(hangMua);
                _context.SaveChanges();
            }    

            return RedirectToAction("Index", "SanPhams");
        }

        public IActionResult Delete(int id)
        {
            var sanPham = _context.GioHangs.Find(id);
            _context.Remove(sanPham);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
