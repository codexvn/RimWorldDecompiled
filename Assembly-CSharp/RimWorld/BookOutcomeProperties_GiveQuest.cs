using System;

namespace RimWorld
{
	// Token: 0x02001936 RID: 6454
	public class BookOutcomeProperties_GiveQuest : BookOutcomeProperties
	{
		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x060092C3 RID: 37571 RVA: 0x002B36DD File Offset: 0x002B18DD
		public override Type DoerClass
		{
			get
			{
				return typeof(BookOutcomeDoer_GiveQuest);
			}
		}

		// Token: 0x04005D9F RID: 23967
		public float questChance;
	}
}
