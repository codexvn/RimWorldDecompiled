using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A85 RID: 14981
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		// Token: 0x0601476E RID: 83822 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601476F RID: 83823 RVA: 0x0061C8B8 File Offset: 0x0061AAB8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.site.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeAs.GetValue(slate), this.site.GetValue(slate).ActualThreatPoints, false);
				return;
			}
			float num = 0f;
			IEnumerable<SitePartDefWithParams> value = this.sitePartsParams.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in value)
				{
					num += sitePartDefWithParams.parms.threatPoints;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400E27F RID: 57983
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E280 RID: 57984
		public SlateRef<Site> site;

		// Token: 0x0400E281 RID: 57985
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
