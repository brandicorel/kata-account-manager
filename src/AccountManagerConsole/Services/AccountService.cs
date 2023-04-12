namespace AccountManagerConsole.Services
{
    internal class AccountService
    {
        private const string DefaultCurrency = "EUR";
        private bool hasloaded = false;

        private readonly IAccountDataAccess accountDataAccess = new AccountDataAccess();

        internal (DateTime, DateTime) GetDateRange()
        {
            throw new NotImplementedException();
        }

        internal double GetBalance(DateTime balanceDate)
        {
            if (!this.hasloaded)
                this.Refresh();

            throw new NotImplementedException();
        }

        private void Refresh()
        {
            var transactions = this.accountDataAccess.LoadData();
            this.hasloaded = true;

        }
    }
}
