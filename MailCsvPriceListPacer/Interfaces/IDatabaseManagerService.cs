using MailCsvPriceListParcer.Models;

namespace MailCsvPriceListParcer.Interfaces
{
	public interface IDatabaseManagerService
	{
		/// <summary>
		/// Соохраняет полученный список сущностей в БД.
		/// </summary>
		/// <param name="priceItems">Список разобранных строк прайс-листа.</param>
		/// <returns>Кол-во успешно сохраненных записей.</returns>
		public Task<int> SavePriceListToDB(List<PriceItem> priceItems);
	}
}
