using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ICSharpCode.SharpZipLib.Tar;

namespace RimWorld.IO;

internal class TarDirectory : VirtualDirectory
{
	[CompilerGenerated]
	private sealed class _003CEnumerateAllChildrenRecursive_003Ed__8 : IEnumerable<TarDirectory>, IEnumerable, IEnumerator<TarDirectory>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TarDirectory _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TarDirectory of;

		public TarDirectory _003C_003E3__of;

		private List<TarDirectory>.Enumerator _003C_003E7__wrap1;

		private TarDirectory _003Cdir_003E5__3;

		private IEnumerator<TarDirectory> _003C_003E7__wrap3;

		TarDirectory IEnumerator<TarDirectory>.Current
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
		public _003CEnumerateAllChildrenRecursive_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || num == 2)
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
			_003C_003E7__wrap1 = default(List<TarDirectory>.Enumerator);
			_003Cdir_003E5__3 = null;
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
					_003C_003E7__wrap1 = of.subDirectories.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					_003C_003E7__wrap3 = EnumerateAllChildrenRecursive(_003Cdir_003E5__3).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00c3;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00c3;
					}
					IL_00c3:
					if (_003C_003E7__wrap3.MoveNext())
					{
						TarDirectory current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					_003Cdir_003E5__3 = null;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Cdir_003E5__3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = _003Cdir_003E5__3;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<TarDirectory>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
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
		IEnumerator<TarDirectory> IEnumerable<TarDirectory>.GetEnumerator()
		{
			_003CEnumerateAllChildrenRecursive_003Ed__8 _003CEnumerateAllChildrenRecursive_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateAllChildrenRecursive_003Ed__ = this;
			}
			else
			{
				_003CEnumerateAllChildrenRecursive_003Ed__ = new _003CEnumerateAllChildrenRecursive_003Ed__8(0);
			}
			_003CEnumerateAllChildrenRecursive_003Ed__.of = _003C_003E3__of;
			return _003CEnumerateAllChildrenRecursive_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TarDirectory>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateAllFilesRecursive_003Ed__9 : IEnumerable<TarFile>, IEnumerable, IEnumerator<TarFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TarFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TarDirectory of;

		public TarDirectory _003C_003E3__of;

		private List<TarFile>.Enumerator _003C_003E7__wrap1;

		private List<TarDirectory>.Enumerator _003C_003E7__wrap2;

		private IEnumerator<TarFile> _003C_003E7__wrap3;

		TarFile IEnumerator<TarFile>.Current
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
		public _003CEnumerateAllFilesRecursive_003Ed__9(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<TarFile>.Enumerator);
			_003C_003E7__wrap2 = default(List<TarDirectory>.Enumerator);
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
					_003C_003E7__wrap1 = of.files.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0070;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0070;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_00fe;
					}
					IL_0070:
					if (_003C_003E7__wrap1.MoveNext())
					{
						TarFile current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<TarFile>.Enumerator);
					_003C_003E7__wrap2 = of.subDirectories.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0118;
					IL_0118:
					if (_003C_003E7__wrap2.MoveNext())
					{
						TarDirectory current2 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap3 = EnumerateAllFilesRecursive(current2).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_00fe;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<TarDirectory>.Enumerator);
					return false;
					IL_00fe:
					if (_003C_003E7__wrap3.MoveNext())
					{
						TarFile current3 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = null;
					goto IL_0118;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
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
		IEnumerator<TarFile> IEnumerable<TarFile>.GetEnumerator()
		{
			_003CEnumerateAllFilesRecursive_003Ed__9 _003CEnumerateAllFilesRecursive_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateAllFilesRecursive_003Ed__ = this;
			}
			else
			{
				_003CEnumerateAllFilesRecursive_003Ed__ = new _003CEnumerateAllFilesRecursive_003Ed__9(0);
			}
			_003CEnumerateAllFilesRecursive_003Ed__.of = _003C_003E3__of;
			return _003CEnumerateAllFilesRecursive_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TarFile>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetDirectories_003Ed__29 : IEnumerable<VirtualDirectory>, IEnumerable, IEnumerator<VirtualDirectory>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VirtualDirectory _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TarDirectory _003C_003E4__this;

		private SearchOption searchOption;

		public SearchOption _003C_003E3__searchOption;

		private string searchPattern;

		public string _003C_003E3__searchPattern;

		private Func<string, bool> _003Cmatcher_003E5__2;

		private IEnumerator<TarDirectory> _003C_003E7__wrap2;

		VirtualDirectory IEnumerator<VirtualDirectory>.Current
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
		public _003CGetDirectories_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cmatcher_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TarDirectory tarDirectory = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					tarDirectory.CheckLazyLoad();
					IEnumerable<TarDirectory> enumerable = tarDirectory.subDirectories;
					if (searchOption == SearchOption.AllDirectories)
					{
						enumerable = EnumerateAllChildrenRecursive(tarDirectory);
					}
					_003Cmatcher_003E5__2 = GetPatternMatcher(searchPattern);
					_003C_003E7__wrap2 = enumerable.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					TarDirectory current = _003C_003E7__wrap2.Current;
					if (_003Cmatcher_003E5__2(current.Name))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<VirtualDirectory> IEnumerable<VirtualDirectory>.GetEnumerator()
		{
			_003CGetDirectories_003Ed__29 _003CGetDirectories_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetDirectories_003Ed__ = this;
			}
			else
			{
				_003CGetDirectories_003Ed__ = new _003CGetDirectories_003Ed__29(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetDirectories_003Ed__.searchPattern = _003C_003E3__searchPattern;
			_003CGetDirectories_003Ed__.searchOption = _003C_003E3__searchOption;
			return _003CGetDirectories_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<VirtualDirectory>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFiles_003Ed__28 : IEnumerable<VirtualFile>, IEnumerable, IEnumerator<VirtualFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VirtualFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TarDirectory _003C_003E4__this;

		private SearchOption searchOption;

		public SearchOption _003C_003E3__searchOption;

		private string searchPattern;

		public string _003C_003E3__searchPattern;

		private Func<string, bool> _003Cmatcher_003E5__2;

		private IEnumerator<TarFile> _003C_003E7__wrap2;

		VirtualFile IEnumerator<VirtualFile>.Current
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
		public _003CGetFiles_003Ed__28(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cmatcher_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TarDirectory tarDirectory = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					tarDirectory.CheckLazyLoad();
					IEnumerable<TarFile> enumerable = tarDirectory.files;
					if (searchOption == SearchOption.AllDirectories)
					{
						enumerable = EnumerateAllFilesRecursive(tarDirectory);
					}
					_003Cmatcher_003E5__2 = GetPatternMatcher(searchPattern);
					_003C_003E7__wrap2 = enumerable.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					TarFile current = _003C_003E7__wrap2.Current;
					if (_003Cmatcher_003E5__2(current.Name))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<VirtualFile> IEnumerable<VirtualFile>.GetEnumerator()
		{
			_003CGetFiles_003Ed__28 _003CGetFiles_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFiles_003Ed__ = this;
			}
			else
			{
				_003CGetFiles_003Ed__ = new _003CGetFiles_003Ed__28(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFiles_003Ed__.searchPattern = _003C_003E3__searchPattern;
			_003CGetFiles_003Ed__.searchOption = _003C_003E3__searchOption;
			return _003CGetFiles_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<VirtualFile>)this).GetEnumerator();
		}
	}

	private static Dictionary<string, TarDirectory> cache = new Dictionary<string, TarDirectory>();

	private string lazyLoadArchive;

	private static readonly TarDirectory NotFound = new TarDirectory();

	private string fullPath;

	private string inArchiveFullPath;

	private string name;

	private bool exists;

	public List<TarDirectory> subDirectories = new List<TarDirectory>();

	public List<TarFile> files = new List<TarFile>();

	public override string Name => name;

	public override string FullPath => fullPath;

	public override bool Exists => exists;

	public static void ClearCache()
	{
		cache.Clear();
	}

	public static TarDirectory ReadFromFileOrCache(string file)
	{
		string key = file.Replace('\\', '/');
		if (!cache.TryGetValue(key, out var value))
		{
			value = new TarDirectory(file, "");
			value.lazyLoadArchive = file;
			cache.Add(key, value);
		}
		return value;
	}

	private void CheckLazyLoad()
	{
		if (lazyLoadArchive == null)
		{
			return;
		}
		using (FileStream inputStream = File.OpenRead(lazyLoadArchive))
		{
			using TarInputStream input = new TarInputStream(inputStream);
			ParseTAR(this, input, lazyLoadArchive);
		}
		lazyLoadArchive = null;
	}

	private static void ParseTAR(TarDirectory root, TarInputStream input, string fullPath)
	{
		Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
		List<TarEntry> list = new List<TarEntry>();
		List<TarDirectory> list2 = new List<TarDirectory>();
		byte[] buffer = new byte[16384];
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				TarEntry nextEntry;
				while ((nextEntry = input.GetNextEntry()) != null)
				{
					ReadTarEntryData(input, memoryStream, buffer);
					dictionary.Add(nextEntry.Name, memoryStream.ToArray());
					list.Add(nextEntry);
					memoryStream.Position = 0L;
					memoryStream.SetLength(0L);
				}
			}
			list2.Add(root);
			foreach (TarEntry item in list.Where((TarEntry e) => e.IsDirectory && !string.IsNullOrEmpty(e.Name)))
			{
				string text = FormatFolderPath(item.Name);
				list2.Add(new TarDirectory(fullPath + "/" + text, text));
			}
			foreach (TarEntry item2 in list.Where((TarEntry e) => !e.IsDirectory))
			{
				string text2 = FormatFolderPath(Path.GetDirectoryName(item2.Name));
				TarDirectory tarDirectory = null;
				foreach (TarDirectory item3 in list2)
				{
					if (item3.inArchiveFullPath == text2)
					{
						tarDirectory = item3;
						break;
					}
				}
				tarDirectory.files.Add(new TarFile(dictionary[item2.Name], fullPath + "/" + item2.Name, Path.GetFileName(item2.Name)));
			}
			foreach (TarDirectory item4 in list2)
			{
				if (string.IsNullOrEmpty(item4.inArchiveFullPath))
				{
					continue;
				}
				string text3 = FormatFolderPath(Path.GetDirectoryName(item4.inArchiveFullPath));
				TarDirectory tarDirectory2 = null;
				foreach (TarDirectory item5 in list2)
				{
					if (item5.inArchiveFullPath == text3)
					{
						tarDirectory2 = item5;
						break;
					}
				}
				tarDirectory2.subDirectories.Add(item4);
			}
		}
		finally
		{
			input.Close();
		}
	}

	private static string FormatFolderPath(string str)
	{
		if (str.Length == 0)
		{
			return str;
		}
		if (str.IndexOf('\\') != -1)
		{
			str = str.Replace('\\', '/');
		}
		if (str[str.Length - 1] == '/')
		{
			str = str.Substring(0, str.Length - 1);
		}
		return str;
	}

	private static void ReadTarEntryData(TarInputStream tarIn, Stream outStream, byte[] buffer = null)
	{
		if (buffer == null)
		{
			buffer = new byte[4096];
		}
		for (int num = tarIn.Read(buffer, 0, buffer.Length); num > 0; num = tarIn.Read(buffer, 0, buffer.Length))
		{
			outStream.Write(buffer, 0, num);
		}
	}

	[IteratorStateMachine(typeof(_003CEnumerateAllChildrenRecursive_003Ed__8))]
	private static IEnumerable<TarDirectory> EnumerateAllChildrenRecursive(TarDirectory of)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateAllChildrenRecursive_003Ed__8(-2)
		{
			_003C_003E3__of = of
		};
	}

	[IteratorStateMachine(typeof(_003CEnumerateAllFilesRecursive_003Ed__9))]
	private static IEnumerable<TarFile> EnumerateAllFilesRecursive(TarDirectory of)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEnumerateAllFilesRecursive_003Ed__9(-2)
		{
			_003C_003E3__of = of
		};
	}

	private static Func<string, bool> GetPatternMatcher(string searchPattern)
	{
		Func<string, bool> func = null;
		if (searchPattern.Length == 1 && searchPattern[0] == '*')
		{
			func = (string str) => true;
		}
		else if (searchPattern.Length > 2 && searchPattern[0] == '*' && searchPattern[1] == '.')
		{
			string extension = searchPattern.Substring(2);
			func = (string str) => str.Substring(str.Length - extension.Length) == extension;
		}
		if (func == null)
		{
			func = (string str) => false;
		}
		return func;
	}

	private TarDirectory(string fullPath, string inArchiveFullPath)
	{
		name = Path.GetFileName(fullPath);
		if (name.IndexOf(".tar") == name.Length - 4)
		{
			name = name.Substring(0, name.Length - 4);
		}
		this.fullPath = fullPath;
		this.inArchiveFullPath = inArchiveFullPath;
		exists = true;
	}

	private TarDirectory()
	{
		exists = false;
	}

	public override VirtualDirectory GetDirectory(string directoryName)
	{
		CheckLazyLoad();
		string text = directoryName;
		if (!string.IsNullOrEmpty(fullPath))
		{
			text = fullPath + "/" + text;
		}
		foreach (TarDirectory subDirectory in subDirectories)
		{
			if (subDirectory.fullPath == text)
			{
				return subDirectory;
			}
		}
		return NotFound;
	}

	public override VirtualFile GetFile(string filename)
	{
		CheckLazyLoad();
		VirtualDirectory virtualDirectory = this;
		string[] array = filename.Split('/', '\\');
		for (int i = 0; i < array.Length - 1; i++)
		{
			virtualDirectory = virtualDirectory.GetDirectory(array[i]);
		}
		filename = array[^1];
		if (virtualDirectory == this)
		{
			foreach (TarFile file in files)
			{
				if (file.Name == filename)
				{
					return file;
				}
			}
			return TarFile.NotFound;
		}
		return virtualDirectory.GetFile(filename);
	}

	[IteratorStateMachine(typeof(_003CGetFiles_003Ed__28))]
	public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFiles_003Ed__28(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__searchPattern = searchPattern,
			_003C_003E3__searchOption = searchOption
		};
	}

	[IteratorStateMachine(typeof(_003CGetDirectories_003Ed__29))]
	public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDirectories_003Ed__29(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__searchPattern = searchPattern,
			_003C_003E3__searchOption = searchOption
		};
	}

	public override string ToString()
	{
		return $"TarDirectory [{fullPath}], {files.Count.ToString()} files";
	}
}
