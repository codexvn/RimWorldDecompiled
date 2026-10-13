using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using UnityEngine;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal static class BundleCache
{
	private const string Version = "v1";

	private static readonly object Gate = new object();

	private static Task _packing = Task.CompletedTask;

	private static bool _swept;

	private static string Root => Path.Combine(UnityRuntime.Current?.App.CacheDataPath ?? Application.persistentDataPath, "BundleCache", "v1");

	public static bool TryOpen(AssetsManager manager, string path, out BundleFileInstance bundle)
	{
		bundle = null;
		try
		{
			string siblingPattern;
			string text = CachePath(path, out siblingPattern);
			if (text == null || !File.Exists(text))
			{
				return false;
			}
			bundle = manager.LoadBundleFile(new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete), path, unpackIfPacked: false);
			if (!bundle.file.DataIsCompressed)
			{
				return true;
			}
			manager.UnloadBundleFile(bundle);
			bundle = null;
			return false;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Bundle cache entry for '" + path + "' is unreadable, unpacking the original: " + ex.Message);
			bundle = null;
			return false;
		}
	}

	public static void StoreLater(string path, Stream unpacked)
	{
		lock (Gate)
		{
			_packing = _packing.ContinueWith(delegate
			{
				Store(path, unpacked);
			}, TaskScheduler.Default);
		}
	}

	private static void Store(string path, Stream unpacked)
	{
		string text = null;
		try
		{
			using (unpacked)
			{
				string siblingPattern;
				string text2 = CachePath(path, out siblingPattern);
				if (text2 == null)
				{
					return;
				}
				Directory.CreateDirectory(Root);
				SweepTempFiles();
				AssetBundleFile assetBundleFile = new AssetBundleFile();
				assetBundleFile.Read(new AssetsFileReader(unpacked));
				text = text2 + "." + Environment.ProcessId + ".tmp";
				using (AssetsFileWriter writer = new AssetsFileWriter(new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.None, 65536)))
				{
					assetBundleFile.Pack(writer, AssetBundleCompressionType.LZ4);
				}
				File.Move(text, text2, overwrite: true);
				text = null;
				foreach (string item in Directory.EnumerateFiles(Root, siblingPattern))
				{
					if (item != text2)
					{
						TryDelete(item);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not cache asset bundle '" + path + "': " + ex.Message);
		}
		finally
		{
			if (text != null)
			{
				TryDelete(text);
			}
		}
	}

	private static string CachePath(string path, out string siblingPattern)
	{
		siblingPattern = null;
		string fullPath = Path.GetFullPath(path);
		FileInfo fileInfo = new FileInfo(fullPath);
		if (!fileInfo.Exists)
		{
			return null;
		}
		string text = Sanitize(Path.GetFileName(fullPath));
		string text2 = Hash(fullPath);
		string value = Hash($"{fileInfo.Length}:{fileInfo.LastWriteTimeUtc.Ticks}");
		siblingPattern = text + "-" + text2 + "-*.bundle";
		return Path.Combine(Root, $"{text}-{text2}-{value}.bundle");
	}

	private static void SweepTempFiles()
	{
		if (_swept)
		{
			return;
		}
		_swept = true;
		string value = $".{Environment.ProcessId}.tmp";
		foreach (string item in Directory.EnumerateFiles(Root, "*.tmp"))
		{
			if (!item.EndsWith(value, StringComparison.Ordinal))
			{
				TryDelete(item);
			}
		}
	}

	private static string Hash(string value)
	{
		return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)), 0, 8).ToLowerInvariant();
	}

	private static string Sanitize(string name)
	{
		char[] array = name.ToCharArray();
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		for (int i = 0; i < array.Length; i++)
		{
			if (Array.IndexOf(invalidFileNameChars, array[i]) >= 0)
			{
				array[i] = '_';
			}
		}
		return new string(array);
	}

	private static void TryDelete(string path)
	{
		try
		{
			File.Delete(path);
		}
		catch
		{
		}
	}
}
