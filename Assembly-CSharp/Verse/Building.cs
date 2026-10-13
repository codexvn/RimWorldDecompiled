using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200086C RID: 2156
	public class Building : ThingWithComps
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600365A RID: 13914 RVA: 0x0012385C File Offset: 0x00121A5C
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600365B RID: 13915 RVA: 0x00123864 File Offset: 0x00121A64
		public ColorDef PaintColorDef
		{
			get
			{
				return this.paintColorDef;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x0012386C File Offset: 0x00121A6C
		public override Color DrawColor
		{
			get
			{
				if (this.paintColorDef != null)
				{
					return this.paintColorDef.color;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600365D RID: 13917 RVA: 0x00123888 File Offset: 0x00121A88
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (set) Token: 0x0600365E RID: 13918 RVA: 0x001238AC File Offset: 0x00121AAC
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
				if (base.Spawned)
				{
					base.Map.events.Notify_BuildingHitPointsChanged(this);
				}
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x001238E7 File Offset: 0x00121AE7
		public virtual int MaxItemsInCell
		{
			get
			{
				return this.def.building.maxItemsInCell;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x001238F9 File Offset: 0x00121AF9
		public bool IsClearableFreeBuilding
		{
			get
			{
				return !this.def.useHitPoints && !this.def.scatterableOnMapGen && this.def.passability == Traversability.Standable && this.GetStatValue(StatDefOf.WorkToBuild, true, -1) == 0f;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x00123938 File Offset: 0x00121B38
		public virtual bool IsAirtight
		{
			get
			{
				return this.def.building.isAirtight || (this.def.building.isStuffableAirtight && base.Stuff.stuffProps.isAirtight);
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x00123975 File Offset: 0x00121B75
		public virtual bool ExchangeVacuum
		{
			get
			{
				return !this.IsAirtight || this.def.building.alwaysExchangeVacuum;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int MinTickIntervalRate
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00123994 File Offset: 0x00121B94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
			Scribe_Defs.Look<ColorDef>(ref this.paintColorDef, "paintColorDef");
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x001239C0 File Offset: 0x00121BC0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (this.def.coversFloor)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Terrain, true, false);
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Buildings);
					if (this.def.blockLight)
					{
						base.Map.glowGrid.LightBlockerAdded(intVec);
					}
					if (!SnowGrid.CanCoexistWithSnow(this.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
					if (ModsConfig.OdysseyActive && !SandGrid.CanCoexistWithSand(this.def))
					{
						base.Map.sandGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer && this.def.building != null && this.def.building.spawnedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(this.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
			base.Map.listerBuldingOfDefInProximity.Notify_BuildingSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
			map.events.Notify_BuildingSpawned(this);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00123BF0 File Offset: 0x00121DF0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			List<Thing> attachedBuildings = GenConstruct.GetAttachedBuildings(this);
			base.DeSpawn(mode);
			if ((!this.def.IsFrame || mode != DestroyMode.Vanish) && (!this.def.building.isNaturalRock || mode != DestroyMode.WillReplace) && mode != DestroyMode.WillReplace)
			{
				foreach (Thing thing in attachedBuildings)
				{
					if (thing.def.Minifiable)
					{
						GenSpawn.Spawn(thing.MakeMinified(DestroyMode.Vanish), thing.Position, map, WipeMode.Vanish);
					}
					else
					{
						thing.Destroy(mode);
					}
				}
			}
			if (this.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
					if (ModsConfig.BiotechActive)
					{
						map.pollutionGrid.Drawer.SetDirty();
					}
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				if (this.def.MakeFog)
				{
					map.fogGrid.Notify_FogBlockerRemoved(this);
				}
				if (this.def.holdsRoof)
				{
					RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
				}
				if (this.def.IsSmoothable)
				{
					SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
				}
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					ulong num = MapMeshFlagDefOf.Buildings;
					if (this.def.coversFloor)
					{
						num |= MapMeshFlagDefOf.Terrain;
					}
					if (this.def.Fillage == FillCategory.Full)
					{
						num |= MapMeshFlagDefOf.Roofs;
						num |= MapMeshFlagDefOf.Snow;
						if (ModsConfig.OdysseyActive)
						{
							num |= MapMeshFlagDefOf.Sand;
						}
					}
					map.mapDrawer.MapMeshDirty(intVec, num);
					if (this.def.blockLight)
					{
						map.glowGrid.LightBlockerRemoved(intVec);
					}
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			map.listerArtificialBuildingsForMeditation.Notify_BuildingDeSpawned(this);
			map.listerBuldingOfDefInProximity.Notify_BuildingDeSpawned(this);
			map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				foreach (IntVec3 c in this.OccupiedRect())
				{
					map.terrainGrid.SetTerrain(c, this.def.building.leaveTerrain);
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (this.def.building.hasFuelingPort)
			{
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation), map);
				if (compLaunchable != null && !base.BeingTransportedOnGravship)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			map.events.Notify_BuildingDespawned(this);
			if (this.MaxItemsInCell >= 2)
			{
				foreach (IntVec3 intVec2 in this.OccupiedRect())
				{
					int itemCount = intVec2.GetItemCount(map);
					if (itemCount > 1 && itemCount > intVec2.GetMaxItemsAllowedInCell(map))
					{
						for (int k = 0; k < itemCount - 1; k++)
						{
							Thing firstItem = intVec2.GetFirstItem(map);
							if (firstItem == null)
							{
								break;
							}
							firstItem.DeSpawn(DestroyMode.Vanish);
							GenPlace.TryPlaceThing(firstItem, intVec2, map, ThingPlaceMode.Near, null, null, null, 1);
							if (intVec2.GetItemCount(map) <= intVec2.GetMaxItemsAllowedInCell(map))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00124020 File Offset: 0x00122220
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_BuildingLost(this, null);
			}
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (spawned)
			{
				if (mode == DestroyMode.Deconstruct)
				{
					SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else if (mode == DestroyMode.KillFinalize)
				{
					this.DoDestroyEffects(map);
				}
			}
			if (spawned)
			{
				Blueprint_Storage blueprint_Storage = ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def) as Blueprint_Storage;
				if (blueprint_Storage != null)
				{
					Building_Storage building_Storage = this as Building_Storage;
					if (building_Storage != null)
					{
						blueprint_Storage.SetStorageGroup(building_Storage.storageGroup, true);
						blueprint_Storage.settings = new StorageSettings();
						blueprint_Storage.settings.CopyFrom(building_Storage.settings);
					}
				}
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x001240E8 File Offset: 0x001222E8
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x001241B4 File Offset: 0x001223B4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			if (this.def.building != null && !this.def.building.canBeDamagedByAttacks)
			{
				absorbed = true;
				return;
			}
			if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
			{
				for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
				{
					Lord lord = base.Map.lordManager.lords[i];
					if (lord.faction == base.Faction)
					{
						lord.Notify_BuildingDamaged(this, dinfo);
					}
				}
			}
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed && base.Faction != null)
			{
				base.Faction.Notify_BuildingTookDamage(this, dinfo);
			}
			if (!absorbed)
			{
				CompStunnable comp = base.GetComp<CompStunnable>();
				if (comp == null)
				{
					return;
				}
				comp.ApplyDamage(dinfo);
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00124293 File Offset: 0x00122493
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x001242B8 File Offset: 0x001224B8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x001242E6 File Offset: 0x001224E6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (((this.def.BuildableByPlayer && this.def.passability != Traversability.Impassable && !this.def.IsDoor) || this.def.building.forceShowRoomStats) && Gizmo_RoomStats.GetRoomToShowStatsFor(this) != null && Find.Selector.SingleSelectedObject == this)
			{
				yield return new Gizmo_RoomStats(this);
			}
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			if (this.def.Minifiable && (base.Faction == Faction.OfPlayer || this.def.building.alwaysUninstallable))
			{
				yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			}
			ColorInt? glowerColorOverride = null;
			CompGlower comp = base.GetComp<CompGlower>();
			if (comp != null && comp.HasGlowColorOverride)
			{
				glowerColorOverride = new ColorInt?(comp.GlowColor);
			}
			if (!this.def.building.neverBuildable)
			{
				Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def, base.Stuff, base.StyleSourcePrecept as Precept_Building, this.StyleDef, true, glowerColorOverride);
				if (command != null)
				{
					yield return command;
				}
			}
			if (base.Faction == Faction.OfPlayer || this.def.building.alwaysShowRelatedBuildCommands)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			Lord lord = this.GetLord();
			if (lord != null && lord.CurLordToil != null)
			{
				foreach (Gizmo gizmo2 in lord.CurLordToil.GetBuildingGizmos(this))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x001242F6 File Offset: 0x001224F6
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.PaintColorDef != null && !this.PaintColorDef.label.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_PaintColor".Translate(), this.PaintColorDef.LabelCap, "Stat_Building_PaintColorDesc".Translate(), 6000, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00124308 File Offset: 0x00122508
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			if (!this.def.Claimable)
			{
				return false;
			}
			if (base.Faction == by)
			{
				return false;
			}
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				if (base.AllComps[i].CompPreventClaimingBy(by))
				{
					return false;
				}
			}
			Faction faction;
			if ((faction = base.Faction) == null)
			{
				Map map = base.Map;
				faction = ((map != null) ? map.ParentFaction : null);
			}
			string value;
			if (base.FactionPreventsClaimingOrAdopting(faction, true, out value))
			{
				return value;
			}
			return true;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x001243A0 File Offset: 0x001225A0
		public virtual AcceptanceReport DeconstructibleBy(Faction faction)
		{
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				if (base.AllComps[i].CompForceDeconstructable())
				{
					return true;
				}
			}
			if (!this.def.building.IsDeconstructible)
			{
				return false;
			}
			if (DebugSettings.godMode)
			{
				return true;
			}
			if (base.Faction == faction)
			{
				return true;
			}
			if (this.def.building.alwaysDeconstructible)
			{
				return true;
			}
			return this.ClaimableBy(faction);
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00002501 File Offset: 0x00000701
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsWorking()
		{
			return true;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x00124434 File Offset: 0x00122634
		private void DoDestroyEffects(Map map)
		{
			if (this.def.building.destroyEffecter != null && !base.Position.Fogged(map))
			{
				Effecter effecter = this.def.building.destroyEffecter.Spawn(base.Position, map, 1f);
				effecter.Trigger(new TargetInfo(base.Position, map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
				return;
			}
			if (!this.def.IsEdifice())
			{
				return;
			}
			SoundDef destroySound = this.GetDestroySound();
			if (destroySound != null)
			{
				destroySound.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			foreach (IntVec3 intVec in this.OccupiedRect())
			{
				int num = this.def.building.isNaturalRock ? 1 : Rand.RangeInclusive(3, 5);
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = this.def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = Building.ShakeAmountPerAreaCurve.Evaluate((float)this.def.Size.Area);
				}
				CompLifespan compLifespan = this.TryGetComp<CompLifespan>();
				if (compLifespan == null || compLifespan.age < compLifespan.Props.lifespanTicks)
				{
					Find.CameraDriver.shaker.DoShake(num2);
				}
			}
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x001245DC File Offset: 0x001227DC
		private SoundDef GetDestroySound()
		{
			if (!this.def.building.destroySound.NullOrUndefined())
			{
				return this.def.building.destroySound;
			}
			StuffCategoryDef stuffCategoryDef;
			if (this.def.MadeFromStuff && base.Stuff != null && !base.Stuff.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
			{
				stuffCategoryDef = base.Stuff.stuffProps.categories[0];
			}
			else
			{
				if (this.def.CostList.NullOrEmpty<ThingDefCountClass>() || !this.def.CostList[0].thingDef.IsStuff || this.def.CostList[0].thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
				{
					return null;
				}
				stuffCategoryDef = this.def.CostList[0].thingDef.stuffProps.categories[0];
			}
			switch (this.def.building.buildingSizeCategory)
			{
			case BuildingSizeCategory.None:
			{
				int area = this.def.Size.Area;
				if (area <= 1 && !stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				if (area <= 4 && !stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			case BuildingSizeCategory.Small:
				if (!stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				break;
			case BuildingSizeCategory.Medium:
				if (!stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				break;
			case BuildingSizeCategory.Large:
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			return null;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x001247A0 File Offset: 0x001229A0
		public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.def.building.paintable && Rand.Value < 0.1f)
			{
				this.ChangePaint((from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Structure
				select x).RandomElement<ColorDef>());
				return;
			}
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00124830 File Offset: 0x00122A30
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (!this.DeconstructibleBy(Faction.OfPlayer) && (this.def.IsNonDeconstructibleAttackableBuilding || this.def.building.quickTargetable) && this.def.building.displayAttackToDestroyOnInspectPane)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "AttackToDestroy".Translate();
			}
			return text;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x001248B4 File Offset: 0x00122AB4
		public void ChangePaint(ColorDef colorDef)
		{
			this.paintColorDef = colorDef;
			this.Notify_ColorChanged();
			if (base.Spawned)
			{
				DrawerType drawerType = this.def.drawerType;
				if (drawerType == DrawerType.MapMeshOnly || drawerType == DrawerType.MapMeshAndRealTime)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Buildings);
				}
			}
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x0012490A File Offset: 0x00122B0A
		public static Gizmo SelectContainedItemGizmo(Thing container, Thing item)
		{
			if (!container.Faction.IsPlayerSafe())
			{
				return null;
			}
			return ContainingSelectionUtility.SelectCarriedThingGizmo(container, item);
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00124922 File Offset: 0x00122B22
		public virtual int HaulToContainerDuration(Thing thing)
		{
			return this.def.building.haulToContainerDuration;
		}

		// Token: 0x040028EC RID: 10476
		private ColorDef paintColorDef;

		// Token: 0x040028ED RID: 10477
		public bool canChangeTerrainOnDestroyed = true;

		// Token: 0x040028EE RID: 10478
		private static readonly SimpleCurve ShakeAmountPerAreaCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.07f),
				true
			},
			{
				new CurvePoint(2f, 0.07f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			},
			{
				new CurvePoint(9f, 0.2f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			}
		};

		// Token: 0x040028EF RID: 10479
		private const float ChanceToGeneratePaintedFromTrader = 0.1f;

		// Token: 0x040028F0 RID: 10480
		private const int BuildingMinTickRate = 6;
	}
}
