using AccountManagerConsole.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccountManagerConsole.Helper
{
    public static class AccountFileParser
    {
        private static readonly string[] currencies = { "EUR", "USD", "JPY" };

        internal static void Parse(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            using var fileStream = fileInfo.OpenRead();
            using var reader = new StreamReader(fileStream);
            do {
                var currentLine = reader.ReadLine();
                if (currentLine.StartsWith("Compte au"))
                    ProcessAccountInfo(currentLine);
                else if (IsForex(currentLine))
                    ProcessForex(currentLine, DateTime.Today);
                else if (currentLine.StartsWith("Date;"))
                    ProcessTransactions(reader);
            } while (!reader.EndOfStream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
            };
            var csv = new CsvReader(reader, config);

            // Skip until transactions headers are found
            //while (csv.ReadHeader())
            //{
            //    if (csv.Context.Record[0].StartsWith("Date;"))
            //    {
            //        csv.Read();
            //        csv.ReadHeader();
            //        break;
            //    }
            //}

            var result = csv.GetRecords<Transaction>();
            Console.WriteLine($"transactions: {result.Count()}");
        }

        private static void ProcessAccountInfo(string currentLine)
        {
            throw new NotImplementedException();
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
                throw new ArgumentException($"Invalid forex row: {input}");

            return new Forex()
            {
                Date = date ?? DateTime.Today,
                CcyFrom = match.Groups["ccyFrom"].Value,
                CcyTo = match.Groups["ccyTo"].Value,
                Value = double.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture)
            };
        }

        private static void ProcessTransactions(StreamReader reader)
        {
            throw new NotImplementedException();
        }

    }
}
