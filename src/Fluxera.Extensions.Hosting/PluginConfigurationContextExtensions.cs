namespace Fluxera.Extensions.Hosting
{
	using System;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using JetBrains.Annotations;

	/// <summary>
	///     Extensions methods to simplify the configuration of plugin sources.
	/// </summary>
	[PublicAPI]
	public static class PluginConfigurationContextExtensions
	{
		/// <summary>
		///     Adds all assemblies containing plugin from a folder.
		/// </summary>
		/// <param name="context">The plugin configuration context.</param>
		/// <param name="pluginAssembliesFolder">The folder to scan for plugin modules.</param>
		/// <returns></returns>
		public static IPluginConfigurationContext AddPlugins(this IPluginConfigurationContext context, string pluginAssembliesFolder)
		{
			context.PluginSources.Add(new FolderPluginSource(pluginAssembliesFolder));

			return context;
		}

		/// <summary>
		///     Adds the given plugin modules types.
		/// </summary>
		/// <param name="context">The plugin configuration context.</param>
		/// <param name="pluginModuleTypes">The plugin modules.</param>
		/// <returns></returns>
		public static IPluginConfigurationContext AddPlugins(this IPluginConfigurationContext context, params Type[] pluginModuleTypes)
		{
			context.PluginSources.Add(new PluginTypeListSource(pluginModuleTypes));

			return context;
		}

		/// <summary>
		///     Adds the plugin module of teh given type.
		/// </summary>
		/// <typeparam name="TPluginModule">The type of the plugin module.</typeparam>
		/// <param name="context">The plugin configuration context.</param>
		/// <returns></returns>
		public static IPluginConfigurationContext AddPlugin<TPluginModule>(this IPluginConfigurationContext context)
			where TPluginModule : class, IModule
		{
			return context.AddPlugin(typeof(TPluginModule));
		}

		/// <summary>
		///     Adds the plugin module of teh given type.
		/// </summary>
		/// <param name="context">The plugin configuration context.</param>
		/// <param name="pluginModuleType">The plugin module type.</param>
		/// <returns></returns>
		public static IPluginConfigurationContext AddPlugin(this IPluginConfigurationContext context, Type pluginModuleType)
		{
			context.PluginSources.Add(new PluginModuleTypeSource(pluginModuleType));

			return context;
		}
	}
}
