using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036FC RID: 14076
	public class StatPart_GearStatFactor : StatPart
	{
		// Token: 0x06013849 RID: 79945 RVA: 0x005CA9C4 File Offset: 0x005C8BC4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing || req.Thing == null)
			{
				return;
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					val *= pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1);
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				val *= pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
			}
		}

		// Token: 0x0601384A RID: 79946 RVA: 0x005CAA78 File Offset: 0x005C8C78
		public override string ExplanationPart(StatRequest req)
		{
			if (!req.HasThing || req.Thing == null)
			{
				return null;
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null || !this.PawnWearingRelevantGear(pawn))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = pawn.apparel.WornApparel[i];
					float statValue = apparel.GetStatValue(this.apparelStat, true, -1);
					if (!Mathf.Approximately(statValue, 1f))
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(apparel, statValue));
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float statValue2 = pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
				if (!Mathf.Approximately(statValue2, 1f))
				{
					stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary, statValue2));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601384B RID: 79947 RVA: 0x005CAB98 File Offset: 0x005C8D98
		private string InfoTextLineFrom(Thing gear, float value)
		{
			return "    " + gear.LabelCap + ": " + value.ToStringByStyle(this.apparelStat.toStringStyle, ToStringNumberSense.Factor);
		}

		// Token: 0x0601384C RID: 79948 RVA: 0x005CABC4 File Offset: 0x005C8DC4
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					if (!Mathf.Approximately(pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1), 1f))
					{
						return true;
					}
				}
			}
			return this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && !Mathf.Approximately(pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1), 1f);
		}

		// Token: 0x0601384D RID: 79949 RVA: 0x005CAC64 File Offset: 0x005C8E64
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			Thing thing = req.Thing;
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				yield break;
			}
			if (pawn.apparel == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i = num + 1)
			{
				Apparel thing2 = pawn.apparel.WornApparel[i];
				if (!Mathf.Approximately(thing2.GetStatValue(this.apparelStat, true, -1), 1f))
				{
					yield return new Dialog_InfoCard.Hyperlink(thing2, -1, false);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400D8A7 RID: 55463
		private StatDef apparelStat;

		// Token: 0x0400D8A8 RID: 55464
		private bool includeWeapon;
	}
}
