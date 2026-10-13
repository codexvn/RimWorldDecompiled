using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D70 RID: 15728
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x060156C5 RID: 87749 RVA: 0x0066930C File Offset: 0x0066750C
		public void RenderMote()
		{
			float num = (Time.time - this.lastOrderedToTileTime) / 0.5f;
			if (num > 1f)
			{
				return;
			}
			if (Caravan_GotoMoteRenderer.cachedMaterial == null)
			{
				Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, 3600);
			}
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
			Color value = new Color(1f, 1f, 1f, 1f - num);
			Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			WorldRendererUtility.DrawQuadTangentialToPlanet(tileCenter, 0.8f * worldGrid.AverageTileSize, 0.05f, Caravan_GotoMoteRenderer.cachedMaterial, 0f, false, false, Caravan_GotoMoteRenderer.propertyBlock);
		}

		// Token: 0x060156C6 RID: 87750 RVA: 0x006693D5 File Offset: 0x006675D5
		public void OrderedToTile(PlanetTile tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x0400EE6D RID: 61037
		private PlanetTile tile;

		// Token: 0x0400EE6E RID: 61038
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x0400EE6F RID: 61039
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400EE70 RID: 61040
		private static Material cachedMaterial;

		// Token: 0x0400EE71 RID: 61041
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, 3600);

		// Token: 0x0400EE72 RID: 61042
		private const float Duration = 0.5f;

		// Token: 0x0400EE73 RID: 61043
		private const float BaseSize = 0.8f;
	}
}
