using CsvHelper.Configuration;
using MailCsvPriceListParcer.Helpers;

namespace MailCsvPriceListParcer.Models
{
	public class PriceItemCSV : ClassMap<PriceItem>
	{
		public PriceItemCSV() 
		{
			Map(m => m.Vendor).Name("Бренд");
			Map(m => m.Number).Name("Каталожный номер");
			Map(m => m.Description).Name("Описание");
			Map(m => m.Price).Name("Цена, руб.").TypeConverter<CustomDecimalConverter>();
			Map(m => m.Count).Name("Наличие").TypeConverter<CustomIntConverter>();

			Map(m => m.ItemId).Ignore();
			Map(m => m.SearchVendor).Ignore();
			Map(m => m.SearchNumber).Ignore();
			Map(m => m.Supplier).Ignore();
		}
	}
}
