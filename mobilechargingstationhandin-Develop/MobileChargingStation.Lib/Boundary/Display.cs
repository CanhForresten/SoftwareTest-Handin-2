using MobileChargingStation.Lib.Boundary.Interfaces;

public class Display: IDisplay
{
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}