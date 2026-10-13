using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026BB RID: 9915
	public class RitualStage_OnTheThrone : RitualStage
	{
		// Token: 0x0600D961 RID: 55649 RVA: 0x00405B73 File Offset: 0x00403D73
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.GetFirstThing(ritual.Map);
		}
	}
}
