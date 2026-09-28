using CoffeeShopManagerApplication.Controller;
using CoffeeShopManagerApplication.Enums;
using CoffeeShopManagerApplication.Model;

namespace CoffeeShopManagerApplication.View;

public class CoffeeMachineView
{
    private readonly CoffeeMachine _coffeeMachine;
    private readonly CoffeeMachineManager _manager;

    private readonly object _consoleLock = new();

    private string _notification = "Coffee machine initialized";
    private DateTime _statusChangedAt = DateTime.Now;

    private bool _exitRequested;

    public CoffeeMachineView(
        CoffeeMachine coffeeMachine,
        CoffeeMachineManager manager)
    {
        _coffeeMachine = coffeeMachine;
        _manager = manager;

        _coffeeMachine.MachineStatusChanged +=
            HandleMachineStatusChanged;

        _coffeeMachine.MachineLidStatusChanged +=
            HandleLidStatusChanged;
    }

    public void HandleNotification(string notification)
    {
        lock (_consoleLock)
        {
            _notification = notification;
        }
    }

    private void HandleMachineStatusChanged(
        object sender,
        string message)
    {
        lock (_consoleLock)
        {
            _statusChangedAt = DateTime.Now;
        }
    }

    private void HandleLidStatusChanged(
        object sender,
        string message)
    {
        // The next UI refresh automatically reads the
        // latest lid state from the machine.
    }

    public void ShowDashboard()
    {
        Task.Run(RenderLoop);

        while (!_exitRequested)
        {
            int option = ReadMenuChoice();

            switch (option)
            {
                case 1:
                    _manager.StartBrewSequence();
                    break;

                case 2:
                    _manager.StopBrewSequence();
                    break;

                case 3:
                    _manager.SimulateError();
                    break;

                case 4:
                    _manager.ToggleLidSensor();
                    break;

                case 5:
                    _manager.ResetMachine();
                    break;

                case 6:
                    ShowLogs();
                    break;

                case 7:
                    _exitRequested = true;
                    break;
            }
        }
    }

    private async Task RenderLoop()
    {
        while (!_exitRequested)
        {
            RenderDashboard();

            await Task.Delay(1000);
        }
    }

    private void RenderDashboard()
    {
        lock (_consoleLock)
        {
            Console.Clear();

            DisplayStatusSection();
            DisplayActivitySection();
            DisplayMenuSection();
        }
    }

    private void DisplayStatusSection()
    {
        Console.WriteLine("COFFEE MACHINE STATUS");
        Console.WriteLine($"Notification : {_notification}");

        Splitter();
    }

    private void DisplayActivitySection()
    {
        MachineStatus status = _coffeeMachine.MachineStatus;
        LidStatus lidStatus = _coffeeMachine.LidStatus;

        Console.WriteLine("MACHINE ACTIVITY");
        Console.WriteLine($"Machine Status : {status}");
        Console.WriteLine($"Lid Status     : {lidStatus}");

        string activity = GetActivity(status);

        Console.WriteLine($"Activity       : {activity}");

        Splitter();
    }

    private string GetActivity(MachineStatus status)
    {
        if (status == MachineStatus.PreHeat)
        {
            return GetRemainingTime(10, "Preheating");
        }

        if (status == MachineStatus.PreInclusion)
        {
            return GetRemainingTime(10, "Pre-inclusion");
        }

        if (status == MachineStatus.Brewing)
        {
            return "Brewing in progress";
        }

        if (status == MachineStatus.Ready)
        {
            return "Machine ready";
        }

        if (status == MachineStatus.Safe)
        {
            return "Machine in safe mode";
        }

        if (status == MachineStatus.Error)
        {
            return "Machine stopped due to error";
        }

        return "No activity";
    }

    private string GetRemainingTime(
        int durationSeconds,
        string activity)
    {
        int elapsed =
            (int)(DateTime.Now - _statusChangedAt).TotalSeconds;

        int remaining =
            Math.Max(0, durationSeconds - elapsed);

        return $"{activity} - {remaining} seconds remaining";
    }

    private void DisplayMenuSection()
    {
        Console.WriteLine("MENU OPTIONS");
        Console.WriteLine("1. Start the sequence");
        Console.WriteLine("2. Stop the sequence");
        Console.WriteLine("3. Simulate Machine Error");
        Console.WriteLine("4. Toggle Brew Lid Sensor");
        Console.WriteLine("5. Reset Machine");
        Console.WriteLine("6. View Event Log");
        Console.WriteLine("7. Exit application");
        Console.Write("Enter your choice [1-7]: ");
    }

    private int ReadMenuChoice()
    {
        ConsoleKey key = Console.ReadKey(true).Key;

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

    private void ShowLogs()
    {
        IEnumerable<string> logs = _manager.ViewEventLogs();

        lock (_consoleLock)
        {
            Console.Clear();

            Console.WriteLine("EVENT LOG");
            Splitter();

            foreach (string log in logs)
            {
                Console.WriteLine(log);
            }

            Splitter();
            Console.WriteLine("Press any key to return...");

            Console.ReadKey(true);
        }
    }

    private void Splitter()
    {
        Console.WriteLine(new string('-', 75));
    }
}