using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B4 RID: 8628
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowBlizzard : SkyOverlay
	{
		// Token: 0x0600BC76 RID: 48246 RVA: 0x00369D89 File Offset: 0x00367F89
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SnowBlizzard.ParticleLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SnowBlizzard.CloudLayer, 0);
		}

		// Token: 0x0600BC77 RID: 48247 RVA: 0x00369DA3 File Offset: 0x00367FA3
		public override void SetOverlayColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x0600BC78 RID: 48248 RVA: 0x00369DAC File Offset: 0x00367FAC
		public override void TickOverlay(Map map, float lerpFactor)
		{
			float num = 0.5f + 0.5f * GenCelestial.CurCelestialSunGlow(map);
			Color color = new Color(this.color.r * num, this.color.g * num, this.color.b * num, this.color.a);
			WeatherOverlay_SnowBlizzard.CloudLayer.color = color;
			WeatherOverlay_SnowBlizzard.ParticleLayer.color = color;
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x040081BE RID: 33214
		private Color color;

		// Token: 0x040081BF RID: 33215
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/SnowBlizzard/CloudLayer", -1);

		// Token: 0x040081C0 RID: 33216
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/SnowBlizzard/ParticleLayer", -1);

		// Token: 0x040081C1 RID: 33217
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.05f),
			new UnityEngine.Keyframe(3f, 1.1f)
		});

		// Token: 0x040081C2 RID: 33218
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.CloudLayer, "_MainTex", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-1f, -0.3f), 0.03f);

		// Token: 0x040081C3 RID: 33219
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.CloudLayer, "_MainTex2", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-1f, -0.28f), 0.06f);

		// Token: 0x040081C4 RID: 33220
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.ParticleLayer, "_MainTex", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-0.5f, -0.2f), 0.05f);

		// Token: 0x040081C5 RID: 33221
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_SnowBlizzard.ParticleLayer, "_MainTex2", WeatherOverlay_SnowBlizzard.speedCurve, new Vector2(-0.5f, -0.3f), 0.06f);
	}
}
