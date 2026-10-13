using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A80 RID: 14976
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		// Token: 0x0601475D RID: 83805 RVA: 0x0061C508 File Offset: 0x0061A708
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601475E RID: 83806 RVA: 0x0061C512 File Offset: 0x0061A712
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0601475F RID: 83807 RVA: 0x0061C520 File Offset: 0x0061A720
		private void SetVars(Slate slate)
		{
			float num = 1f;
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDef sitePartDef in value)
				{
					num *= sitePartDef.activeThreatDisturbanceFactor;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400E26E RID: 57966
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E26F RID: 57967
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
