using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C44 RID: 11332
	public class CompProperties_Chimera : CompProperties
	{
		// Token: 0x0600FE24 RID: 65060 RVA: 0x0049B515 File Offset: 0x00499715
		public CompProperties_Chimera()
		{
			this.compClass = typeof(CompChimera);
		}

		// Token: 0x0400A696 RID: 42646
		public float rageEndHealthPercentThreshold = 0.98f;

		// Token: 0x0400A697 RID: 42647
		[MustTranslate]
		public string simpleAnimalLabel;
	}
}
