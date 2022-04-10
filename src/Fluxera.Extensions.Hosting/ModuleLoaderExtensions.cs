namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.DependencyInjection;

	internal static class ModuleLoaderExtensions
	{
		private const string PreviousModuleKey = "PreviousModule";

		public static void ConfigureServices(this IReadOnlyCollection<IModuleDescriptor> modules, IServiceCollection services)
		{
			ServiceConfigurationContext context = new ServiceConfigurationContext(services);

			// PreConfigureServices
			context.Items.Remove(PreviousModuleKey);
			foreach(IModuleDescriptor module in modules.Where(m => m.Instance is IPreConfigureServices))
			{
				((IPreConfigureServices)module.Instance).PreConfigureServices(context);
				context.Items[PreviousModuleKey] = module.Type.FullName;
			}

			// ConfigureServices
			context.Items.Remove(PreviousModuleKey);
			foreach(IModuleDescriptor module in modules.Where(m => m.Instance is IConfigureServices))
			{
				((IConfigureServices)module.Instance).ConfigureServices(context);
				context.Items[PreviousModuleKey] = module.Type.FullName;
			}

			// PostConfigureServices
			context.Items.Remove(PreviousModuleKey);
			foreach(IModuleDescriptor module in modules.Where(m => m.Instance is IPostConfigureServices))
			{
				((IPostConfigureServices)module.Instance).PostConfigureServices(context);
				context.Items[PreviousModuleKey] = module.Type.FullName;
			}

			// Remove all "ConfigureServices" object accessor instances, because they are only needed for configuring services.
			IList<ServiceDescriptor> serviceDescriptors = services
				.Where(x => x.ServiceType.IsAssignableTo<IObjectAccessor>())
				.Where(x => (x.ImplementationInstance != null) && (((IObjectAccessor)x.ImplementationInstance).Context == ObjectAccessorLifetime.ConfigureServices))
				.ToList();
			foreach(ServiceDescriptor serviceDescriptor in serviceDescriptors)
			{
				services.Remove(serviceDescriptor);
				if(serviceDescriptor != null)
				{
					IObjectAccessor accessor = serviceDescriptor.ImplementationInstance as IObjectAccessor;
					accessor?.Dispose();
				}
			}
		}
	}
}
