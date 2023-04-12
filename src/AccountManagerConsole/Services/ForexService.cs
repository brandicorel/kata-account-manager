using AccountManagerConsole.Models;

namespace AccountManagerConsole.Services
{
    // TODO:
    // - Load independantly from Account
    // - Manage multiple date and getLatest
    // - Expires/Refresh cache
    // - volatile + ConcurrentDictionnary in case of multithreading
    internal class ForexService
    {
        private const string AccountPath = "/Data/account.csv";

        private readonly Dictionary<(string, string), Forex> forexes = new();

        public Forex Get(string ccyFrom, string ccyTo)
        {
            return this.forexes[(ccyFrom, ccyTo)];
        }

        public void Add(Forex forex)
        {
            var invertForex = new Forex()
            {
                Date = forex.Date,
                CcyFrom = forex.CcyTo,
                CcyTo = forex.CcyFrom,
                Value = 1 / forex.Value
            };

            this.forexes.Add((forex.CcyFrom, forex.CcyTo), forex);
            this.forexes.Add((invertForex.CcyFrom, invertForex.CcyTo), invertForex);
        }

        public void Refresh()
        {

        }
    }
}
