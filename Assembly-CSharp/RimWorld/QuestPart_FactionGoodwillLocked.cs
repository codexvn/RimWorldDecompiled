using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_FactionGoodwillLocked : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__3 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_FactionGoodwillLocked _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		Faction IEnumerator<Faction>.Current
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
		public _003Cget_InvolvedFactions_003Ed__3(int _003C_003E1__state)
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
				QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_FactionGoodwillLocked).InvolvedFactions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0073;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0073;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b3;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0073:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Faction current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_FactionGoodwillLocked.faction1 != null)
					{
						_003C_003E2__current = questPart_FactionGoodwillLocked.faction1;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b3;
					IL_00b3:
					if (questPart_FactionGoodwillLocked.faction2 != null)
					{
						_003C_003E2__current = questPart_FactionGoodwillLocked.faction2;
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
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
		IEnumerator<Faction> IEnumerable<Faction>.GetEnumerator()
		{
			_003Cget_InvolvedFactions_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Faction>)this).GetEnumerator();
		}
	}

	public Faction faction1;

	public Faction faction2;

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_FactionRemoved(Faction faction)
	{
		if (faction == faction1)
		{
			faction1 = null;
		}
		if (faction == faction2)
		{
			faction2 = null;
		}
	}

	public bool AppliesTo(Faction a, Faction b)
	{
		if (base.State != QuestPartState.Enabled)
		{
			return false;
		}
		if (faction1 != a || faction2 != b)
		{
			if (faction1 == b)
			{
				return faction2 == a;
			}
			return false;
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref faction1, "faction1");
		Scribe_References.Look(ref faction2, "faction2");
	}
}
