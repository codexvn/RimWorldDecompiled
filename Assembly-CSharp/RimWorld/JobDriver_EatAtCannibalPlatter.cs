using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011A5 RID: 4517
	public class JobDriver_EatAtCannibalPlatter : JobDriver, IEatingDriver
	{
		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06006E79 RID: 28281 RVA: 0x00227907 File Offset: 0x00225B07
		public bool GainingNutritionNow
		{
			get
			{
				return base.CurToil == this.eating;
			}
		}

		// Token: 0x06006E7A RID: 28282 RVA: 0x00227917 File Offset: 0x00225B17
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x06006E7B RID: 28283 RVA: 0x0022793B File Offset: 0x00225B3B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Cannibal eat job"))
			{
				yield break;
			}
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			float totalBuildingNutrition = base.TargetA.Thing.def.CostList.Sum((ThingDefCountClass x) => x.thingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)x.count);
			this.eating = ToilMaker.MakeToil("MakeNewToils");
			this.eating.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceCell(this.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (this.pawn.needs.food != null)
				{
					this.pawn.needs.food.CurLevel += totalBuildingNutrition / (float)this.pawn.GetLord().ownedPawns.Count / (float)this.eating.defaultDuration * (float)delta;
				}
				if (this.pawn.IsHashIntervalTick(40, delta) && Rand.Value < 0.25f)
				{
					IntVec3 c = Rand.Bool ? this.pawn.Position : this.pawn.RandomAdjacentCellCardinal();
					if (c.InBounds(this.pawn.Map))
					{
						FilthMaker.TryMakeFilth(c, this.pawn.Map, ThingDefOf.Human.race.BloodDef, 1, FilthSourceFlags.None, true);
					}
				}
			};
			this.eating.AddFinishAction(delegate
			{
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
			});
			this.eating.WithEffect(EffecterDefOf.EatMeat, TargetIndex.A, null);
			this.eating.PlaySustainerOrSound(SoundDefOf.RawMeat_Eat, 1f);
			this.eating.handlingFacing = true;
			this.eating.defaultCompleteMode = ToilCompleteMode.Delay;
			this.eating.defaultDuration = (this.job.doUntilGatheringEnded ? this.job.expiryInterval : this.job.def.joyDuration);
			yield return this.eating;
			yield break;
		}

		// Token: 0x04004D22 RID: 19746
		private Toil eating;

		// Token: 0x04004D23 RID: 19747
		private const TargetIndex PlatterIndex = TargetIndex.A;

		// Token: 0x04004D24 RID: 19748
		private const TargetIndex CellIndex = TargetIndex.B;

		// Token: 0x04004D25 RID: 19749
		private const int BloodFilthIntervalTick = 40;

		// Token: 0x04004D26 RID: 19750
		private const float ChanceToProduceBloodFilth = 0.25f;
	}
}
