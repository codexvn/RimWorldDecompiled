using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Waylith.Runtime;

public static class PlatformPaths
{
	public static string PersistentDataRoot(string company, string product)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			InlineArray5<string> buffer = default(InlineArray5<string>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 0) = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 1) = "Library";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 2) = "Application Support";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 3) = company;
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 4) = product;
			return Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray5<string>, string>(in buffer, 5));
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return Path.Combine(XdgDirectory("XDG_DATA_HOME", ".local", "share"), company, product);
		}
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), company, product);
	}

	public static string CacheDataRoot(string company, string product)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			InlineArray5<string> buffer = default(InlineArray5<string>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 0) = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 1) = "Library";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 2) = "Caches";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 3) = company;
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 4) = product;
			return Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray5<string>, string>(in buffer, 5));
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return Path.Combine(XdgDirectory("XDG_CACHE_HOME", ".cache"), company, product);
		}
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), company, product);
	}

	public static void DeleteMovedCaches(string formerRoot, string cacheRoot)
	{
		Task.Run(delegate
		{
			if (!IsSameDirectory(formerRoot, cacheRoot))
			{
				string[] array = new string[5] { "TextureCache", "MeshCache", "ShaderCache", "ShaderCache-editor", "HardwareProfile" };
				foreach (string path in array)
				{
					try
					{
						string path2 = Path.Combine(formerRoot, path);
						if (Directory.Exists(path2))
						{
							Directory.Delete(path2, recursive: true);
						}
					}
					catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
					{
					}
				}
			}
		});
	}

	private static string XdgDirectory(string variable, params string[] fallback)
	{
		string environmentVariable = Environment.GetEnvironmentVariable(variable);
		if (!Path.IsPathRooted(environmentVariable))
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			int num = 0;
			string[] array = new string[1 + fallback.Length];
			array[num] = folderPath;
			num++;
			ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(fallback);
			readOnlySpan.CopyTo(new Span<string>(array).Slice(num, readOnlySpan.Length));
			num += readOnlySpan.Length;
			return Path.Combine(new ReadOnlySpan<string>(array));
		}
		return environmentVariable;
	}

	private static bool IsSameDirectory(string a, string b)
	{
		if (string.Equals(Path.GetFullPath(a), Path.GetFullPath(b), StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (!Directory.Exists(a) || !Directory.Exists(b))
		{
			return false;
		}
		string path = $".same-directory-{Guid.NewGuid():N}";
		string path2 = Path.Combine(b, path);
		try
		{
			File.WriteAllBytes(path2, default(ReadOnlySpan<byte>));
			return File.Exists(Path.Combine(a, path));
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
			return true;
		}
		finally
		{
			try
			{
				File.Delete(path2);
			}
			catch (Exception ex2) when (((ex2 is IOException || ex2 is UnauthorizedAccessException) ? 1 : 0) != 0)
			{
			}
		}
	}
}
