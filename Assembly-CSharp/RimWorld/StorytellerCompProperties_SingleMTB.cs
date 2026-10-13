using System;

namespace RimWorld
{
	// Token: 0x02001E6E RID: 7790
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x0600AD0F RID: 44303 RVA: 0x003160BF File Offset: 0x003142BF
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x04007706 RID: 30470
		public IncidentDef incident;

		// Token: 0x04007707 RID: 30471
		public float mtbDays = 13f;
	}
}
