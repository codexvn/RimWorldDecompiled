using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000639 RID: 1593
	public class PawnRenderNodeWorker_Body_Tattoo : PawnRenderNodeWorker_Body
	{
		// Token: 0x06002857 RID: 10327 RVA: 0x000D5B3C File Offset: 0x000D3D3C
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (ModsConfig.IdeologyActive)
			{
				Pawn_StyleTracker style = parms.pawn.style;
				if (((style != null) ? style.BodyTattoo : null) != null)
				{
					return base.CanDrawNow(node, parms) && (parms.pawn.style.BodyTattoo.visibleNorth || !(parms.facing == Rot4.North));
				}
			}
			return false;
		}
	}
}
