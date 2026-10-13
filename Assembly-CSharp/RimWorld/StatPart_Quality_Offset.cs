using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371A RID: 14106
	public class StatPart_Quality_Offset : StatPart
	{
		// Token: 0x060138E4 RID: 80100 RVA: 0x005CC9D5 File Offset: 0x005CABD5
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += this.QualityOffset(req.QualityCategory);
			}
		}

		// Token: 0x060138E5 RID: 80101 RVA: 0x005CC9F4 File Offset: 0x005CABF4
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_QualityOffset".Translate() + ": " + this.QualityOffset(req.QualityCategory).ToString();
		}

		// Token: 0x060138E6 RID: 80102 RVA: 0x005CCA40 File Offset: 0x005CAC40
		private bool ApplyTo(StatRequest req)
		{
			return this.thingDefs == null || (req.Def != null && this.thingDefs.Contains(req.Def)) || (req.Thing != null && this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x060138E7 RID: 80103 RVA: 0x005CCA98 File Offset: 0x005CAC98
		private float QualityOffset(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.offsetAwful;
			case QualityCategory.Poor:
				return this.offsetPoor;
			case QualityCategory.Normal:
				return this.offsetNormal;
			case QualityCategory.Good:
				return this.offsetGood;
			case QualityCategory.Excellent:
				return this.offsetExcellent;
			case QualityCategory.Masterwork:
				return this.offsetMasterwork;
			case QualityCategory.Legendary:
				return this.offsetLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0400D8F0 RID: 55536
		public float offsetAwful;

		// Token: 0x0400D8F1 RID: 55537
		public float offsetPoor;

		// Token: 0x0400D8F2 RID: 55538
		public float offsetNormal;

		// Token: 0x0400D8F3 RID: 55539
		public float offsetGood;

		// Token: 0x0400D8F4 RID: 55540
		public float offsetExcellent;

		// Token: 0x0400D8F5 RID: 55541
		public float offsetMasterwork;

		// Token: 0x0400D8F6 RID: 55542
		public float offsetLegendary;

		// Token: 0x0400D8F7 RID: 55543
		public List<ThingDef> thingDefs;
	}
}
