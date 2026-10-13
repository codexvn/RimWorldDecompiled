using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F47 RID: 12103
	public class CompProperties_Loudspeaker : CompProperties
	{
		// Token: 0x06011116 RID: 69910 RVA: 0x004E74D2 File Offset: 0x004E56D2
		public CompProperties_Loudspeaker()
		{
			this.compClass = typeof(CompLoudspeaker);
		}

		// Token: 0x06011117 RID: 69911 RVA: 0x004E74EA File Offset: 0x004E56EA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(CompPowerTrader)) == null)
			{
				yield return "Can't use CompLoudspeaker without CompPowerTrader.";
			}
			yield break;
			yield break;
		}
	}
}
