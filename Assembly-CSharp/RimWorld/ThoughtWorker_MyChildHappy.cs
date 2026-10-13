using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D6 RID: 6358
	public class ThoughtWorker_MyChildHappy : ThoughtWorker_RelatedChildMoodBase
	{
		// Token: 0x060090DE RID: 37086 RVA: 0x002AA7BA File Offset: 0x002A89BA
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildHappy.HappyMoodRange;
		}

		// Token: 0x060090DF RID: 37087 RVA: 0x002AA7C1 File Offset: 0x002A89C1
		protected override bool Active(Pawn parent)
		{
			return base.ChildrenWithMoodCount(parent) == 1;
		}

		// Token: 0x060090E0 RID: 37088 RVA: 0x002AA7CD File Offset: 0x002A89CD
		protected override string SingleChildLabel()
		{
			return "MyChildIsHappy";
		}

		// Token: 0x04005C62 RID: 23650
		public static readonly FloatRange HappyMoodRange = new FloatRange(0.6f, float.PositiveInfinity);
	}
}
