using CsvHelper;
using CsvHelper.Configuration;
using MailCsvPriceListParcer.Interfaces;
using MailCsvPriceListParcer.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MailCsvPriceListParcer.Services
{
	public class CSVParcerService : ICSVParcerService
	{
		public List<PriceItem> ReadPriceList(string filePath)
		{
			var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				BadDataFound = null,
				MissingFieldFound = null,
				Encoding = System.Text.Encoding.UTF8
			};

			// Вынужденная корректировка файла из-за невозможности повлиять на конкретное поле строки, вызывающей ошибку маппинга.
			string cleanFile = File.ReadAllText(filePath).Replace("\"\"\"\"", "\"\"\"");
			File.WriteAllText(filePath, cleanFile);

			using var reader = new StreamReader(filePath);
			using var csv = new CsvReader(reader, csvConfig);

			csv.Context.RegisterClassMap<PriceItemCSV>();
			var priceItemRecords = csv.GetRecords<PriceItem>().ToList();
			foreach (var priceItemRecord in priceItemRecords)
			{
				priceItemRecord.SearchVendor = Regex.Replace(priceItemRecord.Vendor, "[^A-Za-zА-Яа-я0-9]", "").ToUpper();
				priceItemRecord.SearchNumber = Regex.Replace(priceItemRecord.Number, "[^A-Za-zА-Яа-я0-9]", "").ToUpper();
			}
			return priceItemRecords;
		}
	}
}
