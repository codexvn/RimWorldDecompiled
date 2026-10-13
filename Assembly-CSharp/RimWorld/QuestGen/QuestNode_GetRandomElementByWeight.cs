using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A72 RID: 14962
	public class QuestNode_GetRandomElementByWeight : QuestNode
	{
		// Token: 0x0601472D RID: 83757 RVA: 0x0061BBDF File Offset: 0x00619DDF
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601472E RID: 83758 RVA: 0x0061BBE9 File Offset: 0x00619DE9
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0601472F RID: 83759 RVA: 0x0061BBF8 File Offset: 0x00619DF8
		private void SetVars(Slate slate)
		{
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		// Token: 0x0400E247 RID: 57927
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E248 RID: 57928
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		// Token: 0x02003A73 RID: 14963
		public class Option
		{
			// Token: 0x0400E249 RID: 57929
			public SlateRef<object> element;

			// Token: 0x0400E24A RID: 57930
			public float weight;
		}
	}
}
