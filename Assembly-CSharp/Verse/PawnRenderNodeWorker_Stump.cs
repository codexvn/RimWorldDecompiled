using System;

namespace Verse
{
	// Token: 0x0200063F RID: 1599
	public class PawnRenderNodeWorker_Stump : PawnRenderNodeWorker_Head
	{
		// Token: 0x06002868 RID: 10344 RVA: 0x000D5F95 File Offset: 0x000D4195
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000D5FA3 File Offset: 0x000D41A3
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}
	}
}
