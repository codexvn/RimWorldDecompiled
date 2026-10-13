using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200063E RID: 1598
	public class PawnRenderNodeWorker_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x000D5DAB File Offset: 0x000D3FAB
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000D5DC8 File Offset: 0x000D3FC8
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(node, parms, out pivot) + parms.pawn.Drawer.renderer.BaseHeadOffsetAt(parms.facing);
			if (parms.pawn.story.headType.narrow && node.Props.narrowCrownHorizontalOffset != 0f && parms.facing.IsHorizontal)
			{
				if (parms.facing == Rot4.East)
				{
					result.x -= node.Props.narrowCrownHorizontalOffset;
				}
				else if (parms.facing == Rot4.West)
				{
					result.x += node.Props.narrowCrownHorizontalOffset;
				}
				result.z -= node.Props.narrowCrownHorizontalOffset;
			}
			if (!parms.Portrait && parms.swimming)
			{
				result.z -= 0.5f;
			}
			return result;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000D5EC4 File Offset: 0x000D40C4
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			if (!parms.Portrait && parms.pawn.Crawling)
			{
				quaternion *= PawnRenderUtility.CrawlingHeadAngle(parms.facing).ToQuat();
				if (parms.flipHead)
				{
					quaternion *= 180f.ToQuat();
				}
			}
			if (parms.pawn.IsShambler && parms.pawn.mutant != null && parms.pawn.mutant.HasTurned && !parms.pawn.Dead)
			{
				Quaternion lhs = quaternion;
				Vector3 up = Vector3.up;
				Hediff_Shambler hediff_Shambler = parms.pawn.mutant.Hediff as Hediff_Shambler;
				quaternion = lhs * Quaternion.Euler(up * ((hediff_Shambler != null) ? hediff_Shambler.headRotation : 0f));
			}
			return quaternion;
		}
	}
}
