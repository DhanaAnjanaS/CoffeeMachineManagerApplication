using CoffeeShopManagerApplication.Controller;
using CoffeeShopManagerApplication.Model;
using CoffeeShopManagerApplication.View;

namespace CoffeeShopManagerApplication;

public class Program
{
    public static void Main()
    {
        CoffeeMachine coffeeMachine = new CoffeeMachine();
        CoffeeMachineView coffeeMachineView = new CoffeeMachineView();
        CoffeeMachineManager coffeeMachineManager = new CoffeeMachineManager(coffeeMachineView, coffeeMachine);
        coffeeMachineManager.ShowDashboard();
    }
}
