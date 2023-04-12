namespace AccountManagerConsole.Models
{
    public class Account
    {
        public DateTime AsOf { get; set; } = DateTime.MinValue;
        public double Balance { get; set; }
        public string Currency { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}
