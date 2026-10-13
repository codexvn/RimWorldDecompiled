using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000644 RID: 1604
	public class PawnRenderNodeWorker_OverlayShambler : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06002879 RID: 10361 RVA: 0x000D6121 File Offset: 0x000D4321
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.ShamblerScarDrawer;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000D6133 File Offset: 0x000D4333
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.IsShambler;
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000D6140 File Offset: 0x000D4340
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.pawn.IsShambler && (parms.pawn.mutant == null || parms.pawn.mutant.HasTurned);
		}
	}
}
