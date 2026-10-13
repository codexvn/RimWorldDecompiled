using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000646 RID: 1606
	public class PawnRenderNodeWorker_OverlayStatus : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06002880 RID: 10368 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000D618C File Offset: 0x000D438C
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && !parms.Cache;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000D61B0 File Offset: 0x000D43B0
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			Vector3 vector2;
			Vector3 vector = this.OffsetFor(node, parms, out vector2);
			Quaternion quat = this.RotationFor(node, parms);
			if (node.Props.overlayLayer == PawnOverlayDrawer.OverlayLayer.Head)
			{
				vector += parms.pawn.Drawer.renderer.BaseHeadOffsetAt(Rot4.North);
			}
			parms.pawn.Drawer.renderer.StatusOverlays.RenderStatusOverlays(vector, quat, mesh);
		}
	}
}
