namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public static class ApplicationShutdownContextExtensions
	{
		public static void Log(this IApplicationShutdownContext context,
			Expression<Func<IServiceProvider, IServiceProvider>> useExpression)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useExpression, nameof(useExpression));

			MethodCallExpression methodCallExpression = useExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useExpression.Compile().Invoke(context.ServiceProvider);
			});
		}

		public static void Log(this IApplicationShutdownContext context,
			Expression<Action<IServiceProvider>> useExpression)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useExpression, nameof(useExpression));

			MethodCallExpression methodCallExpression = useExpression.Body as MethodCallExpression;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useExpression.Compile().Invoke(context.ServiceProvider);
			});
		}

		public static void Log(this IApplicationShutdownContext context,
			string methodName,
			Action<IServiceProvider> useFunction)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useFunction, nameof(useFunction));

			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useFunction.Invoke(context.ServiceProvider);
			});
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
	}
}
