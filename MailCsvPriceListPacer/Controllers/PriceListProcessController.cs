using MailCsvPriceListParcer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailCsvPriceListParcer.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class PriceListProcessController : ControllerBase
	{
		private readonly ILogger<PriceListProcessController> _logger;
		private readonly IMailProcessService _mailProcessService;
		private readonly ICSVParcerService _csvParcerService;
		private readonly IDatabaseManagerService _databaseManagerService;

		public PriceListProcessController(ILogger<PriceListProcessController> logger, IMailProcessService mailProcessService, ICSVParcerService cSVParcerService,
			IDatabaseManagerService databaseManagerService)
		{
			_logger = logger;
			_mailProcessService = mailProcessService;
			_csvParcerService = cSVParcerService;
			_databaseManagerService = databaseManagerService;
		}

		[HttpGet]
		public async Task<string> DownloadAndSavePriceList()
		{
			try
			{
				string result = string.Empty;
				var csvFiles = await _mailProcessService.DownloadPriceList();
				foreach (var csvFile in csvFiles)
				{
					var priceItems = _csvParcerService.ReadPriceList(csvFile);

					int addedCount = await _databaseManagerService.SavePriceListToDB(priceItems);

					result += $"Файл: {csvFile}, успешно добавлено записей в БД: {addedCount}" + Environment.NewLine;
				}
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Произошла ошибка при обработке запроса!");
				return $"Произошла ошибка при обработке запроса!{Environment.NewLine}{ex.Message}{ex.StackTrace}";
			}
		}
	}
}
