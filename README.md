# UpdateSpreadsheets Cli Tool

###Description

A cli tool for odering meals for breakfast, lunch and supper.

### Dependencies:

* System.CommandLine - https://github.com/dotnet/command-line-api
* System.CommandLine.NamingConventionBinder (used if deprecated functionality like CommandHandler.Create is needed) - https://github.com/dotnet/command-line-api
* Spectre.Console - https://github.com/spectreconsole/spectre.console
* Microsoft.Extensions.DependencyInjection - https://github.com/dotnet/runtime

### Cook.Cli Tool

* To package and install the cli took go to the solution directory and run "dotnet pack". This should create a nuget package in /nupkg in /cook.cli project directory.
* After the package has been created, you can install the tool by running "dotnet tool install --global --add-source ./nupkg Cook.Cli" in the /cook.cli project directory.
* To uninstall the tool run "dotnet tool uninstall --global cook.cli"