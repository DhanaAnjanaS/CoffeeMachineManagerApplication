using CoffeeShopManagerApplication.Enums;
using CoffeeShopManagerApplication.Model;
using CoffeeShopManagerApplication.Repository;

namespace CoffeeShopManagerApplication.Controller;

public class CoffeeMachineManager
{
    private readonly CoffeeMachine _coffeeMachine;
    private readonly Logger _logger;

    public event Action<string>? NotificationRaised;

    public CoffeeMachineManager(
        CoffeeMachine coffeeMachine,
        Logger logger)
    {
        _coffeeMachine = coffeeMachine;
        _logger = logger;

        _coffeeMachine.MachineStatusChanged += HandleStatusChanged;
        _coffeeMachine.MachineLidStatusChanged += HandleLidStatusChanged;

        InitializeMachine();
    }

    private void InitializeMachine()
    {
        _coffeeMachine.ChangeLidStatus(LidStatus.Open);
        _coffeeMachine.ChangeMachineStatus(MachineStatus.Safe);

        Notify(
            "INIT",
            "Coffee machine initialized"
        );
    }

    private void HandleStatusChanged(
        object sender,
        string message)
    {
        _logger.Log("STATUS", message);
        NotificationRaised?.Invoke(message);
    }

    private void HandleLidStatusChanged(
        object sender,
        string message)
    {
        _logger.Log("LID", message);
        NotificationRaised?.Invoke(message);
    }

    public void StartBrewSequence()
    {
        bool started = _coffeeMachine.StartSequence();

        if (!started)
        {
            Notify(
                "START_REJECTED",
                "Machine must be ready and the lid must be closed before starting"
            );
        }
    }

    public void StopBrewSequence()
    {
        if (!_coffeeMachine.IsRunning)
        {
            Notify(
                "STOP_REJECTED",
                "Coffee machine sequence is not running"
            );

            return;
        }

        _coffeeMachine.StopSequence();

        Notify(
            "STOPPED",
            "Coffee machine brewing stopped"
        );
    }

    public void SimulateError()
    {
        if (!_coffeeMachine.SimulateError())
        {
            Notify(
                "ERROR_REJECTED",
                "Error can be simulated ONLY when machine is in Brewing state"
            );
        }
    }

    public void ToggleLidSensor()
    {
        if (_coffeeMachine.MachineStatus != MachineStatus.Safe &&
            _coffeeMachine.MachineStatus != MachineStatus.Ready)
        {
            Notify(
                "LID_REJECTED",
                $"Coffee machine lid cannot be opened when machine status is {_coffeeMachine.MachineStatus}"
            );

            return;
        }

        LidStatus newStatus =
            _coffeeMachine.LidStatus == LidStatus.Open
                ? LidStatus.Closed
                : LidStatus.Open;

        _coffeeMachine.ChangeLidStatus(newStatus);
    }

    public void ResetMachine()
    {
        if (_coffeeMachine.LidStatus == LidStatus.Open)
        {
            Notify(
                "RESET_REJECTED",
                "Please close the lid before resetting"
            );

            return;
        }

        _coffeeMachine.ChangeMachineStatus(MachineStatus.Ready);

        Notify(
            "RESET",
            "Coffee machine is ready for brewing"
        );
    }

    public IEnumerable<string> ViewEventLogs()
    {
        return _logger.GetAllLogs();
    }

    private void Notify(string eventType, string message)
    {
        _logger.Log(eventType, message);
        NotificationRaised?.Invoke(message);
    }
}