using MailCsvPriceListParcer.Interfaces;
using MailCsvPriceListParcer.Models;
using Microsoft.EntityFrameworkCore;

namespace MailCsvPriceListParcer.Services
{
	public class DatabaseManagerService : IDatabaseManagerService
	{
		private readonly AppDbContext _dbContext;
		private readonly ILogger<DatabaseManagerService> _logger;

		public DatabaseManagerService(AppDbContext dbContext, ILogger<DatabaseManagerService> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task<int> SavePriceListToDB(List<PriceItem> priceItems)
		{
			using var transaction = await _dbContext.Database.BeginTransactionAsync();
			try
			{
				_logger.LogInformation("Начало загрузки данных в БД. Количество записей: {Count}", priceItems.Count);

				// Удаление старых данных поставщика
				var oldItems = await _dbContext.PriceItems.Where(p => p.Supplier == "ООО \"Доставим в срок\"").ToListAsync();
				_dbContext.PriceItems.RemoveRange(oldItems);
				await _dbContext.SaveChangesAsync();
				_logger.LogInformation("Удалено старых записей: {Count}", oldItems.Count);

				// Добавление новых данных
				await _dbContext.PriceItems.AddRangeAsync(priceItems);
				await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				_logger.LogInformation("Успешно загружено новых записей: {Count}", priceItems.Count);
				return priceItems.Count;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при загрузке данных в БД");
				await transaction.RollbackAsync();
				throw;
			}
		}
	}
}
