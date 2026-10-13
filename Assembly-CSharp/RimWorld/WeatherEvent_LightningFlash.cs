using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021A3 RID: 8611
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x0600BC25 RID: 48165 RVA: 0x003688A9 File Offset: 0x00366AA9
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x0600BC26 RID: 48166 RVA: 0x003688B9 File Offset: 0x00366AB9
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x0600BC27 RID: 48167 RVA: 0x003688D4 File Offset: 0x00366AD4
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x0600BC28 RID: 48168 RVA: 0x003688E1 File Offset: 0x00366AE1
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x0600BC29 RID: 48169 RVA: 0x003688E9 File Offset: 0x00366AE9
		protected virtual float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x0600BC2A RID: 48170 RVA: 0x00368918 File Offset: 0x00366B18
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x0600BC2B RID: 48171 RVA: 0x00368964 File Offset: 0x00366B64
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x0600BC2C RID: 48172 RVA: 0x00368976 File Offset: 0x00366B76
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x04008190 RID: 33168
		protected int duration;

		// Token: 0x04008191 RID: 33169
		protected Vector2 shadowVector;

		// Token: 0x04008192 RID: 33170
		protected int age;

		// Token: 0x04008193 RID: 33171
		protected const int FlashFadeInTicks = 3;

		// Token: 0x04008194 RID: 33172
		private const int MinFlashDuration = 15;

		// Token: 0x04008195 RID: 33173
		private const int MaxFlashDuration = 60;

		// Token: 0x04008196 RID: 33174
		protected const float FlashShadowDistance = 5f;

		// Token: 0x04008197 RID: 33175
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.78431374f, 0.8235294f, 0.84705883f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
