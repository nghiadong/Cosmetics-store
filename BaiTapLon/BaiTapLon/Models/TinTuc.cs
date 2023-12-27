using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
	public class TinTuc
	{
		[Key]
		public int ID { get; set; }
		public string TenTinTuc { get; set; }
		public DateTime? NgayDang { get; set; }
		public string? NoiDung { get; set; }
		public string? HinhAnh { get; set; }
	}
}
