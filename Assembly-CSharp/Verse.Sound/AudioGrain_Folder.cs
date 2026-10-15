using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityStubs;

namespace Verse.Sound;

public class AudioGrain_Folder : AudioGrain
{
	[CompilerGenerated]
	private sealed class _003CGetResolvedGrains_003Ed__1 : IEnumerable<ResolvedGrain>, IEnumerable, IEnumerator<ResolvedGrain>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ResolvedGrain _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AudioGrain_Folder _003C_003E4__this;

		private IEnumerator<AudioClip> _003C_003E7__wrap1;

		ResolvedGrain IEnumerator<ResolvedGrain>.Current
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
		public _003CGetResolvedGrains_003Ed__1(int _003C_003E1__state)
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
				AudioGrain_Folder audioGrain_Folder = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ContentFinder<AudioClip>.GetAllInFolder(audioGrain_Folder.clipFolderPath).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					AudioClip current = _003C_003E7__wrap1.Current;
					if (!audioGrain_Folder.forceToMono)
					{
						ImportDefaults.KeepStereo(current);
					}
					_003C_003E2__current = new ResolvedGrain_Clip(current);
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
		IEnumerator<ResolvedGrain> IEnumerable<ResolvedGrain>.GetEnumerator()
		{
			_003CGetResolvedGrains_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetResolvedGrains_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ResolvedGrain>)this).GetEnumerator();
		}
	}

	[LoadAlias("clipPath")]
	[NoTranslate]
	public string clipFolderPath = "";

	[IteratorStateMachine(typeof(_003CGetResolvedGrains_003Ed__1))]
	public override IEnumerable<ResolvedGrain> GetResolvedGrains()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetResolvedGrains_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}
}
