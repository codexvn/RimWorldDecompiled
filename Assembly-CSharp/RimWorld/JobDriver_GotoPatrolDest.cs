using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200118B RID: 4491
	public class JobDriver_GotoPatrolDest : JobDriver
	{
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06006DD3 RID: 28115 RVA: 0x001E5D08 File Offset: 0x001E3F08
		private LocalTargetInfo Destination
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x00225608 File Offset: 0x00223808
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.Destination.Cell);
			return true;
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x00225645 File Offset: 0x00223845
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return toil;
			Toil toil2 = Toils_General.Wait(Rand.Chance(0.35f) ? JobDriver_GotoPatrolDest.WaitAtDestTicks.RandomInRange : 1, TargetIndex.None);
			yield return toil2;
			yield break;
		}

		// Token: 0x04004CC9 RID: 19657
		private const float WaitAtDestChance = 0.35f;

		// Token: 0x04004CCA RID: 19658
		private static readonly IntRange WaitAtDestTicks = new IntRange(120, 9600);
	}
}
