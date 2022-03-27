using System.Diagnostics;
using System.Device.Gpio;

Debug.WriteLine("Blinking LED");

int pin = 13;
bool ledOn = true;

var controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);

try
{
    while (true)
    {
        controller.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
        Thread.Sleep(1000);
        ledOn = !ledOn;
    }
}
finally
{
    controller.ClosePin(pin);
}