using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000643 RID: 1603
	public class PawnRenderNodeWorker_OverlayWounds : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x000D60F5 File Offset: 0x000D42F5
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.WoundOverlays;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000D6107 File Offset: 0x000D4307
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode != RotDrawMode.Dessicated;
		}
	}
}
