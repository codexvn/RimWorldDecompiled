using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000649 RID: 1609
	public class PawnRenderNodeWorker_Eye : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x000D6360 File Offset: 0x000D4560
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			BodyTypeDef.WoundAnchor anchor;
			if (this.TryGetWoundAnchor(node.Props.anchorTag, parms, out anchor))
			{
				Vector3 b;
				float num;
				PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out b, out num);
				vector += b;
			}
			return vector;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000D63AC File Offset: 0x000D45AC
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.ScaleFor(node, parms) * parms.pawn.ageTracker.CurLifeStage.eyeSizeFactor.GetValueOrDefault(1f);
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000D63DC File Offset: 0x000D45DC
		protected bool TryGetWoundAnchor(string anchorTag, PawnDrawParms parms, out BodyTypeDef.WoundAnchor anchor)
		{
			anchor = null;
			if (anchorTag.NullOrEmpty())
			{
				return false;
			}
			List<BodyTypeDef.WoundAnchor> woundAnchors = parms.pawn.story.bodyType.woundAnchors;
			for (int i = 0; i < woundAnchors.Count; i++)
			{
				BodyTypeDef.WoundAnchor woundAnchor = woundAnchors[i];
				if (woundAnchor.tag == anchorTag)
				{
					Rot4? rotation = woundAnchor.rotation;
					Rot4 facing = parms.facing;
					if (rotation != null && (rotation == null || rotation.GetValueOrDefault() == facing) && (parms.facing == Rot4.South || woundAnchor.narrowCrown.GetValueOrDefault() == parms.pawn.story.headType.narrow))
					{
						anchor = woundAnchor;
						return true;
					}
				}
			}
			return false;
		}
	}
}
