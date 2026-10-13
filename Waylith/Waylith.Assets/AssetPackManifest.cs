using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Waylith.Assets;

public static class AssetPackManifest
{
	public const string EntryName = "bake.manifest";

	public static string Stamp(FileInfo source)
	{
		return $"{source.Length}:{source.LastWriteTimeUtc.Ticks}";
	}

	public static string StampsPath(string packPath)
	{
		return packPath + ".stamps";
	}

	public static Dictionary<string, string> ReadContentHashes(AssetPack? pack)
	{
		if (pack == null || !pack.TryRead("bake.manifest", out byte[] data))
		{
			return new Dictionary<string, string>(StringComparer.Ordinal);
		}
		return Parse(Encoding.UTF8.GetString(data));
	}

	public static byte[] WriteContentHashes(IEnumerable<KeyValuePair<string, string>> hashes)
	{
		return Encoding.UTF8.GetBytes(Format(hashes));
	}

	public static Dictionary<string, string> ReadStamps(string packPath)
	{
		return ReadSidecar(StampsPath(packPath));
	}

	public static void WriteStamps(string packPath, IEnumerable<KeyValuePair<string, string>> stamps)
	{
		File.WriteAllText(StampsPath(packPath), Format(stamps));
	}

	public static string RolesPath(string packPath)
	{
		return packPath + ".roles";
	}

	public static Dictionary<string, string> ReadRoles(string packPath)
	{
		return ReadSidecar(RolesPath(packPath));
	}

	public static void WriteRoles(string packPath, IEnumerable<KeyValuePair<string, string>> roles)
	{
		File.WriteAllText(RolesPath(packPath), Format(roles));
	}

	private static Dictionary<string, string> ReadSidecar(string path)
	{
		if (!File.Exists(path))
		{
			return new Dictionary<string, string>(StringComparer.Ordinal);
		}
		return Parse(File.ReadAllText(path));
	}

	private static Dictionary<string, string> Parse(string text)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
		string[] array = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text2 in array)
		{
			int num = text2.LastIndexOf(' ');
			if (num > 0)
			{
				dictionary[text2.Substring(0, num)] = text2.Substring(num + 1);
			}
		}
		return dictionary;
	}

	private static string Format(IEnumerable<KeyValuePair<string, string>> values)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (var (value, value2) in values.OrderBy<KeyValuePair<string, string>, string>((KeyValuePair<string, string> e) => e.Key, StringComparer.Ordinal))
		{
			stringBuilder.Append(value).Append(' ').Append(value2)
				.Append('\n');
		}
		return stringBuilder.ToString();
	}
}
