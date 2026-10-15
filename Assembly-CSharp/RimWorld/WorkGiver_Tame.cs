using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_Tame : WorkGiver_InteractAnimal
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Tame).GetEnumerator();
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

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__0))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__0(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Tame);
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (!(t is Pawn pawn2) || !TameUtility.CanTame(pawn2))
		{
			return null;
		}
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
		{
			return null;
		}
		if (!CanInteractWithAnimal(pawn, pawn2, forced))
		{
			return null;
		}
		if (TameUtility.TriedToTameTooRecently(pawn2))
		{
			JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans);
			return null;
		}
		Thing thing = null;
		int count = -1;
		if (pawn2.RaceProps.EatsFood && pawn2.needs?.food != null && !HasFoodToInteractAnimal(pawn, pawn2))
		{
			thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn2, desperate: false, out var foodDef, FoodPreferability.RawTasty, allowPlant: false, allowDrug: false, allowCorpse: false, allowDispenserFull: false, allowDispenserEmpty: false, allowForbidden: false, allowSociallyImproper: false, allowHarvest: false, forceScanWholeMap: false, ignoreReservations: false, calculateWantedStackCount: false, FoodPreferability.Undefined, JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f);
			if (thing == null)
			{
				JobFailReason.Is("NoFood".Translate());
				return null;
			}
			float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f;
			float nutrition = FoodUtility.GetNutrition(pawn2, thing, foodDef);
			count = Mathf.CeilToInt(num / nutrition);
		}
		Job job = JobMaker.MakeJob(JobDefOf.Tame, t, null, thing);
		job.count = count;
		return job;
	}
}
