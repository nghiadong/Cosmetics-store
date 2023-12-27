using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
	public class DanhMuc
	{
		[Key]
		public int ID { get; set; }
		public string TenDanhMuc { get; set; }
		public string MoTa { get; set; }
	}
}
