using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D8 RID: 6360
	public class ThoughtWorker_MyChildSad : ThoughtWorker_RelatedChildMoodBase
	{
		// Token: 0x060090E5 RID: 37093 RVA: 0x002AA806 File Offset: 0x002A8A06
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildSad.SadMoodRange;
		}

		// Token: 0x060090E6 RID: 37094 RVA: 0x002AA7C1 File Offset: 0x002A89C1
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) == 1;
		}

		// Token: 0x060090E7 RID: 37095 RVA: 0x002AA80D File Offset: 0x002A8A0D
		protected override string SingleChildLabel()
		{
			return "MyChildIsSad";
		}

		// Token: 0x04005C63 RID: 23651
		public static readonly FloatRange SadMoodRange = new FloatRange(float.NegativeInfinity, 0.35f);
	}
}
