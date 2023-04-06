using Quartz;
using WindowsService_Net6Worker_Quartz;
using Serilog;
using Serilog.Events;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();

//var host = CreateHostBuilder(args).Build();
//await host.RunAsync();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(@"C:\\Temp\\StartupLog.txt")
    //.CreateLogger();
    .CreateBootstrapLogger();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog((context, services, configuration) => configuration
            //.WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            //.Enrich.FromLogContext()
            )
        .ConfigureServices((hostContext, services) =>
        {
            ConfigureQuartzService(services, hostContext);

            services.AddScoped<ITaskLogTime, TaskLogTime>();
        });

try
{
    Log.Information("Starting up the service");
    var host = CreateHostBuilder(args).Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the serivce");
    throw;
}
finally
{
    Log.Information("Service successfully stopped");
    Log.CloseAndFlush();
}

static void ConfigureQuartzService(IServiceCollection services, HostBuilderContext hostContext)
{
    // Add the required Quartz.NET services
    services.AddQuartz(q =>
    {
        // Use a Scoped container to create jobs.
        q.UseMicrosoftDependencyInjectionJobFactory();

        //// Create a "key" for the job
        //var jobKey = new JobKey("Task1");

        //// Register the job with the DI container
        //q.AddJob<TaskInfo>(opts => opts.WithIdentity(jobKey));

        //// Create a trigger for the job
        //q.AddTrigger(opts => opts
        //    .ForJob(jobKey) // link to the Task1
        //    .WithIdentity("Task1-trigger") // give the trigger a unique name
        //    .WithCronSchedule("0/10 * * * * ?")); // run every 10 seconds

        q.AddJobAndTrigger<TaskInfo>(hostContext.Configuration);
    });

    // Add the Quartz.NET hosted service
    services.AddQuartzHostedService(
        q => q.WaitForJobsToComplete = true);
}