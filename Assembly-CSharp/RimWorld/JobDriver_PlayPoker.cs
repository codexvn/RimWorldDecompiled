using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011E8 RID: 4584
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x06006FF7 RID: 28663 RVA: 0x0022CD54 File Offset: 0x0022AF54
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position), null);
		}
	}
}
