using AccountManagerConsole.Helper;
using AccountManagerConsole.Models;

namespace AccountManagerConsole.Services
{
    // TODO:
    // - precompute amount with forex
    // - date range based on transactions
    internal class AccountService
    {
        private const string DefaultCurrency = "EUR";
        private Account? account;
        private bool hasloaded = false;

        private readonly IAccountDataAccess accountDataAccess = new AccountDataAccess();

        internal static (DateTime, DateTime) GetDateRange()
        {
            return (new DateTime(2022, 1, 1), new DateTime(2023, 3, 1));
        }

        internal double GetBalance(DateTime balanceDate)
        {
            if (!hasloaded)
                Refresh();

            return account.Balance + account.Transactions
                .Where(t => t.Date >= balanceDate)
                .Sum(t => -t.Amount * ForexService.Singleton().Get(t.Currency, DefaultCurrency));
        }

        private void Refresh()
        {
            account = accountDataAccess.Load();
            hasloaded = true;
        }
    }
}
