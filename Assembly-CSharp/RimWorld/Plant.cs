using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002B2C RID: 11052
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x170026B2 RID: 9906
		// (get) Token: 0x0600F6FE RID: 63230 RVA: 0x0047B957 File Offset: 0x00479B57
		// (set) Token: 0x0600F6FF RID: 63231 RVA: 0x0047B95F File Offset: 0x00479B5F
		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x170026B3 RID: 9907
		// (get) Token: 0x0600F700 RID: 63232 RVA: 0x0047B974 File Offset: 0x00479B74
		// (set) Token: 0x0600F701 RID: 63233 RVA: 0x0047B97C File Offset: 0x00479B7C
		public virtual int Age
		{
			get
			{
				return this.ageInt;
			}
			set
			{
				this.ageInt = value;
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x170026B4 RID: 9908
		// (get) Token: 0x0600F702 RID: 63234 RVA: 0x0047B98C File Offset: 0x00479B8C
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x170026B5 RID: 9909
		// (get) Token: 0x0600F703 RID: 63235 RVA: 0x0047B9BC File Offset: 0x00479BBC
		public bool HarvestableSoon
		{
			get
			{
				if (this.HarvestableNow)
				{
					return true;
				}
				if (!this.def.plant.Harvestable)
				{
					return false;
				}
				float num = Mathf.Max(1f - this.Growth, 0f) * this.def.plant.growDays;
				float num2 = Mathf.Max(1f - this.def.plant.harvestMinGrowth, 0f) * this.def.plant.growDays;
				return (num <= 10f || num2 <= 1f) && this.GrowthRateFactor_Fertility > 0f && this.GrowthRateFactor_Temperature > 0f;
			}
		}

		// Token: 0x170026B6 RID: 9910
		// (get) Token: 0x0600F704 RID: 63236 RVA: 0x0047BA6C File Offset: 0x00479C6C
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.wildPlantSpawner.AllWildPlants.Contains(this.def);
			}
		}

		// Token: 0x170026B7 RID: 9911
		// (get) Token: 0x0600F705 RID: 63237 RVA: 0x0047BAC3 File Offset: 0x00479CC3
		public Blight Blight
		{
			get
			{
				if (!base.Spawned || !this.def.plant.Blightable)
				{
					return null;
				}
				return base.Position.GetFirstBlight(base.Map);
			}
		}

		// Token: 0x170026B8 RID: 9912
		// (get) Token: 0x0600F706 RID: 63238 RVA: 0x0047BAF2 File Offset: 0x00479CF2
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x170026B9 RID: 9913
		// (get) Token: 0x0600F707 RID: 63239 RVA: 0x0047BB00 File Offset: 0x00479D00
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && this.growthInt >= 0.1f && !this.LeaflessNow && (!base.Spawned || Math.Max(base.Position.GetSnowDepth(base.Map), base.Position.GetSandDepth(base.Map)) <= this.def.hideAtSnowOrSandDepth)));
			}
		}

		// Token: 0x170026BA RID: 9914
		// (get) Token: 0x0600F708 RID: 63240 RVA: 0x0047BBA0 File Offset: 0x00479DA0
		public virtual float CurrentDyingDamagePerTick
		{
			get
			{
				if (!base.Spawned)
				{
					return 0f;
				}
				float num = 0f;
				if (this.def.plant.LimitedLifespan && this.ageInt > this.def.plant.LifespanTicks)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (!this.def.plant.diesToLight && this.def.plant.dieIfNoSunlight && this.unlitTicks > 450000)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (this.DyingBecauseExposedToLight)
				{
					float lerpPct = base.Map.glowGrid.GroundGlowAt(base.Position, true, false);
					num = Mathf.Max(num, Plant.DyingDamagePerTickBecauseExposedToLight.LerpThroughRange(lerpPct));
				}
				if (this.DyingBecauseExposedToVacuum)
				{
					num = Mathf.Max(num, 1f * base.Position.GetVacuum(base.Map));
				}
				if (this.DyingFromPollution || this.DyingFromNoPollution)
				{
					num = Mathf.Max(num, Plant.PollutionDamagePerTickRange.RandomInRangeSeeded(base.Position.GetHashCode()));
				}
				if (this.DyingBecauseOfTerrainTags)
				{
					num = Mathf.Max(num, 0.005f);
				}
				return num;
			}
		}

		// Token: 0x170026BB RID: 9915
		// (get) Token: 0x0600F709 RID: 63241 RVA: 0x0047BCDD File Offset: 0x00479EDD
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.diesToLight && base.Spawned && base.Map.glowGrid.GroundGlowAt(base.Position, true, false) > 0f;
			}
		}

		// Token: 0x170026BC RID: 9916
		// (get) Token: 0x0600F70A RID: 63242 RVA: 0x0047BD1A File Offset: 0x00479F1A
		public virtual bool DyingBecauseExposedToVacuum
		{
			get
			{
				return !this.def.plant.vacuumResistant && base.Spawned && base.Position.GetVacuum(base.Map) >= 0.5f;
			}
		}

		// Token: 0x170026BD RID: 9917
		// (get) Token: 0x0600F70B RID: 63243 RVA: 0x0047BD54 File Offset: 0x00479F54
		public virtual bool DyingBecauseOfTerrainTags
		{
			get
			{
				return this.def.plant.WildTerrainTags.Count > 0 && !this.def.plant.WildTerrainTags.Overlaps(base.Position.GetTerrain(base.Map).tags.OrElseEmptyEnumerable<string>());
			}
		}

		// Token: 0x170026BE RID: 9918
		// (get) Token: 0x0600F70C RID: 63244 RVA: 0x0047BDAE File Offset: 0x00479FAE
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x170026BF RID: 9919
		// (get) Token: 0x0600F70D RID: 63245 RVA: 0x0047BDBD File Offset: 0x00479FBD
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x170026C0 RID: 9920
		// (get) Token: 0x0600F70E RID: 63246 RVA: 0x0047BDDC File Offset: 0x00479FDC
		public virtual float GrowthRate
		{
			get
			{
				if (this.Blighted)
				{
					return 0f;
				}
				if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
				{
					return 0f;
				}
				return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light * this.GrowthRateFactor_NoxiousHaze * this.GrowthRateFactor_Drought;
			}
		}

		// Token: 0x170026C1 RID: 9921
		// (get) Token: 0x0600F70F RID: 63247 RVA: 0x0047BE40 File Offset: 0x0047A040
		public virtual string GrowthRateCalcDesc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.GrowthRateFactor_Fertility != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FertilityLower".Translate()) + ": " + this.GrowthRateFactor_Fertility.ToStringPercent());
				}
				if (this.GrowthRateFactor_Temperature != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("TemperatureLower".Translate()) + ": " + this.GrowthRateFactor_Temperature.ToStringPercent());
				}
				if (this.GrowthRateFactor_Light != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("LightLower".Translate()) + ": " + this.GrowthRateFactor_Light.ToStringPercent());
				}
				if (ModsConfig.BiotechActive && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze) && this.GrowthRateFactor_NoxiousHaze != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate(GameConditionDefOf.NoxiousHaze.label) + ": " + this.GrowthRateFactor_NoxiousHaze.ToStringPercent());
				}
				if (ModsConfig.OdysseyActive && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Drought) && this.GrowthRateFactor_Drought != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate(GameConditionDefOf.Drought.label) + ": " + this.GrowthRateFactor_Drought.ToStringPercent());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170026C2 RID: 9922
		// (get) Token: 0x0600F710 RID: 63248 RVA: 0x0047C014 File Offset: 0x0047A214
		protected float GrowthPerTick
		{
			get
			{
				if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
				{
					return 0f;
				}
				if (!this.def.plant.vacuumResistant && base.Position.GetVacuum(base.Map) >= 0.5f)
				{
					return 0f;
				}
				return 1f / (60000f * this.def.plant.growDays) * this.GrowthRate;
			}
		}

		// Token: 0x170026C3 RID: 9923
		// (get) Token: 0x0600F711 RID: 63249 RVA: 0x0047C08B File Offset: 0x0047A28B
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return PlantUtility.GrowthRateFactorFor_Fertility(this.def, base.Map.fertilityGrid.FertilityAt(base.Position));
			}
		}

		// Token: 0x170026C4 RID: 9924
		// (get) Token: 0x0600F712 RID: 63250 RVA: 0x0047C0B0 File Offset: 0x0047A2B0
		public float GrowthRateFactor_Light
		{
			get
			{
				float glow = base.Map.glowGrid.GroundGlowAt(base.Position, false, false);
				return PlantUtility.GrowthRateFactorFor_Light(this.def, glow);
			}
		}

		// Token: 0x170026C5 RID: 9925
		// (get) Token: 0x0600F713 RID: 63251 RVA: 0x0047C0E4 File Offset: 0x0047A2E4
		public float GrowthRateFactor_Temperature
		{
			get
			{
				float cellTemp;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out cellTemp))
				{
					return 1f;
				}
				return PlantUtility.GrowthRateFactorFor_Temperature(this.def, cellTemp);
			}
		}

		// Token: 0x170026C6 RID: 9926
		// (get) Token: 0x0600F714 RID: 63252 RVA: 0x0047C118 File Offset: 0x0047A318
		public float GrowthRateFactor_NoxiousHaze
		{
			get
			{
				if (NoxiousHazeUtility.IsExposedToNoxiousHaze(this))
				{
					return 0.5f;
				}
				return 1f;
			}
		}

		// Token: 0x170026C7 RID: 9927
		// (get) Token: 0x0600F715 RID: 63253 RVA: 0x0047C130 File Offset: 0x0047A330
		public float GrowthRateFactor_Drought
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return 1f;
				}
				if (this.CurrentlyCultivated())
				{
					Building edifice = base.Position.GetEdifice(base.Map);
					if (edifice != null && edifice.def.building.SupportsPlants)
					{
						return 1f;
					}
				}
				if (this.def.plant.cavePlant)
				{
					return 1f;
				}
				if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Drought))
				{
					return 0.3f;
				}
				return 1f;
			}
		}

		// Token: 0x170026C8 RID: 9928
		// (get) Token: 0x0600F716 RID: 63254 RVA: 0x0047C1BC File Offset: 0x0047A3BC
		protected int TicksUntilFullyGrown
		{
			get
			{
				if (this.growthInt > 0.9999f)
				{
					return 0;
				}
				float growthPerTick = this.GrowthPerTick;
				if (growthPerTick == 0f)
				{
					return int.MaxValue;
				}
				return (int)((1f - this.growthInt) / growthPerTick);
			}
		}

		// Token: 0x170026C9 RID: 9929
		// (get) Token: 0x0600F717 RID: 63255 RVA: 0x0047C1FC File Offset: 0x0047A3FC
		protected string GrowthPercentString
		{
			get
			{
				return (Mathf.Floor((this.growthInt + 0.0001f) * 100f) / 100f).ToStringPercent();
			}
		}

		// Token: 0x170026CA RID: 9930
		// (get) Token: 0x0600F718 RID: 63256 RVA: 0x0047C220 File Offset: 0x0047A420
		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(this.GrowthPercentString));
					if (this.Dying)
					{
						stringBuilder.Append(", " + "DyingLower".Translate());
					}
					stringBuilder.Append(")");
					this.cachedLabelMouseover = stringBuilder.ToString();
				}
				return this.cachedLabelMouseover;
			}
		}

		// Token: 0x170026CB RID: 9931
		// (get) Token: 0x0600F719 RID: 63257 RVA: 0x0047C2C6 File Offset: 0x0047A4C6
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x170026CC RID: 9932
		// (get) Token: 0x0600F71A RID: 63258 RVA: 0x0047C2D5 File Offset: 0x0047A4D5
		public virtual PlantLifeStage LifeStage
		{
			get
			{
				if (this.growthInt < 0.0001f)
				{
					return PlantLifeStage.Sowing;
				}
				if (this.growthInt > 0.999f)
				{
					return PlantLifeStage.Mature;
				}
				return PlantLifeStage.Growing;
			}
		}

		// Token: 0x170026CD RID: 9933
		// (get) Token: 0x0600F71B RID: 63259 RVA: 0x0047C2F8 File Offset: 0x0047A4F8
		public override Graphic Graphic
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					return Plant.GraphicSowing;
				}
				if (this.def.plant.pollutedGraphic != null && base.PositionHeld.IsPolluted(base.MapHeld))
				{
					return this.def.plant.pollutedGraphic;
				}
				if (this.def.plant.leaflessImmatureGraphic != null && this.LeaflessNow && !this.HarvestableNow)
				{
					return this.def.plant.leaflessImmatureGraphic;
				}
				if (this.def.plant.leaflessGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					return this.def.plant.leaflessGraphic;
				}
				if (this.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					return this.def.plant.immatureGraphic;
				}
				return base.Graphic;
			}
		}

		// Token: 0x170026CE RID: 9934
		// (get) Token: 0x0600F71C RID: 63260 RVA: 0x0047C3E8 File Offset: 0x0047A5E8
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x170026CF RID: 9935
		// (get) Token: 0x0600F71D RID: 63261 RVA: 0x0047C405 File Offset: 0x0047A605
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				return this.def.plant.minGrowthTemperature + Rand.RangeSeeded(-18f, -10f, this.thingIDNumber ^ 838051265);
			}
		}

		// Token: 0x170026D0 RID: 9936
		// (get) Token: 0x0600F71E RID: 63262 RVA: 0x0047C434 File Offset: 0x0047A634
		public bool IsCrop
		{
			get
			{
				if (!this.def.plant.Sowable)
				{
					return false;
				}
				if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.");
					return false;
				}
				return this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
			}
		}

		// Token: 0x170026D1 RID: 9937
		// (get) Token: 0x0600F71F RID: 63263 RVA: 0x0047C482 File Offset: 0x0047A682
		public bool DyingFromPollution
		{
			get
			{
				return this.def.plant.RequiresNoPollution && base.Position.IsPolluted(base.Map);
			}
		}

		// Token: 0x170026D2 RID: 9938
		// (get) Token: 0x0600F720 RID: 63264 RVA: 0x0047C4A9 File Offset: 0x0047A6A9
		public bool DyingFromNoPollution
		{
			get
			{
				return this.def.plant.RequiresPollution && !base.Position.IsPolluted(base.Map);
			}
		}

		// Token: 0x0600F721 RID: 63265 RVA: 0x0047C4D3 File Offset: 0x0047A6D3
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.CheckMakeLeafless();
			}
		}

		// Token: 0x0600F722 RID: 63266 RVA: 0x0047C4F8 File Offset: 0x0047A6F8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (mode != DestroyMode.WillReplace && firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x0600F723 RID: 63267 RVA: 0x0047C52C File Offset: 0x0047A72C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x0600F724 RID: 63268 RVA: 0x0047C5A1 File Offset: 0x0047A7A1
		public override void PostMapInit()
		{
			this.CheckMakeLeafless();
		}

		// Token: 0x0600F725 RID: 63269 RVA: 0x0047C5AC File Offset: 0x0047A7AC
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true, -1);
			float num = this.growthInt * statValue;
			nutritionIngested = Mathf.Min(nutritionWanted, num);
			if (nutritionIngested >= num)
			{
				numTaken = 1;
				return;
			}
			numTaken = 0;
			this.growthInt -= nutritionIngested / statValue;
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600F726 RID: 63270 RVA: 0x0047C624 File Offset: 0x0047A824
		public virtual void PlantCollected(Pawn by, PlantDestructionMode plantDestructionMode)
		{
			if (this.def.plant.HarvestDestroys)
			{
				if (this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, by.Named(HistoryEventArgsNames.Doer)), true);
					base.Map.treeDestructionTracker.Notify_TreeCut(by);
				}
				if (this.Blighted && plantDestructionMode == PlantDestructionMode.Cut)
				{
					base.Map.floodFiller.FloodFill(base.Position, delegate(IntVec3 cell)
					{
						if (this.Position == cell)
						{
							return true;
						}
						Plant plant = cell.GetPlant(this.Map);
						return plant != null && !this.Map.designationManager.HasMapDesignationOn(plant) && plant.Blighted;
					}, delegate(IntVec3 cell)
					{
						if (this.Position == cell)
						{
							return;
						}
						Plant plant = cell.GetPlant(this.Map);
						this.Map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant, null));
						Pawn_JobTracker jobs2 = by.jobs;
						if (jobs2 == null)
						{
							return;
						}
						Job curJob2 = jobs2.curJob;
						if (curJob2 == null)
						{
							return;
						}
						List<LocalTargetInfo> targetQueueA2 = curJob2.targetQueueA;
						if (targetQueueA2 == null)
						{
							return;
						}
						targetQueueA2.Add(plant);
					}, 100, false, null);
				}
				Thing thing = this.TrySpawnStump(plantDestructionMode);
				Map map = base.Map;
				this.Destroy(DestroyMode.KillFinalizeLeavingsOnly);
				if (thing != null && plantDestructionMode == PlantDestructionMode.Cut && by.Faction == Faction.OfPlayer)
				{
					map.designationManager.AddDesignation(new Designation(thing, DesignationDefOf.CutPlant, null));
					Pawn_JobTracker jobs = by.jobs;
					if (jobs == null)
					{
						return;
					}
					Job curJob = jobs.curJob;
					if (curJob == null)
					{
						return;
					}
					List<LocalTargetInfo> targetQueueA = curJob.targetQueueA;
					if (targetQueueA == null)
					{
						return;
					}
					targetQueueA.Add(thing);
					return;
				}
			}
			else
			{
				this.growthInt = this.def.plant.harvestAfterGrowth;
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600F727 RID: 63271 RVA: 0x0047C7B4 File Offset: 0x0047A9B4
		public Thing TrySpawnStump(PlantDestructionMode treeDestructionMode)
		{
			if (!base.Spawned || this.LifeStage == PlantLifeStage.Sowing)
			{
				return null;
			}
			if (!this.HarvestableNow)
			{
				return null;
			}
			ThingDef thingDef = null;
			switch (treeDestructionMode)
			{
			case PlantDestructionMode.Smash:
				thingDef = this.def.plant.smashedThingDef;
				break;
			case PlantDestructionMode.Flame:
				thingDef = this.def.plant.burnedThingDef;
				break;
			case PlantDestructionMode.Chop:
			case PlantDestructionMode.Cut:
				thingDef = this.def.plant.choppedThingDef;
				break;
			}
			if (thingDef != null)
			{
				Thing thing = GenSpawn.Spawn(thingDef, base.Position, base.Map, WipeMode.Vanish);
				DeadPlant deadPlant = thing as DeadPlant;
				if (deadPlant != null)
				{
					deadPlant.Growth = this.Growth;
				}
				if (Find.Selector.IsSelected(this))
				{
					Find.Selector.Select(thing, false, false);
				}
				return thing;
			}
			return null;
		}

		// Token: 0x0600F728 RID: 63272 RVA: 0x0047C878 File Offset: 0x0047AA78
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && dinfo != null)
			{
				if (dinfo.Value.Def == DamageDefOf.Flame)
				{
					this.TrySpawnStump(PlantDestructionMode.Flame);
				}
				else
				{
					this.TrySpawnStump(PlantDestructionMode.Smash);
				}
				if (this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped)
				{
					base.Map.treeDestructionTracker.Notify_TreeDestroyed(dinfo.Value);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600F729 RID: 63273 RVA: 0x0047C904 File Offset: 0x0047AB04
		protected virtual void CheckMakeLeafless()
		{
			if (this.DyingFromPollution)
			{
				this.MakeLeafless(Plant.LeaflessCause.Pollution, true);
				return;
			}
			if (this.DyingFromNoPollution)
			{
				this.MakeLeafless(Plant.LeaflessCause.NoPollution, true);
				return;
			}
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature && base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold, true);
			}
		}

		// Token: 0x0600F72A RID: 63274 RVA: 0x0047C960 File Offset: 0x0047AB60
		public virtual void MakeLeafless(Plant.LeaflessCause cause, bool sendMessage = true)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == Plant.LeaflessCause.Poison && this.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop && sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			else if (this.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					if (cause == Plant.LeaflessCause.Cold)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Poison)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Pollution)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPollution-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPollution".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.NoPollution && sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfNoPollution-" + this.def.defName, 240f))
					{
						Messages.Message("MessagePlantDiedOfNoPollution".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			else
			{
				this.madeLeaflessTick = Find.TickManager.TicksGame;
			}
			if (flag)
			{
				map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600F72B RID: 63275 RVA: 0x0047CC41 File Offset: 0x0047AE41
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.IsHashIntervalTick(2000, delta))
			{
				this.TickLong();
			}
		}

		// Token: 0x0600F72C RID: 63276 RVA: 0x0047CC60 File Offset: 0x0047AE60
		public override void TickLong()
		{
			this.CheckMakeLeafless();
			if (base.Destroyed)
			{
				return;
			}
			base.TickLong();
			if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
			{
				float num = this.growthInt;
				bool flag = this.LifeStage == PlantLifeStage.Mature;
				this.growthInt += this.GrowthPerTick * 2000f;
				if (this.growthInt > 1f)
				{
					this.growthInt = 1f;
				}
				if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				}
			}
			if (!this.HasEnoughLightToGrow)
			{
				this.unlitTicks += 2000;
			}
			else
			{
				this.unlitTicks = 0;
			}
			this.ageInt += 2000;
			if (this.Dying)
			{
				Map map = base.Map;
				bool isCrop = this.IsCrop;
				bool harvestableNow = this.HarvestableNow;
				bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
				bool dyingBecauseExposedToVacuum = this.DyingBecauseExposedToVacuum;
				int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				if (base.Destroyed && !this.def.plant.skipDeteriorationMessage)
				{
					if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
					{
						string key;
						if (harvestableNow)
						{
							if (ModsConfig.BiotechActive && base.Position.IsPolluted(map))
							{
								key = "MessagePlantDiedOfRot_PollutedTerrain";
							}
							else
							{
								key = "MessagePlantDiedOfRot_LeftUnharvested";
							}
						}
						else if (dyingBecauseExposedToLight)
						{
							key = "MessagePlantDiedOfRot_ExposedToLight";
						}
						else if (dyingBecauseExposedToVacuum)
						{
							key = "MessagePlantDiedOfRot_ExposedToVacuum";
						}
						else
						{
							key = "MessagePlantDiedOfRot";
						}
						Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
					return;
				}
			}
			this.cachedLabelMouseover = null;
			if (this.def.plant.dropLeaves)
			{
				MoteLeaf moteLeaf = MoteMaker.MakeStaticMote(Vector3.zero, base.Map, ThingDefOf.Mote_Leaf, 1f, false, 0f) as MoteLeaf;
				if (moteLeaf != null)
				{
					float num3 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
					float treeHeight = this.def.graphicData.drawSize.x * num3;
					Vector3 vector = Rand.InsideUnitCircleVec3 * Plant.LeafSpawnRadius;
					moteLeaf.Initialize(base.Position.ToVector3Shifted() + Vector3.up * Rand.Range(Plant.LeafSpawnYMin, Plant.LeafSpawnYMax) + vector + Vector3.forward * this.def.graphicData.shadowData.offset.z, Rand.Value * 2000.TicksToSeconds(), vector.z > 0f, treeHeight);
				}
			}
		}

		// Token: 0x0600F72D RID: 63277 RVA: 0x0047CFC0 File Offset: 0x0047B1C0
		protected virtual bool CurrentlyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			if (zone != null && zone is Zone_Growing)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600F72E RID: 63278 RVA: 0x0047D03C File Offset: 0x0047B23C
		public bool DeliberatelyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone_Growing zone_Growing = base.Map.zoneManager.ZoneAt(base.Position) as Zone_Growing;
			if (zone_Growing != null && zone_Growing.GetPlantDefToGrow() == this.def)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600F72F RID: 63279 RVA: 0x0047D0C0 File Offset: 0x0047B2C0
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x0600F730 RID: 63280 RVA: 0x0047D0F0 File Offset: 0x0047B2F0
		public virtual int YieldNow()
		{
			if (!this.CanYieldNow())
			{
				return 0;
			}
			float num = this.def.plant.harvestYield;
			float num2 = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num2 = 0.5f + num2 * 0.5f;
			num *= num2;
			num *= Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints);
			if (this.def.plant.harvestYieldAffectedByDifficulty)
			{
				num *= Find.Storyteller.difficulty.cropYieldFactor;
			}
			return GenMath.RoundRandom(num);
		}

		// Token: 0x0600F731 RID: 63281 RVA: 0x0047D198 File Offset: 0x0047B398
		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)this.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = this.def.graphicData.drawSize.x * num2;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag = false;
			foreach (int num5 in positionIndices)
			{
				Vector3 vector;
				if (this.def.plant.maxMeshCount != 1)
				{
					int num6 = 1;
					int maxMeshCount = this.def.plant.maxMeshCount;
					if (maxMeshCount <= 4)
					{
						if (maxMeshCount != 1)
						{
							if (maxMeshCount != 4)
							{
								goto IL_150;
							}
							num6 = 2;
						}
						else
						{
							num6 = 1;
						}
					}
					else if (maxMeshCount != 9)
					{
						if (maxMeshCount != 16)
						{
							if (maxMeshCount != 25)
							{
								goto IL_150;
							}
							num6 = 5;
						}
						else
						{
							num6 = 4;
						}
					}
					else
					{
						num6 = 3;
					}
					IL_171:
					float num7 = 1f / (float)num6;
					vector = base.Position.ToVector3();
					vector.y = this.def.Altitude;
					vector.x += 0.5f * num7;
					vector.z += 0.5f * num7;
					int num8 = num5 / num6;
					int num9 = num5 % num6;
					vector.x += (float)num8 * num7;
					vector.z += (float)num9 * num7;
					float max = num7 * 0.3f;
					vector += Gen.RandomHorizontalVector(max);
					goto IL_20F;
					IL_150:
					ThingDef def = this.def;
					Log.Error(((def != null) ? def.ToString() : null) + " must have plant.MaxMeshCount that is a perfect square.");
					goto IL_171;
				}
				vector = a + Gen.RandomHorizontalVector(0.05f);
				float num10 = (float)base.Position.z;
				if (vector.z - num2 / 2f < num10)
				{
					vector.z = num10 + num2 / 2f;
					flag = true;
				}
				IL_20F:
				bool @bool = Rand.Bool;
				Material mat = this.Graphic.MatSingleFor(this);
				if (this.Graphic is Graphic_Random)
				{
					mat = this.Graphic.MatSingle;
				}
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, this.def.category.ToAtlasGroup(), @bool, false, out mat, out uvs, out color);
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 size = new Vector2(num3, num3);
				Printer_Plane.PrintPlane(layer, vector, size, mat, 0f, @bool, uvs, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				num4++;
				if (num4 >= num)
				{
					break;
				}
			}
			if (this.def.graphicData.shadowData != null)
			{
				Vector3 center = a + this.def.graphicData.shadowData.offset * num2;
				if (flag)
				{
					center.z = base.Position.ToVector3Shifted().z + this.def.graphicData.shadowData.offset.z;
				}
				center.y -= 0.03658537f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center, volume, Rot4.North);
			}
			Rand.PopState();
		}

		// Token: 0x0600F732 RID: 63282 RVA: 0x0047D514 File Offset: 0x0047B714
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.def.plant.showGrowthInInspectPane)
			{
				if (this.LifeStage == PlantLifeStage.Growing)
				{
					stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
					stringBuilder.Append("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
					if (!this.Blighted)
					{
						string[] array = ArrayPool<string>.Shared.Rent(4);
						int count = 0;
						if (this.Resting)
						{
							Plant.<GetInspectString>g__AddCondition|115_0(array, ref count, "PlantResting".Translate());
						}
						if (!this.HasEnoughLightToGrow)
						{
							Plant.<GetInspectString>g__AddCondition|115_0(array, ref count, "PlantNeedsLightLevel".Translate() + " " + this.def.plant.growMinGlow.ToStringPercent());
						}
						float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
						if (growthRateFactor_Temperature < 0.99f)
						{
							if (Mathf.Approximately(growthRateFactor_Temperature, 0f) || !PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
							{
								Plant.<GetInspectString>g__AddCondition|115_0(array, ref count, "OutOfIdealTemperatureRangeNotGrowing".Translate());
							}
							else
							{
								Plant.<GetInspectString>g__AddCondition|115_0(array, ref count, "OutOfIdealTemperatureRange".Translate(Mathf.Max(1, Mathf.RoundToInt(growthRateFactor_Temperature * 100f)).ToString()));
							}
						}
						if (this.GrowthRateFactor_Drought < 0.99f)
						{
							Plant.<GetInspectString>g__AddCondition|115_0(array, ref count, GameConditionDefOf.Drought.label);
						}
						string text = string.Join(", ", array, 0, count);
						ArrayPool<string>.Shared.Return(array, false);
						if (!text.NullOrEmpty())
						{
							stringBuilder.Append(" (").Append(text).Append(')');
						}
					}
					stringBuilder.AppendLine();
				}
				else if (this.LifeStage == PlantLifeStage.Mature)
				{
					stringBuilder.AppendLine(this.HarvestableNow ? "ReadyToHarvest".Translate() : "Mature".Translate());
				}
				if (this.DyingBecauseExposedToLight)
				{
					stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
				}
				if (this.DyingBecauseExposedToVacuum)
				{
					stringBuilder.AppendLine("DyingBecauseExposedToVacuum".Translate());
				}
				if (this.DyingBecauseOfTerrainTags)
				{
					stringBuilder.AppendLine("DyingBecauseOfTerrain".Translate());
				}
				if (this.Blighted)
				{
					stringBuilder.AppendLine(string.Format("{0} ({1})", "Blighted".Translate(), this.Blight.Severity.ToStringPercent()));
				}
			}
			string text2 = base.InspectStringPartsFromComps();
			if (!text2.NullOrEmpty())
			{
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600F733 RID: 63283 RVA: 0x0047D7EA File Offset: 0x0047B9EA
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600F734 RID: 63284 RVA: 0x0047D80C File Offset: 0x0047BA0C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Blighted)
			{
				Designation designation = base.Map.designationManager.DesignationOn(this);
				if (designation == null || designation.def != DesignationDefOf.CutPlant)
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "CutAllBlight".Translate(),
						defaultDesc = "CutAllBlightDesc".Translate(),
						icon = Plant.CutAllBlightTex,
						action = delegate()
						{
							foreach (Thing thing in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Plant))
							{
								Plant plant = (Plant)thing;
								if (plant != null && plant.Blighted && !base.Map.designationManager.HasMapDesignationOn(plant))
								{
									base.Map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant, null));
								}
							}
						}
					};
					yield return command_Action;
				}
			}
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.Blighted)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Spread blight",
						action = delegate()
						{
							this.Blight.TryReproduceNow();
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Make blighted",
						action = new Action(this.CropBlighted)
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F735 RID: 63285 RVA: 0x0047D81C File Offset: 0x0047BA1C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.def.plant.LimitedLifespan)
			{
				string valueString = this.Age.ToStringTicksToPeriod(true, false, true, true, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Plant_Age".Translate(), valueString, "Stat_Thing_Plant_AgeDesc".Translate(), 4170, null, null, false, false);
			}
			if (this.LifeStage == PlantLifeStage.Growing && base.Spawned)
			{
				TaggedString taggedString = "Stat_Thing_Plant_GrowthRate_Desc".Translate();
				string growthRateCalcDesc = this.GrowthRateCalcDesc;
				if (!growthRateCalcDesc.NullOrEmpty())
				{
					taggedString += "\n\n" + growthRateCalcDesc;
				}
				taggedString += "\n" + "StatsReport_FinalValue".Translate() + ": " + this.GrowthRate.ToStringPercent();
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Plant_GrowthRate".Translate(), this.GrowthRate.ToStringPercent(), taggedString, 4158, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F738 RID: 63288 RVA: 0x0047D8D8 File Offset: 0x0047BAD8
		[CompilerGenerated]
		internal static void <GetInspectString>g__AddCondition|115_0(string[] conditions, ref int count, string condition)
		{
			if (count < conditions.Length)
			{
				int num = count;
				count = num + 1;
				conditions[num] = condition;
				return;
			}
			Log.Error("Too many conditions for plant growth inspect string");
		}

		// Token: 0x0400A28D RID: 41613
		protected float growthInt = 0.15f;

		// Token: 0x0400A28E RID: 41614
		protected int ageInt;

		// Token: 0x0400A28F RID: 41615
		protected int unlitTicks;

		// Token: 0x0400A290 RID: 41616
		protected int madeLeaflessTick = -99999;

		// Token: 0x0400A291 RID: 41617
		public bool sown;

		// Token: 0x0400A292 RID: 41618
		private string cachedLabelMouseover;

		// Token: 0x0400A293 RID: 41619
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x0400A294 RID: 41620
		public const float BaseGrowthPercent = 0.15f;

		// Token: 0x0400A295 RID: 41621
		public const float BaseSownGrowthPercent = 0.0001f;

		// Token: 0x0400A296 RID: 41622
		public const float MinGrowthForAnimalIngestion = 0.1f;

		// Token: 0x0400A297 RID: 41623
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x0400A298 RID: 41624
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x0400A299 RID: 41625
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x0400A29A RID: 41626
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x0400A29B RID: 41627
		public const float DefaultMinGrowthTemperature = 0f;

		// Token: 0x0400A29C RID: 41628
		public const float DefaultMinOptimalGrowthTemperature = 6f;

		// Token: 0x0400A29D RID: 41629
		public const float DefaultMaxOptimalGrowthTemperature = 42f;

		// Token: 0x0400A29E RID: 41630
		public const float DefaultMaxGrowthTemperature = 58f;

		// Token: 0x0400A29F RID: 41631
		private const float MinLeaflessTemperatureOffset = -18f;

		// Token: 0x0400A2A0 RID: 41632
		public const float MaxLeaflessTemperatureOffset = -10f;

		// Token: 0x0400A2A1 RID: 41633
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x0400A2A2 RID: 41634
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x0400A2A3 RID: 41635
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x0400A2A4 RID: 41636
		private static readonly FloatRange PollutionDamagePerTickRange = new FloatRange(1.6666667E-05f, 0.00016666666f);

		// Token: 0x0400A2A5 RID: 41637
		private const float VacuumDamagePerTick = 1f;

		// Token: 0x0400A2A6 RID: 41638
		private static readonly Texture2D CutAllBlightTex = ContentFinder<Texture2D>.Get("UI/Commands/CutAllBlightedPlants", true);

		// Token: 0x0400A2A7 RID: 41639
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x0400A2A8 RID: 41640
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x0400A2A9 RID: 41641
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x02002B2D RID: 11053
		public enum LeaflessCause
		{
			// Token: 0x0400A2AB RID: 41643
			Cold,
			// Token: 0x0400A2AC RID: 41644
			Poison,
			// Token: 0x0400A2AD RID: 41645
			Pollution,
			// Token: 0x0400A2AE RID: 41646
			NoPollution
		}
	}
}
