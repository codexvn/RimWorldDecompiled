using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064B RID: 1611
	public class PawnRenderNodeWorker_FlipWhenCrawling : PawnRenderNodeWorker
	{
		// Token: 0x06002891 RID: 10385 RVA: 0x000D6507 File Offset: 0x000D4707
		protected override Material GetMaterial(PawnRenderNode node, PawnDrawParms parms)
		{
			if (parms.flipHead)
			{
				parms.facing = parms.facing.Opposite;
			}
			return base.GetMaterial(node, parms);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000D652C File Offset: 0x000D472C
		public override float LayerFor(PawnRenderNode node, PawnDrawParms parms)
		{
			if (parms.flipHead)
			{
				parms.facing = parms.facing.Opposite;
			}
			return base.LayerFor(node, parms);
		}
	}
}
