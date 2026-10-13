using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DC RID: 6364
	public class ThoughtWorker_YoungstersHappy : ThoughtWorker_YoungstersMoodBase
	{
		// Token: 0x060090F6 RID: 37110 RVA: 0x002AA7BA File Offset: 0x002A89BA
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildHappy.HappyMoodRange;
		}
	}
}
