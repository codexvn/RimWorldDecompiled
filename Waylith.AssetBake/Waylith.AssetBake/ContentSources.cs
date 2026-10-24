using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Waylith.AssetBake;

internal static class ContentSources
{
	private static readonly string[] Extensions = new string[6] { ".png", ".jpg", ".jpeg", ".tga", ".psd", ".obj" };

	private static readonly string[] PsdShadowingExtensions = new string[6] { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".dds" };

	public static readonly string[] CompressedExtensions = new string[8] { ".ogg", ".mp3", ".opus", ".jpg", ".jpeg", ".png", ".zip", ".zst" };

	private static readonly bool[] Straight = new bool[1];

	private static readonly bool[] Premultiplied = new bool[1] { true };

	public static List<string> Enumerate(IEnumerable<string> roots)
	{
		EnumerationOptions enumerationOptions = new EnumerationOptions
		{
			RecurseSubdirectories = true,
			AttributesToSkip = FileAttributes.ReparsePoint
		};
		List<string> list = new List<string>();
		foreach (string root in roots)
		{
			foreach (string item in Directory.EnumerateFiles(root, "*", enumerationOptions))
			{
				list.Add(Path.GetFullPath(item));
			}
		}
		return list;
	}

	public static List<Source> Select(IReadOnlyCollection<string> files, IReadOnlyList<string> premultipliedPaths)
	{
		HashSet<string> present = new HashSet<string>(files, StringComparer.Ordinal);
		List<Source> list = new List<Source>();
		foreach (string path in files)
		{
			string text = Path.GetExtension(path).ToLowerInvariant();
			if (((ReadOnlySpan<string>)Extensions).Contains(text) && (!(text == ".psd") || !PsdShadowingExtensions.Any((string sibling) => present.Contains(Path.ChangeExtension(path, sibling)))))
			{
				SourceKind kind = ((text == ".obj") ? SourceKind.Mesh : SourceKind.Texture);
				list.Add(new Source(path, Matches(path, premultipliedPaths) ? Premultiplied : Straight, kind));
			}
		}
		return list;
	}

	public static void AddRaw(List<Source> sources, IEnumerable<RawSource> rawFiles)
	{
		foreach (RawSource rawFile in rawFiles)
		{
			string text = rawFile.EntryName.Replace('\\', '/').TrimStart('/');
			if (text.Length == 0 || text.EndsWith(".lz4", StringComparison.Ordinal))
			{
				throw new InvalidDataException("'" + rawFile.EntryName + "' is not a usable raw entry name");
			}
			string fullPath = Path.GetFullPath(rawFile.SourcePath);
			if (!File.Exists(fullPath))
			{
				throw new FileNotFoundException("Raw source for '" + text + "' is missing", fullPath);
			}
			sources.Add(new Source(fullPath, Straight, SourceKind.Raw, text));
		}
	}

	public static bool IsStored(Source source, IReadOnlyCollection<string> storedExtensions)
	{
		string value = Path.GetExtension(source.RawName).ToLowerInvariant();
		if (!((ReadOnlySpan<string>)CompressedExtensions).Contains(value))
		{
			return storedExtensions.Contains<string>(value, StringComparer.OrdinalIgnoreCase);
		}
		return true;
	}

	public static string EntryName(string baseDirectory, Source source, bool premultiplied)
	{
		if (source.Kind == SourceKind.Raw)
		{
			return source.RawName;
		}
		return Path.GetRelativePath(baseDirectory, source.Path).Replace('\\', '/') + (source.IsMesh ? ".nbmesh" : (premultiplied ? "-premul.dds" : ".dds"));
	}

	private static bool Matches(string path, IReadOnlyList<string> patterns)
	{
		string text = path.Replace('\\', '/');
		foreach (string pattern in patterns)
		{
			if (text.Contains(pattern, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
