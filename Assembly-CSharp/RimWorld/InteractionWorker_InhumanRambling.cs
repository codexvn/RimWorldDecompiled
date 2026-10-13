using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023FC RID: 9212
	public class InteractionWorker_InhumanRambling : InteractionWorker
	{
		// Token: 0x0600C7AA RID: 51114 RVA: 0x003A38C2 File Offset: 0x003A1AC2
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return 0f;
			}
			if (initiator.MentalStateDef != MentalStateDefOf.HumanityBreak)
			{
				return 0f;
			}
			return 999f;
		}

		// Token: 0x04008837 RID: 34871
		private const float SelectionWeight_HumanityBreak = 999f;
	}
}
