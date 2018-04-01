# CreateGUIDVSPlugin

Create, Insert, Renew GUID Visual Studio Plugin

[![Build status](https://ci.appveyor.com/api/projects/status/6ms7dwj512lhjiv2/branch/master?svg=true)](https://ci.appveyor.com/project/MasaruTsuchiyama/createguidvsplugin/branch/master)

* This is a plugin for Visual Studio which creates, inserts, renew GUIDs.
* The GUID format can be customizable.
* The solution and project file are for Visual Studio 2017.
* It works on Visual Studio 2015 and 2017.
* Insert GUID to current position on the active document.
* Copy GUID
* Renew GUID in selected text in the active document.

## Get Source Code

	> git clone https://github.com/m-tmatma/CreateGUIDVSPlugin.git

## Build

### Build by IDE

1. Open CreateGUIDVSPlugin.sln with Visual Studio 2017
1. Compile Solution

### Build by Jenkins

1. "C:\Program Files (x86)\NuGet\nuget.exe" restore CreateGUIDVSPlugin.sln
1. "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.com" CreateGUIDVSPlugin.sln /rebuild "Release|Any CPU"
