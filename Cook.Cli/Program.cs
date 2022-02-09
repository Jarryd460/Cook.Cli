// See https://aka.ms/new-console-template for more information

namespace Cook.Cli
{
    using Cook.Cli.Commands;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Spectre.Console;
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Help;
    using System.CommandLine.Hosting;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class Program
    {
        public static IHostEnvironment? HostEnvironment { get; private set; }

        static async Task<int> Main(string[] args)
        {
            /* Specifies the root command of the tool.
             * Some tools have only a single command which means you do not have to specify the command, the tool will execute it automatically.
             * Other tools have multiple commands and use the root command as the help command which details the commands that the tool supports when help command is executed.
            */
            var command = new EntryCommand();
            command.AddCommand(new BreakfastCommand());
            command.AddCommand(new LunchCommand());
            command.AddCommand(new SupperCommand());

            /* CommandHandler.Create was the old way of creating command handlers. It has since been changed.
             * To use the old way, you need to import System.CommandLine.NamingConventionBinder
            */
            //command.Handler = CommandHandler.Create<IHelpBuilder>(help =>
            //{
            //    help.Write(command);
            //    return 1;
            //});

            //Root command is not injected so we have to set the handler ourselves
            command.SetHandler((InvocationContext context) =>
            {
                context.ExitCode = command.Invoke("--help");
            });

            var parser = new CommandLineBuilder(command)
                .UseHost(_ => Host.CreateDefaultBuilder(args), SetupHost)
                // Internally calls .RegisterWithDotnetSuggest() and .UseSuggestDirective() which is required to work with dotnet suggest when
                // tool is not installed globally
                .UseDefaults()
                .UseHelp(CustomizeHelp)
                .UseExceptionHandler(ExceptionHandler)
                .Build();

            //builder.ParseResponseFileAs(ResponseFileHandling.ParseArgsAsSpaceSeparated);
            //builder.UseMiddleware(DefaultOptionsMiddleware);

            return await parser.InvokeAsync(args).ConfigureAwait(false);
        }

        private static void SetupHost(IHostBuilder builder)
        {
            builder.ConfigureServices((hostContext, services) =>
            {
                HostEnvironment = hostContext.HostingEnvironment;

                services.AddMediatR(Assembly.GetExecutingAssembly())
                    .AddCancellationMessage();
            })
            .UseCommandHandler<BreakfastCommand, BreakfastCommand.Handler>()
            .UseCommandHandler<LunchCommand, LunchCommand.Handler>()
            .UseCommandHandler<SupperCommand, SupperCommand.Handler>();
        }

        private static void CustomizeHelp(HelpContext context)
        {
            //Ability to customize any option/command text on the help page
            //context.HelpBuilder.CustomizeSymbol(BreakfastCommand.BreakfastOption, "my custom text for breakfast", "more custom text for breakfast");

            //Ability to add additional content to the help page
            context.HelpBuilder.CustomizeLayout(_ =>
            {
                return HelpBuilder.Default.GetLayout().Prepend(_ =>
                {
                    AnsiConsole.Write(new FigletText("Cooking Tool"));
                });
            });
        }

        private static void ExceptionHandler(Exception ex, InvocationContext context)
        {
            // Logging still needs to be figured out. The container and logger class has already been disposed of at this point in time
            //var test = context.BindingContext.GetService<IHost>();
            //var logger = test.Services.GetService<ConfigurationRoot>();

            if (ex is not OperationCanceledException && ex is not TaskCanceledException)
            {
                if (HostEnvironment.IsProduction())
                {
                    context.Console.Error.Write("Something went wrong. The support team has been notified. Please try again in a couple of minutes.");
                    //AnsiConsole.WriteLine("Something went wrong. The support team has been notified. Please try again in a couple of minutes.");
                }
                else
                {
                    // Adds colourization to exception and stacktrace
                    AnsiConsole.WriteException(ex);
                }

                //logger.LogError(ex, "Something went wrong.The support team has been notified.Please try again in a couple of minutes.");

                context.ExitCode = 1;
            }
            else
            {
                context.ExitCode = -1;
            }
        }
    }
}
