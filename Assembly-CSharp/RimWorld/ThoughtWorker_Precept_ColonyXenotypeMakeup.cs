using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001839 RID: 6201
	public class ThoughtWorker_Precept_ColonyXenotypeMakeup : ThoughtWorker_Precept
	{
		// Token: 0x06008F41 RID: 36673 RVA: 0x002A55A8 File Offset: 0x002A37A8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.Faction == null)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Ideo.PreferredXenotypes.Any<XenotypeDef>() && !p.Ideo.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return ThoughtState.Inactive;
			}
			List<Pawn> list = p.MapHeld.mapPawns.SpawnedPawnsInFaction(p.Faction);
			int num = 0;
			int num2 = 0;
			bool flag = p.IsSlave || p.IsPrisoner;
			foreach (Pawn pawn in list)
			{
				bool flag2 = pawn.IsSlave || pawn.IsPrisoner;
				if (pawn.genes != null && flag == flag2)
				{
					num++;
					if (!p.Ideo.IsPreferredXenotype(pawn))
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			float num3 = (float)num2 / (float)num;
			if (num3 < 0.33f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num3 < 0.66f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
