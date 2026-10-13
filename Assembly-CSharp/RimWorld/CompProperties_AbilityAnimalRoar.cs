using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F3 RID: 8691
	public class CompProperties_AbilityAnimalRoar : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE60 RID: 48736 RVA: 0x00373462 File Offset: 0x00371662
		public CompProperties_AbilityAnimalRoar()
		{
			this.compClass = typeof(CompAbilityEffect_AnimalRoar);
		}

		// Token: 0x040082CD RID: 33485
		public SimpleCurve chanceFromHearingCurve = new SimpleCurve();
	}
}
