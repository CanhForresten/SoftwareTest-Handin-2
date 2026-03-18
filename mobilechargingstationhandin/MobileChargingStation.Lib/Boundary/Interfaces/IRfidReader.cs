


public class DetectedEventArgs : EventArgs
{
    public int Id { get; set; }
}

public interface IRfidReader
{
    public event EventHandler<DetectedEventArgs> RfidDetected;
    public void OnRfidRead(int id);
}