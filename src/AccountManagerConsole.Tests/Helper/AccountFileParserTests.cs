using AccountManagerConsole.Helper;
using AccountManagerConsole.Models;
using AccountManagerConsole.Services;
using System.Text;

namespace AccountManagerConsole.Tests.Helper
{
    public class AccountFileParserTests
    {
        [Fact]
        public void Parse_Should_create_Account()
        {
            // Arrange
            var input = @"Compte au 28/02/2023 : 8300.00 EUR
EUR/JPY : 0.482
EUR/USD : 1.445
Date;Montant;Devise;Catégorie
06/10/2022;-504.61;EUR;Loisir
15/10/2022;-408.61;JPY;Transport";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var expectedAccount = new Account()
            {
                AsOf = new DateTime(2023, 2, 28),
                Balance = 8300,
                Currency = "EUR",
                Transactions = {
                    new Transaction(new DateTime(2022, 10, 06),-504.61, "EUR", "Loisir"),
                    new Transaction(new DateTime(2022, 10, 15),-408.61, "JPY", "Transport")
                }
            };
            Forex[] expectedForex =
            {
                new Forex(new DateTime(2023, 2, 28), "EUR", "JPY", 0.482),
                new Forex(new DateTime(2023, 2, 28), "EUR", "USD", 1.445)
            };

            // Act
            var actual = AccountFileParser.Parse(stream);

            // Assert
            actual.Should().BeEquivalentTo(expectedAccount);
            AssertForexExist(expectedForex);
        }

        [Fact]
        public void IsAccountInfo_Should_match_correct_row()
        {
            // Arrange
            var input = "Compte au 28/02/2023 : 8300.00 EUR";

            // Act
            var actual = AccountFileParser.IsAccountInfo(input);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void ProcessAccountInfo_Should_create_AccountInfo()
        {
            // Arrange
            var input = "Compte au 28/02/2023 : 123.45 EUR";
            var expected = new Account
            {
                AsOf = new DateTime(2023, 2, 28),
                Balance = 123.45,
                Currency = "EUR"
            };

            // Act
            var actual = AccountFileParser.ProcessAccountInfo(input);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("EUR")]
        [InlineData("EUR/JPY : 0.482")]
        [InlineData("USD")]
        [InlineData("JPY")]
        public void IsForex_Should_match_valid_currencies(string input)
        {
            // Arrange
            // Act
            var actual = AccountFileParser.IsForex(input);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsForex_Should_return_false_When_invalid_currency()
        {
            // Arrange
            // Act
            var actual = AccountFileParser.IsForex("bad_input");

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void ProcessForex_Should_create_forex()
        {
            // Arrange
            var input = "EUR/JPY : 0.482";
            var date = new DateTime(2023, 1, 1);
            var expected = new Forex(date, "EUR", "JPY", 0.482);

            // Act
            var actual = AccountFileParser.ProcessForex(input, date);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ProcessTransactions_Should_create_transactions()
        {
            // Arrange
            var headers = "Date;Montant;Devise;Catégorie";
            var input = @"06/10/2022;-504.61;EUR;Loisir
15/10/2022;-408.61;JPY;Transport";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            using var reader = new StreamReader(stream);
            Transaction[] expected =
            {
                new Transaction(new DateTime(2022, 10, 06),-504.61, "EUR", "Loisir"),
                new Transaction(new DateTime(2022, 10, 15),-408.61, "JPY", "Transport")
            };

            // Act
            var actual = AccountFileParser.ProcessTransactions(reader, headers);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        private static void AssertForexExist(Forex[] expectedForex)
        {
            foreach (Forex expected in expectedForex)
            {
                var actual = ForexService.Singleton().Get(expected.CcyFrom, expected.CcyTo);
                actual.Should().Be(expected.Value);
            }
        }

    }
}
