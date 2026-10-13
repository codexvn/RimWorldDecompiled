using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001802 RID: 6146
	public class ThoughtWorker_Precept_IdeoDiversity_Uniform : ThoughtWorker_Precept
	{
		// Token: 0x06008E8E RID: 36494 RVA: 0x002A3620 File Offset: 0x002A1820
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || !p.IsColonist)
			{
				return false;
			}
			List<Pawn> list = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != p && list[i].RaceProps.Humanlike && !list[i].IsSlave && !list[i].IsQuestLodger() && !list[i].IsSubhuman && !list[i].DevelopmentalStage.Baby())
				{
					if (list[i].Ideo != p.Ideo)
					{
						return false;
					}
					num++;
				}
			}
			return num > 0;
		}
	}
}
