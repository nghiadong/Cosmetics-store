using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
	public class DonHangChiTiet
	{
		[Key]
		public int ID { get; set; }
		public int DonHangID { get; set; }
		public int SanPhamID { get; set; }
		public int SoLuong { get; set; }
		public SanPham SanPham { get; set; }
		public DonHang DonHang { get; set; }
	}
}
