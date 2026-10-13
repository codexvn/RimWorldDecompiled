using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A09 RID: 6665
	public class BossDef : Def
	{
		// Token: 0x06009656 RID: 38486 RVA: 0x002C019E File Offset: 0x002BE39E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (DefDatabase<BossDef>.AllDefs.Any((BossDef b) => b != this && b.kindDef == this.kindDef))
			{
				yield return this.kindDef.label + " appears in two BossDefs, only one PawnKind per boss is currently supported.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040062A3 RID: 25251
		public PawnKindDef kindDef;

		// Token: 0x040062A4 RID: 25252
		public int appearAfterTicks = int.MaxValue;
	}
}
