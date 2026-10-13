using Microsoft.Extensions.Logging;

namespace Waylith.Logging;

public delegate void LogStreamHandler(LogLevel level, string category, string message, string? stackTrace);
