using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
    public class GioHang
    {
        [Key]
        public int ID { get; set; }
        public int SoLuong { get; set; }
        public int SanPhamID { get; set; }
        public int NguoiDungID { get; set; }
        public SanPham? SanPham { get; set; }
        public NguoiDung? NguoiDung { get; set; }
    }
}
