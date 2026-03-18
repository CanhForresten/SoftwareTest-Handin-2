using MobileChargingStation.Lib.Boundary;
using MobileChargingStation.Lib.Boundary.Interfaces;
using MobileChargingStation.Lib.Control;
using NSubstitute;

namespace MobileChargingStation.Lib.Test.Control;

[TestFixture]
public class ChargeControlTests
{
    public IUsbCharger _charger;
    public IDisplay _display;
    public IChargeControl _uut;

    [SetUp]
    public void SetUp()
    {
        _display = Substitute.For<IDisplay>();
        _charger = Substitute.For<IUsbCharger>();
        _uut = new ChargeControl(_charger, _display);
    }

    [Test]
    public void StartCharge_CallsUsbCharge()
    {
        _uut.StartCharge();

        _charger.Received(1).StartCharge();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void StartCharge_CurrentIsDoneCharging_StopsCharge(int current)
    {
        _charger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current});

        _charger.Received(1).StopCharge();
        _display.Received(1).ShowMessage("Telefon opladt");
    }
}