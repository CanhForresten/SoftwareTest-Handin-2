using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileChargingStation.Lib.Boundary.Interfaces;

namespace MobileChargingStation.Lib.Control;

public class StationControl
{
    // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
    private enum MobileChargerState
    {
        Available,
        Locked,
        DoorOpen
    };

    // Her mangler flere member variable
    private MobileChargerState _state;
    private IChargeControl _charger;
    private int _oldId;
    private IDoor _door;
    private ILogFile _logFile;
    private IRfidReader _rfidReader;
    private IDisplay _display;


    private string logFile = "logfile.txt"; // Navnet på systemets log-fil

    // Her mangler constructor
    public StationControl(IDoor door, ILogFile logFile, IRfidReader rfidReader, IDisplay display, IChargeControl charger)
    {
        _door = door;
        _logFile = logFile;
        _rfidReader = rfidReader;
        _display = display;
        _charger = charger;

        _state = MobileChargerState.Available;

        _door.DoorOpened += HandleDoorOpend;
        _door.DoorClosed += HandleDoorClosed;
        _rfidReader.RfidDetected += (sender, e) => RfidDetected(e.Id);
        
    }
    

    // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
    private void RfidDetected(int id)
    {
        switch (_state)
        {
            case MobileChargerState.Available:
                // Check for ladeforbindelse
                if (_charger.Connected)
                {
                    _door.LockDoor();
                    _charger.StartCharge();
                    _oldId = id;
                    _logFile.Log(": Skab låst med RFID: {0}", id);

                    Console.WriteLine(
                        "Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op."
                    );
                    _state = MobileChargerState.Locked;
                }
                else
                {
                    Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                }

                break;

            case MobileChargerState.DoorOpen:
                // Ignore
                break;

            case MobileChargerState.Locked:
                // Check for correct ID
                if (id == _oldId)
                {
                    _charger.StopCharge();
                    _door.UnlockDoor();
                    _logFile.Log(": Skab låst op med RFID: {0}",id);
                    Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                    _state = MobileChargerState.Available;
                }
                else
                {
                    Console.WriteLine("Forkert RFID tag");
                }

                break;
        }
    }

    // Her mangler de andre trigger handlere
    private void HandleDoorOpend(object sender, EventArgs e)
    {
        switch (_state)
        {
            case MobileChargerState.Available:
                _display.ShowMessage("Tilslut telefon");
                _state = MobileChargerState.DoorOpen;
                break;

            case MobileChargerState.DoorOpen:
                _display.ShowMessage("Tilslut telefon");
                break;

            case MobileChargerState.Locked:
                // Ignore
                break;
        }
        
    }

    private void HandleDoorClosed(object sender, EventArgs e)
    {
        switch (_state)
        {
            case MobileChargerState.DoorOpen:
                _display.ShowMessage("Indlæs RFID");
                _state = MobileChargerState.Available;
                break;
            
            case MobileChargerState.Available:
                // Ignore
                break;

            case MobileChargerState.Locked:
                // Ignore
                break;
        }
        
    }
} 
