using AccountManagerConsole.Models;

namespace AccountManagerConsole.Services
{
    // TODO:
    // - precompute amount with forex
    // - date range based on transactions
    internal class AccountService
    {
        private Account? account;
        private bool hasloaded = false;

        private readonly IAccountDataAccess accountDataAccess = new AccountDataAccess();

        internal static (DateTime, DateTime) GetDateRange()
        {
            return (new DateTime(2022, 1, 1), new DateTime(2023, 3, 1));
        }

        internal double GetBalance(DateTime balanceDate, string currency)
        {
            if (!hasloaded)
                Refresh();

            return account.Balance + account.Transactions
                .Where(t => t.Date >= balanceDate)
                .Sum(t => -GetConvertedAmount(t, currency));
        }

        internal void DisplayTopExpenses(int top, string currency)
        {
            var spentCategories = account.Transactions
                .Where(t => t.Amount < 0)
                .GroupBy(t => t.Category)
                .Select(g => (Category: g.Key, Total: g.Sum(t => -GetConvertedAmount(t, currency))))
                .OrderByDescending(g => g.Total)
                .Take(top)
                .ToList();

            Console.WriteLine("Top Expenses (all time):");
            for (int i = 0; i < top; i++)
            {
                Console.WriteLine($"{i + 1}) {spentCategories[i].Category}: {spentCategories[i].Total:n2} {currency}");
            }
        }

        private void Refresh()
        {
            account = accountDataAccess.Load();
            hasloaded = true;
        }
        private static double GetConvertedAmount(Transaction t, string currency)
        {
            return t.Amount * ForexService.Singleton().Get(t.Currency, currency);
        }
    }
}
