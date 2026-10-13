using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A08 RID: 14856
	public class QuestNode_Set : QuestNode
	{
		// Token: 0x060145C3 RID: 83395 RVA: 0x0061613D File Offset: 0x0061433D
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060145C4 RID: 83396 RVA: 0x00616147 File Offset: 0x00614347
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060145C5 RID: 83397 RVA: 0x00616154 File Offset: 0x00614354
		private void SetVars(Slate slate)
		{
			object obj = this.value.GetValue(slate);
			if (this.convertTo.GetValue(slate) != null)
			{
				obj = ConvertHelper.Convert(obj, this.convertTo.GetValue(slate));
			}
			slate.Set<object>(this.name.GetValue(slate), obj, false);
		}

		// Token: 0x0400E0D4 RID: 57556
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E0D5 RID: 57557
		public SlateRef<object> value;

		// Token: 0x0400E0D6 RID: 57558
		public SlateRef<Type> convertTo;
	}
}
