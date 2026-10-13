using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A33 RID: 14899
	public class QuestNode_EvaluateSimpleCurve : QuestNode
	{
		// Token: 0x0601464E RID: 83534 RVA: 0x00617FD1 File Offset: 0x006161D1
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601464F RID: 83535 RVA: 0x00617FDB File Offset: 0x006161DB
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014650 RID: 83536 RVA: 0x00617FE8 File Offset: 0x006161E8
		private void SetVars(Slate slate)
		{
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400E16A RID: 57706
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E16B RID: 57707
		public SlateRef<SimpleCurve> curve;

		// Token: 0x0400E16C RID: 57708
		public SlateRef<float> value;

		// Token: 0x0400E16D RID: 57709
		public SlateRef<bool> roundRandom;
	}
}
