using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002409 RID: 9225
	public class InteractionWorker_SanguophageTalk : InteractionWorker
	{
		// Token: 0x0600C7D8 RID: 51160 RVA: 0x003A5C98 File Offset: 0x003A3E98
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (initiator.genes == null || recipient.genes == null || initiator.genes.Xenotype != XenotypeDefOf.Sanguophage || recipient.genes.Xenotype != XenotypeDefOf.Sanguophage)
			{
				return 0f;
			}
			Lord lord = initiator.GetLord();
			Lord lord2 = recipient.GetLord();
			if (lord == null || lord2 == null || lord != lord2)
			{
				return 0f;
			}
			if (lord.CurLordToil == null || !(lord.CurLordToil is LordToil_SanguophageMeeting))
			{
				return 0f;
			}
			return 999f;
		}

		// Token: 0x04008858 RID: 34904
		private const float SelectionWeight_SanguophageMeeting = 999f;
	}
}
