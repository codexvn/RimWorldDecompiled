using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class SkillNeed_BaseBonus : SkillNeed
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SkillNeed_BaseBonus _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SkillNeed_BaseBonus skillNeed_BaseBonus = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((SkillNeed)skillNeed_BaseBonus).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_00d5;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 1;
					goto IL_00e7;
					IL_00e7:
					if (_003Ci_003E5__3 <= 20)
					{
						if (skillNeed_BaseBonus.ValueAtLevel(_003Ci_003E5__3) <= 0f)
						{
							_003C_003E2__current = "SkillNeed yields factor < 0 at skill level " + _003Ci_003E5__3;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_00d5;
					}
					return false;
					IL_00d5:
					_003Ci_003E5__3++;
					goto IL_00e7;
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
			_003CConfigErrors_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__4(0)
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

	private float baseValue = 0.5f;

	private float bonusPerLevel = 0.05f;

	public override float ValueFor(Pawn pawn)
	{
		if (pawn.skills == null)
		{
			return 1f;
		}
		int level = pawn.skills.GetSkill(skill).Level;
		return ValueAtLevel(level);
	}

	private float ValueAtLevel(int level)
	{
		return baseValue + bonusPerLevel * (float)level;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
