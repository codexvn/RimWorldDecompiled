using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000641 RID: 1601
	public abstract class PawnRenderNodeWorker_Overlay : PawnRenderNodeWorker
	{
		// Token: 0x0600286D RID: 10349
		protected abstract PawnOverlayDrawer OverlayDrawer(Pawn pawn);

		// Token: 0x0600286E RID: 10350 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return false;
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000D6030 File Offset: 0x000D4230
		public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			Mesh mesh = node.GetMesh(parms);
			if (mesh != null)
			{
				requests.Add(new PawnGraphicDrawRequest(node, mesh, null));
			}
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000D605C File Offset: 0x000D425C
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			PawnOverlayDrawer pawnOverlayDrawer = this.OverlayDrawer(parms.pawn);
			if (pawnOverlayDrawer == null)
			{
				return;
			}
			pawnOverlayDrawer.RenderPawnOverlay(matrix, mesh, node.Props.overlayLayer, parms, node.Props.overlayOverApparel);
		}
	}
}
