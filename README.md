# ONI Mods
Mods for Klei's Oxygen Not Included

# Reading materials

1. Cairath's ONI Modding Guide: https://github.com/Cairath/Oxygen-Not-Included-Modding/wiki
1. Klei Modding guidelines: https://forums.kleientertainment.com/forums/topic/116697-modding-guidelines/
1. Peter Han's PLib library: https://github.com/peterhaneve/ONIMods/tree/main/PLib

# Dev environment setup for Ubuntu 20.04 LTS

## Install the dotnet sdk & cli

1. wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
1. sudo dpkg -i packages-microsoft-prod.deb
1. rm packages-microsoft-prod.deb
1. sudo apt-get update
1. sudo apt-get install dotnet-sdk-6.0

## Test the dotnet sdk by building a test app

1. dotnet new console -o testapp -f net6.0
1. cd testapp
1. dotnet run
1. cd ..
1. rm -rf testapp

## (Optional) Install .net decompiler

1. Visit https://github.com/codemerx/CodemerxDecompile/releases & install the latest release

# Building this project

1. cd IdleNotificationTweaks; make

# Testing this project

1. make test
1. Start Oxygen Not Included
1. Click Mods and make sure this mod is enabled
1. Restart ONI
1. play the game
1. make Player.log & review it
