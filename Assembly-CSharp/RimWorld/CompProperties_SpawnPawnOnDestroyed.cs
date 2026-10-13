using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002E41 RID: 11841
	public class CompProperties_SpawnPawnOnDestroyed : CompProperties
	{
		// Token: 0x06010ABA RID: 68282 RVA: 0x004CE48E File Offset: 0x004CC68E
		public CompProperties_SpawnPawnOnDestroyed()
		{
			this.compClass = typeof(CompSpawnPawnOnDestroyed);
		}

		// Token: 0x06010ABB RID: 68283 RVA: 0x004CE4A6 File Offset: 0x004CC6A6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.lordJob != null && !typeof(LordJob).IsAssignableFrom(this.lordJob))
			{
				yield return string.Format("lordJob {0} must be of type LordJob", this.lordJob);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400ADC7 RID: 44487
		public PawnKindDef pawnKind;

		// Token: 0x0400ADC8 RID: 44488
		public Type lordJob;
	}
}
