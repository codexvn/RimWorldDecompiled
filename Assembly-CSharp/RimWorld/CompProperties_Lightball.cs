using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F43 RID: 12099
	public class CompProperties_Lightball : CompProperties
	{
		// Token: 0x060110FE RID: 69886 RVA: 0x004E6F57 File Offset: 0x004E5157
		public CompProperties_Lightball()
		{
			this.compClass = typeof(CompLightball);
		}

		// Token: 0x060110FF RID: 69887 RVA: 0x004E6F6F File Offset: 0x004E516F
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(CompPowerTrader)) == null)
			{
				yield return "Can't use CompLightball without CompPowerTrader.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B133 RID: 45363
		public List<SoundDef> soundDefsPerSpeakerCount;

		// Token: 0x0400B134 RID: 45364
		public int maxSpeakerDistance;
	}
}
