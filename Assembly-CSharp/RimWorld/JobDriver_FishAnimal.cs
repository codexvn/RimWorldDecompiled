using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200127A RID: 4730
	public class JobDriver_FishAnimal : JobDriver
	{
		// Token: 0x060073B8 RID: 29624 RVA: 0x0023881C File Offset: 0x00236A1C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x00238871 File Offset: 0x00236A71
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			int ticks = Mathf.RoundToInt(1500f / this.pawn.GetStatValue(StatDefOf.FishingSpeed, true, -1));
			Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, false, true, false, TargetIndex.A, PathEndMode.Touch);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			yield return this.CompleteFishingToil();
			yield break;
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x00238881 File Offset: 0x00236A81
		private Toil CompleteFishingToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
			toil.initAction = delegate()
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.A).Cell;
				bool flag;
				List<Thing> catchesFor = FishingUtility.GetCatchesFor(this.pawn, cell, true, out flag);
				if (!catchesFor.Any<Thing>())
				{
					return;
				}
				bool flag2 = false;
				int num = catchesFor.Sum((Thing x) => x.stackCount);
				foreach (Thing thing in catchesFor)
				{
					flag2 |= GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				if (flag2)
				{
					this.pawn.Map.waterBodyTracker.Notify_Fished(cell, (float)num);
				}
			};
			return toil;
		}

		// Token: 0x04004FB4 RID: 20404
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x04004FB5 RID: 20405
		private const TargetIndex StandInd = TargetIndex.B;
	}
}
