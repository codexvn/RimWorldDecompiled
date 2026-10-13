using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001436 RID: 5174
	public class JobGiver_DropWeapon : ThinkNode_JobGiver
	{
		// Token: 0x06007D93 RID: 32147 RVA: 0x0025D0E0 File Offset: 0x0025B2E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
				{
					if (thingWithComps.def.IsWeapon)
					{
						return JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps);
					}
				}
			}
			return null;
		}
	}
}
