using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001579 RID: 5497
	public class JobDriver_CreateAndEnterCocoon : JobDriver_CreateAndEnterDryadHolder
	{
		// Token: 0x06008260 RID: 33376 RVA: 0x00272F9F File Offset: 0x0027119F
		public override Toil EnterToil()
		{
			return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(ThingDefOf.DryadCocoon, this.job.targetB.Cell, this.pawn.Map, WipeMode.Vanish).TryGetComp<CompDryadCocoon>().TryAcceptPawn(this.pawn);
			});
		}
	}
}
