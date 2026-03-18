using MobileChargingStation.Lib.Boundary;
using MobileChargingStation.Lib.Boundary.Interfaces;
using NSubstitute.Core;
using NUnit.Framework;

namespace MobileChargingStation.Lib.Test.Boundary;

[TestFixture]
public class TestRfidReader
{
    private IRfidReader _uut;

    [SetUp]
    public void Setup()
    {
        _uut = new RfidReader();
    }

    [Test]
    public void OnRfidRead_EventFired()
    {
        bool eventFired = false;

        _uut.RfidDetected += (sender, args) =>
        {
            eventFired = true;
        };

        _uut.OnRfidRead(123);

        Assert.That(eventFired, Is.True);
    }

    [Test]
    public void OnRfidRead_CorretRfid()
    {
        int recivedId = 0;

        _uut.RfidDetected += (sender, args) =>
        {
            recivedId = args.Id;
        };

        _uut.OnRfidRead(24);

        Assert.That(recivedId, Is.EqualTo(24));
    }

    [Test]
    public void OnRfidRead_EventRaisedOnce()
    {
        int callStack = 0;

        _uut.RfidDetected += (sender, args) =>
        {
            callStack++;
        };

        _uut.OnRfidRead(24);

        Assert.That(callStack, Is.EqualTo(1));
    }
}