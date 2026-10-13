using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200157A RID: 5498
	public class JobDriver_CreateAndEnterHealingPod : JobDriver_CreateAndEnterDryadHolder
	{
		// Token: 0x06008263 RID: 33379 RVA: 0x00272FF2 File Offset: 0x002711F2
		public override Toil EnterToil()
		{
			return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(ThingDefOf.DryadHealingPod, this.job.targetB.Cell, this.pawn.Map, WipeMode.Vanish).TryGetComp<CompDryadHealingPod>().TryAcceptPawn(this.pawn);
			});
		}
	}
}
