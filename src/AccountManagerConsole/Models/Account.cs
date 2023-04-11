namespace AccountManagerConsole.Models
{
    internal class Account
    {
        public DateTime AsOf { get; set; } = DateTime.MinValue;
        public List<Transaction> Transactions { get; set; } = new();
    }
}
