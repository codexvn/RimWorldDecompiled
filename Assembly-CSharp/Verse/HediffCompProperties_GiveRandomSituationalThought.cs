using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020006C8 RID: 1736
	public class HediffCompProperties_GiveRandomSituationalThought : HediffCompProperties
	{
		// Token: 0x06002B28 RID: 11048 RVA: 0x000E1FB0 File Offset: 0x000E01B0
		public HediffCompProperties_GiveRandomSituationalThought()
		{
			this.compClass = typeof(HediffComp_GiveRandomSituationalThought);
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000E1FC8 File Offset: 0x000E01C8
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.thoughtDefs.NullOrEmpty<ThoughtDef>())
			{
				yield return "There must be at least one item defined in thoughtDefs";
			}
			yield break;
			yield break;
		}

		// Token: 0x040022D2 RID: 8914
		public List<ThoughtDef> thoughtDefs;
	}
}
