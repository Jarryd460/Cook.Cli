namespace Cook.Cli.Commands
{
    using Cook.Cli.Mediator;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.CommandLine.Invocation;
    using System.CommandLine;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SupperCommand : Command
    {
        public SupperCommand() : base("supper", "Cooks a supper meal.")
        {
            var option = new Option<string>(new[] { "--meal", "-m" }, "The supper meal to cook.")
            {
                // Besides the aliases above which can be used to specify the value, the name can also be used to specify the value
                Name = "Meal",
                // Specifies whether the option needs to be specified (Meal, --meal, -m)
                IsRequired = true,
                // Specifies whether a value needs to be specified when the option is specified
                Arity = ArgumentArity.ExactlyOne
            };

            option.FromAmong("Steak", "Chicken", "Fish");

            AddOption(option);

            // Arguments get set based on the order that they are provided in. If multiple arguments exist, the first value sets the first argument,
            // the second value sets the second argument and so forth.
            AddArgument(new Argument<string>("Drink", "The drink to make.").FromAmong("Beer", "Vodka", "Red wine", "White wine"));
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
                _logger.LogInformation($"Making supper meal: {Meal}");

                return await _meditor.Send(new SupperMealRequest()
                {
                    Meal = Meal,
                    Drink = Drink,
                }, context.GetCancellationToken()).ConfigureAwait(false);
            }
        }
    }
}
