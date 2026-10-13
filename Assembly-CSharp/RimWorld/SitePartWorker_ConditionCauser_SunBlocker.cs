using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002846 RID: 10310
	public class SitePartWorker_ConditionCauser_SunBlocker : SitePartWorker_ConditionCauser
	{
		// Token: 0x0600E180 RID: 57728 RVA: 0x00421C5C File Offset: 0x0041FE5C
		public override bool IsAvailable()
		{
			if (ModsConfig.IdeologyActive)
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.FreeColonistsAndPrisoners)
					{
						if ((pawn.IsFreeNonSlaveColonist || pawn.IsPrisonerOfColony) && !pawn.IsQuestLodger())
						{
							Ideo ideo = pawn.Ideo;
							if (ideo != null && (ideo.HasMeme(MemeDefOf.Darkness) || ideo.HasMeme(MemeDefOf.Tunneler)))
							{
								return false;
							}
						}
					}
				}
			}
			return base.IsAvailable();
		}
	}
}
