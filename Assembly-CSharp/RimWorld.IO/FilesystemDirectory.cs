using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace RimWorld.IO;

internal class FilesystemDirectory : VirtualDirectory
{
	[CompilerGenerated]
	private sealed class _003CGetDirectories_003Ed__9 : IEnumerable<VirtualDirectory>, IEnumerable, IEnumerator<VirtualDirectory>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VirtualDirectory _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FilesystemDirectory _003C_003E4__this;

		private string searchPattern;

		public string _003C_003E3__searchPattern;

		private SearchOption searchOption;

		public SearchOption _003C_003E3__searchOption;

		private DirectoryInfo[] _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

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
		public _003CGetDirectories_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			FilesystemDirectory filesystemDirectory = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = filesystemDirectory.dirInfo.GetDirectories(searchPattern, searchOption);
				_003C_003E7__wrap2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E7__wrap2++;
				break;
			}
			if (_003C_003E7__wrap2 < _003C_003E7__wrap1.Length)
			{
				DirectoryInfo dir = _003C_003E7__wrap1[_003C_003E7__wrap2];
				_003C_003E2__current = new FilesystemDirectory(dir);
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E7__wrap1 = null;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<VirtualDirectory> IEnumerable<VirtualDirectory>.GetEnumerator()
		{
			_003CGetDirectories_003Ed__9 _003CGetDirectories_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetDirectories_003Ed__ = this;
			}
			else
			{
				_003CGetDirectories_003Ed__ = new _003CGetDirectories_003Ed__9(0)
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
	private sealed class _003CGetFiles_003Ed__12 : IEnumerable<VirtualFile>, IEnumerable, IEnumerator<VirtualFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VirtualFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FilesystemDirectory _003C_003E4__this;

		private string searchPattern;

		public string _003C_003E3__searchPattern;

		private SearchOption searchOption;

		public SearchOption _003C_003E3__searchOption;

		private FileInfo[] _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

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
		public _003CGetFiles_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			FilesystemDirectory filesystemDirectory = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = filesystemDirectory.dirInfo.GetFiles(searchPattern, searchOption);
				_003C_003E7__wrap2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E7__wrap2++;
				break;
			}
			if (_003C_003E7__wrap2 < _003C_003E7__wrap1.Length)
			{
				FileInfo fileInfo = _003C_003E7__wrap1[_003C_003E7__wrap2];
				_003C_003E2__current = new FilesystemFile(fileInfo);
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E7__wrap1 = null;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<VirtualFile> IEnumerable<VirtualFile>.GetEnumerator()
		{
			_003CGetFiles_003Ed__12 _003CGetFiles_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFiles_003Ed__ = this;
			}
			else
			{
				_003CGetFiles_003Ed__ = new _003CGetFiles_003Ed__12(0)
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

	private DirectoryInfo dirInfo;

	public override string Name => dirInfo.Name;

	public override string FullPath => dirInfo.FullName;

	public override bool Exists => dirInfo.Exists;

	public FilesystemDirectory(string dir)
	{
		dirInfo = new DirectoryInfo(dir);
	}

	public FilesystemDirectory(DirectoryInfo dir)
	{
		dirInfo = dir;
	}

	[IteratorStateMachine(typeof(_003CGetDirectories_003Ed__9))]
	public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDirectories_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__searchPattern = searchPattern,
			_003C_003E3__searchOption = searchOption
		};
	}

	public override VirtualDirectory GetDirectory(string directoryName)
	{
		return new FilesystemDirectory(Path.Combine(FullPath, directoryName));
	}

	public override VirtualFile GetFile(string filename)
	{
		return new FilesystemFile(new FileInfo(Path.Combine(FullPath, filename)));
	}

	[IteratorStateMachine(typeof(_003CGetFiles_003Ed__12))]
	public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFiles_003Ed__12(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__searchPattern = searchPattern,
			_003C_003E3__searchOption = searchOption
		};
	}
}
