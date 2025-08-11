using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;

namespace MailCsvPriceListParcer.Helpers
{
	public class CustomIntConverter : DefaultTypeConverter
	{
		public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Contains('-'))
					result = text.Split('-').LastOrDefault();
				else
					result = Regex.Replace(text, "[^0-9]", "");
			}
			return !string.IsNullOrEmpty(result) ? int.Parse(result) : 0;
		}
	}
}
