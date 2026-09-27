using CoffeeShopManagerApplication.Enums;
using CoffeeShopManagerApplication.Model;
using CoffeeShopManagerApplication.Repository;
using CoffeeShopManagerApplication.View;

namespace CoffeeShopManagerApplication.Controller;

public class CoffeeMachineManager
{
    private CoffeeMachine _coffeeMachine;
    private CoffeeMachineView _coffeeMachineView;
    private Logger _logger;

    public CoffeeMachineManager(CoffeeMachineView coffeeMachineView, CoffeeMachine coffeeMachine, Logger logger)
    {
        _coffeeMachineView = coffeeMachineView;
        _coffeeMachine = coffeeMachine;
        _logger = logger;
        InitializeMachine();
    }

    public void ShowDashboard()
    {
        _coffeeMachineView.DisplayTopConsole("Coffee machine initialized");
        _logger.Log("INIT", "Coffee machine initialized");
        _coffeeMachineView.DisplayActivity(_coffeeMachine.MachineStatus, _coffeeMachine.LidStatus);
        int option;
        do
        {
            _coffeeMachineView.DisplayMenu();
            option = _coffeeMachineView.ReadMenuChoice();
            switch (option)
            {
                case 1:
                    StartBrewSequence();
                    break;
                case 2:
                    StopBrewSequence();
                    break;
                case 3:
                    SimulateError();
                    break;
                case 4:
                    ToggleLidSensor();
                    break;
                case 5:
                    ResetMachine();
                    break;
                case 6:
                    ViewEventLogs();
                    break;
                case 7:
                    _coffeeMachineView.DisplayExitMessage();
                    break;
            }
            _coffeeMachineView.WaitForConfirmation();

        } while (option != 7);
    }

    public void InitializeMachine()
    {
        _coffeeMachine.LidStatus = LidStatus.Open;
        _coffeeMachine.ChangeMachineStatus(MachineStatus.Safe);
        _logger.Log("STATUS", $"Machine status changed to {MachineStatus.Safe}");
    }

    public void ResetMachine()
    {
        if (_coffeeMachine.LidStatus == LidStatus.Open)
        {
            _coffeeMachineView.DisplayTopConsole("Please close the lid before resetting");
        }
        else if (_coffeeMachine.LidStatus == LidStatus.Closed)
        {
            _coffeeMachine.ChangeMachineStatus(MachineStatus.Ready);
            _logger.Log("STATUS", $"Machine status changed to {MachineStatus.Ready}");
            _coffeeMachineView.DisplayTopConsole("Coffee machine is ready for brewing");
        }
    }

    public void StartBrewSequence()
    {

    }

    public void StopBrewSequence()
    {

    }

    public void SimulateError()
    {

    }

    public void ToggleLidSensor()
    {
        if (_coffeeMachine.LidStatus == LidStatus.Open && _coffeeMachine.MachineStatus == MachineStatus.Safe)
        {
            _coffeeMachine.ChangeLidStatus(LidStatus.Closed);
            _logger.Log("LID", $"Lid status changed to {LidStatus.Closed}");
        }

        if (_coffeeMachine.LidStatus == LidStatus.Closed &&
            (_coffeeMachine.MachineStatus == MachineStatus.PreHeat || _coffeeMachine.MachineStatus == MachineStatus.PreInclusion
            || _coffeeMachine.MachineStatus == MachineStatus.Brewing))
        {
            _coffeeMachineView.DisplayTopConsole($"Coffee machine lid cannot be opened when machine status is {_coffeeMachine.MachineStatus}");
        }
        
        if(_coffeeMachine.LidStatus == LidStatus.Closed && _coffeeMachine.MachineStatus == MachineStatus.Ready)
        {
            _coffeeMachine.ChangeLidStatus(LidStatus.Open);
            _logger.Log("LID", $"Lid status changed to {LidStatus.Open}");
            _coffeeMachineView.DisplayTopConsole($"Coffee machine lid status changed to {LidStatus.Open}");
        }
    }

    public void ViewEventLogs()
    {

    }
}
