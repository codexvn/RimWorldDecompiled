using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using KTrie;
using UnityEngine;

namespace Verse;

public class ModContentHolder<T> where T : class
{
	[CompilerGenerated]
	private sealed class _003CGetAllUnderPath_003Ed__8 : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string pathRoot;

		public string _003C_003E3__pathRoot;

		public ModContentHolder<T> _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		T IEnumerator<T>.Current
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
		public _003CGetAllUnderPath_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ModContentHolder<T> modContentHolder = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					string prefix = ((pathRoot.NullOrEmpty() || pathRoot[pathRoot.Length - 1] == '/') ? pathRoot : (pathRoot + "/"));
					_003C_003E7__wrap1 = modContentHolder.contentListTrie.GetByPrefix(prefix).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = modContentHolder.contentList[current];
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetAllUnderPath_003Ed__8 _003CGetAllUnderPath_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllUnderPath_003Ed__ = this;
			}
			else
			{
				_003CGetAllUnderPath_003Ed__ = new _003CGetAllUnderPath_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetAllUnderPath_003Ed__.pathRoot = _003C_003E3__pathRoot;
			return _003CGetAllUnderPath_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	private ModContentPack mod;

	public Dictionary<string, T> contentList = new Dictionary<string, T>();

	private StringTrieSet contentListTrie = new StringTrieSet();

	internal static bool anyReloaded;

	public ModContentHolder(ModContentPack mod)
	{
		this.mod = mod;
	}

	public void ClearDestroy()
	{
		if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
		{
			foreach (T value in contentList.Values)
			{
				T localObj = value;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					UnityEngine.Object.Destroy((UnityEngine.Object)(object)localObj);
				});
			}
		}
		contentList.Clear();
		contentListTrie.Clear();
	}

	public void ReloadAll(bool hotReload = false)
	{
		anyReloaded = true;
		foreach (Pair<string, LoadedContentItem<T>> item in ModContentLoader<T>.LoadAllForMod(mod))
		{
			string first = item.First;
			first = first.Replace('\\', '/');
			string text = GenFilePaths.ContentPath<T>();
			if (first.StartsWith(text))
			{
				first = first.Substring(text.Length);
			}
			if (first.EndsWith(Path.GetExtension(first)))
			{
				first = first.Substring(0, first.Length - Path.GetExtension(first).Length);
			}
			if (contentList.ContainsKey(first))
			{
				if (!hotReload)
				{
					Log.Warning("Tried to load duplicate " + typeof(T)?.ToString() + " with path: " + item.Second.internalFile?.ToString() + " and internal path: " + first);
				}
			}
			else
			{
				contentList.Add(first, item.Second.contentItem);
				contentListTrie.Add(first);
			}
		}
	}

	public T Get(string path)
	{
		if (contentList.TryGetValue(path, out var value))
		{
			return value;
		}
		return null;
	}

	[IteratorStateMachine(typeof(ModContentHolder<>._003CGetAllUnderPath_003Ed__8))]
	public IEnumerable<T> GetAllUnderPath(string pathRoot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllUnderPath_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pathRoot = pathRoot
		};
	}
}
