export DOTNET_CLI_TELEMETRY_OPTOUT=1
export ONIInstallDir ?= "$(HOME)/.steam/steam/SteamApps/common/OxygenNotIncluded"
export ONIPlayerDir ?= "$(HOME)/.config/unity3d/Klei/Oxygen Not Included"
export ONIModName = BeyondExtraHard
export ONIModDLL = $(ONIModName).dll
export ONIModMergedDLL = $(ONIModName)Merged.dll
export ModVer = $(shell echo $(shell grep version mod_info.yaml | tail -1 | cut -f2 -d:)-$(shell git rev-parse --short HEAD))

.PHONY: build
build: bin/Debug/$(ONIModMergedDLL) bin/Release/$(ONIModMergedDLL)

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
export ILREPACK = dotnet tool run ilrepack --
export NET481_REF_DIR = $(HOME)/.nuget/packages/microsoft.netframework.referenceassemblies.net481/1.0.3/build/.NETFramework/v4.8.1
bin/Debug/$(ONIModMergedDLL): bin/Debug/$(ONIModDLL)
	$(ILREPACK) /lib:bin/Debug /ndebug /out:bin/Debug/$(ONIModMergedDLL) bin/Debug/$(ONIModDLL) bin/Debug/PLib.dll bin/Debug/Newtonsoft.Json.dll /targetplatform:v4,$(NET481_REF_DIR)

bin/Release/$(ONIModMergedDLL): bin/Release/$(ONIModDLL)
	$(ILREPACK) /lib:bin/Release /ndebug /out:bin/Release/$(ONIModMergedDLL) bin/Release/$(ONIModDLL) bin/Release/PLib.dll bin/Release/Newtonsoft.Json.dll /targetplatform:v4,$(NET481_REF_DIR)

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
	cp -r bin/Debug/$(ONIModMergedDLL) bin/Debug/mod_info.yaml bin/Debug/mod.yaml "$(ONIPlayerDir)/mods/Local/$(ONIModName)"

.PHONY: log
log:
	cat "$(ONIPlayerDir)/Player.log"

.PHONY: logf
logf:
	tail -f "$(ONIPlayerDir)/Player.log"

Player.log:
	ln -s "$(ONIPlayerDir)/Player.log" .

deps: dirscheck FORCE
	dotnet add package Microsoft.NETFramework.ReferenceAssemblies.net481 --version 1.0.3
	# Keep ILRepack as a local dotnet tool (cross-platform) instead of the legacy .exe.
	# (See .config/dotnet-tools.json)
	dotnet add package PLib --version 4.20.0.0
	dotnet add package Newtonsoft.Json --version 13.0.2
	dotnet tool restore --tool-manifest .config/dotnet-tools.json
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
