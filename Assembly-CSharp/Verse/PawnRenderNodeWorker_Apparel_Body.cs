using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000636 RID: 1590
	public class PawnRenderNodeWorker_Apparel_Body : PawnRenderNodeWorker_Body
	{
		// Token: 0x0600284E RID: 10318 RVA: 0x000D585A File Offset: 0x000D3A5A
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.flags.FlagSet(PawnRenderFlags.Clothes);
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000D587C File Offset: 0x000D3A7C
		public override Vector3 OffsetFor(PawnRenderNode n, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(n, parms, out pivot);
			PawnRenderNode_Apparel pawnRenderNode_Apparel = (PawnRenderNode_Apparel)n;
			if (pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData != null && pawnRenderNode_Apparel.apparel.RenderAsPack())
			{
				Vector2 vector = pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData.BeltOffsetAt(parms.facing, parms.pawn.story.bodyType);
				result.x += vector.x;
				result.z += vector.y;
			}
			return result;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000D5914 File Offset: 0x000D3B14
		public override Vector3 ScaleFor(PawnRenderNode n, PawnDrawParms parms)
		{
			Vector3 result = base.ScaleFor(n, parms);
			PawnRenderNode_Apparel pawnRenderNode_Apparel = (PawnRenderNode_Apparel)n;
			if (pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData != null && pawnRenderNode_Apparel.apparel.RenderAsPack())
			{
				Vector2 vector = pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData.BeltScaleAt(parms.facing, parms.pawn.story.bodyType);
				result.x *= vector.x;
				result.z *= vector.y;
			}
			return result;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000D59AC File Offset: 0x000D3BAC
		public override float LayerFor(PawnRenderNode n, PawnDrawParms parms)
		{
			if (parms.flipHead && n.Props.oppositeFacingLayerWhenFlipped)
			{
				PawnDrawParms parms2 = parms;
				parms2.facing = parms.facing.Opposite;
				parms2.flipHead = false;
				return base.LayerFor(n, parms2);
			}
			return base.LayerFor(n, parms);
		}
	}
}
