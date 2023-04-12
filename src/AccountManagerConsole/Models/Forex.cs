namespace AccountManagerConsole.Models
{
    public class Forex
    {
        public Forex() { }

        public Forex(DateTime date, string ccyFrom, string ccyTo, double value)
        {
            Date=date;
            CcyFrom=ccyFrom;
            CcyTo=ccyTo;
            Value=value;
        }

        public DateTime Date { get; set; }
        public string CcyFrom { get; set; }
        public string CcyTo { get; set; }
        public double Value { get; set; }
    }
}
