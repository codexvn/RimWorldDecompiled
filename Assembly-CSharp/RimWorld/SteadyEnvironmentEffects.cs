using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02003760 RID: 14176
	public class SteadyEnvironmentEffects
	{
		// Token: 0x06013A59 RID: 80473 RVA: 0x005D5D99 File Offset: 0x005D3F99
		public SteadyEnvironmentEffects(Map map)
		{
			this.map = map;
		}

		// Token: 0x06013A5A RID: 80474 RVA: 0x005D5DA8 File Offset: 0x005D3FA8
		public void SteadyEnvironmentEffectsTick()
		{
			if ((float)Find.TickManager.TicksGame % 97f == 0f && Rand.Chance(0.02f))
			{
				this.RollForRainFire();
			}
			this.outdoorMeltAmount = this.MeltAmountAt(this.map.mapTemperature.OutdoorTemp);
			this.snowRate = this.map.weatherManager.SnowRate;
			this.rainRate = this.map.weatherManager.RainRate;
			this.sandRate = this.map.weatherManager.SandRate;
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.cycleIndex = 0;
				}
				IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.DoCellSteadyEffects(c);
				this.cycleIndex++;
			}
		}

		// Token: 0x06013A5B RID: 80475 RVA: 0x005D5EAC File Offset: 0x005D40AC
		private void DoCellSteadyEffects(IntVec3 c)
		{
			Room room = c.GetRoom(this.map);
			bool flag = this.map.roofGrid.Roofed(c);
			TerrainDef terrain = c.GetTerrain(this.map);
			bool flag2 = room != null && room.UsesOutdoorTemperature;
			if (room == null || flag2)
			{
				if (this.outdoorMeltAmount > 0f)
				{
					this.map.snowGrid.AddDepth(c, -this.outdoorMeltAmount);
				}
				if (!flag)
				{
					if (this.snowRate > 0.001f)
					{
						this.AddFallenSnowAt(c, 0.046f * this.map.weatherManager.SnowRate);
					}
					if (ModsConfig.OdysseyActive)
					{
						if (this.sandRate > 0.001f)
						{
							this.AddFallenSandAt(c, 0.046f * this.map.weatherManager.SandRate);
						}
						else
						{
							this.map.sandGrid.AddDepth(c, -0.0055555557f);
						}
					}
				}
				if (this.map.terrainGrid.TerrainAt(c).meltSnowRadius > 0f)
				{
					WeatherBuildupUtility.AddSnowRadial(c, this.map, this.map.terrainGrid.TerrainAt(c).meltSnowRadius, -0.06f);
				}
			}
			if (room != null)
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					Filth filth = thing as Filth;
					if (filth != null)
					{
						if (!flag && thing.def.filth.rainWashes && Rand.Chance(this.rainRate))
						{
							filth.ThinFilth();
						}
						if (filth.DisappearAfterTicks != 0 && filth.TicksSinceThickened > filth.DisappearAfterTicks && !filth.Destroyed)
						{
							filth.Destroy(DestroyMode.Vanish);
						}
					}
					else
					{
						this.TryDoDeteriorate(thing, flag, flag2, terrain);
					}
				}
				if (!flag2)
				{
					float temperature = room.Temperature;
					if (temperature > 0f)
					{
						float num = this.MeltAmountAt(temperature);
						if (num > 0f)
						{
							this.map.snowGrid.AddDepth(c, -num);
						}
						if (temperature > SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.min)
						{
							float value = Rand.Value;
							if (value < SteadyEnvironmentEffects.AutoIgnitionTemperatureRange.InverseLerpThroughRange(temperature) * 0.7f && Rand.Chance(FireUtility.ChanceToStartFireIn(c, this.map, null)))
							{
								FireUtility.TryStartFireIn(c, this.map, 0.1f, null, null);
							}
							if (value < 0.33f)
							{
								FleckMaker.ThrowHeatGlow(c, this.map, 2.3f);
							}
						}
					}
					if (terrain.heatPerTick > 0f)
					{
						GenTemperature.PushHeat(c, this.map, terrain.heatPerTick * 1666.6666f);
					}
					if (ModsConfig.OdysseyActive)
					{
						this.map.sandGrid.AddDepth(c, -0.0055555557f);
					}
				}
			}
			if (Rand.Chance(terrain.throwFleckChance))
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), this.map, terrain.fleckData.fleck, terrain.fleckData.scaleRange.RandomInRange);
				dataStatic.velocitySpeed = terrain.fleckData.velocitySpeedRange.RandomInRange;
				dataStatic.velocityAngle = terrain.fleckData.velocityAngleRange.RandomInRange;
				dataStatic.rotationRate = terrain.fleckData.rotationSpeedRange.RandomInRange;
				if (!terrain.fleckData.solidTicksRange.IsZeros)
				{
					dataStatic.solidTimeOverride = new float?(terrain.fleckData.solidTicksRange.RandomInRange);
				}
				this.map.flecks.CreateFleck(dataStatic);
			}
			if (terrain.igniteRadius > 0f)
			{
				foreach (IntVec3 c2 in GenRadial.RadialCellsAround(c, terrain.igniteRadius, true))
				{
					if (c2.InBounds(this.map) && Rand.Chance(0.03f) && Rand.Chance(FireUtility.ChanceToStartFireIn(c2, this.map, null)))
					{
						FireUtility.TryStartFireIn(c2, this.map, 0.1f, null, null);
					}
				}
			}
			this.map.gameConditionManager.DoSteadyEffects(c, this.map);
			GasUtility.DoSteadyEffects(c, this.map);
		}

		// Token: 0x06013A5C RID: 80476 RVA: 0x005D62F4 File Offset: 0x005D44F4
		public static bool ProtectedByEdifice(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building != null && edifice.def.building.preventDeteriorationOnTop;
		}

		// Token: 0x06013A5D RID: 80477 RVA: 0x005D632E File Offset: 0x005D452E
		private float MeltAmountAt(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature < 10f)
			{
				return temperature * temperature * 0.0058f * 0.1f;
			}
			return temperature * 0.0058f;
		}

		// Token: 0x06013A5E RID: 80478 RVA: 0x005D6360 File Offset: 0x005D4560
		public void AddFallenSnowAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.03999999910593033, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.snowGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x06013A5F RID: 80479 RVA: 0x005D63EC File Offset: 0x005D45EC
		public void AddFallenSandAt(IntVec3 c, float baseAmount)
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.03999999910593033, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			float num = this.snowNoise.GetValue(c);
			num += 1f;
			num *= 0.5f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			float depthToAdd = baseAmount * num;
			this.map.sandGrid.AddDepth(c, depthToAdd);
		}

		// Token: 0x06013A60 RID: 80480 RVA: 0x005D6478 File Offset: 0x005D4678
		public static float FinalDeteriorationRate(Thing t, List<string> reasons = null)
		{
			if (t.Spawned)
			{
				Room room = t.GetRoom(RegionType.Set_All);
				return SteadyEnvironmentEffects.FinalDeteriorationRate(t, t.Position.Roofed(t.Map), room != null && room.UsesOutdoorTemperature, t.Position.GetTerrain(t.Map), reasons);
			}
			if (t.SpawnedOrAnyParentSpawned && !t.def.canDeteriorateUnspawned)
			{
				return 0f;
			}
			return SteadyEnvironmentEffects.FinalDeteriorationRate(t, false, false, null, reasons);
		}

		// Token: 0x06013A61 RID: 80481 RVA: 0x005D64F4 File Offset: 0x005D46F4
		public static float FinalDeteriorationRate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, TerrainDef terrain, List<string> reasons = null)
		{
			if (!t.def.CanEverDeteriorate)
			{
				return 0f;
			}
			Map mapHeld = t.MapHeld;
			float num = t.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			Genepack genepack = t as Genepack;
			if (ModsConfig.BiotechActive && genepack != null && !genepack.Deteriorating)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				return 0f;
			}
			if (reasons != null)
			{
				if (t.def.deteriorateFromEnvironmentalEffects)
				{
					if (!roofed)
					{
						reasons.Add("DeterioratingUnroofed".Translate());
						if (mapHeld.weatherManager.RainRate > 0f)
						{
							reasons.Add("DeterioratingRaining".Translate());
						}
					}
					if (roomUsesOutdoorTemperature)
					{
						reasons.Add("DeterioratingOutdoors".Translate());
					}
					if (terrain != null && terrain.extraDeteriorationFactor != 0f)
					{
						reasons.Add(terrain.label);
					}
				}
				if (ModsConfig.BiotechActive && genepack != null && genepack.Deteriorating)
				{
					reasons.Add("NotInGeneBank".Translate());
				}
			}
			return num;
		}

		// Token: 0x06013A62 RID: 80482 RVA: 0x005D660C File Offset: 0x005D480C
		private void TryDoDeteriorate(Thing t, bool roofed, bool roomUsesOutdoorTemperature, TerrainDef terrain)
		{
			Corpse corpse = t as Corpse;
			if (corpse != null && corpse.InnerPawn.apparel != null)
			{
				List<Apparel> wornApparel = corpse.InnerPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					this.TryDoDeteriorate(wornApparel[i], roofed, roomUsesOutdoorTemperature, terrain);
				}
			}
			float num = SteadyEnvironmentEffects.FinalDeteriorationRate(t, roofed, roomUsesOutdoorTemperature, terrain, null);
			if (num < 0.001f)
			{
				return;
			}
			if (Rand.Chance(num / 36f))
			{
				IntVec3 position = t.Position;
				Map map = t.Map;
				bool sendMessage = t.IsInAnyStorage();
				SteadyEnvironmentEffects.DoDeteriorationDamage(t, position, map, sendMessage);
			}
		}

		// Token: 0x06013A63 RID: 80483 RVA: 0x005D66AC File Offset: 0x005D48AC
		public static void DoDeteriorationDamage(Thing t, IntVec3 pos, Map map, bool sendMessage)
		{
			t.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			if (sendMessage && t.Destroyed && t.def.messageOnDeteriorateInStorage && (SteadyEnvironmentEffects.lastDeterioratedTick == 0 || GenTicks.TicksGame >= SteadyEnvironmentEffects.lastDeterioratedTick + 60000))
			{
				SteadyEnvironmentEffects.lastDeterioratedTick = GenTicks.TicksGame;
				Messages.Message("MessageDeterioratedAway".Translate(t.Label), new TargetInfo(pos, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06013A64 RID: 80484 RVA: 0x005D6750 File Offset: 0x005D4950
		public static void Reset()
		{
			SteadyEnvironmentEffects.lastDeterioratedTick = 0;
		}

		// Token: 0x06013A65 RID: 80485 RVA: 0x005D6758 File Offset: 0x005D4958
		private void RollForRainFire()
		{
			if (!Rand.Chance(0.2f * (float)this.map.listerBuildings.allBuildingsColonistElecFire.Count * this.map.weatherManager.RainRate))
			{
				return;
			}
			Building building = this.map.listerBuildings.allBuildingsColonistElecFire.RandomElement<Building>();
			if (!this.map.roofGrid.Roofed(building.Position))
			{
				ShortCircuitUtility.TryShortCircuitInRain(building);
			}
		}

		// Token: 0x0400D97B RID: 55675
		private Map map;

		// Token: 0x0400D97C RID: 55676
		private ModuleBase snowNoise;

		// Token: 0x0400D97D RID: 55677
		private int cycleIndex;

		// Token: 0x0400D97E RID: 55678
		private float outdoorMeltAmount;

		// Token: 0x0400D97F RID: 55679
		private float snowRate;

		// Token: 0x0400D980 RID: 55680
		private float rainRate;

		// Token: 0x0400D981 RID: 55681
		private float sandRate;

		// Token: 0x0400D982 RID: 55682
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x0400D983 RID: 55683
		private const float RainFireCheckInterval = 97f;

		// Token: 0x0400D984 RID: 55684
		private const float RainFireChanceOverall = 0.02f;

		// Token: 0x0400D985 RID: 55685
		private const float RainFireChancePerBuilding = 0.2f;

		// Token: 0x0400D986 RID: 55686
		private const float SnowFallRateFactor = 0.046f;

		// Token: 0x0400D987 RID: 55687
		private const float SandFallRateFactor = 0.046f;

		// Token: 0x0400D988 RID: 55688
		private const float SnowMeltRateFactor = 0.0058f;

		// Token: 0x0400D989 RID: 55689
		private const float SandDissipateRate = 0.0055555557f;

		// Token: 0x0400D98A RID: 55690
		private static readonly FloatRange AutoIgnitionTemperatureRange = new FloatRange(240f, 1000f);

		// Token: 0x0400D98B RID: 55691
		private const float AutoIgnitionChanceFactor = 0.7f;

		// Token: 0x0400D98C RID: 55692
		private const float FireGlowRate = 0.33f;

		// Token: 0x0400D98D RID: 55693
		private const float IgniteChancePerCheck = 0.03f;

		// Token: 0x0400D98E RID: 55694
		private static int lastDeterioratedTick;

		// Token: 0x0400D98F RID: 55695
		private const int MessageCooldownTicks = 60000;
	}
}
