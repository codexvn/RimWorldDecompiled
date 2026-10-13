using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.Rendering;

public static class ShaderRegistry
{
	private static readonly ConcurrentDictionary<string, Shader> ShadersByKey;

	private static readonly ConcurrentDictionary<string, Shader> ShadersByPath;

	private static readonly ConcurrentDictionary<string, byte> WarnedFallbackKeys;

	public static readonly Shader Default;

	public static IReadOnlyList<Shader> AllShaders
	{
		get
		{
			List<Shader> list = new List<Shader>(ShadersByKey.Count);
			foreach (Shader value in ShadersByKey.Values)
			{
				list.Add(value);
			}
			return list;
		}
	}

	static ShaderRegistry()
	{
		ShadersByKey = new ConcurrentDictionary<string, Shader>(StringComparer.Ordinal);
		ShadersByPath = new ConcurrentDictionary<string, Shader>(StringComparer.OrdinalIgnoreCase);
		WarnedFallbackKeys = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
		Default = new Shader("scene-default", "Map/Default", new ShaderProgramDefinition(), null, null, null);
		Register(Default);
	}

	public static Shader Register(Shader shader)
	{
		ArgumentNullException.ThrowIfNull(shader, "shader");
		ShadersByKey[shader.Key] = shader;
		if (!string.IsNullOrWhiteSpace(shader.ShaderPath))
		{
			ShadersByPath[shader.ShaderPath] = shader;
		}
		return shader;
	}

	public static bool TryResolve(string keyOrPath, [NotNullWhen(true)] out Shader? shader)
	{
		if (ShadersByKey.TryGetValue(keyOrPath, out shader))
		{
			return true;
		}
		return ShadersByPath.TryGetValue(NormalizePath(keyOrPath), out shader);
	}

	public static Shader Resolve(string keyOrPath)
	{
		if (TryResolve(keyOrPath, out Shader shader))
		{
			return shader;
		}
		if (WarnedFallbackKeys.TryAdd(keyOrPath, 0))
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(66, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("ShaderRegistry: shader '");
				message.AppendFormatted(keyOrPath);
				message.AppendLiteral("' not registered, falling back to default!");
			}
			log.Warning(message, "Resolve", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderRegistry.cs", 63);
		}
		return Default;
	}

	public static void Clear()
	{
		ShadersByKey.Clear();
		ShadersByPath.Clear();
		WarnedFallbackKeys.Clear();
		ShaderSourceLoader.ClearLayoutCaches();
		Register(Default);
	}

	public static string NormalizePath(string shaderPath)
	{
		if (string.IsNullOrWhiteSpace(shaderPath))
		{
			return string.Empty;
		}
		string text = shaderPath.Replace('\\', '/').Trim();
		if (text.StartsWith("Shaders/", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring("Shaders/".Length);
		}
		int num = text.LastIndexOf('.');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		return text;
	}
}
