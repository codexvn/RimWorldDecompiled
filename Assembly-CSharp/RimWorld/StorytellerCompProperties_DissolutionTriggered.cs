using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E40 RID: 7744
	public class StorytellerCompProperties_DissolutionTriggered : StorytellerCompProperties
	{
		// Token: 0x0600AC51 RID: 44113 RVA: 0x0031402B File Offset: 0x0031222B
		public StorytellerCompProperties_DissolutionTriggered()
		{
			this.compClass = typeof(StorytellerComp_DissolutionTriggered);
		}

		// Token: 0x0400767A RID: 30330
		public IncidentDef incident;

		// Token: 0x0400767B RID: 30331
		public ThingDef thing;

		// Token: 0x0400767C RID: 30332
		public int delayTicks;
	}
}
