using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Replaceator
{
    static class Program
    {
        private static IServiceProvider _serviceProvider;

        private static async Task Main(string[] args)
        {
            RegisterServices(args);

            var service = _serviceProvider.GetService<IReplaceator>();
            if (service != null)
            {
                await service.Generate().ConfigureAwait(false);
            }

            DisposeServices();
        }

        private static void RegisterServices(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ILogger, Logger>();
            collection.AddScoped<IReplaceator>(
                x =>
                {
                    var options = x.GetRequiredService<IReplaceatorOptions>().Parse(args);
                    if (options != null)
                    {
                        return new Replaceator(options, x.GetRequiredService<ILogger>());
                    }
                    else
                    {
                        return null;
                    }
                });

            collection.AddScoped<IReplaceatorOptions, ReplaceatorOptions>();
            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }

        }
    }
}
