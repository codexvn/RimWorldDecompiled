using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A0C RID: 14860
	public class QuestNode_SubScript : QuestNode
	{
		// Token: 0x060145D0 RID: 83408 RVA: 0x006165CC File Offset: 0x006147CC
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
				result = this.def.GetValue(slate).root.TestRun(slate);
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

		// Token: 0x060145D1 RID: 83409 RVA: 0x00616660 File Offset: 0x00614860
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
				this.def.GetValue(slate).Run();
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

		// Token: 0x060145D2 RID: 83410 RVA: 0x00616704 File Offset: 0x00614904
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " (";
			SlateRef<QuestScriptDef> slateRef = this.def;
			return str + str2 + slateRef.ToString() + ")";
		}

		// Token: 0x0400E0E6 RID: 57574
		[TranslationHandle(Priority = 100)]
		public SlateRef<QuestScriptDef> def;

		// Token: 0x0400E0E7 RID: 57575
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x0400E0E8 RID: 57576
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x0400E0E9 RID: 57577
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x0400E0EA RID: 57578
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();
	}
}
