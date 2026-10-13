using System;
using System.IO;

namespace Waylith.Utils;

public static class GamePathUtils
{
	public static string? ResolveProjectRoot()
	{
		for (string text = Path.GetDirectoryName(AppContext.BaseDirectory); text != null; text = new DirectoryInfo(text).Parent?.FullName)
		{
			if (Directory.Exists(Path.Combine(text, "Assets")))
			{
				return text;
			}
		}
		return null;
	}
}
