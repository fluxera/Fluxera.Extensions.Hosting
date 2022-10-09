namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	public static partial class LoggerExtensions
	{
		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Host configuration starting.")]
		public static partial void LogHostConfigurationStarting(this ILogger logger);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Running host using {HostLifetime}.")]
		public static partial void LogHostLifetime(this ILogger logger, string hostLifetime);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Host configured in {Duration} ms.")]
		public static partial void LogHostConfigurationDuration(this ILogger logger, long duration);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Critical, "Application terminated unexpectedly.")]
		public static partial void LogHostTerminatedUnexpectedly(this ILogger logger, Exception exception);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Executing service configuration {CallerMemberName}: {MethodName}")]
		internal static partial void LogServiceConfiguration(this ILogger logger, string callerMemberName, string methodName);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Error, "An error occurred trying to invoke the service configuration.")]
		internal static partial void LogServiceConfigurationError(this ILogger logger, Exception exception);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Information, "Loaded module: {Module}")]
		internal static partial void LogLoadedModule(this ILogger logger, string module);

		[DebuggerStepThrough]
		[LoggerMessage(0, LogLevel.Debug, "Initialized all modules.")]
		internal static partial void LogModulesInitialized(this ILogger logger);
	}
}
