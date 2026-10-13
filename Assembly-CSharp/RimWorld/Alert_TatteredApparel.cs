using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003061 RID: 12385
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x17002CFF RID: 11519
		// (get) Token: 0x0601163C RID: 71228 RVA: 0x004FD0FB File Offset: 0x004FB2FB
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		// Token: 0x0601163D RID: 71229 RVA: 0x004FD102 File Offset: 0x004FB302
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
