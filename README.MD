# Visual Studio Code Remote Debug for Raspberry Pi

## 1. Install Ubuntu on a Raspberry Pi
Follow this [tutorial](https://ubuntu.com/tutorials/how-to-install-ubuntu-on-your-raspberry-pi#1-overview) to walk you through the process of installing Ubuntu Server on a Raspberry Pi.

## 2. Setup device hostname and required software
```bash
# Setup the device hostname
sudo hostnamectl set-hostname [hostname]

# Get updated software for Ubuntu
sudo apt update
sudo apt upgrade

# Install the current version of .NET 
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc

# Install the latest virsion of Visual Studio Remote Debugger
curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/.vsdbg

# Enable user access to GPIO, I2C, GPIO and UART
sudo apt install rpi.gpio-common

# Restart the remote device
sudo reboot
```

## 3. Enable SSH key authentication on the Raspberry Pi
```bash
# Use ssh-keygen with default settings to create our SSH key pair and copy to device
ssh-keygen
scp .ssh/id_rsa.pub ubuntu@[hostname]: # Replace with set hostname

# Login to remote device
ssh ubuntu@[hostname] # Replace with set hostname

# Let’s append the public key we transferred to our authorized keys
chmod 700 .ssh
touch .ssh/authorized_keys
chmod 644 .ssh/authorized_keys
cat id_rsa.pub >> .ssh/authorized_keys
```

# Visual Studio Code Remote Debugging for Raspberry Pi
Create the following NET6.0 Linux Arm64 project

program.cs
```csharp
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
```

/rpi.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.Device.Gpio" Version="2.1.0" />
  </ItemGroup>
</Project>
```

/.vs/tasks.json
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Remote Console",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "deploy",
            "program": "~/.dotnet/dotnet",
            "stopAtEntry": false,
            "args": [
                "/home/ubuntu/${workspaceFolderBasename}/Rpi.dll"
            ],
            "cwd": "~/${workspaceFolderBasename}",
            "console": "internalConsole",
            "pipeTransport": {
                "pipeCwd": "${workspaceRoot}",
                "pipeProgram": "ssh",
                "pipeArgs": [
                    "ubuntu@[hostname]" // Replace with set hostname
                ],
                "debuggerPath": "~/.vsdbg/vsdbg"
            },
            "sourceFileMap": {
                "~/${workspaceFolderBasename}/": "${workspaceRoot}"
            },
            "logging": {
                "moduleLoad": false
            },
        }
    ]
}
```

/.vs/tasks.json
```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "-r",
                "linux-arm64",
                "${workspaceFolder}/Rpi.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary",
                "--no-self-contained"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "deploy",
            "type": "shell",
            "dependsOn": "build",
            "command": "scp",
            "args": [
                "-r",
                "-p",
                "bin/Debug/net6.0/linux-arm64/*",
                "ubuntu@[hostname]\"~/${workspaceFolderBasename}\"" // Replace with set hostname
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```

# Helpful Links
- [https://code.visualstudio.com/docs/remote/ssh](https://code.visualstudio.com/docs/remote/ssh)
- [https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md](https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md)
- [https://www.petecodes.co.uk/deploying-and-debugging-raspberry-pi-net-applications-using-vs-code/](https://www.petecodes.co.uk/deploying-and-debugging-raspberry-pi-net-applications-using-vs-code/)

# Give a Star! :star:

If you like or are using this project to start your solution, please give it a star. Thanks!

# Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

# Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.