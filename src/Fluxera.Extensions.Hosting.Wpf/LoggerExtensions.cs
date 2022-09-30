namespace Fluxera.Extensions.Hosting
{
	using System.Diagnostics;
	using Microsoft.Extensions.Logging;

	internal static partial class LoggerExtensions
	{
		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Hosting environment: {EnvironmentName}.")]
		public static partial void LogEnvironment(this ILogger logger, string environmentName);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Content root path: {ContentRootPath}.")]
		public static partial void LogContentRootPath(this ILogger logger, string contentRootPath);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Application started.")]
		public static partial void LogApplicationStarted(this ILogger logger);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Application is stopping ...")]
		public static partial void LogApplicationStopping(this ILogger logger);
	}
}
