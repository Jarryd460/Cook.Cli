namespace Cook.Cli.Mediator
{
    using MediatR;
    using System.CommandLine;
    using System.Threading;
    using System.Threading.Tasks;

    internal class BreakfastMealRequest : IRequest<int>
    {
        public string? Meal { get; init; }
        public string? Drink { get; init; }
    }

    internal class BreakfastMealRequestHandler : IRequestHandler<BreakfastMealRequest, int>
    {
        private readonly IConsole _console;

        public BreakfastMealRequestHandler(IConsole console)
        {
            _console = console;
        }

        public Task<int> Handle(BreakfastMealRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string drink = string.Empty;

            if (!string.IsNullOrEmpty(request.Drink))
            {
                drink = $" with {request.Drink}";
            }

            _console.WriteLine($"Your {request.Meal} breakfast meal is complete{drink}.");

            return Task.FromResult(0);
        }
    }
}
