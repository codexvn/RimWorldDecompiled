using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064C RID: 1612
	public class PawnRenderNodeWorker_Hediff : PawnRenderNodeWorker
	{
		// Token: 0x06002894 RID: 10388 RVA: 0x000D6554 File Offset: 0x000D4754
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = Vector3.zero;
			if (node.Props.drawData != null && node.hediff != null && node.Props.drawData.useBodyPartAnchor)
			{
				foreach (BodyTypeDef.WoundAnchor anchor in PawnDrawUtility.FindAnchors(parms.pawn, node.hediff.Part))
				{
					if (PawnDrawUtility.AnchorUsable(parms.pawn, anchor, parms.facing))
					{
						float num;
						PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out vector, out num);
					}
				}
			}
			vector += base.OffsetFor(node, parms, out pivot);
			DrawData drawData = node.Props.drawData;
			if (drawData != null && !drawData.useBodyPartAnchor)
			{
				Hediff hediff = node.hediff;
				bool? flag;
				if (hediff == null)
				{
					flag = null;
				}
				else
				{
					BodyPartRecord part = hediff.Part;
					flag = ((part != null) ? new bool?(part.flipGraphic) : null);
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					vector.x *= -1f;
				}
			}
			return vector;
		}
	}
}
