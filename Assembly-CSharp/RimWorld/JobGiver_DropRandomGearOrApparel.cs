using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001527 RID: 5415
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		// Token: 0x060080DC RID: 32988 RVA: 0x0026CF34 File Offset: 0x0026B134
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null && pawn.equipment.HasAnything())
			{
				return JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.AllEquipmentListForReading.RandomElement<ThingWithComps>());
			}
			if (pawn.apparel != null && pawn.apparel.WornApparel.Any<Apparel>())
			{
				return JobMaker.MakeJob(JobDefOf.RemoveApparel, pawn.apparel.WornApparel.RandomElement<Apparel>());
			}
			return null;
		}
	}
}
