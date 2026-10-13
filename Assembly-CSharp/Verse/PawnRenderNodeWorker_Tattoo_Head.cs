using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000640 RID: 1600
	public class PawnRenderNodeWorker_Tattoo_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x0600286B RID: 10347 RVA: 0x000D5FC8 File Offset: 0x000D41C8
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (ModsConfig.IdeologyActive)
			{
				Pawn_StyleTracker style = parms.pawn.style;
				if (((style != null) ? style.FaceTattoo : null) != null)
				{
					return base.CanDrawNow(node, parms) && (parms.pawn.style.FaceTattoo.visibleNorth || !(parms.facing == Rot4.North));
				}
			}
			return false;
		}
	}
}
