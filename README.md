# UpdateSpreadsheets Cli Tool

### Description

A cli tool for odering meals for breakfast, lunch and supper.

### Dependencies:

* System.CommandLine - https://github.com/dotnet/command-line-api
* System.CommandLine.NamingConventionBinder (used if deprecated functionality like CommandHandler.Create is needed) - https://github.com/dotnet/command-line-api
* Spectre.Console - https://github.com/spectreconsole/spectre.console
* Microsoft.Extensions.DependencyInjection - https://github.com/dotnet/runtime
* Dotnet-Suggest - A dotnet tool that suggests commands when a dotnet cli tool is built with System.CommandLine

### Cook.Cli Tool

* To package and install the cli took go to the solution directory and run "dotnet pack". This should create a nuget package in /nupkg in /cook.cli project directory.
* After the package has been created, you can install the tool by running "dotnet tool install --global --add-source ./nupkg Cook.Cli" in the /cook.cli project directory.
* To uninstall the tool run "dotnet tool uninstall --global cook.cli"

### Dotnet-Suggest

* To get dotnet-suggest working, one has to install the tool by running "dotnet tool install -g dotnet-suggest".
	* If you have nuget sources that require authentication but are not actually needed to install dotnet-suggest, you can add the flag "--ignore-failed-sources"
* Once installed, run "$profile" to find the location of your shell configuration file.
	* Go to the location and if not already created, create a file called Microsoft.Powershell_profile.ps1 and copy the contents of dotnet-suggest-shim.ps1.
* Run your dotnet cli tool for the first time. You might need to execute a command or just execute the help command.
* Once your dotnet cli tool has run, you should retype the toolname in the command line and then press "ctrl + space" which should bring up suggestions of what should be specified next.
* RegisterWithDotnetSuggest()
	* As the name implies, this line will register our application with dotnet-suggest. Applications that are .NET Global Tools will be automatically discovered (by nature of being in the .NET Global Tool installation directory), but this line is needed when running our own binaries elsewhere on the filesystem.
	* Registration happens by writing to the ~/.dotnet-suggest-registration.txt file. This file is simply a list of executables and their paths. It’s read by the code snippet we put in our shell profile, so dotnet-suggest doesn’t try to autocomplete every application on our system; only the ones that actually support it.
	* This registration only happens once; when registration is complete a file will be written to our filesystem, and future registrations will be skipped if this file already exists. On Windows, this file is in ~/AppData/Local/Temp/system-commandline-sentinel-files. More generally, it’s in the path returned by Path.GetTempPath().
* UseSuggestDirective()
	* This function allows dotnet-suggest to query our application for available commandline options. dotnet-suggest will send queries to our application as special command line parameters, and our application responds by writing to stdout (i.e. it uses Console.WriteLine).
	* We can see how this works by pretending to be dotnet-suggest and sending our own command line parameter queries. We’ll use what System.CommandLine calls a "directive" which is just a keyword surrounded by square brackets, used as in-band signalling:

### References

* https://fuqua.io/blog/2021/09/enabling-command-line-completions-with-dotnet-suggest/