using System;
using System.Collections.Concurrent;
using System.IO;

namespace Waylith.Rendering;

public static class TextureUsageHints
{
	private static readonly ConcurrentDictionary<string, TextureUsage> Hints = new ConcurrentDictionary<string, TextureUsage>(StringComparer.OrdinalIgnoreCase);

	public static void Register(string texturePath, TextureUsage usage)
	{
		if (!string.IsNullOrEmpty(texturePath) && usage != 0)
		{
			Hints.AddOrUpdate(texturePath.Replace('\\', '/'), usage, (string _, TextureUsage existing) => (existing != usage) ? TextureUsage.ArrayLayer : usage);
		}
	}

	public static void RegisterSource(string sourcePath, TextureUsage usage)
	{
		string text = KeyOf(sourcePath);
		if (text != null)
		{
			Register(text, usage);
		}
	}

	public static void Clear()
	{
		Hints.Clear();
	}

	public static TextureUsage For(string? sourcePath)
	{
		if (sourcePath != null && !Hints.IsEmpty)
		{
			string text = KeyOf(sourcePath);
			if (text != null && Hints.TryGetValue(text, out var value))
			{
				return value;
			}
		}
		return TextureUsage.Default;
	}

	private static string? KeyOf(string sourcePath)
	{
		string text = sourcePath.Replace('\\', '/');
		int num = text.LastIndexOf("/Textures/", StringComparison.OrdinalIgnoreCase);
		if (num >= 0)
		{
			return Path.ChangeExtension(text.Substring(num + "/Textures/".Length), null);
		}
		return null;
	}
}
