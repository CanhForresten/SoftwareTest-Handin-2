using MobileChargingStation.Lib.Boundary;
using MobileChargingStation.Lib.Boundary.Interfaces;
using NSubstitute.Core;
using NUnit.Framework;

namespace MobileChargingStation.Lib.Test.Boundary;

[TestFixture]
public class TestDoor
{
    private IDoor _uut;

    [SetUp]
    public void Setup()
    {
        _uut = new Door();
    }

    [Test]
    public void DoorClosed_OpenDoor_RaiseDoorOpened()
    {
        bool eventFired = false;

        _uut.DoorOpened += (sender, args) =>
        {
            eventFired = true;
        };

        _uut.OnDoorOpen();

        Assert.That(eventFired, Is.True);
    }

    [Test]
    public void DoorOpen_OnDoorClose_EventFired()
    {
        bool eventFired = false;

        _uut.OnDoorOpen();

        _uut.DoorClosed += (sender, args) =>
        {
            eventFired = true;
        };

        _uut.OnDoorClose();

        Assert.That(eventFired, Is.True);
    }

    [Test]
    public void DoorAlreadyOpen_OpenDoor_NoEvent()
    {
        int callcount = 0;

        _uut.DoorOpened += (sender, args) => 
        {
            callcount++;
        };

        _uut.OnDoorOpen();
        _uut.OnDoorOpen();

        Assert.That(callcount, Is.EqualTo(1));
    }

    [Test]
    public void DoorAlreadyClosed_CloseDoor_NoEvent()
    {
        int callcount = 0;

        _uut.DoorClosed += (sender, args) =>
        {
            callcount++;
        };

        _uut.OnDoorClose();

        Assert.That(callcount, Is.EqualTo(0));
    }


    [Test]
    public void OnDoorOpened_WhenLocked_DoesNotRaiseEvent()
    {
        bool eventFired = false;

        _uut.LockDoor();

        _uut.DoorOpened += (sender, args) =>
        {
            eventFired = true;
        };

        _uut.OnDoorOpen();

        Assert.That(eventFired, Is.False);
        
    }

    [Test]
    public void DoorOpen_LockDoor_DoorStillOpen()
    {
        bool closeEvent = false;

        _uut.DoorClosed += (sender, args) =>
        {
            closeEvent = true;
        };

        _uut.OnDoorOpen();
        _uut.LockDoor();

        Assert.That(closeEvent, Is.False);
    }

    [Test]
    public void DoorLocked_UnlockDoor_CanOpen()
    {
        bool UnlockEvent = false;

        _uut.DoorOpened += (sender, args) =>
        {
            UnlockEvent = true;
        };

        _uut.LockDoor();
        _uut.UnlockDoor();
        _uut.OnDoorOpen();

        Assert.That(UnlockEvent, Is.True);

    }

}

