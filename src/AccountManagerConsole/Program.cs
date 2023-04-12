using AccountManagerConsole.Services;
using System.Globalization;

internal class Program
{
    // TODO:
    // - split into different projects
    // - manage args
    // loop and exit
    public static void Main(string[] args)
    {
        Console.WriteLine("Account Manager Console");
        var accountService = new AccountService();

        do
        {
            var balanceDate = DateTime.Today;
            Console.WriteLine();
            Console.WriteLine("Please enter a date (format: yyyy-MM-dd):");
            while (!ReadDate(out balanceDate))
                Console.WriteLine($"The input date is invalid.\nPlease enter a valid date (format: yyyy-MM-dd):");

            var balance = accountService.GetBalance(balanceDate, "EUR");
            Console.WriteLine($"Account as of {balanceDate:yyyy-MM-dd}: {balance:n2} EUR");

            Console.WriteLine();
            accountService.DisplayTopExpenses(3, "EUR");
            Console.WriteLine("Continue ? y / n:");
        } while (Console.ReadLine().ToLowerInvariant() != "n");
    }

    internal static bool ReadDate(out DateTime result)
    {
        var input = Console.ReadLine();
        return DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
}
