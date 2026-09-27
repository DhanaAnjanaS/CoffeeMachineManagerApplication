using CoffeeShopManagerApplication.Enums;

namespace CoffeeShopManagerApplication.View;

public class CoffeeMachineView
{
    public void DisplayTopConsole(string notification)
    {
        Console.WriteLine("Coffee Machine Status");
        Console.WriteLine($"Notification :{notification}");
    }

    public void DisplayActivity(MachineStatus machineStatus, LidStatus lidStatus)
    {
        Console.WriteLine($"Machine Status: {machineStatus}");
        Console.WriteLine($"Lid Status: {lidStatus}");
    }

    public void DisplayLogs(IEnumerable<string> logs)
    {
        foreach(var log in logs)
        {
            Console.WriteLine(log);
        }
    }

    public void Splitter()
    {
        Console.WriteLine(new string('-', 75));
    }

    public void DisplayExitMessage()
    {
        Console.WriteLine("Thank you for using our coffee machine!!!");
    }

    public void WaitForConfirmation()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public void DisplayMenu()
    {
        Console.WriteLine("MENU OPTIONS");
        Console.WriteLine("1. Start the sequence");
        Console.WriteLine("2. Stop the sequence");
        Console.WriteLine("3. Simulate Machine Error");
        Console.WriteLine("4. Toggle Brew Lid Sensor");
        Console.WriteLine("5. Reset Machine");
        Console.WriteLine("6. View Event Log");
        Console.WriteLine("7. Exit application");
        Console.Write($"Enter your choice [{1}-{7}]: ");
    }

    public int ReadMenuChoice()
    {
        ConsoleKey key = Console.ReadKey().Key;
        return key switch
        {
            ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
            ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
            ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
            ConsoleKey.D4 or ConsoleKey.NumPad4 => 4,
            ConsoleKey.D5 or ConsoleKey.NumPad5 => 5,
            ConsoleKey.D6 or ConsoleKey.NumPad6 => 6,
            ConsoleKey.D7 or ConsoleKey.NumPad7 => 7,
            _ => 0
        };
    }
}
