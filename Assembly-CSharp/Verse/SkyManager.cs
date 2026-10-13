using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000541 RID: 1345
	public class SkyManager
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x000B5870 File Offset: 0x000B3A70
		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000B5878 File Offset: 0x000B3A78
		public SkyTarget CurSky
		{
			get
			{
				return this.curSky;
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000B5880 File Offset: 0x000B3A80
		public SkyManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000B58A8 File Offset: 0x000B3AA8
		public void SkyManagerUpdate()
		{
			this.curSky = this.CurrentSkyTarget();
			this.curSkyGlowInt = this.curSky.glow;
			if (this.map == Find.CurrentMap)
			{
				if (this.map.Biome != null && this.map.Biome.disableSkyLighting)
				{
					MatBases.LightOverlay.color = new Color(1f, 1f, 1f, 0f);
					MatBases.FogOfWar.color = (this.map.FogOfWarColor ?? SkyManager.FogOfWarBaseColor);
				}
				else
				{
					MatBases.LightOverlay.color = this.curSky.colors.sky;
					Find.CameraColor.saturation = this.curSky.colors.saturation;
					Color color = this.curSky.colors.sky;
					color.a = 1f;
					color *= (this.map.FogOfWarColor ?? SkyManager.FogOfWarBaseColor);
					MatBases.FogOfWar.color = color;
				}
				Color color2 = this.curSky.colors.shadow;
				Vector3? overridenShadowVector = this.GetOverridenShadowVector();
				if (overridenShadowVector != null)
				{
					this.SetSunShadowVector(overridenShadowVector.Value);
				}
				else
				{
					this.SetSunShadowVector(GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow).vector);
					color2 = Color.Lerp(Color.white, color2, GenCelestial.CurShadowStrength(this.map));
				}
				GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingSun);
				GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingMoon);
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectSun, new Vector4(lightSourceInfo.vector.x, 0f, lightSourceInfo.vector.y, lightSourceInfo.intensity));
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectMoon, new Vector4(lightSourceInfo2.vector.x, 0f, lightSourceInfo2.vector.y, lightSourceInfo2.intensity));
				Shader.SetGlobalFloat(SkyManager.LightsourceShineSizeReduction, 20f * (1f / this.curSky.lightsourceShineSize));
				Shader.SetGlobalFloat(SkyManager.LightsourceShineIntensity, this.curSky.lightsourceShineIntensity);
				Shader.SetGlobalFloat(SkyManager.DayPercent, GenLocalDate.DayPercent(this.map));
				MatBases.SunShadow.color = color2;
				MatBases.SunShadowFade.color = color2;
				this.UpdateOverlays(this.curSky);
			}
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000B5B2C File Offset: 0x000B3D2C
		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000B5B38 File Offset: 0x000B3D38
		private void UpdateOverlays(SkyTarget curSky)
		{
			this.tempOverlays.Clear();
			List<SkyOverlay> overlays = this.map.weatherManager.curWeather.Worker.overlays;
			for (int i = 0; i < overlays.Count; i++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays[i], this.map.weatherManager.TransitionLerpFactor));
			}
			List<SkyOverlay> overlays2 = this.map.weatherManager.lastWeather.Worker.overlays;
			for (int j = 0; j < overlays2.Count; j++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays2[j], 1f - this.map.weatherManager.TransitionLerpFactor));
			}
			for (int k = 0; k < this.map.gameConditionManager.ActiveConditions.Count; k++)
			{
				GameCondition gameCondition = this.map.gameConditionManager.ActiveConditions[k];
				List<SkyOverlay> list = gameCondition.SkyOverlays(this.map);
				if (list != null)
				{
					for (int l = 0; l < list.Count; l++)
					{
						this.AddTempOverlay(new Pair<SkyOverlay, float>(list[l], gameCondition.SkyTargetLerpFactor(this.map)));
					}
				}
			}
			for (int m = 0; m < this.tempOverlays.Count; m++)
			{
				Color overlayColor;
				if (this.tempOverlays[m].First.ForcedOverlayColor != null)
				{
					overlayColor = this.tempOverlays[m].First.ForcedOverlayColor.Value;
				}
				else
				{
					overlayColor = curSky.colors.overlay;
				}
				overlayColor.a = this.tempOverlays[m].Second;
				this.tempOverlays[m].First.SetOverlayColor(overlayColor);
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000B5D34 File Offset: 0x000B3F34
		private void AddTempOverlay(Pair<SkyOverlay, float> pair)
		{
			for (int i = 0; i < this.tempOverlays.Count; i++)
			{
				if (this.tempOverlays[i].First == pair.First)
				{
					this.tempOverlays[i] = new Pair<SkyOverlay, float>(this.tempOverlays[i].First, Mathf.Clamp01(this.tempOverlays[i].Second + pair.Second));
					return;
				}
			}
			this.tempOverlays.Add(pair);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000B5DC7 File Offset: 0x000B3FC7
		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000B5DF4 File Offset: 0x000B3FF4
		private SkyTarget CurrentSkyTarget()
		{
			SkyTarget b = this.map.weatherManager.curWeather.Worker.CurSkyTarget(this.map);
			SkyTarget skyTarget = SkyTarget.Lerp(this.map.weatherManager.lastWeather.Worker.CurSkyTarget(this.map), b, this.map.weatherManager.TransitionLerpFactor);
			this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, this.tempAllGameConditionsAffectingMap);
			for (int i = 0; i < this.tempAllGameConditionsAffectingMap.Count; i++)
			{
				SkyTarget? skyTarget2 = this.tempAllGameConditionsAffectingMap[i].SkyTarget(this.map);
				if (skyTarget2 != null)
				{
					skyTarget = SkyTarget.LerpDarken(skyTarget, skyTarget2.Value, this.tempAllGameConditionsAffectingMap[i].SkyTargetLerpFactor(this.map));
				}
			}
			this.tempAllGameConditionsAffectingMap.Clear();
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			for (int j = 0; j < liveEventsListForReading.Count; j++)
			{
				if (liveEventsListForReading[j].CurrentlyAffectsSky)
				{
					skyTarget = SkyTarget.Lerp(skyTarget, liveEventsListForReading[j].SkyTarget, liveEventsListForReading[j].SkyTargetLerpFactor);
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			for (int k = 0; k < list.Count; k++)
			{
				CompAffectsSky compAffectsSky = list[k].TryGetComp<CompAffectsSky>();
				if (compAffectsSky.LerpFactor > 0f)
				{
					if (compAffectsSky.Props.lerpDarken)
					{
						skyTarget = SkyTarget.LerpDarken(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
					else
					{
						skyTarget = SkyTarget.Lerp(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
				}
			}
			return skyTarget;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000B5FC0 File Offset: 0x000B41C0
		private Vector3? GetOverridenShadowVector()
		{
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			int i = 0;
			while (i < liveEventsListForReading.Count)
			{
				Vector2? overrideShadowVector = liveEventsListForReading[i].OverrideShadowVector;
				if (overrideShadowVector != null)
				{
					Vector2? vector = overrideShadowVector;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					i++;
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			int j = 0;
			while (j < list.Count)
			{
				Vector2? overrideShadowVector2 = list[j].TryGetComp<CompAffectsSky>().OverrideShadowVector;
				if (overrideShadowVector2 != null)
				{
					Vector2? vector = overrideShadowVector2;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					j++;
				}
			}
			return null;
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000B60AC File Offset: 0x000B42AC
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.CurrentMap).ToString());
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04001D2D RID: 7469
		private readonly Map map;

		// Token: 0x04001D2E RID: 7470
		private float curSkyGlowInt;

		// Token: 0x04001D2F RID: 7471
		private SkyTarget curSky;

		// Token: 0x04001D30 RID: 7472
		private readonly List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		// Token: 0x04001D31 RID: 7473
		public const float NightMaxCelGlow = 0.1f;

		// Token: 0x04001D32 RID: 7474
		public const float DuskMaxCelGlow = 0.6f;

		// Token: 0x04001D33 RID: 7475
		private static readonly Color FogOfWarBaseColor = new Color32(77, 69, 66, byte.MaxValue);

		// Token: 0x04001D34 RID: 7476
		private readonly List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();

		// Token: 0x04001D35 RID: 7477
		private static readonly int LightsourceShineIntensity = Shader.PropertyToID("_LightsourceShineIntensity");

		// Token: 0x04001D36 RID: 7478
		private static readonly int LightsourceShineSizeReduction = Shader.PropertyToID("_LightsourceShineSizeReduction");

		// Token: 0x04001D37 RID: 7479
		private static readonly int DayPercent = Shader.PropertyToID("_DayPercent");
	}
}
