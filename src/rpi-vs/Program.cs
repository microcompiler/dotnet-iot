using System.Device.Gpio;

Console.WriteLine("Blinking LED.");

int pin = 13;
bool ledOn = true;

var controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);

while (true)
{
    controller.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
    Thread.Sleep(1000);
    ledOn = !ledOn;
}