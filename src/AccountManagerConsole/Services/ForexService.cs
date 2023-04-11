using AccountManagerConsole.Models;

namespace AccountManagerConsole.Services
{
    internal class ForexService
    {
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
    }
}
