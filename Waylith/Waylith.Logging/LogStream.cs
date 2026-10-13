using Microsoft.Extensions.Logging;

namespace Waylith.Logging;

public static class LogStream
{
	internal static bool HasListeners => LogStream.MessageReceived != null;

	public static event LogStreamHandler? MessageReceived;

	internal static void Publish(LogLevel level, string category, string message, string? stackTrace)
	{
		LogStream.MessageReceived?.Invoke(level, category, message, stackTrace);
	}
}
