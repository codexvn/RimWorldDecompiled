using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompUseEffect_OffsetDeathrestCapacity : CompUseEffect
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__4 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompUseEffect_OffsetDeathrestCapacity _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompUseEffect_OffsetDeathrestCapacity compUseEffect_OffsetDeathrestCapacity = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ModsConfig.BiotechActive)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "DeathrestCapacity".Translate().CapitalizeFirst(), compUseEffect_OffsetDeathrestCapacity.Props.offset.ToStringWithSign(), "DeathrestCapacityDesc".Translate(), 1010);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private CompProperties_UseEffectOffsetDeathrestCapacity Props => (CompProperties_UseEffectOffsetDeathrestCapacity)props;

	public override void DoEffect(Pawn user)
	{
		if (ModsConfig.BiotechActive)
		{
			base.DoEffect(user);
			user.genes?.GetFirstGeneOfType<Gene_Deathrest>()?.OffsetCapacity(Props.offset);
		}
	}

	public override AcceptanceReport CanBeUsedBy(Pawn p)
	{
		if (p.genes?.GetFirstGeneOfType<Gene_Deathrest>() == null)
		{
			return "CannotDeathrest".Translate();
		}
		return base.CanBeUsedBy(p);
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__4))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
