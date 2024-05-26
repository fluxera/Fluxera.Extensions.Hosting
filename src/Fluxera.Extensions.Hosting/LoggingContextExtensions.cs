// ReSharper disable PossibleNullReferenceException

namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extension methods to support easier logging in service configuration methods of modules.
	/// </summary>
	[PublicAPI]
	public static class LoggingContextExtensions
	{
		/// <summary>
		///     Logs the service configuration action.
		/// </summary>
		public static void Log<T>(this ILoggingContext<T> context, Expression<Func<T, T>> expression, [CallerMemberName] string callerMemberName = null)
			where T : class
		{
			Guard.ThrowIfNull(context);
			Guard.ThrowIfNull(expression);

			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			Guard.ThrowIfNull(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogServiceConfiguration(callerMemberName, methodName);

			ExecuteTryCatch(context.Logger, () =>
			{
				expression.Compile().Invoke(context.LogContextData);
			});
		}

		/// <summary>
		///     Logs the service configuration action.
		/// </summary>
		public static void Log<T>(this ILoggingContext<T> context, Expression<Action<T>> expression, [CallerMemberName] string callerMemberName = null)
			where T : class
		{
			Guard.ThrowIfNull(context);
			Guard.ThrowIfNull(expression);

			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			Guard.ThrowIfNull(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogServiceConfiguration(callerMemberName, methodName);

			ExecuteTryCatch(context.Logger, () =>
			{
				expression.Compile().Invoke(context.LogContextData);
			});
		}

		/// <summary>
		///     Logs the service configuration action.
		/// </summary>
		public static void Log<T>(this ILoggingContext<T> context, string methodName, Action<T> function, [CallerMemberName] string callerMemberName = null)
			where T : class
		{
			Guard.ThrowIfNull(context);
			Guard.ThrowIfNull(function);

			context.Logger.LogServiceConfiguration(callerMemberName, methodName);

			ExecuteTryCatch(context.Logger, () =>
			{
				function.Invoke(context.LogContextData);
			});
		}

		/// <summary>
		///     Logs the service configuration action.
		/// </summary>
		public static TResult Log<T, TResult>(this ILoggingContext<T> context, Expression<Func<T, TResult>> expression, [CallerMemberName] string callerMemberName = null)
			where T : class
		{
			Guard.ThrowIfNull(context, nameof(context));
			Guard.ThrowIfNull(expression, nameof(expression));

			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			Guard.ThrowIfNull(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogServiceConfiguration(callerMemberName, methodName);

			return ExecuteTryCatch(context.Logger, () => expression.Compile().Invoke(context.LogContextData));
		}

		/// <summary>
		///     Logs the service configuration action.
		/// </summary>
		/// <returns></returns>
		public static TResult Log<T, TResult>(this ILoggingContext<T> context, string methodName, Func<T, TResult> function, [CallerMemberName] string callerMemberName = null)
			where T : class
		{
			Guard.ThrowIfNull(context, nameof(context));
			Guard.ThrowIfNull(function, nameof(function));

			context.Logger.LogServiceConfiguration(callerMemberName, methodName);

			return ExecuteTryCatch(context.Logger, () => function.Invoke(context.LogContextData));
		}

		private static void ExecuteTryCatch(ILogger logger, Action action)
		{
			try
			{
				action.Invoke();
			}
			catch(Exception ex)
			{
				logger.LogServiceConfigurationError(ex);
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
				logger.LogServiceConfigurationError(ex);
				throw;
			}
		}
	}
}
