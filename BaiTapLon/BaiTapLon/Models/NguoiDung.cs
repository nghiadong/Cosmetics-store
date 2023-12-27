using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
	public class NguoiDung
	{
		[Key]
		public int ID { get; set; }
		public string HoTen { get; set; }
		public string DiaChi { get; set; }
		public string SDT { get; set; }
		public string Email { get; set; }
		public string TaiKhoan { get; set; }
		public string MatKhau { get; set; }
		public int Quyen { get; set; }
	}
}