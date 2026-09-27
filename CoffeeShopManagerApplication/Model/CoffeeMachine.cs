using CoffeeShopManagerApplication.Enums;

namespace CoffeeShopManagerApplication.Model;

public delegate void CoffeeMachineEventHandler(object sender, string message);
public class CoffeeMachine
{
    public Guid ID { get; set; }

    public MachineStatus MachineStatus { get; private set; }

    public LidStatus LidStatus { get; set; }

    public event CoffeeMachineEventHandler? MachineStatusChanged;

    public void ChangeMachineStatus(MachineStatus newStatus)
    {
        if (MachineStatus != newStatus)
        {
            MachineStatus = newStatus;
            MachineStatusChanged?.Invoke(this, $"Coffee machine status changed to state {newStatus}");
        }
    }
}
