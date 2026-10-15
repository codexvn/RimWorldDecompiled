using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class RoomContents_DeadBody : RoomContentsWorker
{
	[CompilerGenerated]
	private sealed class _003CGetPossibleKinds_003Ed__17 : IEnumerable<PawnKindDef>, IEnumerable, IEnumerator<PawnKindDef>, IEnumerator, IDisposable
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
		public _003CGetPossibleKinds_003Ed__17(int _003C_003E1__state)
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
				_003C_003E2__current = PawnKindDefOf.Villager;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = PawnKindDefOf.PirateBoss;
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			return new _003CGetPossibleKinds_003Ed__17(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnKindDef>)this).GetEnumerator();
		}
	}

	private int? setAgeTicks;

	protected virtual FloatRange CorpseAgeDaysRange => new FloatRange(3f, 30f);

	protected virtual IntRange SurvivalPacksCountRange => new IntRange(0, 2);

	protected virtual IntRange BloodFilthRange => new IntRange(1, 5);

	protected virtual IntRange CorpseRange => new IntRange(1, 1);

	protected virtual bool AllHaveSameDeathAge => false;

	protected abstract ThingDef KillerThing { get; }

	protected abstract DamageDef DamageType { get; }

	protected abstract Tool ToolUsed { get; }

	[IteratorStateMachine(typeof(_003CGetPossibleKinds_003Ed__17))]
	protected virtual IEnumerable<PawnKindDef> GetPossibleKinds()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPossibleKinds_003Ed__17(-2);
	}

	public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
	{
		SpawnCorpses(map, room);
		base.FillRoom(map, room, faction, threatPoints);
	}

	protected virtual void SpawnCorpses(Map map, LayoutRoom room)
	{
		int randomInRange = CorpseRange.RandomInRange;
		if (AllHaveSameDeathAge)
		{
			setAgeTicks = Mathf.RoundToInt(CorpseAgeDaysRange.RandomInRange * 60000f);
		}
		else
		{
			setAgeTicks = null;
		}
		for (int i = 0; i < randomInRange; i++)
		{
			Corpse corpse = SpawnCorpse(map, room);
			int randomInRange2 = SurvivalPacksCountRange.RandomInRange;
			for (int j = 0; j < randomInRange2; j++)
			{
				GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack), corpse.Position, map, ThingPlaceMode.Near, out var _);
			}
		}
	}

	protected Corpse SpawnCorpse(Map map, LayoutRoom room)
	{
		if (!room.TryGetRandomCellInRoom(map, out var cell, 1))
		{
			return null;
		}
		return SpawnCorpse(cell, map);
	}

	protected Corpse SpawnCorpse(IntVec3 cell, Map map)
	{
		PawnKindDef kind = GetPossibleKinds().RandomElement();
		int deadTicks = setAgeTicks ?? Mathf.RoundToInt(CorpseAgeDaysRange.RandomInRange * 60000f);
		return SpawnCorpse(cell, kind, deadTicks, map, null);
	}

	protected Corpse SpawnCorpse(IntVec3 cell, PawnKindDef kind, int deadTicks, Map map, float? fixedAge = null, bool forceNoGear = false)
	{
		return RoomGenUtility.SpawnCorpse(cell, kind, deadTicks, map, DamageType, fixedAge, forceNoGear, KillerThing, ToolUsed, BloodFilthRange);
	}
}
