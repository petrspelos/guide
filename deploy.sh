cd src/Guide
dotnet publish -r linux-arm -c Release
cd bin/Release/netcoreapp2.2/linux-arm/publish/
screen -S guide ./Guide
