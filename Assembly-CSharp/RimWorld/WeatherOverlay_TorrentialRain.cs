using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021BA RID: 8634
	[StaticConstructorOnStartup]
	public class WeatherOverlay_TorrentialRain : SkyOverlay
	{
		// Token: 0x0600BC85 RID: 48261 RVA: 0x0036A1E9 File Offset: 0x003683E9
		public override void DrawOverlay(Map map)
		{
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_TorrentialRain.CloudLayer, 0);
			SkyOverlay.DrawWorldOverlay(map, WeatherOverlay_TorrentialRain.ParticleLayer, 0);
		}

		// Token: 0x0600BC86 RID: 48262 RVA: 0x0036A203 File Offset: 0x00368403
		public override void SetOverlayColor(Color color)
		{
			WeatherOverlay_TorrentialRain.CloudLayer.color = color;
			WeatherOverlay_TorrentialRain.ParticleLayer.color = color;
		}

		// Token: 0x0600BC87 RID: 48263 RVA: 0x0036A21B File Offset: 0x0036841B
		public override void TickOverlay(Map map, float lerpFactor)
		{
			this.panner0.Tick();
			this.panner1.Tick();
			this.panner2.Tick();
			this.panner3.Tick();
		}

		// Token: 0x040081CB RID: 33227
		private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/TorrentialRain/CloudLayer", -1);

		// Token: 0x040081CC RID: 33228
		private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/TorrentialRain/ParticleLayer", -1);

		// Token: 0x040081CD RID: 33229
		private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
		{
			new UnityEngine.Keyframe(0f, 0f),
			new UnityEngine.Keyframe(1f, 1f),
			new UnityEngine.Keyframe(2f, 1.075f),
			new UnityEngine.Keyframe(3f, 1.15f)
		});

		// Token: 0x040081CE RID: 33230
		private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.CloudLayer, "_MainTex", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.25f, -1f), 0.05f);

		// Token: 0x040081CF RID: 33231
		private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.CloudLayer, "_MainTex2", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.2f, -1f), 0.06f);

		// Token: 0x040081D0 RID: 33232
		private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.ParticleLayer, "_MainTex", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.34f, -1f), 0.06f);

		// Token: 0x040081D1 RID: 33233
		private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(WeatherOverlay_TorrentialRain.ParticleLayer, "_MainTex2", WeatherOverlay_TorrentialRain.speedCurve, new Vector2(-0.2f, -1f), 0.05f);
	}
}
