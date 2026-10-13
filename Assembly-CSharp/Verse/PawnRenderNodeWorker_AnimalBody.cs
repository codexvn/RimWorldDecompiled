using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000635 RID: 1589
	public class PawnRenderNodeWorker_AnimalBody : PawnRenderNodeWorker
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x000D57DC File Offset: 0x000D39DC
		protected override GraphicStateDef GetGraphicState(PawnRenderNode node, PawnDrawParms parms)
		{
			if (node.tree.currentAnimation == null && parms.pawn.DrawNonHumanlikeSwimmingGraphic)
			{
				return GraphicStateDefOf.Swimming;
			}
			if (node.tree.currentAnimation == null && parms.pawn.DrawNonHumanlikeStationaryGraphic)
			{
				return GraphicStateDefOf.Stationary;
			}
			return base.GetGraphicState(node, parms);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000D5831 File Offset: 0x000D3A31
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			return base.OffsetFor(node, parms, out pivot) + node.PrimaryGraphic.DrawOffset(parms.facing);
		}
	}
}
