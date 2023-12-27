using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
	public class DonHang
	{
		[Key]
		public int ID { get; set; }
		public int NguoiDungID { get; set; }
		public DateTime NgayDatHang { get; set; }
		public string DiaChi { get; set; }
		public int TrangThai { get; set; }
		public double TongTien { get; set; }
		public NguoiDung NguoiDung { get; set; }
	}
}
