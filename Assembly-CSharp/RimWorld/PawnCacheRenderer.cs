using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023B4 RID: 9140
	public class PawnCacheRenderer : MonoBehaviour
	{
		// Token: 0x0600C65C RID: 50780 RVA: 0x0039C0D4 File Offset: 0x0039A2D4
		public void RenderPawn(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom, float angle, Rot4 rotation, bool renderHead = true, bool renderHeadgear = true, bool renderClothes = true, bool portrait = false, Vector3 positionOffset = default(Vector3), IReadOnlyDictionary<Apparel, Color> overrideApparelColor = null, Color? overrideHairColor = null, bool stylingStation = false)
		{
			this.rotation = rotation;
			this.renderHead = renderHead;
			this.renderHeadgear = renderHeadgear;
			this.renderClothes = renderClothes;
			this.portrait = portrait;
			this.angle = angle;
			this.positionOffset = positionOffset;
			this.overrideApparelColor = overrideApparelColor;
			this.overrideHairColor = overrideHairColor;
			this.stylingStation = stylingStation;
			Camera pawnCacheCamera = Find.PawnCacheCamera;
			Vector3 position = pawnCacheCamera.transform.position;
			float orthographicSize = pawnCacheCamera.orthographicSize;
			pawnCacheCamera.transform.position += cameraOffset;
			pawnCacheCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			pawnCacheCamera.SetTargetBuffers(renderTexture.colorBuffer, renderTexture.depthBuffer);
			pawnCacheCamera.Render();
			this.pawn = null;
			pawnCacheCamera.transform.position = position;
			pawnCacheCamera.orthographicSize = orthographicSize;
			pawnCacheCamera.targetTexture = null;
		}

		// Token: 0x0600C65D RID: 50781 RVA: 0x0039C1B0 File Offset: 0x0039A3B0
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderCache(this.rotation, this.angle, this.positionOffset, this.renderHead, this.portrait, this.renderHeadgear, this.renderClothes, this.overrideApparelColor, this.overrideHairColor, this.stylingStation);
		}

		// Token: 0x0400871E RID: 34590
		private Pawn pawn;

		// Token: 0x0400871F RID: 34591
		private Rot4 rotation;

		// Token: 0x04008720 RID: 34592
		private bool renderHead;

		// Token: 0x04008721 RID: 34593
		private bool renderHeadgear;

		// Token: 0x04008722 RID: 34594
		private bool renderClothes;

		// Token: 0x04008723 RID: 34595
		private bool portrait;

		// Token: 0x04008724 RID: 34596
		private float angle;

		// Token: 0x04008725 RID: 34597
		private Vector3 positionOffset;

		// Token: 0x04008726 RID: 34598
		private IReadOnlyDictionary<Apparel, Color> overrideApparelColor;

		// Token: 0x04008727 RID: 34599
		private Color? overrideHairColor;

		// Token: 0x04008728 RID: 34600
		private bool stylingStation;
	}
}
