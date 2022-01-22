namespace Cook.Cli.Mediator
{
    using MediatR;
using System.CommandLine;
    using System.Threading;
    using System.Threading.Tasks;

    internal class LunchMealRequest : IRequest<int>
    {
        public string? Meal { get; init; }
        public string? Drink { get; init; }
    }

    internal class LunchMealRequestHandler : IRequestHandler<LunchMealRequest, int>
    {
        private readonly IConsole _console;

        public LunchMealRequestHandler(IConsole console)
        {
            _console = console;
        }

        public Task<int> Handle(LunchMealRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string drink = string.Empty;

            if (!string.IsNullOrEmpty(request.Drink))
            {
                drink = $" with {request.Drink}";
            }

            _console.WriteLine($"Your {request.Meal} lunch meal is complete{drink}.");

            return Task.FromResult(0);
        }
    }
}
