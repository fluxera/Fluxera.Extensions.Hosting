namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

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

		/// <summary>
		///     Logs the plugin configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="addExpression"></param>
		/// <param name="callerMemberName"></param>
		public static void Log(this IPluginConfigurationContext context,
			Expression<Func<IPluginSourceList, IPluginSourceList>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = (addExpression.Body as MethodCallExpression)!;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addExpression.Compile().Invoke(context.PluginSources);
			});
		}

		/// <summary>
		///     Logs the plugin configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="addExpression"></param>
		/// <param name="callerMemberName"></param>
		public static void Log(this IPluginConfigurationContext context,
			Expression<Action<IPluginSourceList>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = (addExpression.Body as MethodCallExpression)!;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addExpression.Compile().Invoke(context.PluginSources);
			});
		}

		/// <summary>
		///     Logs the plugin configuration action.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="addExpression"></param>
		/// <param name="callerMemberName"></param>
		/// <returns></returns>
		public static T Log<T>(this IPluginConfigurationContext context,
			Expression<Func<IPluginSourceList, T>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = (addExpression.Body as MethodCallExpression)!;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			return ExecuteTryCatch(context.Logger, () => addExpression.Compile().Invoke(context.PluginSources));
		}

		/// <summary>
		///     Logs the plugin configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="methodName"></param>
		/// <param name="addFunction"></param>
		/// <param name="callerMemberName"></param>
		public static void Log(this IPluginConfigurationContext context,
			string methodName,
			Action<IPluginSourceList> addFunction,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addFunction, nameof(addFunction));

			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addFunction.Invoke(context.PluginSources);
			});
		}

		/// <summary>
		///     Logs the plugin configuration action.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="methodName"></param>
		/// <param name="addFunction"></param>
		/// <param name="callerMemberName"></param>
		/// <returns></returns>
		public static T Log<T>(this IPluginConfigurationContext context,
			string methodName,
			Func<IPluginSourceList, T> addFunction,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addFunction, nameof(addFunction));

			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			return ExecuteTryCatch(context.Logger, () => addFunction.Invoke(context.PluginSources));
		}

		private static void ExecuteTryCatch(ILogger logger, Action action)
		{
			try
			{
				action.Invoke();
			}
			catch(Exception ex)
			{
				logger.LogCritical(ex, ex.Message);
				throw;
			}
		}

		private static T ExecuteTryCatch<T>(ILogger logger, Func<T> func)
		{
			try
			{
				return func.Invoke();
			}
			catch(Exception ex)
			{
				logger.LogCritical(ex, ex.Message);
				throw;
			}
		}
	}
}
