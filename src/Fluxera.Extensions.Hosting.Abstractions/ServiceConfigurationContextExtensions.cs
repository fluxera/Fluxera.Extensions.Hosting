namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public static class ServiceConfigurationContextExtensions
	{
		public static void Log(this IServiceConfigurationContext context,
			Expression<Func<IServiceCollection, IServiceCollection>> addExpression,
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
				addExpression.Compile().Invoke(context.Services);
			});
		}

		public static void Log(this IServiceConfigurationContext context,
			Expression<Action<IServiceCollection>> addExpression,
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
				addExpression.Compile().Invoke(context.Services);
			});
		}

		public static T Log<T>(this IServiceConfigurationContext context,
			Expression<Func<IServiceCollection, T>> addExpression,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addExpression, nameof(addExpression));

			MethodCallExpression methodCallExpression = addExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			return ExecuteTryCatch(context.Logger, () => addExpression.Compile().Invoke(context.Services));
		}

		public static void Log(this IServiceConfigurationContext context,
			string methodName,
			Action<IServiceCollection> addFunction,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addFunction, nameof(addFunction));

			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				addFunction.Invoke(context.Services);
			});
		}

		public static T Log<T>(this IServiceConfigurationContext context,
			string methodName,
			Func<IServiceCollection, T> addFunction,
			[CallerMemberName] string callerMemberName = null!)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(addFunction, nameof(addFunction));

			context.Logger.LogDebug($"{callerMemberName}: {methodName}");

			return ExecuteTryCatch(context.Logger, () => addFunction.Invoke(context.Services));
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
