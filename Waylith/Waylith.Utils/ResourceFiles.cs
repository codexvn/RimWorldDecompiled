using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Waylith.Utils;

public static class ResourceFiles
{
	private sealed class DirectoryListing
	{
		public static readonly DirectoryListing Unlistable = new DirectoryListing();

		public readonly HashSet<string> ExactFiles = new HashSet<string>(StringComparer.Ordinal);

		public readonly HashSet<string> ExactDirectories = new HashSet<string>(StringComparer.Ordinal);

		public readonly Dictionary<string, string> FilesByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public readonly Dictionary<string, string> DirectoriesByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public readonly List<string> Files = new List<string>();

		public readonly List<string> Directories = new List<string>();

		public readonly HashSet<string> DiskDirectories = new HashSet<string>(StringComparer.Ordinal);

		public void AddFile(string name, string path)
		{
			if (ExactFiles.Add(name))
			{
				FilesByName.TryAdd(name, path);
				Files.Add(path);
			}
		}

		public void AddDirectory(string name, string path)
		{
			if (ExactDirectories.Add(name))
			{
				DirectoriesByName.TryAdd(name, path);
				Directories.Add(path);
			}
		}
	}

	private sealed class VirtualRoot
	{
		public required string Root { get; init; }

		public required Dictionary<string, (HashSet<string> Files, HashSet<string> Directories)> Entries { get; init; }
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateCandidatePaths_003Ed__25 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IReadOnlyList<string> roots;

		public IReadOnlyList<string> _003C_003E3__roots;

		private bool includeBareIfHasExtension;

		public bool _003C_003E3__includeBareIfHasExtension;

		private string normalizedPath;

		public string _003C_003E3__normalizedPath;

		private string[] extensions;

		public string[] _003C_003E3__extensions;

		private IEnumerator<string> _003C_003E7__wrap1;

		private string _003Croot_003E5__3;

		private string[] _003C_003E7__wrap3;

		private int _003C_003E7__wrap4;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEnumerateCandidatePaths_003Ed__25(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003Croot_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = roots.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0117;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0098;
				case 2:
					{
						_003C_003E1__state = -3;
						_003C_003E7__wrap4++;
						goto IL_00f9;
					}
					IL_0117:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Croot_003E5__3 = _003C_003E7__wrap1.Current;
						if (includeBareIfHasExtension && Path.HasExtension(normalizedPath))
						{
							_003C_003E2__current = Path.Combine(_003Croot_003E5__3, normalizedPath);
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_0098;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_0098:
					_003C_003E7__wrap3 = extensions;
					_003C_003E7__wrap4 = 0;
					goto IL_00f9;
					IL_00f9:
					if (_003C_003E7__wrap4 < _003C_003E7__wrap3.Length)
					{
						string text = _003C_003E7__wrap3[_003C_003E7__wrap4];
						_003C_003E2__current = Path.Combine(_003Croot_003E5__3, normalizedPath + text);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap3 = null;
					_003Croot_003E5__3 = null;
					goto IL_0117;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CEnumerateCandidatePaths_003Ed__25 _003CEnumerateCandidatePaths_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateCandidatePaths_003Ed__ = this;
			}
			else
			{
				_003CEnumerateCandidatePaths_003Ed__ = new _003CEnumerateCandidatePaths_003Ed__25(0);
			}
			_003CEnumerateCandidatePaths_003Ed__.roots = _003C_003E3__roots;
			_003CEnumerateCandidatePaths_003Ed__.normalizedPath = _003C_003E3__normalizedPath;
			_003CEnumerateCandidatePaths_003Ed__.extensions = _003C_003E3__extensions;
			_003CEnumerateCandidatePaths_003Ed__.includeBareIfHasExtension = _003C_003E3__includeBareIfHasExtension;
			return _003CEnumerateCandidatePaths_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateFiles_003Ed__12 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string directory;

		public string _003C_003E3__directory;

		private bool recursive;

		public bool _003C_003E3__recursive;

		private DirectoryListing _003Clisting_003E5__2;

		private List<string>.Enumerator _003C_003E7__wrap2;

		private IEnumerator<string> _003C_003E7__wrap3;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEnumerateFiles_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003Clisting_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<string>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Clisting_003E5__2 = GetListing(Normalize(directory));
					if (_003Clisting_003E5__2 == null)
					{
						return false;
					}
					_003C_003E7__wrap2 = _003Clisting_003E5__2.Files.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0095;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0095;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_0133;
					}
					IL_0133:
					if (_003C_003E7__wrap3.MoveNext())
					{
						string current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = null;
					goto IL_014d;
					IL_0095:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<string>.Enumerator);
					if (!recursive)
					{
						return false;
					}
					_003C_003E7__wrap2 = _003Clisting_003E5__2.Directories.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_014d;
					IL_014d:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current3 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap3 = EnumerateFiles(current3, recursive: true).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0133;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<string>.Enumerator);
					return false;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CEnumerateFiles_003Ed__12 _003CEnumerateFiles_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateFiles_003Ed__ = this;
			}
			else
			{
				_003CEnumerateFiles_003Ed__ = new _003CEnumerateFiles_003Ed__12(0);
			}
			_003CEnumerateFiles_003Ed__.directory = _003C_003E3__directory;
			_003CEnumerateFiles_003Ed__.recursive = _003C_003E3__recursive;
			return _003CEnumerateFiles_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private static readonly ConcurrentDictionary<string, DirectoryListing?> Listings = new ConcurrentDictionary<string, DirectoryListing>(StringComparer.Ordinal);

	private static volatile VirtualRoot[] VirtualRoots = Array.Empty<VirtualRoot>();

	private static readonly Lazy<bool> DiskIgnoresCase = new Lazy<bool>(delegate
	{
		string text = Path.TrimEndingDirectorySeparator(AppContext.BaseDirectory);
		return Directory.Exists(text.ToUpperInvariant()) && Directory.Exists(text.ToLowerInvariant());
	});

	private static readonly ConcurrentDictionary<string, string?> ResolvedDirectories = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

	public static string NormalizePath(string? path)
	{
		return (path ?? string.Empty).Replace('\\', '/').TrimStart('/');
	}

	public static void SetVirtualFiles(string root, IEnumerable<string> fullPaths)
	{
		VirtualRoots = Array.Empty<VirtualRoot>();
		AddVirtualFiles(root, fullPaths);
	}

	public static void AddVirtualFiles(string root, IEnumerable<string> fullPaths)
	{
		root = Normalize(root);
		Dictionary<string, (HashSet<string>, HashSet<string>)> dictionary = new Dictionary<string, (HashSet<string>, HashSet<string>)>(StringComparer.Ordinal);
		foreach (string fullPath in fullPaths)
		{
			string path = Normalize(fullPath);
			bool flag = true;
			while (true)
			{
				string directoryName = Path.GetDirectoryName(path);
				if (directoryName == null || directoryName.Length <= 0 || (!(directoryName == root) && !directoryName.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.Ordinal)))
				{
					break;
				}
				if (!dictionary.TryGetValue(directoryName, out var value))
				{
					value = (dictionary[directoryName] = (new HashSet<string>(StringComparer.Ordinal), new HashSet<string>(StringComparer.Ordinal)));
				}
				HashSet<string> hashSet;
				if (flag)
				{
					(hashSet, _) = value;
				}
				else
				{
					hashSet = value.Item2;
				}
				if (!hashSet.Add(Path.GetFileName(path)) && !flag)
				{
					break;
				}
				path = directoryName;
				flag = false;
			}
		}
		if (dictionary.Count > 0)
		{
			string directoryName2 = Path.GetDirectoryName(root);
			if (directoryName2 != null && directoryName2.Length > 0)
			{
				if (!dictionary.TryGetValue(directoryName2, out var value2))
				{
					value2 = (dictionary[directoryName2] = (new HashSet<string>(StringComparer.Ordinal), new HashSet<string>(StringComparer.Ordinal)));
				}
				value2.Item2.Add(Path.GetFileName(root));
			}
		}
		List<VirtualRoot> list = new List<VirtualRoot>();
		list.AddRange(VirtualRoots.Where((VirtualRoot r) => r.Root != root));
		list.Add(new VirtualRoot
		{
			Root = root,
			Entries = dictionary
		});
		VirtualRoots = list.ToArray();
		Listings.Clear();
		ResolvedDirectories.Clear();
	}

	public static void RemoveVirtualFiles(string root)
	{
		root = Normalize(root);
		VirtualRoots = VirtualRoots.Where((VirtualRoot r) => r.Root != root).ToArray();
		Listings.Clear();
		ResolvedDirectories.Clear();
	}

	private static (HashSet<string> Files, HashSet<string> Directories)? VirtualEntriesOf(string directory)
	{
		HashSet<string> hashSet = null;
		HashSet<string> hashSet2 = null;
		bool flag = false;
		VirtualRoot[] virtualRoots = VirtualRoots;
		for (int i = 0; i < virtualRoots.Length; i++)
		{
			if (!virtualRoots[i].Entries.TryGetValue(directory, out (HashSet<string>, HashSet<string>) value))
			{
				continue;
			}
			if (hashSet == null || hashSet2 == null)
			{
				(hashSet, hashSet2) = value;
				continue;
			}
			if (!flag)
			{
				hashSet = new HashSet<string>(hashSet, StringComparer.Ordinal);
				hashSet2 = new HashSet<string>(hashSet2, StringComparer.Ordinal);
				flag = true;
			}
			hashSet.UnionWith(value.Item1);
			hashSet2.UnionWith(value.Item2);
		}
		if (hashSet != null && hashSet2 != null)
		{
			return (hashSet, hashSet2);
		}
		return null;
	}

	public static bool FileExists(string path)
	{
		return ExistsAsTheDiskDoes(Normalize(path), expectFile: true);
	}

	public static bool DirectoryExists(string path)
	{
		return ExistsAsTheDiskDoes(Normalize(path), expectFile: false);
	}

	public static void ForgetListings()
	{
		Listings.Clear();
		ResolvedDirectories.Clear();
	}

	[IteratorStateMachine(typeof(_003CEnumerateFiles_003Ed__12))]
	public static IEnumerable<string> EnumerateFiles(string directory, bool recursive)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateFiles_003Ed__12(-2)
		{
			_003C_003E3__directory = directory,
			_003C_003E3__recursive = recursive
		};
	}

	private static string Normalize(string path)
	{
		return Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
	}

	private static DirectoryListing? GetListing(string directory)
	{
		return Listings.GetOrAdd(directory, delegate(string d)
		{
			(HashSet<string>, HashSet<string>)? tuple = VirtualEntriesOf(Normalize(d));
			bool flag = IsOnDisk(d);
			if (!flag && !tuple.HasValue)
			{
				return (DirectoryListing?)null;
			}
			DirectoryListing directoryListing = new DirectoryListing();
			try
			{
				if (flag)
				{
					foreach (string item in Directory.EnumerateFiles(d))
					{
						directoryListing.AddFile(Path.GetFileName(item), item);
					}
					foreach (string item2 in Directory.EnumerateDirectories(d))
					{
						directoryListing.AddDirectory(Path.GetFileName(item2), item2);
						directoryListing.DiskDirectories.Add(Path.GetFileName(item2));
					}
				}
			}
			catch
			{
				return DirectoryListing.Unlistable;
			}
			if (tuple.HasValue)
			{
				(HashSet<string>, HashSet<string>) valueOrDefault = tuple.GetValueOrDefault();
				foreach (string item3 in valueOrDefault.Item1.Order<string>(StringComparer.Ordinal))
				{
					directoryListing.AddFile(item3, Path.Combine(d, item3));
				}
				foreach (string item4 in valueOrDefault.Item2.Order<string>(StringComparer.Ordinal))
				{
					directoryListing.AddDirectory(item4, Path.Combine(d, item4));
				}
			}
			return directoryListing;
		});
	}

	private static bool IsOnDisk(string directory)
	{
		string directoryName = Path.GetDirectoryName(directory);
		if (string.IsNullOrEmpty(directoryName))
		{
			return Directory.Exists(directory);
		}
		DirectoryListing listing = GetListing(directoryName);
		if (listing == DirectoryListing.Unlistable)
		{
			return Directory.Exists(directory);
		}
		if (listing == null)
		{
			return false;
		}
		string fileName = Path.GetFileName(directory);
		if (!listing.DiskDirectories.Contains(fileName))
		{
			return IsCaseOnlyMissOnDisk(listing, directory, expectFile: false);
		}
		return true;
	}

	private static bool IsCaseOnlyMissOnDisk(DirectoryListing listing, string fullPath, bool expectFile)
	{
		string fileName = Path.GetFileName(fullPath);
		if (expectFile)
		{
			if (listing.FilesByName.ContainsKey(fileName))
			{
				return File.Exists(fullPath);
			}
			return false;
		}
		if (listing.DirectoriesByName.ContainsKey(fileName))
		{
			return Directory.Exists(fullPath);
		}
		return false;
	}

	private static bool ExistsAsTheDiskDoes(string fullPath, bool expectFile)
	{
		if (ExistsExact(fullPath, expectFile))
		{
			return true;
		}
		string directoryName = Path.GetDirectoryName(fullPath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			DirectoryListing listing = GetListing(directoryName);
			if (listing != null)
			{
				if (!IsCaseOnlyMissOnDisk(listing, fullPath, expectFile))
				{
					return IsCaseOnlyMissInPack(listing, directoryName, fullPath, expectFile);
				}
				return true;
			}
		}
		return false;
	}

	private static bool IsCaseOnlyMissInPack(DirectoryListing listing, string parent, string fullPath, bool expectFile)
	{
		if (DiskIgnoresCase.Value && (expectFile ? listing.FilesByName : listing.DirectoriesByName).TryGetValue(Path.GetFileName(fullPath), out string value))
		{
			(HashSet<string>, HashSet<string>)? tuple = VirtualEntriesOf(parent);
			if (tuple.HasValue)
			{
				(HashSet<string>, HashSet<string>) valueOrDefault = tuple.GetValueOrDefault();
				HashSet<string> hashSet;
				if (expectFile)
				{
					(hashSet, _) = valueOrDefault;
				}
				else
				{
					hashSet = valueOrDefault.Item2;
				}
				return hashSet.Contains(Path.GetFileName(value));
			}
		}
		return false;
	}

	private static bool ExistsExact(string fullPath, bool expectFile)
	{
		string directoryName = Path.GetDirectoryName(fullPath);
		string fileName = Path.GetFileName(fullPath);
		if (string.IsNullOrEmpty(directoryName) || string.IsNullOrEmpty(fileName))
		{
			if (!expectFile)
			{
				return Directory.Exists(fullPath);
			}
			return File.Exists(fullPath);
		}
		DirectoryListing listing = GetListing(directoryName);
		if (listing != null)
		{
			return (expectFile ? listing.ExactFiles : listing.ExactDirectories).Contains(fileName);
		}
		return false;
	}

	public static string? FindExistingFile(IEnumerable<string> candidatePaths)
	{
		IReadOnlyList<string> readOnlyList = (candidatePaths as IReadOnlyList<string>) ?? candidatePaths.ToList();
		foreach (string item in readOnlyList)
		{
			if (ExistsExact(item, expectFile: true))
			{
				return item;
			}
		}
		foreach (string item2 in readOnlyList)
		{
			string text = TryResolveCaseInsensitive(item2, expectFile: true);
			if (text != null)
			{
				return text;
			}
		}
		return null;
	}

	public static string? TryResolveCaseInsensitive(string fullPath, bool expectFile)
	{
		fullPath = Path.TrimEndingDirectorySeparator(fullPath);
		if (!expectFile)
		{
			return ResolvedDirectories.GetOrAdd(fullPath, (string d) => ResolveCaseInsensitive(d, expectFile: false));
		}
		return ResolveCaseInsensitive(fullPath, expectFile: true);
	}

	private static string? ResolveCaseInsensitive(string fullPath, bool expectFile)
	{
		string directoryName = Path.GetDirectoryName(fullPath);
		string fileName = Path.GetFileName(fullPath);
		if (string.IsNullOrEmpty(directoryName) || string.IsNullOrEmpty(fileName))
		{
			if (!(expectFile ? File.Exists(fullPath) : Directory.Exists(fullPath)))
			{
				return null;
			}
			return fullPath;
		}
		string text = TryResolveCaseInsensitive(directoryName, expectFile: false);
		if (text == null)
		{
			return null;
		}
		DirectoryListing listing = GetListing(text);
		if (listing == null)
		{
			return null;
		}
		if ((expectFile ? listing.ExactFiles : listing.ExactDirectories).Contains(fileName))
		{
			return Path.Combine(text, fileName);
		}
		if (!(expectFile ? listing.FilesByName : listing.DirectoriesByName).TryGetValue(fileName, out string value))
		{
			return null;
		}
		return value;
	}

	[IteratorStateMachine(typeof(_003CEnumerateCandidatePaths_003Ed__25))]
	public static IEnumerable<string> EnumerateCandidatePaths(IReadOnlyList<string> roots, string normalizedPath, string[] extensions, bool includeBareIfHasExtension = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateCandidatePaths_003Ed__25(-2)
		{
			_003C_003E3__roots = roots,
			_003C_003E3__normalizedPath = normalizedPath,
			_003C_003E3__extensions = extensions,
			_003C_003E3__includeBareIfHasExtension = includeBareIfHasExtension
		};
	}
}
