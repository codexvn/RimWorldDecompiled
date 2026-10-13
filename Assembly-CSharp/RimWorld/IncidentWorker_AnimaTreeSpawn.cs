using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD9 RID: 7641
	public class IncidentWorker_AnimaTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600AA67 RID: 43623 RVA: 0x0030BD0C File Offset: 0x00309F0C
		protected override bool SendLetter
		{
			get
			{
				return PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn c) => c.HasPsylink && MeditationFocusDefOf.Natural.CanPawnUse(c));
			}
		}
	}
}
