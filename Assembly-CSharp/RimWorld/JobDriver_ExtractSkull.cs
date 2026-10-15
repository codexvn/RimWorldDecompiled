using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ExtractSkull : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExtractSkull _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ExtractSkull CS_0024_003C_003E8__locals23 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals23.FailOn(() => CS_0024_003C_003E8__locals23.Corpse.Destroyed || !CS_0024_003C_003E8__locals23.Corpse.Spawned);
				CS_0024_003C_003E8__locals23.FailOn(() => CS_0024_003C_003E8__locals23.Map.designationManager.DesignationOn(CS_0024_003C_003E8__locals23.TargetA.Thing, DesignationDefOf.ExtractSkull) == null);
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(180);
				toil.WithProgressBarToilDelay(TargetIndex.A);
				toil.PlaySustainerOrSound(SoundDefOf.Recipe_Surgery);
				toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals23.Corpse.CurRotDrawMode != RotDrawMode.Dessicated && Rand.Chance(1f / 60f * (float)delta))
					{
						IntVec3 randomCell = new CellRect(CS_0024_003C_003E8__locals23.Corpse.PositionHeld.x - 1, CS_0024_003C_003E8__locals23.Corpse.PositionHeld.z - 1, 3, 3).RandomCell;
						if (randomCell.InBounds(CS_0024_003C_003E8__locals23.Map) && GenSight.LineOfSight(randomCell, CS_0024_003C_003E8__locals23.Corpse.PositionHeld, CS_0024_003C_003E8__locals23.Map))
						{
							FilthMaker.TryMakeFilth(randomCell, CS_0024_003C_003E8__locals23.Corpse.MapHeld, CS_0024_003C_003E8__locals23.Corpse.InnerPawn.RaceProps.BloodDef, CS_0024_003C_003E8__locals23.Corpse.InnerPawn.LabelIndefinite());
						}
					}
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals23.Corpse.InnerPawn.health.AddHediff(HediffDefOf.MissingBodyPart, CS_0024_003C_003E8__locals23.Corpse.InnerPawn.health.hediffSet.GetNotMissingParts().First((BodyPartRecord p) => p.def == BodyPartDefOf.Head), null);
					CS_0024_003C_003E8__locals23.Corpse.Map.designationManager.RemoveDesignation(CS_0024_003C_003E8__locals23.Corpse.Map.designationManager.DesignationOn(CS_0024_003C_003E8__locals23.Corpse, DesignationDefOf.ExtractSkull));
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Skull);
					thing.TryGetComp<CompHasSources>()?.AddSource(CS_0024_003C_003E8__locals23.Corpse.InnerPawn.LabelShort);
					GenPlace.TryPlaceThing(thing, CS_0024_003C_003E8__locals23.Corpse.PositionHeld, CS_0024_003C_003E8__locals23.Corpse.Map, ThingPlaceMode.Near, null, null, null);
				});
				_003C_003E1__state = 4;
				return true;
			case 4:
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

	public const int ExtractionTimeTicks = 180;

	private const TargetIndex CorpseInd = TargetIndex.A;

	protected Corpse Corpse => (Corpse)job.targetA.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
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
