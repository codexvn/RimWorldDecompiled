using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200063B RID: 1595
	public class PawnRenderNodeWorker_Swaddle : PawnRenderNodeWorker_Body
	{
		// Token: 0x0600285B RID: 10331 RVA: 0x000D5BBF File Offset: 0x000D3DBF
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.DevelopmentalStage.Baby();
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000D5BD1 File Offset: 0x000D3DD1
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.pawn.SwaddleBaby();
		}
	}
}
