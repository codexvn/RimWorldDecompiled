using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PlantSow : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public JobDriver_PlantSow _003C_003E4__this;

		public Toil sowToil;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return PlantUtility.AdjacentSowBlocker(_003C_003E4__this.job.plantDefToSow, _003C_003E4__this.TargetA.Cell, _003C_003E4__this.Map) != null;
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			return !_003C_003E4__this.job.plantDefToSow.CanNowPlantAt(_003C_003E4__this.TargetLocA, _003C_003E4__this.Map);
		}

		internal bool _003CMakeNewToils_003Eb__2()
		{
			List<Thing> thingList = _003C_003E4__this.TargetA.Cell.GetThingList(_003C_003E4__this.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == _003C_003E4__this.job.plantDefToSow)
				{
					return true;
				}
			}
			return false;
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			_003C_003E4__this.TargetThingA = GenSpawn.Spawn(_003C_003E4__this.job.plantDefToSow, _003C_003E4__this.TargetLocA, _003C_003E4__this.Map);
			_003C_003E4__this.pawn.Reserve(_003C_003E4__this.TargetThingA, sowToil.actor.CurJob);
			Plant plant = _003C_003E4__this.Plant;
			plant.Growth = 0f;
			plant.sown = true;
		}

		internal void _003CMakeNewToils_003Eb__4(int delta)
		{
			Pawn actor = sowToil.actor;
			actor.skills?.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
			Plant plant = _003C_003E4__this.Plant;
			if (plant.LifeStage != 0)
			{
				Log.Error($"{_003C_003E4__this} getting sowing work while not in Sowing life stage.");
			}
			_003C_003E4__this.sowWorkDone += actor.GetStatValue(StatDefOf.PlantWorkSpeed) * (float)delta;
			if (!(_003C_003E4__this.sowWorkDone < plant.def.plant.sowWork))
			{
				plant.Growth = 0.0001f;
				_003C_003E4__this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlagDefOf.Things);
				actor.records.Increment(RecordDefOf.PlantsSown);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, actor.Named(HistoryEventArgsNames.Doer)));
				if (plant.def.plant.humanFoodPlant)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, actor.Named(HistoryEventArgsNames.Doer)));
				}
				_003C_003E4__this.ReadyForNextToil();
			}
		}

		internal float _003CMakeNewToils_003Eb__5()
		{
			return _003C_003E4__this.sowWorkDone / _003C_003E4__this.Plant.def.plant.sowWork;
		}

		internal void _003CMakeNewToils_003Eb__7()
		{
			if (_003C_003E4__this.TargetThingA != null)
			{
				Plant plant = (Plant)sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
				if (_003C_003E4__this.sowWorkDone < plant.def.plant.sowWork && !_003C_003E4__this.TargetThingA.Destroyed)
				{
					_003C_003E4__this.TargetThingA.Destroy();
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlantSow _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn(() => PlantUtility.AdjacentSowBlocker(_003C_003E8__1._003C_003E4__this.job.plantDefToSow, _003C_003E8__1._003C_003E4__this.TargetA.Cell, _003C_003E8__1._003C_003E4__this.Map) != null).FailOn(() => !_003C_003E8__1._003C_003E4__this.job.plantDefToSow.CanNowPlantAt(_003C_003E8__1._003C_003E4__this.TargetLocA, _003C_003E8__1._003C_003E4__this.Map))
					.FailOn((Func<bool>)delegate
					{
						List<Thing> thingList = _003C_003E8__1._003C_003E4__this.TargetA.Cell.GetThingList(_003C_003E8__1._003C_003E4__this.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							if (thingList[i].def == _003C_003E8__1._003C_003E4__this.job.plantDefToSow)
							{
								return true;
							}
						}
						return false;
					});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.sowToil = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.sowToil.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.TargetThingA = GenSpawn.Spawn(_003C_003E8__1._003C_003E4__this.job.plantDefToSow, _003C_003E8__1._003C_003E4__this.TargetLocA, _003C_003E8__1._003C_003E4__this.Map);
					_003C_003E8__1._003C_003E4__this.pawn.Reserve(_003C_003E8__1._003C_003E4__this.TargetThingA, _003C_003E8__1.sowToil.actor.CurJob);
					Plant plant = _003C_003E8__1._003C_003E4__this.Plant;
					plant.Growth = 0f;
					plant.sown = true;
				};
				_003C_003E8__1.sowToil.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.sowToil.actor;
					actor.skills?.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
					Plant plant2 = _003C_003E8__1._003C_003E4__this.Plant;
					if (plant2.LifeStage != 0)
					{
						Log.Error($"{_003C_003E8__1._003C_003E4__this} getting sowing work while not in Sowing life stage.");
					}
					_003C_003E8__1._003C_003E4__this.sowWorkDone = _003C_003E8__1._003C_003E4__this.sowWorkDone + actor.GetStatValue(StatDefOf.PlantWorkSpeed) * (float)delta;
					if (!(_003C_003E8__1._003C_003E4__this.sowWorkDone < plant2.def.plant.sowWork))
					{
						plant2.Growth = 0.0001f;
						_003C_003E8__1._003C_003E4__this.Map.mapDrawer.MapMeshDirty(plant2.Position, MapMeshFlagDefOf.Things);
						actor.records.Increment(RecordDefOf.PlantsSown);
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, actor.Named(HistoryEventArgsNames.Doer)));
						if (plant2.def.plant.humanFoodPlant)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, actor.Named(HistoryEventArgsNames.Doer)));
						}
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				_003C_003E8__1.sowToil.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E8__1.sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A, null);
				_003C_003E8__1.sowToil.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.sowWorkDone / _003C_003E8__1._003C_003E4__this.Plant.def.plant.sowWork, interpolateBetweenActorAndTarget: true);
				_003C_003E8__1.sowToil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow);
				_003C_003E8__1.sowToil.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.TargetThingA != null)
					{
						Plant plant3 = (Plant)_003C_003E8__1.sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
						if (_003C_003E8__1._003C_003E4__this.sowWorkDone < plant3.def.plant.sowWork && !_003C_003E8__1._003C_003E4__this.TargetThingA.Destroyed)
						{
							_003C_003E8__1._003C_003E4__this.TargetThingA.Destroy();
						}
					}
				});
				_003C_003E8__1.sowToil.activeSkill = () => SkillDefOf.Plants;
				_003C_003E2__current = _003C_003E8__1.sowToil;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private Plant Plant => (Plant)job.GetTarget(TargetIndex.A).Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref sowWorkDone, "sowWorkDone", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
