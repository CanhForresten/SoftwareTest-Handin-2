using MobileChargingStation.Lib.Boundary.Interfaces;

namespace MobileChargingStation.Lib.Boundary;

public class RfidReader : IRfidReader
{
    public event EventHandler<DetectedEventArgs> RfidDetected;
    public void OnRfidRead(int id)
    {
        RfidDetected?.Invoke(this, new DetectedEventArgs {Id = id});
    }
}