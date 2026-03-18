using MobileChargingStation.Lib.Boundary.Interfaces;

namespace MobileChargingStation.Lib.Boundary;

public class Door : IDoor
{
    public event EventHandler? DoorOpened;
    public event EventHandler? DoorClosed;

    private bool _isOpen = false;
    private bool _isLocked = false;

    public void LockDoor()
    {
        if (!_isOpen)
        {
            _isLocked = true;
            Console.WriteLine("Døren låses");
        }
        else
        {
            Console.WriteLine("Kan ikke låse døren mens den er åben");
        }
    }

    public void UnlockDoor()
    {
        _isLocked = false;
        Console.WriteLine("Døren låses op");
    }

    public void OnDoorOpen()
    {
        if (_isLocked)
        {
            Console.WriteLine("Døren er låst og kan ikke åbnes");
            return;
        }

        if (!_isOpen)
        {
            Console.WriteLine("Brugeren åbner døren");
            _isOpen = true;
            DoorOpened?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnDoorClose()
    {
        if (_isOpen)
        {
            Console.WriteLine("Brugeren lukker døren");
            _isOpen = false;
            DoorClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}