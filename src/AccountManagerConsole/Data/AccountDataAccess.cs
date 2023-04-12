using AccountManagerConsole.Helper;
using AccountManagerConsole.Models;

namespace AccountManagerConsole
{
    internal interface IAccountDataAccess
    {
        Account Load();
    }

    internal class AccountDataAccess : IAccountDataAccess
    {
        private const string AccountPath = "./Data/account.csv";

        Account IAccountDataAccess.Load()
        {
            return AccountFileParser.Parse(AccountPath);
        }
    }
}