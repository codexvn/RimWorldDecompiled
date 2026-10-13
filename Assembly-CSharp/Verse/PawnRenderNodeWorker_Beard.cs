using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200063D RID: 1597
	public class PawnRenderNodeWorker_Beard : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x000D5CD5 File Offset: 0x000D3ED5
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && (parms.facing != Rot4.North || parms.flipHead);
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000D5D00 File Offset: 0x000D3F00
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			HeadTypeDef headType = parms.pawn.story.headType;
			Vector3 a = base.OffsetFor(node, parms, out pivot);
			if (parms.facing == Rot4.East)
			{
				a += Vector3.right * headType.beardOffsetXEast;
			}
			else if (parms.facing == Rot4.West)
			{
				a += Vector3.left * headType.beardOffsetXEast;
			}
			return a + (headType.beardOffset + parms.pawn.style.beardDef.GetOffset(headType, parms.facing));
		}
	}
}
