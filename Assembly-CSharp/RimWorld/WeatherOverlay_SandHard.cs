using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B3 RID: 8627
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SandHard : SkyOverlay
	{
		// Token: 0x0600BC71 RID: 48241 RVA: 0x00369B62 File Offset: 0x00367D62
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SandHard.ParticleLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_SandHard.CloudLayer, 0);
		}

		// Token: 0x0600BC72 RID: 48242 RVA: 0x00369B7C File Offset: 0x00367D7C
		public override void SetOverlayColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x0600BC73 RID: 48243 RVA: 0x00369B88 File Offset: 0x00367D88
		public override void TickOverlay(Map map, float lerpFactor)
		{
			float num = 0.5f + 0.5f * GenCelestial.CurCelestialSunGlow(map);
			Color color = new Color(this.color.r * num * num, this.color.g * num * num, this.color.b * num, this.color.a);
			WeatherOverlay_SandHard.CloudLayer.color = color;
			WeatherOverlay_SandHard.ParticleLayer.color = color;
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x040081B6 RID: 33206
		private Color color;

		// Token: 0x040081B7 RID: 33207
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/Sandstorm/CloudLayer", -1);

		// Token: 0x040081B8 RID: 33208
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/Sandstorm/ParticleLayer", -1);

		// Token: 0x040081B9 RID: 33209
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.075f),
			new UnityEngine.Keyframe(3f, 1.15f)
		});

		// Token: 0x040081BA RID: 33210
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.CloudLayer, "_MainTex", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.3f), 0.04f);

		// Token: 0x040081BB RID: 33211
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.CloudLayer, "_MainTex2", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.28f), 0.08f);

		// Token: 0x040081BC RID: 33212
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.ParticleLayer, "_MainTex", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.3f), 0.03f);

		// Token: 0x040081BD RID: 33213
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_SandHard.ParticleLayer, "_MainTex2", WeatherOverlay_SandHard.speedCurve, new Vector2(1f, -0.28f), 0.06f);
	}
}
