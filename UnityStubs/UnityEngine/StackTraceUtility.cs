using System;
using System.Diagnostics;

namespace UnityEngine;

public static class StackTraceUtility
{
	public static string ExtractStackTrace()
	{
		return Environment.StackTrace;
	}

	public static string ExtractFormattedStackTrace(StackTrace stackTrace)
	{
		return stackTrace?.ToString() ?? string.Empty;
	}

	public static string ExtractStringFromException(object exception)
	{
		if (!(exception is Exception ex))
		{
			throw new ArgumentException("ExtractStringFromException requires an Exception", "exception");
		}
		return $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
	}
}
