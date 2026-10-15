using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class GraphicDatabaseUtility
{
	[CompilerGenerated]
	private sealed class _003CGraphicNamesInFolder_003Ed__0 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string folderPath;

		public string _003C_003E3__folderPath;

		private HashSet<string> _003CloadedAssetNames_003E5__2;

		private Texture2D[] _003C_003E7__wrap2;

		private int _003C_003E7__wrap3;

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
		public _003CGraphicNamesInFolder_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CloadedAssetNames_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0104;
			}
			_003C_003E1__state = -1;
			_003CloadedAssetNames_003E5__2 = new HashSet<string>();
			_003C_003E7__wrap2 = Resources.LoadAll<Texture2D>("Textures/" + folderPath);
			_003C_003E7__wrap3 = 0;
			goto IL_0112;
			IL_0104:
			_003C_003E7__wrap3++;
			goto IL_0112;
			IL_0112:
			if (_003C_003E7__wrap3 < _003C_003E7__wrap2.Length)
			{
				string[] array = _003C_003E7__wrap2[_003C_003E7__wrap3].name.Split('_');
				string item = "";
				if (array.Length <= 2)
				{
					item = array[0];
				}
				else if (array.Length == 3)
				{
					item = array[0] + "_" + array[1];
				}
				else if (array.Length == 4)
				{
					item = array[0] + "_" + array[1] + "_" + array[2];
				}
				else
				{
					Log.Error("Cannot load assets with >3 pieces.");
				}
				if (!_003CloadedAssetNames_003E5__2.Contains(item))
				{
					_003CloadedAssetNames_003E5__2.Add(item);
					_003C_003E2__current = item;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0104;
			}
			_003C_003E7__wrap2 = null;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGraphicNamesInFolder_003Ed__0 _003CGraphicNamesInFolder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGraphicNamesInFolder_003Ed__ = this;
			}
			else
			{
				_003CGraphicNamesInFolder_003Ed__ = new _003CGraphicNamesInFolder_003Ed__0(0);
			}
			_003CGraphicNamesInFolder_003Ed__.folderPath = _003C_003E3__folderPath;
			return _003CGraphicNamesInFolder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGraphicNamesInFolder_003Ed__0))]
	public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGraphicNamesInFolder_003Ed__0(-2)
		{
			_003C_003E3__folderPath = folderPath
		};
	}
}
