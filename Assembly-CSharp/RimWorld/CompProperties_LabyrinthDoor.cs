using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D05 RID: 11525
	public class CompProperties_LabyrinthDoor : CompProperties_Interactable
	{
		// Token: 0x06010317 RID: 66327 RVA: 0x004AECDF File Offset: 0x004ACEDF
		public CompProperties_LabyrinthDoor()
		{
			this.compClass = typeof(CompLabyrinthDoor);
		}

		// Token: 0x0400A91C RID: 43292
		public GraphicData jammed;

		// Token: 0x0400A91D RID: 43293
		public float unlockedChance = 0.3f;
	}
}
