using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class JobDriver_EatAtCannibalPlatter : JobDriver, IEatingDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public JobDriver_EatAtCannibalPlatter _003C_003E4__this;

		public float totalBuildingNutrition;

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			_003C_003E4__this.pawn.rotationTracker.FaceCell(_003C_003E4__this.TargetA.Thing.OccupiedRect().ClosestCellTo(_003C_003E4__this.pawn.Position));
			_003C_003E4__this.pawn.GainComfortFromCellIfPossible(delta);
			if (_003C_003E4__this.pawn.needs.food != null)
			{
				_003C_003E4__this.pawn.needs.food.CurLevel += totalBuildingNutrition / (float)_003C_003E4__this.pawn.GetLord().ownedPawns.Count / (float)_003C_003E4__this.eating.defaultDuration * (float)delta;
			}
			if (_003C_003E4__this.pawn.IsHashIntervalTick(40, delta) && Rand.Value < 0.25f)
			{
				IntVec3 c = (Rand.Bool ? _003C_003E4__this.pawn.Position : _003C_003E4__this.pawn.RandomAdjacentCellCardinal());
				if (c.InBounds(_003C_003E4__this.pawn.Map))
				{
					FilthMaker.TryMakeFilth(c, _003C_003E4__this.pawn.Map, ThingDefOf.Human.race.BloodDef);
				}
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			if (_003C_003E4__this.pawn.mindState != null)
			{
				_003C_003E4__this.pawn.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, _003C_003E4__this.pawn.Named(HistoryEventArgsNames.Doer)));
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EatAtCannibalPlatter _003C_003E4__this;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			JobDriver_EatAtCannibalPlatter jobDriver_EatAtCannibalPlatter = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckIdeology("Cannibal eat job"))
				{
					return false;
				}
				jobDriver_EatAtCannibalPlatter.EndOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.totalBuildingNutrition = jobDriver_EatAtCannibalPlatter.TargetA.Thing.def.CostList.Sum((ThingDefCountClass x) => x.thingDef.GetStatValueAbstract(StatDefOf.Nutrition) * (float)x.count);
				jobDriver_EatAtCannibalPlatter.eating = ToilMaker.MakeToil("MakeNewToils");
				jobDriver_EatAtCannibalPlatter.eating.tickIntervalAction = delegate(int delta)
				{
					_003C_003E8__1._003C_003E4__this.pawn.rotationTracker.FaceCell(_003C_003E8__1._003C_003E4__this.TargetA.Thing.OccupiedRect().ClosestCellTo(_003C_003E8__1._003C_003E4__this.pawn.Position));
					_003C_003E8__1._003C_003E4__this.pawn.GainComfortFromCellIfPossible(delta);
					if (_003C_003E8__1._003C_003E4__this.pawn.needs.food != null)
					{
						_003C_003E8__1._003C_003E4__this.pawn.needs.food.CurLevel += _003C_003E8__1.totalBuildingNutrition / (float)_003C_003E8__1._003C_003E4__this.pawn.GetLord().ownedPawns.Count / (float)_003C_003E8__1._003C_003E4__this.eating.defaultDuration * (float)delta;
					}
					if (_003C_003E8__1._003C_003E4__this.pawn.IsHashIntervalTick(40, delta) && Rand.Value < 0.25f)
					{
						IntVec3 c = (Rand.Bool ? _003C_003E8__1._003C_003E4__this.pawn.Position : _003C_003E8__1._003C_003E4__this.pawn.RandomAdjacentCellCardinal());
						if (c.InBounds(_003C_003E8__1._003C_003E4__this.pawn.Map))
						{
							FilthMaker.TryMakeFilth(c, _003C_003E8__1._003C_003E4__this.pawn.Map, ThingDefOf.Human.race.BloodDef);
						}
					}
				};
				jobDriver_EatAtCannibalPlatter.eating.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.mindState != null)
					{
						_003C_003E8__1._003C_003E4__this.pawn.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
					}
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, _003C_003E8__1._003C_003E4__this.pawn.Named(HistoryEventArgsNames.Doer)));
				});
				jobDriver_EatAtCannibalPlatter.eating.WithEffect(EffecterDefOf.EatMeat, TargetIndex.A, null);
				jobDriver_EatAtCannibalPlatter.eating.PlaySustainerOrSound(SoundDefOf.RawMeat_Eat);
				jobDriver_EatAtCannibalPlatter.eating.handlingFacing = true;
				jobDriver_EatAtCannibalPlatter.eating.defaultCompleteMode = ToilCompleteMode.Delay;
				jobDriver_EatAtCannibalPlatter.eating.defaultDuration = (jobDriver_EatAtCannibalPlatter.job.doUntilGatheringEnded ? jobDriver_EatAtCannibalPlatter.job.expiryInterval : jobDriver_EatAtCannibalPlatter.job.def.joyDuration);
				_003C_003E2__current = jobDriver_EatAtCannibalPlatter.eating;
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private Toil eating;

	private const TargetIndex PlatterIndex = TargetIndex.A;

	private const TargetIndex CellIndex = TargetIndex.B;

	private const int BloodFilthIntervalTick = 40;

	private const float ChanceToProduceBloodFilth = 0.25f;

	public bool GainingNutritionNow => base.CurToil == eating;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.ReserveSittableOrSpot(job.targetB.Cell, job, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
