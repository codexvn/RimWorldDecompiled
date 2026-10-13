using System;
using System.IO;

namespace Waylith.UnityCompat;

public static class UnityUtils
{
	public static string? ResolveUnityProjectRoot()
	{
		for (string text = Path.GetDirectoryName(Environment.ProcessPath); text != null; text = new DirectoryInfo(text).Parent?.FullName)
		{
			if (Directory.Exists(Path.Combine(text, "Assets")))
			{
				return text;
			}
		}
		return null;
	}
}
