using CoffeeShopManagerApplication.Controller;
using CoffeeShopManagerApplication.Model;
using CoffeeShopManagerApplication.Repository;
using CoffeeShopManagerApplication.View;

namespace CoffeeShopManagerApplication;

public class Program
{
    public static void Main()
    {
        CoffeeMachine coffeeMachine = new();
        Logger logger = new("CoffeeMachineLog.csv");

        CoffeeMachineManager coffeeMachineManager =
            new(coffeeMachine, logger);

        CoffeeMachineView coffeeMachineView =
            new(coffeeMachine, coffeeMachineManager);

        coffeeMachineManager.NotificationRaised +=
            coffeeMachineView.HandleNotification;

        coffeeMachineView.ShowDashboard();
    }
}
