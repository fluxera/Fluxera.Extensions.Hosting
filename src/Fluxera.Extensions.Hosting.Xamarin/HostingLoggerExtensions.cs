namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq;
	using System.Reflection;
	using Microsoft.Extensions.Logging;

	internal static class HostingLoggerExtensions
	{
		public static void LogCriticalEx(this ILogger logger, string message, Exception exception)
		{
			if(exception is ReflectionTypeLoadException reflectionTypeLoadException)
			{
				message = reflectionTypeLoadException.LoaderExceptions.Aggregate(message, (current, ex) => current + Environment.NewLine + ex.Message);
			}

			logger.LogCritical(message, exception);
		}
	}
}
