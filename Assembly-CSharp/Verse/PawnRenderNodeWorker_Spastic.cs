using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064E RID: 1614
	public class PawnRenderNodeWorker_Spastic : PawnRenderNodeWorker
	{
		// Token: 0x0600289A RID: 10394 RVA: 0x000D6760 File Offset: 0x000D4960
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic != null && pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				vector += Vector3.Lerp(spasmData.offsetStart, spasmData.offsetTarget, t);
			}
			return vector;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000D67A8 File Offset: 0x000D49A8
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			if (pawnRenderNode_Spastic == null)
			{
				return quaternion;
			}
			float num = 0f;
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = node.Props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null && pawnRenderNodeProperties_Spastic.rotateFacing)
			{
				num += parms.facing.AsAngle;
			}
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				num += Mathf.Lerp(spasmData.rotationStart, spasmData.rotationTarget, t);
			}
			return quaternion * num.ToQuat();
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000D6828 File Offset: 0x000D4A28
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 vector = base.ScaleFor(node, parms);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic != null && pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				vector *= Mathf.Lerp(spasmData.scaleStart, spasmData.scaleTarget, t);
				vector.y = 1f;
			}
			return vector;
		}
	}
}
