using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000637 RID: 1591
	public class PawnRenderNodeWorker_AttachmentBody : PawnRenderNodeWorker_Body
	{
		// Token: 0x06002853 RID: 10323 RVA: 0x000D5A04 File Offset: 0x000D3C04
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 a = base.ScaleFor(node, parms);
			Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
			return a * ((bodyGraphicScale.x + bodyGraphicScale.y) / 2f);
		}
	}
}
