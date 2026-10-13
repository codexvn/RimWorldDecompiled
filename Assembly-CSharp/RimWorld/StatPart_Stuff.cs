using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003725 RID: 14117
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x06013911 RID: 80145 RVA: 0x005CD448 File Offset: 0x005CB648
		public override string ExplanationPart(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.BuildableDef.MadeFromStuff)
			{
				string t = (req.StuffDef != null) ? req.StuffDef.label : "None".TranslateSimple();
				string t2 = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null).ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute) : "0";
				stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + t + "): " + t2);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + this.GetMultiplier(req).ToStringPercent("F0"));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06013912 RID: 80146 RVA: 0x005CD548 File Offset: 0x005CB748
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x06013913 RID: 80147 RVA: 0x005CD587 File Offset: 0x005CB787
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true, -1);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		// Token: 0x06013914 RID: 80148 RVA: 0x005CD5BA File Offset: 0x005CB7BA
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (req.StuffDef != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(req.StuffDef, -1);
			}
			yield break;
		}

		// Token: 0x0400D907 RID: 55559
		public StatDef stuffPowerStat;

		// Token: 0x0400D908 RID: 55560
		public StatDef multiplierStat;
	}
}
