using System;

namespace RimWorld
{
	// Token: 0x02001553 RID: 5459
	public class WorkGiver_TakeRoamingAnimalsToPen : WorkGiver_TakeToPen
	{
		// Token: 0x0600819B RID: 33179 RVA: 0x0027053A File Offset: 0x0026E73A
		public WorkGiver_TakeRoamingAnimalsToPen()
		{
			this.targetRoamingAnimals = true;
			this.allowUnenclosedPens = true;
		}
	}
}
