using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PlantSeed : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlantSeed _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
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
			JobDriver_PlantSeed CS_0024_003C_003E8__locals29 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals29.FailOn(() => PlantUtility.AdjacentSowBlocker(CS_0024_003C_003E8__locals29.job.plantDefToSow, CS_0024_003C_003E8__locals29.PlantCell, CS_0024_003C_003E8__locals29.Map) != null || !CS_0024_003C_003E8__locals29.job.plantDefToSow.CanEverPlantAt(CS_0024_003C_003E8__locals29.PlantCell, CS_0024_003C_003E8__locals29.Map, canWipePlantsExceptTree: false, checkMapTemperature: false) || !CS_0024_003C_003E8__locals29.PlantableComp.PlantCells.Contains(CS_0024_003C_003E8__locals29.PlantCell));
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					LocalTargetInfo target = CS_0024_003C_003E8__locals29.job.GetTarget(TargetIndex.A);
					if (target.HasThing && target.Thing.stackCount > CS_0024_003C_003E8__locals29.job.count)
					{
						CS_0024_003C_003E8__locals29.job.SetTarget(TargetIndex.C, target.Thing);
					}
				});
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					LocalTargetInfo target2 = CS_0024_003C_003E8__locals29.job.GetTarget(TargetIndex.C);
					if (target2.IsValid && target2.HasThing)
					{
						target2.Thing.TryGetComp<CompPlantable>()?.Notify_SeedRemovedFromStackForPlantingAt(CS_0024_003C_003E8__locals29.PlantCell);
					}
					CS_0024_003C_003E8__locals29.PlantableComp?.Notify_RemovedFromStackForPlantingAt(CS_0024_003C_003E8__locals29.PlantCell);
				});
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 5;
				return true;
			case 5:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals29.sowWorkDone += CS_0024_003C_003E8__locals29.pawn.GetStatValue(StatDefOf.PlantWorkSpeed) * (float)delta;
					if (CS_0024_003C_003E8__locals29.pawn.skills != null)
					{
						CS_0024_003C_003E8__locals29.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
					}
					if (CS_0024_003C_003E8__locals29.sowWorkDone >= CS_0024_003C_003E8__locals29.job.plantDefToSow.plant.sowWork)
					{
						CS_0024_003C_003E8__locals29.ReadyForNextToil();
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.WithEffect(EffecterDefOf.Sow, TargetIndex.B, null);
				toil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow);
				toil.WithProgressBar(TargetIndex.B, () => CS_0024_003C_003E8__locals29.sowWorkDone / CS_0024_003C_003E8__locals29.job.plantDefToSow.plant.sowWork, interpolateBetweenActorAndTarget: true);
				toil.activeSkill = () => SkillDefOf.Plants;
				_003C_003E2__current = toil;
				_003C_003E1__state = 6;
				return true;
			}
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals29.PlantableComp.DoPlant(CS_0024_003C_003E8__locals29.pawn, CS_0024_003C_003E8__locals29.PlantCell, CS_0024_003C_003E8__locals29.pawn.Map);
				});
				_003C_003E1__state = 7;
				return true;
			case 7:
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private float sowWorkDone;

	private const TargetIndex SeedIndex = TargetIndex.A;

	private const TargetIndex PlantCellIndex = TargetIndex.B;

	private const TargetIndex OldSeedStackIndex = TargetIndex.C;

	private IntVec3 PlantCell => job.GetTarget(TargetIndex.B).Cell;

	private CompPlantable PlantableComp => job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompPlantable>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, 1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref sowWorkDone, "sowWorkDone", 0f);
	}
}
