using Microsoft.Extensions.DependencyInjection;

string url = "https://localhost:4200//";
Uri uri = new Uri(url);

IServiceCollection services = new ServiceCollection();

services.AddDemoService(o => o.IsActivated = true);

IServiceProvider serviceProvider = services.BuildServiceProvider();

IDemoService service = serviceProvider.GetRequiredService<IDemoService>();
service.DoSomething();


interface IDemoService
{
    void DoSomething();
}

class DemoService : IDemoService
{
    public bool Activated { get; internal set; }

    public void DoSomething()
    {
        if(Activated)
            Console.WriteLine("I do something...");
        else
            Console.WriteLine("The service is not activated...");
    }
}

class DemoServiceOptionsBuilder
{
    public bool IsActivated { get; set; }
}

static class IServicesCollectionExtensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddDemoService(Action<DemoServiceOptionsBuilder>? optionsBuilder = null)
        {
            DemoServiceOptionsBuilder builder = new DemoServiceOptionsBuilder();

            if (optionsBuilder is not null)
                optionsBuilder(builder);

            services.AddSingleton<IDemoService>(sp => new DemoService() { Activated = builder.IsActivated });

            return services;
        }
    }
}