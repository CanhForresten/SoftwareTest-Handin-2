

public interface IChargeControl
{
    bool Connected { get; }

    void IsConnected();
    void StartCharge();
    void StopCharge();
}