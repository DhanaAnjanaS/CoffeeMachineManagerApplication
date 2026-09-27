using CoffeeShopManagerApplication.Enums;

namespace CoffeeShopManagerApplication.Model;

public delegate void CoffeeMachineEventHandler(object sender, string message);
public class CoffeeMachine
{
    public Guid ID { get; set; }

    public MachineStatus MachineStatus { get; private set; }

    public LidStatus LidStatus { get; set; }

    public event CoffeeMachineEventHandler? MachineStatusChanged;

    public event CoffeeMachineEventHandler? MachineLidStatusChanged;

    public bool IsRunning;


    public void ChangeLidStatus(LidStatus lidStatus)
    {
        if (LidStatus != lidStatus)
        {
            LidStatus = lidStatus;
            MachineStatusChanged?.Invoke(this, $"Coffee machine status changed to state {lidStatus}");
        }
    }

    public void ChangeMachineStatus(MachineStatus newStatus)
    {
        if (MachineStatus != newStatus)
        {
            MachineStatus = newStatus;
            MachineStatusChanged?.Invoke(this, $"Coffee machine status changed to state {newStatus}");
        }
    }

    public async void StartSequence()
    {
        await RunSequence();
    }

    public async void StopSequence()
    {
        IsRunning = false;
    }

    public async Task RunSequence()
    {
        while(this.MachineStatus != MachineStatus.Error && IsRunning)
        {
            ChangeMachineStatus(MachineStatus.PreHeat);
            await Task.Delay(10000);
            ChangeMachineStatus(MachineStatus.PreInclusion);
            await Task.Delay(10000);
            ChangeMachineStatus(MachineStatus.Brewing);
            IsRunning = false;
        }
    }
}
