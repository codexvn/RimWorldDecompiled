using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_HunterHunt : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Designation> _003C_003E7__wrap1;

		Thing IEnumerator<Thing>.Current
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
		public _003CPotentialWorkThingsGlobal_003Ed__0(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.target.Thing;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialWorkThingsGlobal_003Ed__0 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__0(0);
			}
			_003CPotentialWorkThingsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkThingsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public override PathEndMode PathEndMode => PathEndMode.OnCell;

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__0))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__0(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override Danger MaxPathDanger(Pawn pawn)
	{
		return Danger.Deadly;
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		if (!HasHuntingWeapon(pawn))
		{
			return true;
		}
		if (HasShieldAndRangedWeapon(pawn))
		{
			return true;
		}
		if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt))
		{
			return true;
		}
		return false;
	}

	public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (!(t is Pawn pawn2) || !pawn2.AnimalOrWildMan())
		{
			return false;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) == null)
		{
			return false;
		}
		if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return false;
		}
		if (pawn.Ideo != null && pawn.Ideo.IsVeneratedAnimal(pawn2) && !new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return false;
		}
		if (!CanFindHuntingPosition(pawn, pawn2))
		{
			return false;
		}
		return true;
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		return JobMaker.MakeJob(JobDefOf.Hunt, t);
	}

	public static bool HasHuntingWeapon(Pawn p)
	{
		if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth() && !p.equipment.PrimaryEq.PrimaryVerb.UsesExplosiveProjectiles())
		{
			return true;
		}
		return false;
	}

	private bool CanFindHuntingPosition(Pawn hunter, Pawn animal)
	{
		CastPositionRequest castPositionRequest = default(CastPositionRequest);
		castPositionRequest.caster = hunter;
		castPositionRequest.target = animal;
		castPositionRequest.verb = hunter.TryGetAttackVerb(animal);
		castPositionRequest.wantCoverFromTarget = false;
		CastPositionRequest newReq = castPositionRequest;
		newReq.maxRangeFromTarget = (animal.Downed ? Mathf.Min(newReq.verb.EffectiveRange, animal.RaceProps.executionRange) : Mathf.Max(newReq.verb.EffectiveRange * 0.95f, 1.42f));
		IntVec3 dest;
		return CastPositionFinder.TryFindCastPosition(newReq, out dest);
	}

	public static bool HasShieldAndRangedWeapon(Pawn p)
	{
		if (p.equipment.Primary != null && p.equipment.Primary.def.IsWeaponUsingProjectiles)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].def.IsShieldThatBlocksRanged)
				{
					return true;
				}
			}
		}
		return false;
	}
}
