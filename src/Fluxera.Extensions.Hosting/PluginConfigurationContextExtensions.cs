namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;
	using Fluxera.Extensions.Hosting.Plugins;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public static class PluginConfigurationContextExtensions
	{
		public static IPluginConfigurationContext AddPlugins(this IPluginConfigurationContext context,
			string pluginAssembliesFolder)
		{
			context.PluginSources.Add(new FolderPluginSource(pluginAssembliesFolder));

			return context;
		}

		public static IPluginConfigurationContext AddPlugins(this IPluginConfigurationContext context,
			params Type[] pluginModuleTypes)
		{
			context.PluginSources.Add(new PluginTypeListSource(pluginModuleTypes));

			return context;
		}

		public static IPluginConfigurationContext AddPlugin<TPluginModule>(this IPluginConfigurationContext context)
			where TPluginModule : class, IModule
		{
			return context.AddPlugin(typeof(TPluginModule));
		}

		public static IPluginConfigurationContext AddPlugin(this IPluginConfigurationContext context,
			Type pluginModuleType)
		{
			context.PluginSources.Add(new PluginModuleTypeSource(pluginModuleType));

			return context;
		}

		public static void Log(this IPluginConfigurationContext context,
			Expression<Func<IPluginSourceList, IPluginSourceList>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = addExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addExpression.Compile().Invoke(context.PluginSources);
			});
		}

		public static void Log(this IPluginConfigurationContext context,
			Expression<Action<IPluginSourceList>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = addExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addExpression.Compile().Invoke(context.PluginSources);
			});
		}

		public static T Log<T>(this IPluginConfigurationContext context,
			Expression<Func<IPluginSourceList, T>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = addExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			return ExecuteTryCatch(context.Logger, () => addExpression.Compile().Invoke(context.PluginSources));
		}

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
