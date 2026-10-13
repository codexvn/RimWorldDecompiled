using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Waylith.Graphics.Meshes;
using Waylith.Math;
using Waylith.Rendering;
using Waylith.Runtime;
using Waylith.Utils;

namespace Waylith;

public static class Resources
{
	[CompilerGenerated]
	private sealed class _003CEnumerateResourcePaths_003Ed__9 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string path;

		public string _003C_003E3__path;

		private string[] extensions;

		public string[] _003C_003E3__extensions;

		private HashSet<string> _003CseenPaths_003E5__2;

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
		public _003CEnumerateResourcePaths_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CseenPaths_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<string>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00b5;
				}
				_003C_003E1__state = -1;
				_003CseenPaths_003E5__2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				_003C_003E7__wrap2 = GetSearchRoots(requireExists: true, includeDataPath: false).GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00cf;
				IL_00b5:
				while (_003C_003E7__wrap3.MoveNext())
				{
					string current = _003C_003E7__wrap3.Current;
					if (_003CseenPaths_003E5__2.Add(current))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap3 = null;
				goto IL_00cf;
				IL_00cf:
				if (_003C_003E7__wrap2.MoveNext())
				{
					string current2 = _003C_003E7__wrap2.Current;
					_003C_003E7__wrap3 = EnumerateResourcePathsUnder(current2, path, extensions).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00b5;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = default(List<string>.Enumerator);
				return false;
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
			_003C_003E1__state = -3;
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
			_003CEnumerateResourcePaths_003Ed__9 _003CEnumerateResourcePaths_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateResourcePaths_003Ed__ = this;
			}
			else
			{
				_003CEnumerateResourcePaths_003Ed__ = new _003CEnumerateResourcePaths_003Ed__9(0);
			}
			_003CEnumerateResourcePaths_003Ed__.path = _003C_003E3__path;
			_003CEnumerateResourcePaths_003Ed__.extensions = _003C_003E3__extensions;
			return _003CEnumerateResourcePaths_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateResourcePathsUnder_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string[] extensions;

		public string[] _003C_003E3__extensions;

		private string path;

		public string _003C_003E3__path;

		private string root;

		public string _003C_003E3__root;

		private string _003CtargetName_003E5__2;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CEnumerateResourcePathsUnder_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CtargetName_003E5__2 = null;
			_003C_003E7__wrap2 = null;
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
				{
					_003C_003E1__state = -1;
					if (extensions.Length == 0)
					{
						return false;
					}
					string path = ResourceFiles.NormalizePath(this.path);
					string fullPath = Path.Combine(root, path);
					string text = ResourceFiles.TryResolveCaseInsensitive(fullPath, expectFile: false);
					if (text != null)
					{
						_003C_003E7__wrap2 = ResourceFiles.EnumerateFiles(text, recursive: true).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00db;
					}
					string directoryName = Path.GetDirectoryName(fullPath);
					string text2 = ((directoryName != null) ? ResourceFiles.TryResolveCaseInsensitive(directoryName, expectFile: false) : null);
					if (text2 == null)
					{
						return false;
					}
					_003CtargetName_003E5__2 = Path.GetFileName(path);
					_003C_003E7__wrap2 = ResourceFiles.EnumerateFiles(text2, recursive: false).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_00db;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_00db:
					while (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						if (HasExtension(current, extensions))
						{
							string text3 = Path.GetRelativePath(root, current).Replace('\\', '/');
							_003C_003E2__current = Path.ChangeExtension(text3, null) ?? text3;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					return false;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					string current2 = _003C_003E7__wrap2.Current;
					if (HasExtension(current2, extensions) && string.Equals(Path.GetFileNameWithoutExtension(current2), _003CtargetName_003E5__2, StringComparison.OrdinalIgnoreCase))
					{
						string text4 = Path.GetRelativePath(root, current2).Replace('\\', '/');
						_003C_003E2__current = Path.ChangeExtension(text4, null) ?? text4;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				return false;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CEnumerateResourcePathsUnder_003Ed__16 _003CEnumerateResourcePathsUnder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateResourcePathsUnder_003Ed__ = this;
			}
			else
			{
				_003CEnumerateResourcePathsUnder_003Ed__ = new _003CEnumerateResourcePathsUnder_003Ed__16(0);
			}
			_003CEnumerateResourcePathsUnder_003Ed__.root = _003C_003E3__root;
			_003CEnumerateResourcePathsUnder_003Ed__.path = _003C_003E3__path;
			_003CEnumerateResourcePathsUnder_003Ed__.extensions = _003C_003E3__extensions;
			return _003CEnumerateResourcePathsUnder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private static readonly ConcurrentDictionary<string, Lazy<object?>> FileBackedResourceCache = new ConcurrentDictionary<string, Lazy<object>>(StringComparer.OrdinalIgnoreCase);

	private static readonly string[] TextureExtensions = new string[7] { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".dds", ".psd" };

	private static readonly string[] PsdFallbackExtensions = new string[6] { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".dds" };

	private static readonly string[] MeshExtensions = new string[1] { ".obj" };

	public static bool IsResourceType(Type type)
	{
		if (!(type == typeof(Texture2D)) && !(type == typeof(Texture3D)))
		{
			return type == typeof(Mesh);
		}
		return true;
	}

	public static T? Load<T>(string path) where T : class
	{
		return (T)TryLoadFileBackedResource(path, typeof(T));
	}

	public static object? Load(string path, Type systemTypeInstance)
	{
		return TryLoadFileBackedResource(path, systemTypeInstance);
	}

	public static object[] LoadAll(string path, Type systemTypeInstance)
	{
		List<object> list = new List<object>();
		HashSet<object> hashSet = new HashSet<object>();
		foreach (string item in EnumerateResourcePaths(path, LoadAllExtensionsFor(systemTypeInstance)))
		{
			object obj = TryLoadFileBackedResource(item, systemTypeInstance);
			if (obj != null && hashSet.Add(obj))
			{
				list.Add(obj);
			}
		}
		return list.ToArray();
	}

	public static string? FindFile(string path, string[] extensions, bool includeBareIfHasExtension = false)
	{
		List<string> searchRoots = GetSearchRoots(requireExists: false);
		if (searchRoots.Count == 0)
		{
			return null;
		}
		return ResourceFiles.FindExistingFile(ResourceFiles.EnumerateCandidatePaths(searchRoots, ResourceFiles.NormalizePath(path), extensions, includeBareIfHasExtension));
	}

	[IteratorStateMachine(typeof(_003CEnumerateResourcePaths_003Ed__9))]
	public static IEnumerable<string> EnumerateResourcePaths(string path, string[] extensions)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateResourcePaths_003Ed__9(-2)
		{
			_003C_003E3__path = path,
			_003C_003E3__extensions = extensions
		};
	}

	private static object? TryLoadFileBackedResource(string path, Type requestedType)
	{
		string key = requestedType.FullName + ":" + ResourceFiles.NormalizePath(path);
		return FileBackedResourceCache.GetOrAdd(key, (string _) => new Lazy<object>(() => LoadFileBackedResource(path, requestedType))).Value;
	}

	private static object? LoadFileBackedResource(string path, Type requestedType)
	{
		object obj = null;
		if (requestedType == typeof(Texture2D))
		{
			string text = FindFile(path, TextureExtensions);
			if (text != null)
			{
				if (IsPsdAsset(text))
				{
					string text2 = FindPsdFallbackTexture(text);
					if (text2 != null)
					{
						obj = Texture2D.LoadFromFile(text2);
					}
					else
					{
						try
						{
							obj = Texture2D.LoadFromFile(text);
						}
						catch
						{
							obj = Texture2D.CreatePlaceholder(Path.GetFileNameWithoutExtension(text), new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue));
						}
					}
				}
				else
				{
					obj = Texture2D.LoadFromFile(text);
				}
			}
		}
		if (obj == null && requestedType == typeof(Texture3D))
		{
			string text3 = FindFile(path, TextureExtensions);
			if (text3 != null)
			{
				obj = Texture3D.LoadLutFromFile(text3);
			}
		}
		if (obj == null && requestedType == typeof(Mesh))
		{
			string text4 = FindFile(path, MeshExtensions, includeBareIfHasExtension: true);
			if (text4 != null)
			{
				Mesh mesh = ObjMeshLoader.LoadFromFile(text4);
				mesh.DebugName = "Content/" + Path.GetFileNameWithoutExtension(text4);
				obj = mesh;
			}
		}
		return obj;
	}

	public static string[] LoadAllExtensionsFor(Type requestedType)
	{
		if (requestedType == typeof(Texture2D))
		{
			return TextureExtensions;
		}
		if (requestedType == typeof(Mesh))
		{
			return MeshExtensions;
		}
		return Array.Empty<string>();
	}

	private static bool IsPsdAsset(string filePath)
	{
		return string.Equals(Path.GetExtension(filePath), ".psd", StringComparison.OrdinalIgnoreCase);
	}

	private static string? FindPsdFallbackTexture(string psdPath)
	{
		string[] psdFallbackExtensions = PsdFallbackExtensions;
		foreach (string extension in psdFallbackExtensions)
		{
			string text = Path.ChangeExtension(psdPath, extension);
			if (ResourceFiles.FileExists(text))
			{
				return text;
			}
		}
		return null;
	}

	private static List<string> GetSearchRoots(bool requireExists, bool includeDataPath = true)
	{
		List<string> list = new List<string>();
		string text = WaylithRuntime.Current?.App.DataPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			return list;
		}
		string text2 = Path.Combine(text, "Resources");
		if (!requireExists || ResourceFiles.DirectoryExists(text2))
		{
			list.Add(text2);
		}
		if (includeDataPath && (!requireExists || ResourceFiles.DirectoryExists(text)))
		{
			list.Add(text);
		}
		return list;
	}

	[IteratorStateMachine(typeof(_003CEnumerateResourcePathsUnder_003Ed__16))]
	private static IEnumerable<string> EnumerateResourcePathsUnder(string root, string path, string[] extensions)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateResourcePathsUnder_003Ed__16(-2)
		{
			_003C_003E3__root = root,
			_003C_003E3__path = path,
			_003C_003E3__extensions = extensions
		};
	}

	private static bool HasExtension(string filePath, string[] extensions)
	{
		string extension = Path.GetExtension(filePath);
		foreach (string value in extensions)
		{
			if (extension.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
