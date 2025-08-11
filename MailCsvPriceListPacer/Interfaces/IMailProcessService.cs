namespace MailCsvPriceListParcer.Interfaces
{
	public interface IMailProcessService
	{
		/// <summary>
		/// Разбирает папку Входящие и вытаскивает все .csv файлы из всех сообщений конкретного поставщика.
		/// </summary>
		/// <returns>Список загруженных .csv файлов.</returns>
		public Task<List<string>> DownloadPriceList();
	}
}
