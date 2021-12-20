namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Reflection;
	using Microsoft.Extensions.Logging;

	internal static class HostingLoggerExtensions
	{
		public static void ApplicationError(this ILogger logger, EventId eventId, string message, Exception exception)
		{
			if(exception is ReflectionTypeLoadException reflectionTypeLoadException)
			{
				foreach(Exception? ex in reflectionTypeLoadException.LoaderExceptions)
				{
					message = message + Environment.NewLine + ex.Message;
				}
			}

			logger.LogCritical(eventId, message: message, exception: exception);
		}

		public static void Sleeping(this ILogger logger)
		{
			if(logger.IsEnabled(LogLevel.Debug))
			{
				logger.LogDebug(
					LoggerEventIds.Sleeping,
					"Application sleeping");
			}
		}

		public static void Resuming(this ILogger logger)
		{
			if(logger.IsEnabled(LogLevel.Debug))
			{
				logger.LogDebug(
					LoggerEventIds.Resuming,
					"Application resuming");
			}
		}

		public static void StoppedWithException(this ILogger logger, Exception ex)
		{
			if(logger.IsEnabled(LogLevel.Debug))
			{
				logger.LogDebug(
					LoggerEventIds.StoppedWithException,
					ex,
					"Hosting shutdown exception");
			}
		}
	}
}
