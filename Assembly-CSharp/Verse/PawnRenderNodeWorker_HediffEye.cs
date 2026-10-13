using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064A RID: 1610
	public class PawnRenderNodeWorker_HediffEye : PawnRenderNodeWorker_Eye
	{
		// Token: 0x0600288F RID: 10383 RVA: 0x000D64AC File Offset: 0x000D46AC
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			BodyPartRecord bodyPart = node.bodyPart;
			BodyTypeDef.WoundAnchor anchor;
			if (base.TryGetWoundAnchor((bodyPart != null) ? bodyPart.woundAnchorTag : null, parms, out anchor))
			{
				Vector3 b;
				float num;
				PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out b, out num);
				vector += b;
			}
			return vector;
		}
	}
}
