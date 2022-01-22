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

    internal class LunchCommand : Command
    {
        public LunchCommand() : base("lunch", "Cooks a lunch meal.")
        {
            var option = new Option<string>(new[] { "--meal", "-m" }, "The lunch meal to cook.")
            {
                // Besides the aliases above which can be used to specify the value, the name can also be used to specify the value
                Name = "Meal",
                // Specifies whether the option needs to be specified (Meal, --meal, -m)
                IsRequired = true,
                // Specifies whether a value needs to be specified when the option is specified
                Arity = ArgumentArity.ExactlyOne
            };

            option.FromAmong("Fruit Salad", "Yoghurt");

            AddOption(option);

            // Arguments get set based on the order that they are provided in. If multiple arguments exist, the first value sets the first argument,
            // the second value sets the second argument and so forth.
            AddArgument((new Argument<string>("Drink", "The drink to make.")).FromAmong("Juice", "Water"));
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
                _logger.LogInformation($"Making supper lunch: {Meal}");

                return await _meditor.Send(new LunchMealRequest()
                {
                    Meal = Meal,
                    Drink = Drink,
                }, context.GetCancellationToken()).ConfigureAwait(false);
            }
        }
    }
}
