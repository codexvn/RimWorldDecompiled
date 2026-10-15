using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_StudyUnlocks : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_StudyUnlocks _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private StudyNote _003Cnote_003E5__4;

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
		public _003CConfigErrors_003Ed__4(int _003C_003E1__state)
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
			_003Cnote_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompProperties_StudyUnlocks compProperties_StudyUnlocks = _003C_003E4__this;
				float num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_StudyUnlocks).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_022a;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_022a;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					num2 = 0f;
					_003Ci_003E5__3 = 0;
					goto IL_0273;
					IL_022a:
					num2 = ((_003Cnote_003E5__4.threshold != 0f) ? _003Cnote_003E5__4.threshold : _003Cnote_003E5__4.thresholdRange.max);
					_003Cnote_003E5__4 = null;
					_003Ci_003E5__3++;
					goto IL_0273;
					IL_0273:
					if (_003Ci_003E5__3 < compProperties_StudyUnlocks.studyNotes.Count)
					{
						_003Cnote_003E5__4 = compProperties_StudyUnlocks.studyNotes[_003Ci_003E5__3];
						if (num2 != 0f)
						{
							if (_003Cnote_003E5__4.threshold != 0f && _003Cnote_003E5__4.threshold <= num2)
							{
								_003C_003E2__current = $"Threshold for note at index {_003Ci_003E5__3} had a threshold value ({_003Cnote_003E5__4.threshold}) lower than the previous maximum ({num2})";
								_003C_003E1__state = 2;
								return true;
							}
							if (_003Cnote_003E5__4.threshold == 0f && _003Cnote_003E5__4.thresholdRange.min <= num2)
							{
								_003C_003E2__current = $"Threshold for note at index {_003Ci_003E5__3} had a min threshold value ({_003Cnote_003E5__4.thresholdRange.min}) lower than the previous maximum ({num2})";
								_003C_003E1__state = 3;
								return true;
							}
						}
						goto IL_022a;
					}
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
			_003CConfigErrors_003Ed__4 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<StudyNote> studyNotes = new List<StudyNote>();

	public float? defaultStudyAmount;

	public KnowledgeCategoryDef defaultCategoryOverride;

	public CompProperties_StudyUnlocks()
	{
		compClass = typeof(CompStudyUnlocks);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
