using MailCsvPriceListParcer.Models;
using Microsoft.EntityFrameworkCore;

namespace MailCsvPriceListParcer
{
	public class AppDbContext : DbContext
	{
		public DbSet<PriceItem> PriceItems { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PriceItem>(item => 
			{
				item.HasKey(i => i.ItemId);
				item.Property(i => i.Vendor).IsRequired().HasMaxLength(64);
				item.Property(i => i.Number).IsRequired().HasMaxLength(64);
				item.Property(i => i.SearchVendor).IsRequired().HasMaxLength(64);
				item.Property(i => i.SearchNumber).IsRequired().HasMaxLength(64);
				item.Property(i => i.Description).IsRequired().HasMaxLength(512);
				item.Property(i => i.Price).IsRequired().HasPrecision(18, 2);
				item.Property(i => i.Count).IsRequired();
				item.Property(e => e.Supplier).HasDefaultValue("ООО \"Доставим в срок\"");
			});
		}
	}
}
