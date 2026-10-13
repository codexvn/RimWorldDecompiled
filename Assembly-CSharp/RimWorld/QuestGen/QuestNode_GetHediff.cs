using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A47 RID: 14919
	public class QuestNode_GetHediff : QuestNode
	{
		// Token: 0x06014693 RID: 83603 RVA: 0x00618D22 File Offset: 0x00616F22
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014694 RID: 83604 RVA: 0x00618D2C File Offset: 0x00616F2C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014695 RID: 83605 RVA: 0x00618D3C File Offset: 0x00616F3C
		private void SetVars(Slate slate)
		{
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		// Token: 0x0400E1A9 RID: 57769
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		// Token: 0x0400E1AA RID: 57770
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		// Token: 0x0400E1AB RID: 57771
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		// Token: 0x02003A48 RID: 14920
		public class Option
		{
			// Token: 0x0400E1AC RID: 57772
			public HediffDef def;

			// Token: 0x0400E1AD RID: 57773
			public List<BodyPartDef> partsToAffect;

			// Token: 0x0400E1AE RID: 57774
			public float weight = 1f;
		}
	}
}
