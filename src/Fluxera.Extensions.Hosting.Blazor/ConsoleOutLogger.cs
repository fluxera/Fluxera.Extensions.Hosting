﻿namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Diagnostics;
	using System.Text;
	using Microsoft.Extensions.Logging;

	internal sealed class ConsoleOutLogger : ILogger
	{
		private const string LogLevelPadding = ": ";
		private static readonly string MessagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LogLevelPadding.Length);
		private static readonly string NewLineWithMessagePadding = Environment.NewLine + MessagePadding;
		private static readonly StringBuilder LogBuilder = new StringBuilder();

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

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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
				WriteMessage(logLevel, this.name, eventId.Id, message, exception);
			}
		}

		private static void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
		{
			lock(LogBuilder)
			{
				try
				{
					CreateDefaultLogMessage(LogBuilder, logLevel, logName, eventId, message, exception);
					string formattedMessage = LogBuilder.ToString();

					switch(logLevel)
					{
						case LogLevel.Trace:
						case LogLevel.Debug:
						case LogLevel.Information:
						case LogLevel.Warning:
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
					LogBuilder.Clear();
				}
			}
		}

		private static void CreateDefaultLogMessage(StringBuilder stringBuilder, LogLevel logLevel, string logName, int eventId, string message, Exception exception)
		{
			stringBuilder.Append(GetLogLevelString(logLevel));
			stringBuilder.Append(LogLevelPadding);
			stringBuilder.Append(logName);
			stringBuilder.Append('[');
			stringBuilder.Append(eventId);
			stringBuilder.Append(']');

			if(!string.IsNullOrEmpty(message))
			{
				// message
				stringBuilder.AppendLine();
				stringBuilder.Append(MessagePadding);

				int len = stringBuilder.Length;
				stringBuilder.Append(message);
				stringBuilder.Replace(Environment.NewLine, NewLineWithMessagePadding, len, message.Length);
			}

			// Example:
			// System.InvalidOperationException
			//    at Namespace.Class.Function() in File:line X
			if(exception != null)
			{
				// exception message
				stringBuilder.AppendLine();
				stringBuilder.Append(exception);
			}
		}

		private static string GetLogLevelString(LogLevel logLevel)
		{
			// ReSharper disable StringLiteralTypo
			return logLevel switch
			{
				LogLevel.Trace => "trce",
				LogLevel.Debug => "dbug",
				LogLevel.Information => "info",
				LogLevel.Warning => "warn",
				LogLevel.Error => "fail",
				LogLevel.Critical => "crit",
				LogLevel.None => string.Empty,
				_ => throw new ArgumentOutOfRangeException(nameof(logLevel))
			};
			// ReSharper restore StringLiteralTypo
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
