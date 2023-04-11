namespace AccountManagerConsole
{
    internal interface IAccountDataAccess
    {
        IEnumerable<Transaction> LoadData();
    }

    internal class AccountDataAccess : IAccountDataAccess
    {
        IEnumerable<Transaction> IAccountDataAccess.LoadData()
        {
            return new List<Transaction>();
        }
    }
}