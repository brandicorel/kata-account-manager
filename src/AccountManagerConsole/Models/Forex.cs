namespace AccountManagerConsole.Models
{
    internal class Forex
    {
        public DateTime Date { get; set; }
        public string CcyFrom { get; set; }
        public string CcyTo { get; set; }
        public decimal Value { get; set; }
    }
}
