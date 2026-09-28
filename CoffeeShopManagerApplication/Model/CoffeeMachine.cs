using CoffeeShopManagerApplication.Enums;

namespace CoffeeShopManagerApplication.Model;

public delegate void CoffeeMachineEventHandler(object sender, string message);

public class CoffeeMachine
{
    private readonly object _lock = new();
    private CancellationTokenSource? _sequenceCancellation;

    public Guid ID { get; set; }

    public MachineStatus MachineStatus { get; private set; }

    public LidStatus LidStatus { get; private set; }

    public bool IsRunning
    {
        get
        {
            lock (_lock)
            {
                return _sequenceCancellation != null;
            }
        }
    }

    public event CoffeeMachineEventHandler? MachineStatusChanged;
    public event CoffeeMachineEventHandler? MachineLidStatusChanged;

    public void ChangeLidStatus(LidStatus lidStatus)
    {
        lock (_lock)
        {
            if (LidStatus == lidStatus)
                return;

            LidStatus = lidStatus;
        }

        MachineLidStatusChanged?.Invoke(this, $"Coffee machine lid changed to state {lidStatus}");
    }

    public void ChangeMachineStatus(MachineStatus newStatus)
    {
        lock (_lock)
        {
            if (MachineStatus == newStatus)
                return;

            MachineStatus = newStatus;
        }

        MachineStatusChanged?.Invoke(this, $"Coffee machine status changed to state {newStatus}");
    }

    public bool StartSequence()
    {
        lock (_lock)
        {
            if (MachineStatus != MachineStatus.Ready || LidStatus != LidStatus.Closed || _sequenceCancellation != null)
            {
                return false;
            }

            _sequenceCancellation = new CancellationTokenSource();
        }

        _ = RunSequenceAsync(_sequenceCancellation.Token);

        return true;
    }

    public void StopSequence()
    {
        CancellationTokenSource? cancellation;

        lock (_lock)
        {
            cancellation = _sequenceCancellation;
            _sequenceCancellation = null;
        }

        cancellation?.Cancel();
        cancellation?.Dispose();

        ChangeMachineStatus(MachineStatus.Ready);
    }

    public bool SimulateError()
    {
        lock (_lock)
        {
            if (MachineStatus != MachineStatus.Brewing)
                return false;
        }

        CancellationTokenSource? cancellation;

        lock (_lock)
        {
            cancellation = _sequenceCancellation;
            _sequenceCancellation = null;
        }

        cancellation?.Cancel();
        cancellation?.Dispose();

        ChangeMachineStatus(MachineStatus.Error);

        return true;
    }

    private async Task RunSequenceAsync(CancellationToken cancellationToken)
    {
        try
        {
            ChangeMachineStatus(MachineStatus.PreHeat);

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            ChangeMachineStatus(MachineStatus.PreInclusion);

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            ChangeMachineStatus(MachineStatus.Brewing);

            lock (_lock)
            {
                _sequenceCancellation = null;
            }
        }
        catch (OperationCanceledException)
        {
            // StopSequence() or SimulateError() already
            // performed the appropriate state transition.
        }
    }
}