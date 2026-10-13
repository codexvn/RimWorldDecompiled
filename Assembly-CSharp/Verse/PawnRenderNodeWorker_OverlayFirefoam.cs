using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000642 RID: 1602
	public class PawnRenderNodeWorker_OverlayFirefoam : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06002872 RID: 10354 RVA: 0x000D608E File Offset: 0x000D428E
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.FirefoamOverlays;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000D60A0 File Offset: 0x000D42A0
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000D60BC File Offset: 0x000D42BC
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode == RotDrawMode.Fresh && parms.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
		}
	}
}
