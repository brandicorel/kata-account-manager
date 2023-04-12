using AccountManagerConsole.Helper;
using AccountManagerConsole.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccountManagerConsole.Tests.Helper
{
    public class AccountFileParserTests
    {
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
            var expected = new Forex
            {
                Date = date,
                CcyFrom = "EUR",
                CcyTo = "JPY",
                Value = 0.482
            };

            // Act
            var actual = AccountFileParser.ProcessForex(input, date);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        private static void ProcessAccountInfo(string input)
        {
            throw new NotImplementedException();
        }

        private static void ProcessTransactions(StreamReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
