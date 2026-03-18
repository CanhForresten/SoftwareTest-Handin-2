using MobileChargingStation.Lib.Boundary.Interfaces;
using MobileChargingStation.Lib.Control;
using NSubstitute;

namespace MobileChargingStation.Lib.Test.Control;

[TestFixture]
public class StationControlTests
{
    private StationControl _uut;
    private IDoor _door;
    private IChargeControl _charger;
    private IDisplay _display;
    private ILogFile _logFile;
    private IRfidReader _rfidReader;

    [SetUp]
    public void Setup()
    {
        _door = Substitute.For<IDoor>();
        _charger = Substitute.For<IChargeControl>();
        _display = Substitute.For<IDisplay>();
        _logFile = Substitute.For<ILogFile>();
        _rfidReader = Substitute.For<IRfidReader>();

        _uut = new StationControl(_door, _logFile, _rfidReader, _display, _charger);
    }

    [Test]
    public void RfidDetected_WhenAvaliableAndConnected_LocksDoor()
    {
        _charger.Connected.Returns(true);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs {Id = 1});

        _door.Received(1).LockDoor();
        _charger.Received(1).StartCharge();
    }

    [Test]
    public void RfidDetected_WhenLockedWithWrongId_DoesNotUnlockDoor()
    {
        _charger.Connected.Returns(true);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 1});
        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 10});

        _door.Received(1).LockDoor();
        _door.DidNotReceive().UnlockDoor();
        _charger.Received(1).StartCharge();
        _charger.DidNotReceive().StopCharge();
    }

    [Test]
    public void RfidDetected_WhenLockedWithRightId_UnlocksDoor()
    {
        _charger.Connected.Returns(true);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 1});
        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 1});

        _door.Received(1).LockDoor();
        _door.Received(1).UnlockDoor();
        _charger.Received(1).StartCharge();
        _charger.Received(1).StopCharge();
    }

    [Test]
    public void RfidDetected_WhenNotConnected_DoesNotLockDoor()
    {
        _charger.Connected.Returns(false);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 1 });

        _door.DidNotReceive().LockDoor();
        _charger.DidNotReceive().StartCharge();
    }

    [Test]
    public void RfidDetected_WhenDoorIsOpen_DoesNothing()
    {
        _door.DoorOpened += Raise.EventWith(EventArgs.Empty);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs {Id = 1});

        _door.DidNotReceive().LockDoor();
        _charger.DidNotReceive().StartCharge();
    }

    [Test]
    public void DoorClosed_FromDoorOpen_ShowMessage()
    {
        _door.DoorOpened += Raise.EventWith(EventArgs.Empty);
        _door.DoorClosed += Raise.EventWith(EventArgs.Empty);

        _display.Received().ShowMessage("Indlæs RFID");
    }

    [Test]
    public void DoorClosed_WhenNotOpen_NotShowingMessage()
    {
        _door.DoorClosed += Raise.EventWith(EventArgs.Empty);

        _display.DidNotReceive().ShowMessage(Arg.Any<string>());
    }

    [Test]
    public void DoorOpen_WhenLocked_DoesNothing()
    {
        _charger.Connected.Returns(true);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs {Id = 1});

        _door.DoorOpened += Raise.EventWith(EventArgs.Empty);

        _door.DidNotReceive().UnlockDoor();
        _charger.DidNotReceive().StopCharge();
    }

    [Test]
    public void DoorClosed_WhenInAvaliableState_DoesNothing()
    {
        _door.DoorClosed += Raise.EventWith(EventArgs.Empty);

        _display.DidNotReceive().ShowMessage(Arg.Any<string>());
        _door.DidNotReceive().UnlockDoor();
        _charger.DidNotReceive().StartCharge();
    }

    [Test]
    public void DoorOpen_OpeningDoorWhenAlreadyOpen_ShowsSameMessageTwice()
    {
        _door.DoorOpened += Raise.EventWith(EventArgs.Empty);
        _door.DoorOpened += Raise.EventWith(EventArgs.Empty);

        _display.Received(2).ShowMessage("Tilslut telefon");
    }

    [Test]
    public void DoorClosed_WhenDoorAlreadyLocked_DoesNothing()
    {
         _charger.Connected.Returns(true);

        _rfidReader.RfidDetected += Raise.EventWith(new DetectedEventArgs { Id = 1 });

        _door.DoorClosed += Raise.EventWith(EventArgs.Empty);

        _door.Received(1).LockDoor();
        _door.DidNotReceive().UnlockDoor();
        _charger.Received(1).StartCharge();
        _charger.DidNotReceive().StopCharge();
    }
}   

