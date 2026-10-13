using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D49 RID: 7497
	public class GameCondition_NoxiousHaze : GameCondition
	{
		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x0600A7F5 RID: 42997 RVA: 0x00301765 File Offset: 0x002FF965
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x0600A7F6 RID: 42998 RVA: 0x0030176C File Offset: 0x002FF96C
		public override string Description
		{
			get
			{
				float num = WorldPollutionUtility.CalculateNearbyPollutionScore(base.SingleMap.Tile);
				float num2 = this.def.mtbOverNearbyPollutionCurve.Evaluate(num);
				return this.def.description.Formatted(num.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute).Named("NEARBYPOLLUTION"), num2.Named("MTB"));
			}
		}

		// Token: 0x0600A7F7 RID: 42999 RVA: 0x003017D4 File Offset: 0x002FF9D4
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			foreach (SkyOverlay skyOverlay in this.overlays)
			{
				foreach (Map map in affectedMaps)
				{
					if (!base.HiddenByOtherCondition(map))
					{
						skyOverlay.TickOverlay(map, 1f);
					}
				}
			}
		}

		// Token: 0x0600A7F8 RID: 43000 RVA: 0x00301878 File Offset: 0x002FFA78
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				for (int i = 0; i < this.overlays.Count; i++)
				{
					this.overlays[i].DrawOverlay(map);
				}
			}
		}

		// Token: 0x0600A7F9 RID: 43001 RVA: 0x003018B6 File Offset: 0x002FFAB6
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x0600A7FA RID: 43002 RVA: 0x003018CA File Offset: 0x002FFACA
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.45f, GameCondition_NoxiousHaze.NoxiousHazeColors, 1f, 1f));
		}

		// Token: 0x0600A7FB RID: 43003 RVA: 0x003018EA File Offset: 0x002FFAEA
		public override float AnimalDensityFactor(Map map)
		{
			return (float)(base.HiddenByOtherCondition(map) ? 1 : 0);
		}

		// Token: 0x0600A7FC RID: 43004 RVA: 0x003018EA File Offset: 0x002FFAEA
		public override float PlantDensityFactor(Map map)
		{
			return (float)(base.HiddenByOtherCondition(map) ? 1 : 0);
		}

		// Token: 0x0600A7FD RID: 43005 RVA: 0x003018FA File Offset: 0x002FFAFA
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return base.HiddenByOtherCondition(map);
		}

		// Token: 0x0600A7FE RID: 43006 RVA: 0x00301903 File Offset: 0x002FFB03
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x04007443 RID: 29763
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04007444 RID: 29764
		private const float SkyGlow = 0.45f;

		// Token: 0x04007445 RID: 29765
		private static readonly SkyColorSet NoxiousHazeColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new ColorInt(207, 248, 0).ToColor, 0.45f);

		// Token: 0x04007446 RID: 29766
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_NoxiousHaze()
		};

		// Token: 0x04007447 RID: 29767
		public const float PlantGrowthRateFactor = 0.5f;
	}
}
