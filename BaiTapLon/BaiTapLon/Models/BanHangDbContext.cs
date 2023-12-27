using Microsoft.EntityFrameworkCore;

namespace BaiTapLon.Models
{
	public class BanHangDbContext: DbContext
	{ 
		public BanHangDbContext(DbContextOptions<BanHangDbContext> options) : base(options) { }
		public DbSet<DanhMuc> DanhMucs { get; set; }
		public DbSet<DonHang> DonHangs { get; set; }
		public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }
		public DbSet<NguoiDung> NguoiDungs { get; set; }
		public DbSet<SanPham> SanPhams { get; set; }
		public DbSet<GioHang> GioHangs { get; set; }
		public DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }
		public DbSet<TinTuc> TinTucs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DanhMuc>().ToTable("DanhMuc");
			modelBuilder.Entity<DonHang>().ToTable("DonHang");
			modelBuilder.Entity<DonHangChiTiet>().ToTable("DonHangChiTiet");
			modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
			modelBuilder.Entity<SanPham>().ToTable("SanPham");
            modelBuilder.Entity<GioHang>().ToTable("GioHang");
			modelBuilder.Entity<DanhGiaSanPham>().ToTable("DanhGiaSanPham");
			modelBuilder.Entity<TinTuc>().ToTable("TinTuc");
		}
	}
}
