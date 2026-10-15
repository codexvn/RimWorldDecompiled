using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public abstract class JobDriver_PlantWork : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public JobDriver_PlantWork _003C_003E4__this;

		public Toil cut;

		internal bool _003CMakeNewToils_003Eb__0(Thing t)
		{
			return _003C_003E4__this.Map.designationManager.DesignationOn(t, _003C_003E4__this.RequiredDesignation) != null;
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Pawn actor = cut.actor;
			if (actor.skills != null)
			{
				actor.skills.Learn(SkillDefOf.Plants, _003C_003E4__this.xpPerTick * (float)delta);
			}
			Plant plant = _003C_003E4__this.Plant;
			_003C_003E4__this.workDone += WorkDonePerTick(actor, plant) * (float)delta;
			if (_003C_003E4__this.workDone < plant.def.plant.harvestWork)
			{
				return;
			}
			if (plant.def.plant.harvestedThingDef != null)
			{
				StatDef stat = ((plant.def.plant.harvestedThingDef.IsDrug || plant.def.plant.drugForHarvestPurposes) ? StatDefOf.DrugHarvestYield : StatDefOf.PlantHarvestYield);
				float statValue = actor.GetStatValue(stat);
				if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && !plant.Blighted && Rand.Value > statValue)
				{
					MoteMaker.ThrowText((_003C_003E4__this.pawn.DrawPos + plant.DrawPos) / 2f, _003C_003E4__this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
				}
				else
				{
					int num = plant.YieldNow();
					if (statValue > 1f)
					{
						num = GenMath.RoundRandom((float)num * statValue);
					}
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
						thing.stackCount = num;
						if (actor.Faction != Faction.OfPlayer)
						{
							thing.SetForbidden(value: true);
						}
						Find.QuestManager.Notify_PlantHarvested(actor, thing);
						GenPlace.TryPlaceThing(thing, actor.Position, _003C_003E4__this.Map, ThingPlaceMode.Near, null, null, null);
						actor.records.Increment(RecordDefOf.PlantsHarvested);
					}
					if (plant.HarvestableNow)
					{
						foreach (ThingComp allComp in plant.AllComps)
						{
							foreach (ThingDefCountClass item in allComp.GetAdditionalHarvestYield())
							{
								Thing thing2 = ThingMaker.MakeThing(item.thingDef);
								thing2.stackCount = item.count;
								GenPlace.TryPlaceThing(thing2, actor.Position, _003C_003E4__this.Map, ThingPlaceMode.Near, null, null, null);
							}
						}
					}
				}
			}
			plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
			plant.PlantCollected(_003C_003E4__this.pawn, _003C_003E4__this.PlantDestructionMode);
			_003C_003E4__this.workDone = 0f;
			_003C_003E4__this.ReadyForNextToil();
		}

		internal float _003CMakeNewToils_003Eb__2()
		{
			return _003C_003E4__this.workDone / _003C_003E4__this.Plant.def.plant.harvestWork;
		}

		internal SoundDef _003CMakeNewToils_003Eb__3()
		{
			return _003C_003E4__this.Plant.def.plant.soundHarvesting;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__11 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlantWork _003C_003E4__this;

		private _003C_003Ec__DisplayClass11_0 _003C_003E8__1;

		private Toil _003CinitExtractTargetFromQueue_003E5__2;

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
		public _003CMakeNewToils_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CinitExtractTargetFromQueue_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PlantWork jobDriver_PlantWork = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass11_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_PlantWork.Init();
				_003C_003E2__current = Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003CinitExtractTargetFromQueue_003E5__2 = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (jobDriver_PlantWork.RequiredDesignation != null) ? ((Func<Thing, bool>)((Thing t) => _003C_003E8__1._003C_003E4__this.Map.designationManager.DesignationOn(t, _003C_003E8__1._003C_003E4__this.RequiredDesignation) != null)) : null);
				_003C_003E2__current = _003CinitExtractTargetFromQueue_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil f = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CinitExtractTargetFromQueue_003E5__2);
				if (jobDriver_PlantWork.RequiredDesignation != null)
				{
					f.FailOnThingMissingDesignation(TargetIndex.A, jobDriver_PlantWork.RequiredDesignation);
				}
				_003C_003E2__current = f;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1.cut = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.cut.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.cut.actor;
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Plants, _003C_003E8__1._003C_003E4__this.xpPerTick * (float)delta);
					}
					Plant plant = _003C_003E8__1._003C_003E4__this.Plant;
					_003C_003E8__1._003C_003E4__this.workDone = _003C_003E8__1._003C_003E4__this.workDone + WorkDonePerTick(actor, plant) * (float)delta;
					if (!(_003C_003E8__1._003C_003E4__this.workDone < plant.def.plant.harvestWork))
					{
						if (plant.def.plant.harvestedThingDef != null)
						{
							StatDef stat = ((plant.def.plant.harvestedThingDef.IsDrug || plant.def.plant.drugForHarvestPurposes) ? StatDefOf.DrugHarvestYield : StatDefOf.PlantHarvestYield);
							float statValue = actor.GetStatValue(stat);
							if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && !plant.Blighted && Rand.Value > statValue)
							{
								MoteMaker.ThrowText((_003C_003E8__1._003C_003E4__this.pawn.DrawPos + plant.DrawPos) / 2f, _003C_003E8__1._003C_003E4__this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
							}
							else
							{
								int num2 = plant.YieldNow();
								if (statValue > 1f)
								{
									num2 = GenMath.RoundRandom((float)num2 * statValue);
								}
								if (num2 > 0)
								{
									Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
									thing.stackCount = num2;
									if (actor.Faction != Faction.OfPlayer)
									{
										thing.SetForbidden(value: true);
									}
									Find.QuestManager.Notify_PlantHarvested(actor, thing);
									GenPlace.TryPlaceThing(thing, actor.Position, _003C_003E8__1._003C_003E4__this.Map, ThingPlaceMode.Near, null, null, null);
									actor.records.Increment(RecordDefOf.PlantsHarvested);
								}
								if (plant.HarvestableNow)
								{
									foreach (ThingComp allComp in plant.AllComps)
									{
										foreach (ThingDefCountClass item in allComp.GetAdditionalHarvestYield())
										{
											Thing thing2 = ThingMaker.MakeThing(item.thingDef);
											thing2.stackCount = item.count;
											GenPlace.TryPlaceThing(thing2, actor.Position, _003C_003E8__1._003C_003E4__this.Map, ThingPlaceMode.Near, null, null, null);
										}
									}
								}
							}
						}
						plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
						plant.PlantCollected(_003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1._003C_003E4__this.PlantDestructionMode);
						_003C_003E8__1._003C_003E4__this.workDone = 0f;
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				_003C_003E8__1.cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				if (jobDriver_PlantWork.RequiredDesignation != null)
				{
					_003C_003E8__1.cut.FailOnThingMissingDesignation(TargetIndex.A, jobDriver_PlantWork.RequiredDesignation);
				}
				_003C_003E8__1.cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.cut.defaultCompleteMode = ToilCompleteMode.Never;
				Toil toil2 = _003C_003E8__1.cut;
				Plant plant2 = jobDriver_PlantWork.Plant;
				toil2.WithEffect((plant2 != null && plant2.def.plant.IsTree) ? EffecterDefOf.Harvest_Tree : EffecterDefOf.Harvest_Plant, TargetIndex.A, null);
				_003C_003E8__1.cut.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.workDone / _003C_003E8__1._003C_003E4__this.Plant.def.plant.harvestWork, interpolateBetweenActorAndTarget: true);
				_003C_003E8__1.cut.PlaySustainerOrSound(() => _003C_003E8__1._003C_003E4__this.Plant.def.plant.soundHarvesting);
				_003C_003E8__1.cut.activeSkill = () => SkillDefOf.Plants;
				_003C_003E2__current = _003C_003E8__1.cut;
				_003C_003E1__state = 6;
				return true;
			}
			case 6:
			{
				_003C_003E1__state = -1;
				Toil toil = jobDriver_PlantWork.PlantWorkDoneToil();
				if (toil != null)
				{
					_003C_003E2__current = toil;
					_003C_003E1__state = 7;
					return true;
				}
				goto IL_02d2;
			}
			case 7:
				_003C_003E1__state = -1;
				goto IL_02d2;
			case 8:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_02d2:
				_003C_003E2__current = Toils_Jump.Jump(_003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E1__state = 8;
				return true;
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
			_003CMakeNewToils_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__11(0)
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

	private float workDone;

	protected float xpPerTick;

	protected const TargetIndex PlantInd = TargetIndex.A;

	protected Plant Plant => (Plant)job.targetA.Thing;

	protected virtual DesignationDef RequiredDesignation => null;

	protected virtual PlantDestructionMode PlantDestructionMode => PlantDestructionMode.Smash;

	public static float WorkDonePerTick(Pawn actor, Plant plant)
	{
		return actor.GetStatValue(StatDefOf.PlantWorkSpeed) * Mathf.Lerp(3.3f, 1f, plant.Growth);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		LocalTargetInfo target = job.GetTarget(TargetIndex.A);
		if (target.IsValid && !pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__11))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workDone, "workDone", 0f);
	}

	protected virtual void Init()
	{
	}

	protected virtual Toil PlantWorkDoneToil()
	{
		return null;
	}
}
