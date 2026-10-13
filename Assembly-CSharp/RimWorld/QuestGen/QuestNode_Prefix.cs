using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A02 RID: 14850
	public class QuestNode_Prefix : QuestNode
	{
		// Token: 0x060145AB RID: 83371 RVA: 0x00615C1C File Offset: 0x00613E1C
		protected override bool TestRunInt(Slate slate)
		{
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, slate);
			if (!value.NullOrEmpty())
			{
				slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, slate);
			}
			return result;
		}

		// Token: 0x060145AC RID: 83372 RVA: 0x00615CA4 File Offset: 0x00613EA4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, QuestGen.slate);
			if (!value.NullOrEmpty())
			{
				QuestGen.slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			try
			{
				this.node.Run();
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					QuestGen.slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, QuestGen.slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, QuestGen.slate);
			}
		}

		// Token: 0x0400E0C1 RID: 57537
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x0400E0C2 RID: 57538
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x0400E0C3 RID: 57539
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x0400E0C4 RID: 57540
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();

		// Token: 0x0400E0C5 RID: 57541
		public QuestNode node;
	}
}
