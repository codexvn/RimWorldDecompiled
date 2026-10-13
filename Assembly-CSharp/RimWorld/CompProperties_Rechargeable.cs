using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DD6 RID: 11734
	public class CompProperties_Rechargeable : CompProperties
	{
		// Token: 0x06010803 RID: 67587 RVA: 0x004C248E File Offset: 0x004C068E
		public CompProperties_Rechargeable()
		{
			this.compClass = typeof(CompRechargeable);
		}

		// Token: 0x06010804 RID: 67588 RVA: 0x004C24A6 File Offset: 0x004C06A6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.ticksToRecharge <= 0)
			{
				yield return "ticksToRecharge must be a positive value";
			}
			yield break;
		}

		// Token: 0x0400AC17 RID: 44055
		public int ticksToRecharge;

		// Token: 0x0400AC18 RID: 44056
		public SoundDef chargedSoundDef;

		// Token: 0x0400AC19 RID: 44057
		public SoundDef dischargeSoundDef;
	}
}
