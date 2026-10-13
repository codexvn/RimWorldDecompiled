using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054A RID: 1354
	[StaticConstructorOnStartup]
	public class WeatherOverlayDualPanner : SkyOverlay
	{
		// Token: 0x0600231E RID: 8990 RVA: 0x000B6AA9 File Offset: 0x000B4CA9
		public WeatherOverlayDualPanner()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(Color.clear);
			});
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000B6AD8 File Offset: 0x000B4CD8
		public override void TickOverlay(Map map, float lerpFactor)
		{
			if (this.worldOverlayMat == null)
			{
				return;
			}
			this.worldPan1 -= this.worldPanDir1 * (this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale(WeatherOverlayDualPanner.MainTex).x * Find.TickManager.TickRateMultiplier);
			this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex, this.worldPan1);
			if (this.worldOverlayMat.HasProperty(WeatherOverlayDualPanner.MainTex2))
			{
				this.worldPan2 -= this.worldPanDir2 * (this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale(WeatherOverlayDualPanner.MainTex2).x * Find.TickManager.TickRateMultiplier);
				this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex2, this.worldPan2);
			}
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000B6BB8 File Offset: 0x000B4DB8
		public override void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				SkyOverlay.DrawWorldOverlay(map, this.worldOverlayMat, this.GetRenderLayer());
			}
			if (this.screenOverlayMat != null)
			{
				SkyOverlay.DrawScreenOverlay(this.screenOverlayMat, this.GetRenderLayer(), null);
			}
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000B6C05 File Offset: 0x000B4E05
		public override void SetOverlayColor(Color color)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.color = color;
			}
			if (this.screenOverlayMat != null)
			{
				this.screenOverlayMat.color = color;
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000B6C3B File Offset: 0x000B4E3B
		protected virtual int GetRenderLayer()
		{
			return WeatherOverlayDualPanner.RenderLayer;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000B6C44 File Offset: 0x000B4E44
		public override void Reset()
		{
			this.worldPan1 = Vector2.zero;
			this.worldPan2 = Vector2.zero;
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex, this.worldPan1);
				if (this.worldOverlayMat.HasProperty(WeatherOverlayDualPanner.MainTex2))
				{
					this.worldOverlayMat.SetTextureOffset(WeatherOverlayDualPanner.MainTex2, this.worldPan2);
				}
			}
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000B6CB3 File Offset: 0x000B4EB3
		public override string ToString()
		{
			if (this.worldOverlayMat != null)
			{
				return this.worldOverlayMat.name;
			}
			if (this.screenOverlayMat != null)
			{
				return this.screenOverlayMat.name;
			}
			return base.ToString();
		}

		// Token: 0x04001D4E RID: 7502
		private Vector2 worldPan1 = Vector2.zero;

		// Token: 0x04001D4F RID: 7503
		private Vector2 worldPan2 = Vector2.zero;

		// Token: 0x04001D50 RID: 7504
		public Material worldOverlayMat;

		// Token: 0x04001D51 RID: 7505
		public Material screenOverlayMat;

		// Token: 0x04001D52 RID: 7506
		protected float worldOverlayPanSpeed1;

		// Token: 0x04001D53 RID: 7507
		protected float worldOverlayPanSpeed2;

		// Token: 0x04001D54 RID: 7508
		protected Vector2 worldPanDir1;

		// Token: 0x04001D55 RID: 7509
		protected Vector2 worldPanDir2;

		// Token: 0x04001D56 RID: 7510
		private static readonly int RenderLayer = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x04001D57 RID: 7511
		private static readonly int MainTex2 = Shader.PropertyToID("_MainTex2");

		// Token: 0x04001D58 RID: 7512
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");
	}
}
