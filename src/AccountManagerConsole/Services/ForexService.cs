using AccountManagerConsole.Models;

namespace AccountManagerConsole.Services
{
    // TODO:
    // - Load independantly from Account
    // - Manage multiple date and getLatest
    // - Expires/Refresh cache
    // - volatile + ConcurrentDictionnary in case of multithreading
    public class ForexService
    {
        private static ForexService instance = new();

        public static ForexService Singleton()
        {
            return instance;
        }

        private readonly Dictionary<(string, string), Forex> forexes = new();

        public double Get(string ccyFrom, string ccyTo)
        {
            if (ccyFrom == ccyTo)
                return 1;

            return forexes[(ccyFrom, ccyTo)].Value;
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

            forexes.Add((forex.CcyFrom, forex.CcyTo), forex);
            forexes.Add((invertForex.CcyFrom, invertForex.CcyTo), invertForex);
        }
    }
}
