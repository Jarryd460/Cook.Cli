namespace Cook.Cli.Mediator
{
    using MediatR;
    using System.CommandLine;
    using System.Threading;
    using System.Threading.Tasks;

    internal class SupperMealRequest : IRequest<int>
    {
        public string? Meal { get; init; }
        public string? Drink { get; init; }
    }

    internal class SupperMealRequestHandler : IRequestHandler<BreakfastMealRequest, int>
    {
        private readonly IConsole _console;

        public SupperMealRequestHandler(IConsole console)
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

            _console.WriteLine($"Your {request.Meal} supper meal is complete{drink}.");

            return Task.FromResult(0);
        }
    }
}
