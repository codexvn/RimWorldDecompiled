using System;

namespace RimWorld
{
	// Token: 0x0200187B RID: 6267
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker_ApparelThought
	{
		// Token: 0x06008FE0 RID: 36832 RVA: 0x002A76E4 File Offset: 0x002A58E4
		protected override bool ApparelCounts(Apparel apparel)
		{
			return apparel.WornByCorpse;
		}
	}
}
