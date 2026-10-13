using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Waylith.Runtime;

namespace Waylith.Utils;

public static class CacheFileWriter
{
	public readonly record struct InstallRecord(string Id, string Version, string[] Roots);

	private static readonly TimeSpan AbandonedAfter;

	public static readonly TimeSpan UnusedFor;

	private const string InstallsDirectoryName = "installs";

	private static readonly BlockingCollection<(string Path, byte[] Contents)> Pending;

	public static string InstallRoot
	{
		get
		{
			string text = WaylithRuntime.Current?.App.DataPath;
			return Path.TrimEndingDirectorySeparator(Path.GetFullPath((text != null && text.Length > 0) ? text : AppContext.BaseDirectory));
		}
	}

	public static string InstallId => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(InstallRoot)), 0, 8);

	static CacheFileWriter()
	{
		AbandonedAfter = TimeSpan.FromHours(1);
		UnusedFor = TimeSpan.FromDays(7);
		Pending = new BlockingCollection<(string, byte[])>(128);
		for (int i = 0; i < 2; i++)
		{
			Thread thread = new Thread(Run);
			thread.IsBackground = true;
			thread.Name = $"CacheFileWriter {i}";
			thread.Priority = ThreadPriority.BelowNormal;
			thread.Start();
		}
	}

	public static void Write(string path, byte[] contents)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		string text = path + "." + Path.GetRandomFileName() + ".tmp";
		try
		{
			File.WriteAllBytes(text, contents);
			File.Move(text, path, overwrite: true);
		}
		catch
		{
			File.Delete(text);
			throw;
		}
	}

	public static void Enqueue(string path, byte[] contents)
	{
		Pending.Add((path, contents));
	}

	private static void Run()
	{
		foreach (var (path, contents) in Pending.GetConsumingEnumerable())
		{
			try
			{
				Write(path, contents);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
	}

	public static Task SweepLater(string versionDirectory)
	{
		return Task.Run(delegate
		{
			string directoryName = Path.GetDirectoryName(versionDirectory);
			string fileName = Path.GetFileName(versionDirectory);
			try
			{
				RecordInstall(directoryName, fileName);
				HashSet<string> hashSet = (from install in OtherInstalls(directoryName)
					select install.Version).ToHashSet<string>(StringComparer.Ordinal);
				foreach (string item in Directory.EnumerateDirectories(directoryName))
				{
					string fileName2 = Path.GetFileName(item);
					if (fileName2 != fileName && fileName2.Length >= 2 && fileName2[0] == 'v' && int.TryParse(fileName2.AsSpan(1), out var _) && !hashSet.Contains(fileName2))
					{
						TryDelete(new DirectoryInfo(item));
					}
				}
			}
			catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
			{
			}
			DeleteAbandonedTemporaries(versionDirectory);
			DeleteAbandonedTemporaries(Path.Combine(directoryName, "installs"));
		});
	}

	public static void RecordInstall(string cacheDirectory, string version, IReadOnlyCollection<string>? roots = null)
	{
		string text = Path.Combine(cacheDirectory, "installs", InstallId);
		if (roots == null)
		{
			roots = (TryReadInstall(text, out var install) ? ((IReadOnlyCollection<string>?)(object)install.Roots) : ((IReadOnlyCollection<string>?)(object)Array.Empty<string>()));
		}
		string path = text;
		Encoding uTF = Encoding.UTF8;
		IReadOnlyCollection<string> readOnlyCollection = roots;
		int num = 0;
		string[] array = new string[1 + readOnlyCollection.Count];
		array[num] = version;
		num++;
		foreach (string item in readOnlyCollection)
		{
			array[num] = item;
			num++;
		}
		Write(path, uTF.GetBytes(string.Join('\n', new ReadOnlySpan<string>(array))));
	}

	public static List<InstallRecord> OtherInstalls(string cacheDirectory)
	{
		List<InstallRecord> list = new List<InstallRecord>();
		string installId = InstallId;
		DateTime dateTime = DateTime.UtcNow - UnusedFor;
		try
		{
			foreach (FileInfo item in new DirectoryInfo(Path.Combine(cacheDirectory, "installs")).EnumerateFiles())
			{
				if (!(item.Name == installId) && !item.Name.EndsWith(".tmp", StringComparison.Ordinal))
				{
					InstallRecord install;
					if (item.LastWriteTimeUtc < dateTime)
					{
						TryDelete(item);
					}
					else if (TryReadInstall(item.FullName, out install))
					{
						list.Add(install);
					}
				}
			}
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
		}
		return list;
	}

	private static bool TryReadInstall(string path, out InstallRecord install)
	{
		install = default(InstallRecord);
		try
		{
			string[] array = File.ReadAllLines(path);
			if (array.Length == 0)
			{
				return false;
			}
			install = new InstallRecord(Path.GetFileName(path), array[0], array[1..]);
			return true;
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
			return false;
		}
	}

	public static void DeleteAbandonedTemporaries(string directory)
	{
		DeleteUnwrittenFor(directory, "*.tmp", AbandonedAfter);
	}

	public static void DeleteUnwrittenFor(string directory, string pattern, TimeSpan age)
	{
		DateTime dateTime = DateTime.UtcNow - age;
		try
		{
			foreach (FileInfo item in new DirectoryInfo(directory).EnumerateFiles(pattern, SearchOption.AllDirectories))
			{
				if (item.LastWriteTimeUtc < dateTime)
				{
					TryDelete(item);
				}
			}
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
		}
	}

	private static void TryDelete(FileSystemInfo entry)
	{
		try
		{
			if (entry is DirectoryInfo directoryInfo)
			{
				directoryInfo.Delete(recursive: true);
			}
			else
			{
				entry.Delete();
			}
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
		}
	}
}
