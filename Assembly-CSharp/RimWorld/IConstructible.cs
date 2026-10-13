using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200295C RID: 10588
	public interface IConstructible
	{
		// Token: 0x0600E83D RID: 59453
		List<ThingDefCountClass> TotalMaterialCost();

		// Token: 0x0600E83E RID: 59454
		bool IsCompleted();

		// Token: 0x0600E83F RID: 59455
		int ThingCountNeeded(ThingDef stuff);

		// Token: 0x0600E840 RID: 59456
		ThingDef EntityToBuildStuff();
	}
}
