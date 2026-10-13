using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036FE RID: 14078
	public class StatPart_GearStatOffset : StatPart
	{
		// Token: 0x06013857 RID: 79959 RVA: 0x005CADF4 File Offset: 0x005C8FF4
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
					float num = pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1);
					num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[i], this.apparelStat);
					if (this.subtract)
					{
						val -= num;
					}
					else
					{
						val += num;
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float num2 = pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
				num2 += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat);
				if (this.subtract)
				{
					val -= num2;
					return;
				}
				val += num2;
			}
		}

		// Token: 0x06013858 RID: 79960 RVA: 0x005CAF00 File Offset: 0x005C9100
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
					Apparel gear = pawn.apparel.WornApparel[i];
					float num = this.GetStatValueForGear(gear);
					if (this.subtract)
					{
						num = -num;
					}
					if (!Mathf.Approximately(num, 0f))
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(gear, num));
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float num2 = this.GetStatValueForGear(pawn.equipment.Primary);
				if (this.subtract)
				{
					num2 = -num2;
				}
				if (!Mathf.Approximately(num2, 0f))
				{
					stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary, num2));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06013859 RID: 79961 RVA: 0x005CB032 File Offset: 0x005C9232
		private string InfoTextLineFrom(Thing gear, float value)
		{
			return "    " + gear.LabelCap + ": " + value.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x0601385A RID: 79962 RVA: 0x005CB05B File Offset: 0x005C925B
		private float GetStatValueForGear(Thing gear)
		{
			return gear.GetStatValue(this.apparelStat, true, -1) + StatWorker.StatOffsetFromGear(gear, this.apparelStat);
		}

		// Token: 0x0601385B RID: 79963 RVA: 0x005CB078 File Offset: 0x005C9278
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = pawn.apparel.WornApparel[i];
					if (apparel.GetStatValue(this.apparelStat, true, -1) != 0f)
					{
						return true;
					}
					if (StatWorker.StatOffsetFromGear(apparel, this.apparelStat) != 0f)
					{
						return true;
					}
				}
			}
			return this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat) != 0f;
		}

		// Token: 0x0601385C RID: 79964 RVA: 0x005CB123 File Offset: 0x005C9323
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
				if (!Mathf.Approximately(thing2.GetStatValue(this.apparelStat, true, -1), 0f))
				{
					yield return new Dialog_InfoCard.Hyperlink(thing2, -1, false);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400D8B1 RID: 55473
		private StatDef apparelStat;

		// Token: 0x0400D8B2 RID: 55474
		private bool subtract;

		// Token: 0x0400D8B3 RID: 55475
		private bool includeWeapon;
	}
}
