using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GLTV.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IEmailSender _userService;
        private IHostingEnvironment _hostingEnvironment;
        

        public TimedHostedService(ILogger<TimedHostedService> logger, IEmailSender userService, IHostingEnvironment env)
        {
            _logger = logger;
            _userService = userService;
            _hostingEnvironment = env;
            

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
            Console.WriteLine("Development environment: not sending email notifications about new inzeraty");

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionHostedServiceExtensions
    {
        /// <summary>
        /// Add an <see cref="IHostedService"/> registration for the given type.
        /// </summary>
        /// <typeparam name="THostedService">An <see cref="IHostedService"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services)
            where THostedService : class, IHostedService
            => services.AddTransient<IHostedService, THostedService>();
    }
}

internal interface IScopedProcessingService
{
    void DoWork(object state);
}

internal class ScopedProcessingService : IScopedProcessingService
{
    private readonly ILogger _logger;
    private readonly INotificationService _userService;
    private IHostingEnvironment _hostingEnvironment;

    public ScopedProcessingService(ILogger<ScopedProcessingService> logger, INotificationService userService, IHostingEnvironment env)
    {
        _logger = logger;
        _userService = userService;
        _hostingEnvironment = env;
    }

    public void DoWork(object state)
    {
        Console.WriteLine("Scoped Processing Service is working.");
        
        if (!_hostingEnvironment.IsProduction())
        {
            _userService.SendNewInzeratyNotifications();
        }
        else
        {
            Console.WriteLine("Development environment: not sending email notifications about new inzeraty");
        }
    }
}

internal class ConsumeScopedServiceHostedService : IHostedService
{
    private readonly ILogger _logger;
    

    public ConsumeScopedServiceHostedService(IServiceProvider services,
        ILogger<ConsumeScopedServiceHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Consume Scoped Service Hosted Service is starting.");

        DoWork();
        
        return Task.CompletedTask;
    }

    private void DoWork()
    {
        Console.WriteLine("Consume Scoped Service Hosted Service is working.");

        using (var scope = Services.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IScopedProcessingService>();

            //scopedProcessingService.DoWork();
            Timer _timer = new Timer(scopedProcessingService.DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Consume Scoped Service Hosted Service is stopping.");

        return Task.CompletedTask;
    }
}