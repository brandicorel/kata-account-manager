using AccountManagerConsole.Models;
using AccountManagerConsole.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AccountManagerConsole.Helper
{
    public static class AccountFileParser
    {
        private const string TransactionHeaders = "Date;Montant;Devise;Catégorie";
        private static readonly string[] currencies = { "EUR", "USD", "JPY" };

        internal static Account Parse(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            using var fileStream = fileInfo.OpenRead();
            return Parse(fileStream);
        }

        public static Account Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);

            var currentLine = reader.ReadLine();
            var account = ProcessAccountInfo(currentLine);

            currentLine = reader.ReadLine();
            var isForex = IsForex(currentLine);
            while (isForex && !reader.EndOfStream)
            {
                var forex = ProcessForex(currentLine, account.AsOf);
                ForexService.Singleton().Add(forex);
                currentLine = reader.ReadLine();
                isForex = IsForex(currentLine);
            }

            account.Transactions = ProcessTransactions(reader, currentLine).ToList();

            return account;
        }

        public static bool IsAccountInfo(string? currentLine)
        {
            return currentLine.StartsWith("Compte au");
        }

        public static Account ProcessAccountInfo(string input)
        {
            string pattern = @"\bCompte au (?<date>\d{2}/\d{2}/\d{4}) : (?<balance>(\d*\.?(\d*)?){1}) (?<ccy>\w+)";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(input);

            if (!match.Success)
                throw new ArgumentException($"Invalid account info row: '{input}'");

            return new Account()
            {
                AsOf = DateTime.ParseExact(match.Groups["date"].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Balance = double.Parse(match.Groups["balance"].Value, CultureInfo.InvariantCulture),
                Currency = match.Groups["ccy"].Value
            };
        }

        public static bool IsForex(string currentLine)
        {
            return currencies.Contains(currentLine[..3]);
        }

        public static Forex ProcessForex(string input, DateTime? date)
        {
            string pattern = @"\b(?<ccyFrom>\w+)/(?<ccyTo>\w+) : (?<value>(\d*\.?(\d*)?){1})";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(input);

            if (!match.Success)
                throw new ArgumentException($"Invalid forex row: '{input}'");

            return new Forex()
            {
                Date = date ?? DateTime.Today,
                CcyFrom = match.Groups["ccyFrom"].Value,
                CcyTo = match.Groups["ccyTo"].Value,
                Value = double.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture)
            };
        }

        public static IEnumerable<Transaction> ProcessTransactions(StreamReader reader, string headers)
        {
            if (headers != TransactionHeaders)
                throw new ArgumentException($"Invalid headers: '{headers}', expecting '{TransactionHeaders}'");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
            };
            var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<TransactionMap>();
            var result = csv.GetRecords<Transaction>();
            return result;
        }
    }

    internal class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.Date).TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Amount);
            Map(m => m.Currency);
            Map(m => m.Category);
        }
    }
}
