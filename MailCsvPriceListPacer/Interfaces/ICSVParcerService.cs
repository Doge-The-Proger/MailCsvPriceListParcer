using MailCsvPriceListParcer.Models;

namespace MailCsvPriceListParcer.Interfaces
{
	public interface ICSVParcerService
	{
		/// <summary>
		/// Разбирает загруженный прайс-лист в соответствии с созданным профилем маппинга (PriceItemCSV).
		/// </summary>
		/// <param name="filePath">Путь до .csv файла.</param>
		/// <returns>Список смапленных экземпляров строк прайс-листа.</returns>
		public List<PriceItem> ReadPriceList(string filePath);
	}
}
