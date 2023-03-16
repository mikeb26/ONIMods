export DOTNET_CLI_TELEMETRY_OPTOUT=1
export ONIInstallDir ?= "$(HOME)/.steam/steam/SteamApps/common/OxygenNotIncluded"
export ONIPlayerDir ?= "$(HOME)/.config/unity3d/Klei/Oxygen Not Included"
export ONIModName = "CGSM"
export ONIModDLL = $(ONIModName).dll
export ONIModMergedDLL = $(ONIModName)Merged.dll
export ModVer = $(shell echo $(shell grep version mod_info.yaml | tail -1 | cut -f2 -d:)-$(shell git rev-parse --short HEAD))

.PHONY: build
build: bin/Debug/$(ONIModMergedDLL) bin/Release/$(ONIModMergedDLL) bin/Debug/worldgen bin/Release/worldgen bin/Debug/templates bin/Release/templates

bin/Debug/$(ONIModDLL): Util.cs depscheck FORCE
	dotnet build -c Debug

bin/Release/$(ONIModDLL): Util.cs depscheck FORCE
	dotnet build -c Release

Util.cs: Util.cs.tmpl FORCE
	sed s/%VERSION%/$(ModVer)/ Util.cs.tmpl > Util.cs

# https://github.com/peterhaneve/ONIMods/tree/main/PLib
#
# @todo there's probably a cleaner way to do this via PostBuildEvent in csproj but all of the
# examples I could find assume visual studio
#
export ILREPACK = "$(HOME)/.nuget/packages/ilrepack/2.0.18/tools/ILRepack.exe"
bin/Debug/$(ONIModMergedDLL): bin/Debug/$(ONIModDLL)
	cd bin/Debug; $(ILREPACK) /ndebug /out:$(ONIModMergedDLL) $(ONIModDLL) PLib.dll Newtonsoft.Json.dll /targetplatform:v4

bin/Release/$(ONIModMergedDLL): bin/Release/$(ONIModDLL)
	cd bin/Release; $(ILREPACK) /ndebug /out:$(ONIModMergedDLL) $(ONIModDLL) PLib.dll Newtonsoft.Json.dll /targetplatform:v4

# https://stackoverflow.com/questions/119271/copy-all-files-and-folders-using-msbuild
#
# @todo cleaner to do this within csproj but using the above reference it doesn't work
bin/Debug/worldgen: FORCE
	rm -rf bin/Debug/worldgen
	cp -a worldgen bin/Debug/worldgen
bin/Release/worldgen: FORCE
	rm -rf bin/Release/worldgen
	cp -a worldgen bin/Release/worldgen
bin/Debug/templates: FORCE
	rm -rf bin/Debug/templates
	cp -a templates bin/Debug/templates
bin/Release/templates: FORCE
	rm -rf bin/Release/templates
	cp -a templates bin/Release/templates

.PHONY: dirscheck
dirscheck:
	if [ ! -d $(ONIInstallDir) ]; then echo "Cannot find ONI Install; please set ONIInstallDir env var" 1>&2; exit 1; fi
	if [ ! -d $(ONIPlayerDir) ]; then echo "Cannot find ONI Player config; please set ONIPlayerDir env var" 1>&2; exit 1; fi

.PHONY: depscheck
depscheck:
	if [ ! -d deps ]; then make deps; fi

.PHONY: test
test: build FORCE
	rm -rf "$(ONIPlayerDir)/mods/Local/$(ONIModName)"
	mkdir "$(ONIPlayerDir)/mods/Local/$(ONIModName)"
	cp -r bin/Debug/$(ONIModMergedDLL) bin/Debug/mod_info.yaml bin/Debug/mod.yaml bin/Debug/worldgen bin/Debug/templates "$(ONIPlayerDir)/mods/Local/$(ONIModName)"

.PHONY: log
log:
	cat "$(ONIPlayerDir)/Player.log"

.PHONY: logf
logf:
	tail -f "$(ONIPlayerDir)/Player.log"

Player.log:
	ln -s "$(ONIPlayerDir)/Player.log" .

deps: dirscheck FORCE
	dotnet add package Microsoft.NETFramework.ReferenceAssemblies.net471 --version 1.0.3
	dotnet add package ILRepack --version 2.0.18
	dotnet add package PLib --version 4.12.0
	dotnet add package Newtonsoft.Json --version 13.0.2
	rm -rf deps
	mkdir deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/Assembly-CSharp.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/Assembly-CSharp-firstpass.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/0Harmony.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/UnityEngine.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/UnityEngine.CoreModule.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/UnityEngine.UI.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/Unity.TextMeshPro.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/UnityEngine.TextRenderingModule.dll deps
	cp $(ONIInstallDir)/OxygenNotIncluded_Data/Managed/UnityEngine.ImageConversionModule.dll deps

.PHONY: clean
clean:
	dotnet clean
	rm -rf bin obj Util.cs

FORCE:
