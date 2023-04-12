using AccountManagerConsole.Services;
using System.Globalization;

internal class Program
{
    // TODO:
    // - split into different projects
    // - manage args
    // Get top 3 categories
    // loop and exit
    public static void Main(string[] args)
    {
        Console.WriteLine("Account Manager Console");

        var accountService = new AccountService();
        // Load data
        // Get date boundaries

        Console.WriteLine("Please enter a date (format: yyyy-MM-dd):");
        var balanceDate = DateTime.Today;
        while (!ReadDate(out balanceDate))
            Console.WriteLine($"The input date is invalid.\nPlease enter a valid date (format: yyyy-MM-dd):");

        var balance = accountService.GetBalance(balanceDate);
        Console.WriteLine($"Account as of {balanceDate:yyyy-MM-dd}: {balance}");

        // check date is valid
        // display result
    }

    internal static bool ReadDate(out DateTime result)
    {
        var input = Console.ReadLine();
        return DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
}
