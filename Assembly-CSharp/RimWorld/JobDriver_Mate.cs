using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001115 RID: 4373
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0021A780 File Offset: 0x00218980
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x0021A7A6 File Offset: 0x002189A6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 500, false, false, false, TargetIndex.None, PathEndMode.Touch);
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(100, delta))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
				if (this.Female.IsHashIntervalTick(100, delta))
				{
					FleckMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			};
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}

		// Token: 0x04004B41 RID: 19265
		private const int MateDuration = 500;

		// Token: 0x04004B42 RID: 19266
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x04004B43 RID: 19267
		public const int TicksBetweenHeartMotes = 100;
	}
}
