using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace MailCsvPriceListParcer.Helpers
{
	public class CustomDecimalConverter : DefaultTypeConverter
	{
		public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
		{
			if (decimal.TryParse(text, out decimal result))
				return result;
			else return -1;
		}
	}
}
