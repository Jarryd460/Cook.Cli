namespace Cook.Cli
{
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCancellationMessage(this IServiceCollection services)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                AnsiConsole.WriteLine("Cancelling operation...");
                e.Cancel = true;
            };

            return services;
        }
    }
}
