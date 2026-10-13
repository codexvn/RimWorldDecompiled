using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DD RID: 6365
	public class ThoughtWorker_YoungstersSad : ThoughtWorker_YoungstersMoodBase
	{
		// Token: 0x060090F8 RID: 37112 RVA: 0x002AA806 File Offset: 0x002A8A06
		protected override FloatRange MoodRange()
		{
			return ThoughtWorker_MyChildSad.SadMoodRange;
		}
	}
}
