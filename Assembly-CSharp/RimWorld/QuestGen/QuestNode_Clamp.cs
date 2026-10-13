using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FD RID: 14845
	public class QuestNode_Clamp : QuestNode
	{
		// Token: 0x0601459B RID: 83355 RVA: 0x00615713 File Offset: 0x00613913
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0601459C RID: 83356 RVA: 0x0061571D File Offset: 0x0061391D
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0601459D RID: 83357 RVA: 0x0061572C File Offset: 0x0061392C
		private void DoWork(Slate slate)
		{
			double num = this.value.GetValue(slate);
			if (this.min.GetValue(slate) != null)
			{
				num = Math.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Math.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<double>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400E0A3 RID: 57507
		public SlateRef<double> value;

		// Token: 0x0400E0A4 RID: 57508
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E0A5 RID: 57509
		public SlateRef<double?> min;

		// Token: 0x0400E0A6 RID: 57510
		public SlateRef<double?> max;
	}
}
