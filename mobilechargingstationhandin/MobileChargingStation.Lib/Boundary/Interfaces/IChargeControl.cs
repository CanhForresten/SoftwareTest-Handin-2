

public interface IChargeControl
{
    public bool Connected { get; }
    void StartCharge();
    void StopCharge();
}