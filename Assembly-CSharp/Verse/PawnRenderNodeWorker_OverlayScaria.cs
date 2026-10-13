using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000645 RID: 1605
	public class PawnRenderNodeWorker_OverlayScaria : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x0600287D RID: 10365 RVA: 0x000D617A File Offset: 0x000D437A
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.ScariaSoreDrawer;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000D6107 File Offset: 0x000D4307
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode != RotDrawMode.Dessicated;
		}
	}
}
