https://code.visualstudio.com/docs/remote/ssh
https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md



curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc

sudo apt install rpi.gpio-common
sudo reboot