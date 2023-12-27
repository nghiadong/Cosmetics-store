using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
    public class DanhGiaSanPham
    {
        [Key]
        public int ID { get; set; }
        public int NguoiDungID { get; set; }
        public int SanPhamID { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public SanPham? SanPham { get; set; }
        public NguoiDung? NguoiDung { get; set; }
    }
}
