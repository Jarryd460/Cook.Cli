namespace Cook.Cli.Commands
{
    using Cook.Cli;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;
    using Cook.Cli.Mediator;
    using System.Collections.Generic;

    //internal static class BreakfastCommand
    //{
    //    private static readonly Option<string> _breakfastOption = new Option<string>(new[] { "--meal", "-m" }, "The breakfast meal to cook.")
    //    {
    //        IsRequired = true,
    //        Arity = ArgumentArity.ExactlyOne
    //    };

    //    public static Command CreateBreakfastCommand()
    //    {
    //        var command = new Command("breakfast", "Cooks a breakfast meal.");

    //        //var argument = CreateBreakfastArgument();
    //        //command.AddArgument(argument);
    //        command.AddOption(BreakfastOption);

    //        //var binder = new BreakfastBinder(option);

    //        command.SetHandler((string meal, IConsole console) =>
    //        {
    //            console.Out.Write(meal);
    //        }, BreakfastOption);

    //        return command;
    //    }

    //    private static Argument<string> CreateBreakfastArgument()
    //    {
    //        return new Argument<string>()
    //        {
    //            Name = "meal",
    //            Description = "The breakfast meal to cook.",
    //            Arity = ArgumentArity.ZeroOrOne,
    //        };
    //    }

    //    // Must use Option<string> instead of Option otherwise binding fails when setting the handler
    //    internal static Option<string> BreakfastOption => _breakfastOption;
    //}

    internal class BreakfastCommand : Command
    {
        public BreakfastCommand() : base("breakfast", "Cooks a breakfast meal.")
        {
            var option = new Option<string>(new[] { "--meal", "-m" }, "The breakfast meal to cook.")
            {
                // Besides the aliases above which can be used to specify the value, the name can also be used to specify the value
                Name = "Meal",
                // Specifies whether the option needs to be specified (Meal, --meal, -m)
                IsRequired = true,
                // Specifies whether a value needs to be specified when the option is specified
                Arity = ArgumentArity.ExactlyOne
            };

            option.FromAmong("Bacon", "Omellete", "Sausage");

            AddOption(option);

            // Arguments get set based on the order that they are provided in. If multiple arguments exist, the first value sets the first argument,
            // the second value sets the second argument and so forth.
            AddArgument(new Argument<string>("Drink", "The drink to make.").FromAmong("Juice", "Protein shake"));
        }

        public new class Handler : ICommandHandler
        {
            private readonly IMediator _meditor;
            private readonly ILogger _logger;

            public string? Meal { get; init; }
            public string? Drink { get; init; }

            public Handler(IMediator meditor, ILogger<Program> logger)
            {
                _meditor = meditor ?? throw new ArgumentNullException(nameof(meditor));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                _logger.LogInformation($"Making breakfast meal: {Meal}");

                return await _meditor.Send(new BreakfastMealRequest()
                {
                    Meal = Meal,
                    Drink = Drink
                }, context.GetCancellationToken()).ConfigureAwait(false);
            }
        }
    }
}
