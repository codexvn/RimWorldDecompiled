using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011FC RID: 4604
	public class JobDriver_Floordrawing : JobDriver
	{
		// Token: 0x0600707A RID: 28794 RVA: 0x0022E5F3 File Offset: 0x0022C7F3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(base.TargetB, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600707B RID: 28795 RVA: 0x0022E631 File Offset: 0x0022C831
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnForbidden(TargetIndex.A);
			this.FailOnForbidden(TargetIndex.B);
			this.FailOnChildLearningConditions<JobDriver_Floordrawing>();
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.jobs.posture = PawnPosture.Standing;
			};
			toil.handlingFacing = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceCell(base.TargetB.Cell);
				if (LearningUtility.LearningTickCheckEnd(this.pawn, delta, false))
				{
					return;
				}
				if (this.pawn.IsHashIntervalTick(2500, delta))
				{
					if (!FilthMaker.TryMakeFilth(base.TargetB.Cell, this.pawn.Map, ThingDefOf.Filth_Floordrawing, 1, FilthSourceFlags.None, true))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
						return;
					}
					List<Thing> thingList = base.TargetB.Cell.GetThingList(this.pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == ThingDefOf.Filth_Floordrawing)
						{
							this.pawn.Reserve(thingList[i], this.job, 1, -1, null, true, false);
						}
					}
				}
			};
			toil.WithEffect(EffecterDefOf.Floordrawing, TargetIndex.A, null);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.learningDuration;
			yield return toil;
			yield break;
		}

		// Token: 0x04004E28 RID: 20008
		private const int DrawingIntervalTicks = 2500;
	}
}
