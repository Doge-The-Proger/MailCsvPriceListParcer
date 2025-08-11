using MailCsvPriceListParcer.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;

namespace MailCsvPriceListParcer.Services
{
	public class MailProcessService : IMailProcessService
	{
		private readonly IConfiguration _config;

		public MailProcessService(IConfiguration config)
		{
			_config = config;
		}

		public async Task<List<string>> DownloadPriceList()
		{
			// Создаем директорию для вложений, если ее не было, и удаляем из нее старые файлы, если они есть.
			string? attachmentDirPath = _config["EmailSettings:AttachmentDir"];
			DirectoryInfo di = new DirectoryInfo(attachmentDirPath);
			if (!di.Exists)
				di.Create();
			foreach (FileInfo file in di.GetFiles())
				file.Delete();

			var downloadedFiles = new List<string>();

			// Подключаемся к почте и открываем папку входящих
			using var client = new ImapClient();
			await client.ConnectAsync(_config["EmailSettings:ImapHost"], int.Parse(_config["EmailSettings:ImapPort"]), SecureSocketOptions.SslOnConnect);
			await client.AuthenticateAsync(_config["EmailSettings:EmailUser"], _config["EmailSettings:EmailPassword"]);
			await client.Inbox.OpenAsync(FolderAccess.ReadWrite);

			// Ищем непрочитанные письма от поставщика
			var query = SearchQuery.And(
				SearchQuery.FromContains(_config["EmailSettings:SupplierEmail"]),
				SearchQuery.NotSeen
			);

			// Разбираем каждое найденное сообщение, достаем csv-вложение, загружаем его в папку и сохраняем путь к нему
			var uids = await client.Inbox.SearchAsync(query);
			foreach (var uid in uids)
			{
				var message = await client.Inbox.GetMessageAsync(uid);

				foreach (var attachment in message.Attachments)
				{
					if (attachment is MimePart part && (part.ContentType.MimeType == "text/csv" || part.FileName.EndsWith(".csv")))
					{
						var filePath = Path.Combine(attachmentDirPath, part.FileName);
						using var stream = File.Create(filePath);
						await part.Content.DecodeToAsync(stream);
						downloadedFiles.Add(filePath);
					}
				}
				// Помечаем письмо как прочитанное
				await client.Inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
			}

			await client.DisconnectAsync(true);
			return downloadedFiles;
		}
	}
}