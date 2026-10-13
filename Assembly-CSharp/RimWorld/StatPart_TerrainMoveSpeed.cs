using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003727 RID: 14119
	public class StatPart_TerrainMoveSpeed : StatPart
	{
		// Token: 0x0601391E RID: 80158 RVA: 0x000026EA File Offset: 0x000008EA
		public override void TransformValue(StatRequest req, ref float val)
		{
		}

		// Token: 0x0601391F RID: 80159 RVA: 0x005CD6A0 File Offset: 0x005CB8A0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					if (pawn.kindDef.moveSpeedFactorByTerrainTag == null)
					{
						return null;
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, float> keyValuePair in pawn.kindDef.moveSpeedFactorByTerrainTag)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						TaggedString value = ("TerrainTag" + keyValuePair.Key).Translate();
						stringBuilder.AppendLine("StatsReport_TerrainSpeedMultiplier".Translate(value) + ": x" + keyValuePair.Value.ToStringPercent());
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}
	}
}
