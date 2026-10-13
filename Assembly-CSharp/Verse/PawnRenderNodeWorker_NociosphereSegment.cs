using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064D RID: 1613
	public class PawnRenderNodeWorker_NociosphereSegment : PawnRenderNodeWorker
	{
		// Token: 0x06002896 RID: 10390 RVA: 0x000D6680 File Offset: 0x000D4880
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			PawnRenderNodeProperties_NociosphereSegment pawnRenderNodeProperties_NociosphereSegment = node.Props as PawnRenderNodeProperties_NociosphereSegment;
			if (pawnRenderNodeProperties_NociosphereSegment == null)
			{
				return vector;
			}
			CompNociosphere compNociosphere;
			if (!parms.pawn.TryGetComp(out compNociosphere))
			{
				return vector;
			}
			vector += pawnRenderNodeProperties_NociosphereSegment.offset.ToVector3() / 6f;
			Vector3 vector2 = vector * compNociosphere.segScale;
			return new Vector3(vector2.x, 0f, vector2.z);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000D66F8 File Offset: 0x000D48F8
		public override MaterialPropertyBlock GetMaterialPropertyBlock(PawnRenderNode node, Material material, PawnDrawParms parms)
		{
			MaterialPropertyBlock materialPropertyBlock = base.GetMaterialPropertyBlock(node, material, parms);
			CompActivity compActivity;
			if (!parms.pawn.TryGetComp(out compActivity))
			{
				return materialPropertyBlock;
			}
			Color lineColor = PawnRenderNodeWorker_NociosphereSegment.LineColor;
			lineColor.a = Mathf.Clamp01(compActivity.ActivityLevel);
			materialPropertyBlock.SetColor(ShaderPropertyIDs.ColorTwo, lineColor);
			return materialPropertyBlock;
		}

		// Token: 0x04002178 RID: 8568
		private static readonly Color LineColor = new Color(0.89f, 0.21f, 0.13f);
	}
}
