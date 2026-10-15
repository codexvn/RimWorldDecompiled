using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RoomContents_ReactorCorpseRoom : RoomContents_DeadBody
{
	[CompilerGenerated]
	private sealed class _003CGetPossibleKinds_003Ed__14 : IEnumerable<PawnKindDef>, IEnumerable, IEnumerator<PawnKindDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PawnKindDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		PawnKindDef IEnumerator<PawnKindDef>.Current
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
		public _003CGetPossibleKinds_003Ed__14(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = PawnKindDefOf.Pirate;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<PawnKindDef> IEnumerable<PawnKindDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetPossibleKinds_003Ed__14(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnKindDef>)this).GetEnumerator();
		}
	}

	protected override ThingDef KillerThing => null;

	protected override DamageDef DamageType => DamageDefOf.Bullet;

	protected override Tool ToolUsed => null;

	protected override FloatRange CorpseAgeDaysRange => new FloatRange(300f, 900f);

	protected override IntRange SurvivalPacksCountRange => IntRange.Zero;

	protected override IntRange CorpseRange => new IntRange(4, 6);

	protected override bool AllHaveSameDeathAge => true;

	[IteratorStateMachine(typeof(_003CGetPossibleKinds_003Ed__14))]
	protected override IEnumerable<PawnKindDef> GetPossibleKinds()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPossibleKinds_003Ed__14(-2);
	}
}
