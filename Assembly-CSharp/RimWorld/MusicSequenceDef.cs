using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class MusicSequenceDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__19 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MusicSequenceDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
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
				MusicSequenceDef musicSequenceDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (musicSequenceDef.workerType != null && !typeof(MusicSequenceWorker).IsAssignableFrom(musicSequenceDef.workerType))
					{
						_003C_003E2__current = "Music condition type is not a subclass of MusicSequenceWorker, type was: " + musicSequenceDef.workerType.FullName;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0087;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0087;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d6;
				case 3:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_00d6:
					_003C_003E7__wrap1 = ((Def)musicSequenceDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					break;
					IL_0087:
					if (musicSequenceDef.song != null && musicSequenceDef.songs != null && musicSequenceDef.workerType == typeof(MusicSequenceWorker))
					{
						_003C_003E2__current = "You are using both a single song and a list, multiple songs won't play with default worker implementation, use a custom worker or remove one of the two options. In sequence def: " + musicSequenceDef.defName;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d6;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 3;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type workerType = typeof(MusicSequenceWorker);

	public SongDef song;

	public List<SongDef> songs;

	public MusicSequenceDef nextSequence;

	public bool loop;

	public bool canBeInterrupted = true;

	public bool useTransitionForLifetime = true;

	public bool transitionOnDanger;

	public bool transitionOnNoDanger;

	public bool endOnNoDanger;

	public bool endOnDanger;

	public FloatRange loopDelayRange = FloatRange.Zero;

	public float fadeoutLastSongDuration = 10f;

	public float fadeInDuration;

	public bool loopFadeout;

	public float? overrideFadeout;

	public float? overrideVolume;

	public float? minTimeToPlay;

	public float? pausedVolumeFactor;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__19))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}
}
