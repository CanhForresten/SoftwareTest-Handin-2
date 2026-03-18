using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileChargingStation.Lib.Boundary;
using MobileChargingStation.Lib.Boundary.Interfaces;

namespace MobileChargingStation.Lib.Control;

public class ChargeControl : IChargeControl
{
    private IUsbCharger _usb;
    public bool Connected => _usb.Connected;
    private IDisplay _display;

    public ChargeControl(IUsbCharger usb, IDisplay display)
    {
        _display = display;
        _usb = usb;
        _usb.CurrentValueEvent += HandleCurrentEvent;
    }
    public void StartCharge()
    {
        _usb.StartCharge();
    }
    public void StopCharge()
    {
        _usb.StopCharge();
    }

    public void HandleCurrentEvent(object sender, CurrentEventArgs e)
    {
        var Current = e.Current;

        switch (Current)
        {
            case 0:
                // Ingenting til display
                break;
            
            case double n when n > 0 && n <= 5:
                _display.ShowMessage("Telefon opladt");
                _usb.StopCharge();
                break;
            
            case double n when n > 5 && n <= 500:
                _display.ShowMessage("Oplader telefon");
                break;
            
            case double n when n > 500:
                _display.ShowMessage("Opladningsfejl");
                StopCharge();
                break;
        }
    }
}