namespace MailCsvPriceListParcer.Models
{
	public class PriceItem
	{
		public int ItemId { get; set; }

		public required string Vendor {  get; set; }

		public required string Number { get; set; }

		public required string SearchVendor { get; set; }

		public required string SearchNumber { get; set; }

		public required string Description { get; set; }

		public decimal Price { get; set; }

		public int Count { get; set; }

		public string Supplier { get; set; } = "ООО \"Доставим в срок\"";
	}
}