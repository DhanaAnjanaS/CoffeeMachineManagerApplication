using CoffeeShopManagerApplication.Controller;
using CoffeeShopManagerApplication.Repository;
using CoffeeShopManagerApplication.Model;
using CoffeeShopManagerApplication.View;

namespace CoffeeShopManagerApplication;

public class Program
{
    public static void Main()
    {
        CoffeeMachine coffeeMachine = new CoffeeMachine();
        Logger logger = new Logger("CoffeeMachineLog.csv");
        CoffeeMachineView coffeeMachineView = new CoffeeMachineView();
        CoffeeMachineManager coffeeMachineManager = new CoffeeMachineManager(coffeeMachineView, coffeeMachine, logger);
        coffeeMachineManager.ShowDashboard();
    }
}
