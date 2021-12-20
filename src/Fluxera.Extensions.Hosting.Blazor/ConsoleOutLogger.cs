namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Text;
	using Microsoft.Extensions.Logging;

	internal sealed class ConsoleOutLogger : ILogger
	{
		private const string LogLevelPadding = ": ";
		private static readonly string messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LogLevelPadding.Length);
		private static readonly string newLineWithMessagePadding = Environment.NewLine + messagePadding;
		private static readonly StringBuilder logBuilder = new StringBuilder();

		private readonly string name;

		public ConsoleOutLogger(string name)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return NoOpDisposable.Instance;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel != LogLevel.None;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			if(!this.IsEnabled(logLevel))
			{
				return;
			}

			if(formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}

			string message = formatter(state, exception);

			if(!string.IsNullOrEmpty(message) || (exception != null))
			{
				this.WriteMessage(logLevel, this.name, eventId.Id, message, exception);
			}
		}

		private void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception? exception)
		{
			lock(logBuilder)
			{
				try
				{
					this.CreateDefaultLogMessage(logBuilder, logLevel, logName, eventId, message, exception);
					string formattedMessage = logBuilder.ToString();

					switch(logLevel)
					{
						case LogLevel.Trace:
						case LogLevel.Debug:
						case LogLevel.Information:
						case LogLevel.Warning:
							// Although https://console.spec.whatwg.org/#loglevel-severity claims that
							// "console.debug" and "console.log" are synonyms, that doesn't match the
							// behavior of browsers in the real world. Chromium only displays "debug"
							// messages if you enable "Verbose" in the filter dropdown (which is off
							// by default). As such "console.debug" is the best choice for messages
							// with a lower severity level than "Information".
							//_jsRuntime.InvokeVoid("console.debug", formattedMessage);
							Console.Out.WriteLine(formattedMessage);
							break;
						case LogLevel.Error:
						case LogLevel.Critical:
							Console.Out.WriteLine(formattedMessage);
							break;
						default: // invalid enum values
							Debug.Assert(logLevel != LogLevel.None, "This method is never called with LogLevel.None.");
							Console.Out.WriteLine(formattedMessage);
							break;
					}
				}
				finally
				{
					logBuilder.Clear();
				}
			}
		}

		private void CreateDefaultLogMessage(StringBuilder logBuilder, LogLevel logLevel, string logName, int eventId, string message, Exception? exception)
		{
			logBuilder.Append(GetLogLevelString(logLevel));
			logBuilder.Append(LogLevelPadding);
			logBuilder.Append(logName);
			logBuilder.Append('[');
			logBuilder.Append(eventId);
			logBuilder.Append(']');

			if(!string.IsNullOrEmpty(message))
			{
				// message
				logBuilder.AppendLine();
				logBuilder.Append(messagePadding);

				int len = logBuilder.Length;
				logBuilder.Append(message);
				logBuilder.Replace(Environment.NewLine, newLineWithMessagePadding, len, message.Length);
			}

			// Example:
			// System.InvalidOperationException
			//    at Namespace.Class.Function() in File:line X
			if(exception != null)
			{
				// exception message
				logBuilder.AppendLine();
				logBuilder.Append(exception);
			}
		}

		private static string GetLogLevelString(LogLevel logLevel)
		{
			switch(logLevel)
			{
				case LogLevel.Trace:
					return "trce";
				case LogLevel.Debug:
					return "dbug";
				case LogLevel.Information:
					return "info";
				case LogLevel.Warning:
					return "warn";
				case LogLevel.Error:
					return "fail";
				case LogLevel.Critical:
					return "crit";
				default:
					throw new ArgumentOutOfRangeException(nameof(logLevel));
			}
		}

		private class NoOpDisposable : IDisposable
		{
			public static readonly NoOpDisposable Instance = new NoOpDisposable();

			public void Dispose()
			{
			}
		}
	}
}
