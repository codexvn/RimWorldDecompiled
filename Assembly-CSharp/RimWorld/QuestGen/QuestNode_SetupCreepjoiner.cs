using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B3F RID: 15167
	public class QuestNode_SetupCreepjoiner : QuestNode
	{
		// Token: 0x060149F2 RID: 84466 RVA: 0x006284DC File Offset: 0x006266DC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			CreepJoinerFormKindDef value = this.form.GetValue(slate);
			CreepJoinerBenefitDef value2 = this.benefit.GetValue(slate);
			CreepJoinerDownsideDef value3 = this.downside.GetValue(slate);
			CreepJoinerAggressiveDef value4 = this.aggressive.GetValue(slate);
			CreepJoinerRejectionDef value5 = this.rejection.GetValue(slate);
			CreepJoinerUtility.GetCreepjoinerSpecifics(map, ref value, ref value2, ref value3, ref value4, ref value5);
			if (this.storeFormAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerFormKindDef>(this.storeFormAs.GetValue(slate), value, false);
			}
			if (this.storeBenefitAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerBenefitDef>(this.storeBenefitAs.GetValue(slate), value2, false);
			}
			if (this.storeDownsideAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerDownsideDef>(this.storeDownsideAs.GetValue(slate), value3, false);
			}
			if (this.storeAggressiveAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerAggressiveDef>(this.storeAggressiveAs.GetValue(slate), value4, false);
			}
			if (this.storeRejectionAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerRejectionDef>(this.storeRejectionAs.GetValue(slate), value5, false);
			}
		}

		// Token: 0x060149F3 RID: 84467 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E4EC RID: 58604
		public SlateRef<CreepJoinerFormKindDef> form;

		// Token: 0x0400E4ED RID: 58605
		public SlateRef<CreepJoinerBenefitDef> benefit;

		// Token: 0x0400E4EE RID: 58606
		public SlateRef<CreepJoinerDownsideDef> downside;

		// Token: 0x0400E4EF RID: 58607
		public SlateRef<CreepJoinerAggressiveDef> aggressive;

		// Token: 0x0400E4F0 RID: 58608
		public SlateRef<CreepJoinerRejectionDef> rejection;

		// Token: 0x0400E4F1 RID: 58609
		[NoTranslate]
		public SlateRef<string> storeFormAs;

		// Token: 0x0400E4F2 RID: 58610
		[NoTranslate]
		public SlateRef<string> storeBenefitAs;

		// Token: 0x0400E4F3 RID: 58611
		[NoTranslate]
		public SlateRef<string> storeDownsideAs;

		// Token: 0x0400E4F4 RID: 58612
		[NoTranslate]
		public SlateRef<string> storeAggressiveAs;

		// Token: 0x0400E4F5 RID: 58613
		[NoTranslate]
		public SlateRef<string> storeRejectionAs;
	}
}
