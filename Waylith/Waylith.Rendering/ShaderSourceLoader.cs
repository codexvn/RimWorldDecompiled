using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Waylith.Rendering;

public static class ShaderSourceLoader
{
	private readonly record struct ShaderFileStamp(string Path, long Length, long WriteTimeUtcTicks)
	{
		public static ShaderFileStamp Of(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			return new ShaderFileStamp(path, fileInfo.Exists ? fileInfo.Length : (-1), fileInfo.Exists ? fileInfo.LastWriteTimeUtc.Ticks : 0);
		}

		public bool IsCurrent()
		{
			return Of(Path) == this;
		}
	}

	private sealed record ExpandedShaderSource(string Text, ShaderFileStamp[] Dependencies, ShaderLayoutCache.Include[] Includes)
	{
		public ShaderLayoutCache.Dependency[] CacheDependencies()
		{
			ShaderLayoutCache.Dependency[] array = new ShaderLayoutCache.Dependency[Dependencies.Length];
			for (int i = 0; i < Dependencies.Length; i++)
			{
				array[i] = new ShaderLayoutCache.Dependency(Dependencies[i].Path, Dependencies[i].Length, Dependencies[i].WriteTimeUtcTicks);
			}
			return array;
		}

		public bool IsCurrent()
		{
			ShaderFileStamp[] dependencies = Dependencies;
			foreach (ShaderFileStamp shaderFileStamp in dependencies)
			{
				if (!shaderFileStamp.IsCurrent())
				{
					return false;
				}
			}
			return true;
		}
	}

	private static readonly object SearchDirectoriesSync = new object();

	private static readonly List<string> SearchDirectories = new List<string>();

	private static string[] overrideDirectories = Array.Empty<string>();

	private static readonly ConcurrentDictionary<string, Lazy<MaterialLayout>> MaterialLayoutCache = new ConcurrentDictionary<string, Lazy<MaterialLayout>>(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, Lazy<ShaderVertexInputLayout>> VertexInputLayoutCache = new ConcurrentDictionary<string, Lazy<ShaderVertexInputLayout>>(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, Dictionary<string, string>> LeafIndexByRoot = new ConcurrentDictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, ExpandedShaderSource> ExpandedSourceCache = new ConcurrentDictionary<string, ExpandedShaderSource>(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, string> ResolvedPathCache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<(string vertex, string fragment), Lazy<bool>> StageInterfaceErrorCache = new ConcurrentDictionary<(string, string), Lazy<bool>>();

	public static void RegisterShaderSourceDirectory(string directory)
	{
		if (string.IsNullOrWhiteSpace(directory))
		{
			return;
		}
		string fullPath = Path.GetFullPath(directory);
		lock (SearchDirectoriesSync)
		{
			for (int i = 0; i < SearchDirectories.Count; i++)
			{
				if (string.Equals(SearchDirectories[i], fullPath, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			SearchDirectories.Add(fullPath);
		}
	}

	public static void SetOverrideShaderDirectories(IEnumerable<string> directories)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		List<string> list = new List<string>();
		foreach (string directory in directories)
		{
			if (!string.IsNullOrWhiteSpace(directory))
			{
				string fullPath = Path.GetFullPath(directory);
				if (hashSet.Add(fullPath))
				{
					list.Add(fullPath);
				}
			}
		}
		lock (SearchDirectoriesSync)
		{
			overrideDirectories = list.ToArray();
		}
	}

	public static MaterialLayout CreateMaterialLayout(string fragmentShaderFileName)
	{
		string key = ResolveShaderPath(fragmentShaderFileName);
		return MaterialLayoutCache.GetOrAdd(key, (string path, string fileName) => new Lazy<MaterialLayout>(delegate
		{
			if (ShaderLayoutCache.TryLoadMaterialLayout(path, out MaterialLayout layout))
			{
				return layout;
			}
			ExpandedShaderSource expandedShaderSource = ReadExpandedShaderSource(path, new HashSet<string>());
			bool loggedErrors;
			MaterialLayout materialLayout = ShaderMetadataParser.ExtractMaterialLayout(expandedShaderSource.Text, fileName, 2, out loggedErrors);
			if (!loggedErrors)
			{
				ShaderLayoutCache.Store(path, materialLayout, expandedShaderSource.CacheDependencies(), expandedShaderSource.Includes);
			}
			return materialLayout;
		}), fragmentShaderFileName).Value;
	}

	public static ShaderVertexInputLayout CreateVertexInputLayout(string vertexShaderFileName)
	{
		string key = ResolveShaderPath(vertexShaderFileName);
		return VertexInputLayoutCache.GetOrAdd(key, (string path, string fileName) => new Lazy<ShaderVertexInputLayout>(delegate
		{
			if (ShaderLayoutCache.TryLoadVertexInputLayout(path, out ShaderVertexInputLayout layout))
			{
				return layout;
			}
			ExpandedShaderSource expandedShaderSource = ReadExpandedShaderSource(path, new HashSet<string>());
			ShaderVertexInputLayout shaderVertexInputLayout = ShaderMetadataParser.ExtractVertexInputLayout(expandedShaderSource.Text, fileName);
			ShaderLayoutCache.Store(path, shaderVertexInputLayout, expandedShaderSource.CacheDependencies(), expandedShaderSource.Includes);
			return shaderVertexInputLayout;
		}), vertexShaderFileName).Value;
	}

	public static bool HasStageInterfaceErrors(string vertexShaderFileName, string fragmentShaderFileName)
	{
		(string, string) key = (ResolveShaderPath(vertexShaderFileName), ResolveShaderPath(fragmentShaderFileName));
		return StageInterfaceErrorCache.GetOrAdd(key, ((string vertex, string fragment) pair, (string vertexShaderFileName, string fragmentShaderFileName) names) => new Lazy<bool>(() => ShaderMetadataParser.ValidateStageInterface(ReadShaderSourceFile(pair.vertex, new HashSet<string>()), ReadShaderSourceFile(pair.fragment, new HashSet<string>()), names.vertexShaderFileName, names.fragmentShaderFileName)), (vertexShaderFileName, fragmentShaderFileName)).Value;
	}

	public static void ClearLayoutCaches()
	{
		MaterialLayoutCache.Clear();
		VertexInputLayoutCache.Clear();
		StageInterfaceErrorCache.Clear();
		ExpandedSourceCache.Clear();
		ResolvedPathCache.Clear();
	}

	public static string ReadShaderSource(string fileName)
	{
		return ReadShaderSource(fileName, new HashSet<string>());
	}

	public static string ResolveShaderPath(string fileName)
	{
		if (TryResolveShaderPath(fileName, out string resolvedPath))
		{
			return resolvedPath;
		}
		throw new FileNotFoundException("Unable to locate shader source '" + fileName + "'.", fileName);
	}

	internal static string[] GetShaderRoots()
	{
		List<string> list = new List<string>(GetOverrideDirectoriesSnapshot()) { Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Rendering", "Shaders")) };
		list.AddRange(GetSearchDirectoriesSnapshot());
		InlineArray6<string> buffer = default(InlineArray6<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 0) = AppContext.BaseDirectory;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 1) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 2) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 3) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 4) = "Rendering";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 5) = "Shaders";
		list.Add(Path.GetFullPath(Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray6<string>, string>(in buffer, 6))));
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!hashSet.Add(list[num]))
			{
				list.RemoveAt(num);
			}
		}
		return list.ToArray();
	}

	internal static bool TryRelativizeToShaderRoot(string fullPath, out string relative)
	{
		string fullPath2 = Path.GetFullPath(fullPath);
		string[] shaderRoots = GetShaderRoots();
		for (int i = 0; i < shaderRoots.Length; i++)
		{
			string relativePath = Path.GetRelativePath(shaderRoots[i], fullPath2);
			if (relativePath != fullPath2 && !relativePath.StartsWith("..", StringComparison.Ordinal) && !Path.IsPathRooted(relativePath))
			{
				relative = ((relativePath == ".") ? string.Empty : relativePath.Replace('\\', '/'));
				return true;
			}
		}
		relative = string.Empty;
		return false;
	}

	public static bool TryResolveShaderPath(string fileName, out string resolvedPath)
	{
		if (ResolvedPathCache.TryGetValue(fileName, out string value))
		{
			if (File.Exists(value))
			{
				resolvedPath = value;
				return true;
			}
			ResolvedPathCache.TryRemove(fileName, out string _);
		}
		if (TryResolveShaderPathUncached(fileName, out resolvedPath))
		{
			ResolvedPathCache[fileName] = resolvedPath;
			return true;
		}
		return false;
	}

	private static bool TryResolveShaderPathUncached(string fileName, out string resolvedPath)
	{
		string baseDirectory = AppContext.BaseDirectory;
		string[] overrideDirectoriesSnapshot = GetOverrideDirectoriesSnapshot();
		for (int i = 0; i < overrideDirectoriesSnapshot.Length; i++)
		{
			if (TryResolveInRoot(overrideDirectoriesSnapshot[i], fileName, out resolvedPath))
			{
				return true;
			}
		}
		if (TryResolveInRoot(Path.Combine(baseDirectory, "Rendering", "Shaders"), fileName, out resolvedPath))
		{
			return true;
		}
		overrideDirectoriesSnapshot = GetSearchDirectoriesSnapshot();
		for (int i = 0; i < overrideDirectoriesSnapshot.Length; i++)
		{
			if (TryResolveInRoot(overrideDirectoriesSnapshot[i], fileName, out resolvedPath))
			{
				return true;
			}
		}
		InlineArray6<string> buffer = default(InlineArray6<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 0) = baseDirectory;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 1) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 2) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 3) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 4) = "Rendering";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 5) = "Shaders";
		if (TryResolveInRoot(Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray6<string>, string>(in buffer, 6)), fileName, out resolvedPath))
		{
			return true;
		}
		resolvedPath = string.Empty;
		return false;
	}

	private static bool TryResolveInRoot(string root, string fileName, out string resolvedPath)
	{
		string path = Path.Combine(root, fileName);
		if (File.Exists(path))
		{
			resolvedPath = Path.GetFullPath(path);
			return true;
		}
		if (Directory.Exists(root))
		{
			string fileName2 = Path.GetFileName(fileName);
			if (GetLeafIndex(root).TryGetValue(fileName2, out string value))
			{
				if (File.Exists(value))
				{
					resolvedPath = value;
					return true;
				}
				LeafIndexByRoot.TryRemove(root, out Dictionary<string, string> _);
			}
			using IEnumerator<string> enumerator = Directory.EnumerateFiles(root, fileName2, SearchOption.AllDirectories).GetEnumerator();
			if (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				resolvedPath = Path.GetFullPath(current);
				return true;
			}
		}
		resolvedPath = string.Empty;
		return false;
	}

	private static Dictionary<string, string> GetLeafIndex(string root)
	{
		return LeafIndexByRoot.GetOrAdd(root, delegate(string key)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			try
			{
				foreach (string item in Directory.EnumerateFiles(key, "*", SearchOption.AllDirectories))
				{
					dictionary.TryAdd(Path.GetFileName(item), Path.GetFullPath(item));
				}
			}
			catch (Exception)
			{
			}
			return dictionary;
		});
	}

	private static string ReadShaderSource(string fileName, HashSet<string> includeStack)
	{
		if (TryResolveShaderPath(fileName, out string resolvedPath))
		{
			return ReadShaderSourceFile(resolvedPath, includeStack);
		}
		throw new FileNotFoundException("Unable to locate shader source '" + fileName + "'.", Path.Combine(AppContext.BaseDirectory, "Rendering", "Shaders", fileName));
	}

	private static string[] GetSearchDirectoriesSnapshot()
	{
		lock (SearchDirectoriesSync)
		{
			return SearchDirectories.ToArray();
		}
	}

	private static string[] GetOverrideDirectoriesSnapshot()
	{
		lock (SearchDirectoriesSync)
		{
			return overrideDirectories;
		}
	}

	internal static string[] GetCompilerSearchDirectories(string sourceName)
	{
		List<string> list = new List<string>();
		string directoryName = Path.GetDirectoryName(sourceName);
		if (!string.IsNullOrWhiteSpace(directoryName))
		{
			list.Add(Path.GetFullPath(directoryName));
		}
		list.AddRange(GetOverrideDirectoriesSnapshot());
		list.Add(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Rendering", "Shaders")));
		list.AddRange(GetSearchDirectoriesSnapshot());
		InlineArray6<string> buffer = default(InlineArray6<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 0) = AppContext.BaseDirectory;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 1) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 2) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 3) = "..";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 4) = "Rendering";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray6<string>, string>(ref buffer, 5) = "Shaders";
		list.Add(Path.GetFullPath(Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray6<string>, string>(in buffer, 6))));
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!Directory.Exists(list[num]) || !hashSet.Add(list[num]))
			{
				list.RemoveAt(num);
			}
		}
		return list.ToArray();
	}

	private static string ReadShaderSourceFile(string path, HashSet<string> includeStack)
	{
		return ReadExpandedShaderSource(path, includeStack).Text;
	}

	private static ExpandedShaderSource ReadExpandedShaderSource(string path, HashSet<string> includeStack)
	{
		path = Path.GetFullPath(path);
		if (ExpandedSourceCache.TryGetValue(path, out ExpandedShaderSource value) && value.IsCurrent())
		{
			return value;
		}
		if (!includeStack.Add(path))
		{
			throw new InvalidOperationException("Shader include cycle detected at '" + path + "'.");
		}
		try
		{
			List<ShaderFileStamp> list = new List<ShaderFileStamp> { ShaderFileStamp.Of(path) };
			List<ShaderLayoutCache.Include> list2 = new List<ShaderLayoutCache.Include>();
			ExpandedShaderSource expandedShaderSource = new ExpandedShaderSource(ExpandIncludes(File.ReadAllText(path), Path.GetDirectoryName(path) ?? string.Empty, includeStack, list, list2), list.ToArray(), list2.ToArray());
			ExpandedSourceCache[path] = expandedShaderSource;
			return expandedShaderSource;
		}
		finally
		{
			includeStack.Remove(path);
		}
	}

	private static string ExpandIncludes(string source, string directory, HashSet<string> includeStack, List<ShaderFileStamp> dependencies, List<ShaderLayoutCache.Include> includes)
	{
		using StringReader stringReader = new StringReader(source);
		StringBuilder stringBuilder = new StringBuilder(source.Length);
		while (true)
		{
			string text = stringReader.ReadLine();
			if (text == null)
			{
				break;
			}
			if (TryParseQuotedInclude(text, out string includePath))
			{
				string text2 = ResolveIncludePath(directory, includePath);
				ExpandedShaderSource expandedShaderSource = ReadExpandedShaderSource(text2, includeStack);
				stringBuilder.AppendLine(expandedShaderSource.Text);
				dependencies.AddRange(expandedShaderSource.Dependencies);
				includes.Add(new ShaderLayoutCache.Include(directory, includePath, text2));
				includes.AddRange(expandedShaderSource.Includes);
			}
			else
			{
				stringBuilder.AppendLine(text);
			}
		}
		return stringBuilder.ToString();
	}

	internal static string ResolveIncludePath(string includerDirectory, string includePath)
	{
		string fullPath = Path.GetFullPath(Path.Combine(includerDirectory, includePath));
		if (File.Exists(fullPath))
		{
			return fullPath;
		}
		if (!TryResolveShaderPath(includePath, out string resolvedPath))
		{
			return fullPath;
		}
		return resolvedPath;
	}

	internal static bool TryParseQuotedInclude(string line, out string includePath)
	{
		includePath = string.Empty;
		string text = line.Trim();
		if (!text.StartsWith("#include", StringComparison.Ordinal))
		{
			return false;
		}
		int num = text.IndexOf('"');
		if (num < 0)
		{
			return false;
		}
		int num2 = text.IndexOf('"', num + 1);
		if (num2 <= num + 1)
		{
			return false;
		}
		includePath = text.Substring(num + 1, num2 - num - 1);
		return true;
	}
}
