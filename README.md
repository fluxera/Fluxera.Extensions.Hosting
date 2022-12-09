[![Build Status](https://dev.azure.com/fluxera/Foundation/_apis/build/status/GitHub/fluxera.Fluxera.Extensions.Hosting?branchName=main&stageName=BuildAndTest)](https://dev.azure.com/fluxera/Foundation/_build/latest?definitionId=87&branchName=main)

# Fluxera.Extensions.Hosting
A library that extends the Microsoft.Extensions.Hosting library with modular host implementations 
for various application platforms.

The library uses the generic host implementation and build a modular structure upon it. It is
possible to split your application into login modules that can be shared between different types
of applications.

## Available Hosts

The modular host is available for the following applicattion types:

- ASP.NET Core
- Blazor WebAssembly
- Console / Windows Service
- WPF
- MAUI

## Usage

Every application needs a host and a startup module class. The application is composed of modules
that define dependencies on other modules and optional modules that are loaded as plugins.

```C#
public class ConsoleApplicationModule : ConfigureServicesModule
{
	public override void ConfigureServices(IServiceConfigurationContext context)
	{
		context.Services.AddHostedService<ConsoleHostedService>();
		context.Services.AddSingleton<IWeatherService, WeatherService>();
		context.Services.AddOptions<WeatherSettings>().Bind(context.Configuration.GetSection("Weather"));
	}
}
```

This startup module just configures some services and regiosters them in the service collection.
The service confiuration pipelineis split up into three steps: pre-configure-, configure- and 
post-configure-services. Each methos is executed in every module, before moving to the next method.

If you need to configure the application after the creation of the service provider you can just
use the base class ```ConfigureApplicationModule``` which provides a similar three step 
pipeline for initializing the application: pre-configure, configure and post-configure. Additionally
this base class provides a methos that is executed on every module when the application shuts down.

In addtition to the two base classes ```ConfigureServicesModule``` and ```ConfigureApplicationModule```
you are free to use one of the module interfaces to meet you configuration needs.

- IModule
  - IConfigureServicesModule
    - IPreConfigureServices
    - IConfigureServices
    - IPostConfigureServices
  - IConfigureApplicationModule
    - IPreConfigureApplication
    - IConfigureApplication
    - IPostConfigureApplication
  - IShutdownApplicationModule

You can even just implement the ```IModule``` interface on your module class, if you don't need any
configuration and application initialization.

```C#
public class ConsoleApplicationHost : ConsoleApplicationHost<ConsoleApplicationModule>
{
}
```

The simplest application host class just inherits from one of the available base classes for the
application type you are running.

Each base class provides several methods you can overide to configure additional plugin modules, or
a custom logger to use while bootstrapping the host. Please refer to the samples for more information.

```C#
public static class Program
{
	public static async Task Main(string[] args)
	{
		await ApplicationHost.RunAsync<ConsoleApplicationHost>(args);

		Console.WriteLine();
		Console.WriteLine("Press any key to quit...");
		Console.ReadKey(true);
	}
}
```

All what's left to do is to run the host using one of the available static entry-points.
