using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021A6 RID: 8614
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrikeDelayed : WeatherEvent_LightningFlash
	{
		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x0600BC36 RID: 48182 RVA: 0x00368BD7 File Offset: 0x00366DD7
		public override bool Expired
		{
			get
			{
				return this.age > this.duration + this.delay;
			}
		}

		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x0600BC37 RID: 48183 RVA: 0x00368BF0 File Offset: 0x00366DF0
		protected override float LightningBrightness
		{
			get
			{
				if (this.age < this.delay)
				{
					return -1f;
				}
				if (this.age - this.delay <= 3)
				{
					return (float)(this.age - this.delay) / 3f;
				}
				return 1f - (float)(this.age - this.delay) / (float)this.duration;
			}
		}

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x0600BC38 RID: 48184 RVA: 0x00368C54 File Offset: 0x00366E54
		public override Vector2? OverrideShadowVector
		{
			get
			{
				if (this.age >= this.delay)
				{
					return new Vector2?(this.shadowVector);
				}
				return null;
			}
		}

		// Token: 0x0600BC39 RID: 48185 RVA: 0x00368C84 File Offset: 0x00366E84
		public WeatherEvent_LightningStrikeDelayed(Map map) : base(map)
		{
		}

		// Token: 0x0600BC3A RID: 48186 RVA: 0x00368C98 File Offset: 0x00366E98
		public WeatherEvent_LightningStrikeDelayed(Map map, IntVec3 forcedStrikeLoc, int delay) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
			this.delay = delay;
		}

		// Token: 0x0600BC3B RID: 48187 RVA: 0x000026EA File Offset: 0x000008EA
		public override void FireEvent()
		{
		}

		// Token: 0x0600BC3C RID: 48188 RVA: 0x00368CBA File Offset: 0x00366EBA
		public override void WeatherEventTick()
		{
			this.age++;
			if (this.age == this.delay)
			{
				WeatherEvent_LightningStrike.DoStrike(this.strikeLoc, this.map, ref this.boltMesh);
			}
		}

		// Token: 0x0600BC3D RID: 48189 RVA: 0x00368CEF File Offset: 0x00366EEF
		public override void WeatherEventDraw()
		{
			if (this.age >= this.delay)
			{
				Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrikeDelayed.LightningMat, this.LightningBrightness), 0);
			}
		}

		// Token: 0x0400819C RID: 33180
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x0400819D RID: 33181
		private Mesh boltMesh;

		// Token: 0x0400819E RID: 33182
		private int delay;

		// Token: 0x0400819F RID: 33183
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}
