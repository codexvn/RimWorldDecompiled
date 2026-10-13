using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001942 RID: 6466
	public class BuildingProperties
	{
		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x06009302 RID: 37634 RVA: 0x002B3DC1 File Offset: 0x002B1FC1
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06009303 RID: 37635 RVA: 0x002B3DCC File Offset: 0x002B1FCC
		public int EffectiveMineableYield
		{
			get
			{
				return Mathf.RoundToInt((float)this.mineableYield * Find.Storyteller.difficulty.mineYieldFactor);
			}
		}

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x06009304 RID: 37636 RVA: 0x002B3DEA File Offset: 0x002B1FEA
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x06009305 RID: 37637 RVA: 0x002B3DF5 File Offset: 0x002B1FF5
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06009306 RID: 37638 RVA: 0x002B3E14 File Offset: 0x002B2014
		public bool IsMortar
		{
			get
			{
				if (!this.IsTurret)
				{
					return false;
				}
				List<VerbProperties> verbs = this.turretGunDef.Verbs;
				for (int i = 0; i < verbs.Count; i++)
				{
					if (verbs[i].isPrimary && verbs[i].defaultProjectile != null && verbs[i].defaultProjectile.projectile.flyOverhead)
					{
						return true;
					}
				}
				if (this.turretGunDef.HasComp(typeof(CompChangeableProjectile)))
				{
					if (this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive))
					{
						return true;
					}
					foreach (ThingDef thingDef in this.turretGunDef.building.fixedStorageSettings.filter.AllowedThingDefs)
					{
						if (thingDef.projectileWhenLoaded != null && thingDef.projectileWhenLoaded.projectile.flyOverhead)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06009307 RID: 37639 RVA: 0x002B3F28 File Offset: 0x002B2128
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.isTrap && !this.isEdifice)
			{
				yield return "isTrap but is not edifice. Code will break.";
			}
			if (this.alwaysDeconstructible && !this.deconstructible)
			{
				yield return "alwaysDeconstructible=true but deconstructible=false";
			}
			if (parent.holdsRoof && !this.isEdifice)
			{
				yield return "holds roof but is not an edifice.";
			}
			if (this.buildingTags.Contains("MechClusterCombatThreat") && this.combatPower <= 0f)
			{
				yield return "has MechClusterCombatThreat tag but 0 combatPower and thus no points cost; this will make an infinite loop during mech cluster building selection";
			}
			if (parent.IsGibbetCage && this.gibbetCageTopGraphicData == null)
			{
				yield return "Gibbet cage has no graphic data for gibbet cage top set.";
			}
			if (parent.IsMechGestator)
			{
				if (this.mechGestatorCylinderGraphic == null)
				{
					yield return "Mech gestator has no cylinder graphic set.";
				}
				if (this.mechGestatorTopGraphic == null)
				{
					yield return "Mech gestator has no top graphic set.";
				}
			}
			if (this.isInsectCocoon && this.combatPower <= 0f)
			{
				yield return "Insect cocoon requires combat power value > 0.";
			}
			if (typeof(BuildingGroundSpawner).IsAssignableFrom(parent.thingClass) && this.groundSpawnerThingToSpawn == null)
			{
				yield return "Building ground spawner must have thing to spawn.";
			}
			yield break;
		}

		// Token: 0x06009308 RID: 37640 RVA: 0x000026EA File Offset: 0x000008EA
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x06009309 RID: 37641 RVA: 0x002B3F40 File Offset: 0x002B2140
		public void ResolveReferencesSpecial()
		{
			if (this.soundDoorOpenPowered == null)
			{
				this.soundDoorOpenPowered = SoundDefOf.Door_OpenPowered;
			}
			if (this.soundDoorClosePowered == null)
			{
				this.soundDoorClosePowered = SoundDefOf.Door_ClosePowered;
			}
			if (this.soundDoorOpenManual == null)
			{
				this.soundDoorOpenManual = SoundDefOf.Door_OpenManual;
			}
			if (this.soundDoorCloseManual == null)
			{
				this.soundDoorCloseManual = SoundDefOf.Door_CloseManual;
			}
			if (this.turretGunDef != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretGunDef.graphicData.texPath);
				});
			}
			if (this.turretTopLoadedGraphic != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopLoadedMat = MaterialPool.MatFrom(this.turretTopLoadedGraphic.texPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null && this.fixedStorageSettings != null)
			{
				this.defaultStorageSettings = new StorageSettings();
				this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x002B4028 File Offset: 0x002B2228
		public static void FinalizeInit()
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				BuildingProperties building = thingDef.building;
				if (((building != null) ? building.smoothedThing : null) != null)
				{
					ThingDef thingDef2 = thingDef.building.smoothedThing;
					if (thingDef2.building == null)
					{
						Log.Error(string.Format("{0} is smoothable to non-building {1}", thingDef, thingDef2));
					}
					else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
					{
						thingDef2.building.unsmoothedThing = thingDef;
					}
					else
					{
						Log.Error(string.Format("{0} and {1} both smooth to {2}", thingDef, thingDef2.building.unsmoothedThing, thingDef2));
					}
				}
			}
		}

		// Token: 0x0600930B RID: 37643 RVA: 0x002B40DA File Offset: 0x002B22DA
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			if (this.joyKind != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_RecreationType_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_JoyKind_AllTypes".Translate() + ":");
				foreach (JoyKindDef joyKindDef in DefDatabase<JoyKindDef>.AllDefs)
				{
					stringBuilder.AppendLine("  - " + joyKindDef.LabelCap);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_JoyKind".Translate(), this.joyKind.LabelCap, stringBuilder.ToString(), 4750, this.joyKind.LabelCap, null, false, false);
			}
			if (parentDef.Minifiable)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_WorkToUninstall".Translate(), this.uninstallWork.ToStringWorkAmount(), "Stat_Thing_WorkToUninstall_Desc".Translate(), 1102, null, null, false, false);
			}
			if (typeof(Building_TrapDamager).IsAssignableFrom(parentDef.thingClass))
			{
				float f = StatDefOf.TrapMeleeDamage.Worker.GetValue(req, true) * 0.015f;
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "TrapArmorPenetration".Translate(), f.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 3000, null, null, false, false);
			}
			if (this.isFence)
			{
				TaggedString taggedString = "Stat_Thing_Fence_Desc".Translate();
				BuildingProperties.tmpFenceBlockedAnimals.Clear();
				BuildingProperties.tmpFenceBlockedAnimals.AddRange(from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Animal && k.RaceProps.FenceBlocked
				select k into g
				select g.LabelCap.Resolve() into s
				orderby s
				select s);
				taggedString += ":\n\n";
				taggedString += BuildingProperties.tmpFenceBlockedAnimals.ToLineList("- ");
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_Fence".Translate(), "Yes".Translate(), taggedString, 4800, null, null, false, false);
				BuildingProperties.tmpFenceBlockedAnimals.Clear();
			}
			if (this.workTableRoomRole != null && !Mathf.Approximately(this.workTableNotInRoomRoleFactor, 1f))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_OptimalRoom".Translate(), this.workTableRoomRole.LabelCap, "Stat_Building_OptimalRoom_Desc".Translate(), 4850, null, null, false, false);
			}
			if (ModsConfig.IdeologyActive)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					int num;
					for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
					{
						Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
						if (precept_Building != null && precept_Building.ThingDef == parentDef && precept_Building.presenceDemand != null)
						{
							IdeoBuildingPresenceDemand presenceDemand = precept_Building.presenceDemand;
							if (!presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
							{
								string valueString = (from r in presenceDemand.roomRequirements
								select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
								string reportText = (from r in presenceDemand.roomRequirements
								select r.LabelCap(null)).ToLineList("  - ", false);
								yield return new StatDrawEntry(StatCategoryDefOf.Building, "RoomRequirements".Translate(), valueString, reportText, 2101, null, null, false, false);
							}
						}
						num = i;
					}
					ideo = null;
				}
				IEnumerator<Ideo> enumerator2 = null;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_Paintable".Translate(), this.paintable.ToStringYesNo(), "Stat_Building_PaintableDesc".Translate(), 6000, null, null, false, false);
			yield break;
			yield break;
		}

		// Token: 0x0600930C RID: 37644 RVA: 0x002B40F8 File Offset: 0x002B22F8
		public GenDraw.FillableBarRequest BarDrawDataFor(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.barDrawData.north;
			case 1:
				return this.barDrawData.east;
			case 2:
				return this.barDrawData.south;
			case 3:
				return this.barDrawData.west;
			default:
				return this.barDrawData.north;
			}
		}

		// Token: 0x04005DBA RID: 23994
		public bool isEdifice = true;

		// Token: 0x04005DBB RID: 23995
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x04005DBC RID: 23996
		public bool isInert;

		// Token: 0x04005DBD RID: 23997
		public bool isTargetable = true;

		// Token: 0x04005DBE RID: 23998
		private bool deconstructible = true;

		// Token: 0x04005DBF RID: 23999
		public bool alwaysDeconstructible;

		// Token: 0x04005DC0 RID: 24000
		public bool alwaysUninstallable;

		// Token: 0x04005DC1 RID: 24001
		public List<ThingDef> leavingsBlacklist;

		// Token: 0x04005DC2 RID: 24002
		public List<ThingDef> forcedCostLeavings;

		// Token: 0x04005DC3 RID: 24003
		public bool claimable = true;

		// Token: 0x04005DC4 RID: 24004
		public bool isSittable;

		// Token: 0x04005DC5 RID: 24005
		public bool multiSittable;

		// Token: 0x04005DC6 RID: 24006
		public bool sitIgnoreOrientation;

		// Token: 0x04005DC7 RID: 24007
		public ConceptDef spawnedConceptLearnOpportunity;

		// Token: 0x04005DC8 RID: 24008
		public ConceptDef boughtConceptLearnOpportunity;

		// Token: 0x04005DC9 RID: 24009
		public bool expandHomeArea = true;

		// Token: 0x04005DCA RID: 24010
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x04005DCB RID: 24011
		public GraphicData blueprintGraphicData;

		// Token: 0x04005DCC RID: 24012
		public float uninstallWork = 200f;

		// Token: 0x04005DCD RID: 24013
		public bool forceShowRoomStats;

		// Token: 0x04005DCE RID: 24014
		public bool neverBuildable;

		// Token: 0x04005DCF RID: 24015
		public bool isWall;

		// Token: 0x04005DD0 RID: 24016
		public bool alwaysExchangeVacuum;

		// Token: 0x04005DD1 RID: 24017
		public bool canExchangeVacuum;

		// Token: 0x04005DD2 RID: 24018
		public bool draftAttackNonDeconstructable = true;

		// Token: 0x04005DD3 RID: 24019
		public bool wantsHopperAdjacent;

		// Token: 0x04005DD4 RID: 24020
		public bool allowWireConnection = true;

		// Token: 0x04005DD5 RID: 24021
		public bool shipPart;

		// Token: 0x04005DD6 RID: 24022
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x04005DD7 RID: 24023
		public float heatPerTickWhileWorking;

		// Token: 0x04005DD8 RID: 24024
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x04005DD9 RID: 24025
		public bool canPlaceOverWall;

		// Token: 0x04005DDA RID: 24026
		public bool isPlaceOverableWall;

		// Token: 0x04005DDB RID: 24027
		public bool allowAutoroof = true;

		// Token: 0x04005DDC RID: 24028
		public bool preventDeteriorationOnTop;

		// Token: 0x04005DDD RID: 24029
		public bool preventDeteriorationInside;

		// Token: 0x04005DDE RID: 24030
		public bool isMealSource;

		// Token: 0x04005DDF RID: 24031
		public bool isNaturalRock;

		// Token: 0x04005DE0 RID: 24032
		public bool isResourceRock;

		// Token: 0x04005DE1 RID: 24033
		public bool repairable = true;

		// Token: 0x04005DE2 RID: 24034
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x04005DE3 RID: 24035
		public bool hasFuelingPort;

		// Token: 0x04005DE4 RID: 24036
		public ThingDef smoothedThing;

		// Token: 0x04005DE5 RID: 24037
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		// Token: 0x04005DE6 RID: 24038
		public TerrainDef naturalTerrain;

		// Token: 0x04005DE7 RID: 24039
		public TerrainDef leaveTerrain;

		// Token: 0x04005DE8 RID: 24040
		public float combatPower;

		// Token: 0x04005DE9 RID: 24041
		public int minMechClusterPoints;

		// Token: 0x04005DEA RID: 24042
		public float destroyShakeAmount = -1f;

		// Token: 0x04005DEB RID: 24043
		public SoundDef destroySound;

		// Token: 0x04005DEC RID: 24044
		public SoundDef soundMeleeHitOverride;

		// Token: 0x04005DED RID: 24045
		public EffecterDef destroyEffecter;

		// Token: 0x04005DEE RID: 24046
		public BuildingSizeCategory buildingSizeCategory;

		// Token: 0x04005DEF RID: 24047
		public bool isFence;

		// Token: 0x04005DF0 RID: 24048
		public List<ThingDef> relatedBuildCommands;

		// Token: 0x04005DF1 RID: 24049
		public List<TerrainDef> relatedTerrain;

		// Token: 0x04005DF2 RID: 24050
		public bool alwaysShowRelatedBuildCommands;

		// Token: 0x04005DF3 RID: 24051
		public bool useIdeoColor;

		// Token: 0x04005DF4 RID: 24052
		public bool wakeDormantPawnsOnConstruction = true;

		// Token: 0x04005DF5 RID: 24053
		public int maxItemsInCell = 1;

		// Token: 0x04005DF6 RID: 24054
		public bool paintable;

		// Token: 0x04005DF7 RID: 24055
		public bool canBeDamagedByAttacks = true;

		// Token: 0x04005DF8 RID: 24056
		public bool isHopper;

		// Token: 0x04005DF9 RID: 24057
		public bool quickTargetable;

		// Token: 0x04005DFA RID: 24058
		public bool displayAttackToDestroyOnInspectPane;

		// Token: 0x04005DFB RID: 24059
		public bool isEscapableContainer;

		// Token: 0x04005DFC RID: 24060
		public bool isPowerConduit;

		// Token: 0x04005DFD RID: 24061
		public bool biomeSpecific;

		// Token: 0x04005DFE RID: 24062
		public bool crater;

		// Token: 0x04005DFF RID: 24063
		public bool canLandGravshipOn;

		// Token: 0x04005E00 RID: 24064
		public bool isAirtight;

		// Token: 0x04005E01 RID: 24065
		public bool isStuffableAirtight;

		// Token: 0x04005E02 RID: 24066
		public bool canConstructInCorner;

		// Token: 0x04005E03 RID: 24067
		public bool isAttachment;

		// Token: 0x04005E04 RID: 24068
		public bool supportsWallAttachments;

		// Token: 0x04005E05 RID: 24069
		public bool isPlayerEjectable;

		// Token: 0x04005E06 RID: 24070
		public SoundDef openingStartedSound;

		// Token: 0x04005E07 RID: 24071
		public GraphicData fullGraveGraphicData;

		// Token: 0x04005E08 RID: 24072
		public float bed_healPerDay;

		// Token: 0x04005E09 RID: 24073
		public bool bed_defaultMedical;

		// Token: 0x04005E0A RID: 24074
		public bool bed_showSleeperBody;

		// Token: 0x04005E0B RID: 24075
		public bool bed_humanlike = true;

		// Token: 0x04005E0C RID: 24076
		public float bed_maxBodySize = 9999f;

		// Token: 0x04005E0D RID: 24077
		public bool bed_caravansCanUse;

		// Token: 0x04005E0E RID: 24078
		public bool bed_slabBed;

		// Token: 0x04005E0F RID: 24079
		public bool bed_crib;

		// Token: 0x04005E10 RID: 24080
		public float bed_pawnDrawOffset;

		// Token: 0x04005E11 RID: 24081
		public bool bed_canBeMedical = true;

		// Token: 0x04005E12 RID: 24082
		public bool bed_DisplayOwnerType = true;

		// Token: 0x04005E13 RID: 24083
		public bool bed_DisplayOwnersInInspectString = true;

		// Token: 0x04005E14 RID: 24084
		public bool bed_countsForBedroomOrBarracks = true;

		// Token: 0x04005E15 RID: 24085
		public bool bed_emptyCountsForBarracks = true;

		// Token: 0x04005E16 RID: 24086
		public bool bed_UseSheetColor = true;

		// Token: 0x04005E17 RID: 24087
		public float nutritionCostPerDispense;

		// Token: 0x04005E18 RID: 24088
		public SoundDef soundDispense;

		// Token: 0x04005E19 RID: 24089
		public ThingDef turretGunDef;

		// Token: 0x04005E1A RID: 24090
		public FloatRange turretBurstWarmupTime = new FloatRange(0.25f, 0.75f);

		// Token: 0x04005E1B RID: 24091
		public float turretBurstCooldownTime = -1f;

		// Token: 0x04005E1C RID: 24092
		public float turretInitialCooldownTime;

		// Token: 0x04005E1D RID: 24093
		[Unsaved(false)]
		public Material turretTopMat;

		// Token: 0x04005E1E RID: 24094
		[Unsaved(false)]
		public Material turretTopLoadedMat;

		// Token: 0x04005E1F RID: 24095
		public float turretTopDrawSize = 2f;

		// Token: 0x04005E20 RID: 24096
		public Vector2 turretTopOffset;

		// Token: 0x04005E21 RID: 24097
		public bool playTargetAcquiredSound = true;

		// Token: 0x04005E22 RID: 24098
		public GraphicData turretTopLoadedGraphic;

		// Token: 0x04005E23 RID: 24099
		public List<MechWeightClassDef> requiredMechWeightClasses;

		// Token: 0x04005E24 RID: 24100
		public bool ai_combatDangerous;

		// Token: 0x04005E25 RID: 24101
		public bool ai_chillDestination = true;

		// Token: 0x04005E26 RID: 24102
		public bool ai_neverTrashThis;

		// Token: 0x04005E27 RID: 24103
		public bool preferConnectingToFences;

		// Token: 0x04005E28 RID: 24104
		public bool roamerCanOpen;

		// Token: 0x04005E29 RID: 24105
		public SoundDef soundDoorOpenPowered;

		// Token: 0x04005E2A RID: 24106
		public SoundDef soundDoorClosePowered;

		// Token: 0x04005E2B RID: 24107
		public SoundDef soundDoorOpenManual;

		// Token: 0x04005E2C RID: 24108
		public SoundDef soundDoorCloseManual;

		// Token: 0x04005E2D RID: 24109
		public SoundDef soundDoorCloseEnd;

		// Token: 0x04005E2E RID: 24110
		public float poweredDoorOpenSpeedFactor = 1f;

		// Token: 0x04005E2F RID: 24111
		public float poweredDoorCloseSpeedFactor = 1f;

		// Token: 0x04005E30 RID: 24112
		public float unpoweredDoorOpenSpeedFactor = 1f;

		// Token: 0x04005E31 RID: 24113
		public float unpoweredDoorCloseSpeedFactor = 1f;

		// Token: 0x04005E32 RID: 24114
		public int doorTempEqualizeIntervalClosed = 375;

		// Token: 0x04005E33 RID: 24115
		public int doorTempEqualizeIntervalOpen = 34;

		// Token: 0x04005E34 RID: 24116
		public float doorTempEqualizeRate = 1f;

		// Token: 0x04005E35 RID: 24117
		[NoTranslate]
		public string sowTag;

		// Token: 0x04005E36 RID: 24118
		public ThingDef defaultPlantToGrow;

		// Token: 0x04005E37 RID: 24119
		public ThingDef mineableThing;

		// Token: 0x04005E38 RID: 24120
		public int mineableYield = 1;

		// Token: 0x04005E39 RID: 24121
		public bool veinMineable;

		// Token: 0x04005E3A RID: 24122
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x04005E3B RID: 24123
		public float mineableDropChance = 1f;

		// Token: 0x04005E3C RID: 24124
		public bool mineableYieldWasteable = true;

		// Token: 0x04005E3D RID: 24125
		public float mineableScatterCommonality;

		// Token: 0x04005E3E RID: 24126
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x04005E3F RID: 24127
		public bool mineablePreventMeteorite;

		// Token: 0x04005E40 RID: 24128
		public bool mineablePreventNaturalRockOnSurface;

		// Token: 0x04005E41 RID: 24129
		public StorageSettings fixedStorageSettings;

		// Token: 0x04005E42 RID: 24130
		public StorageSettings defaultStorageSettings;

		// Token: 0x04005E43 RID: 24131
		public bool ignoreStoredThingsBeauty;

		// Token: 0x04005E44 RID: 24132
		public string storageGroupTag;

		// Token: 0x04005E45 RID: 24133
		public string groupingLabel;

		// Token: 0x04005E46 RID: 24134
		public int groupingOrder;

		// Token: 0x04005E47 RID: 24135
		public bool isTrap;

		// Token: 0x04005E48 RID: 24136
		public bool trapDestroyOnSpring;

		// Token: 0x04005E49 RID: 24137
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		// Token: 0x04005E4A RID: 24138
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x04005E4B RID: 24139
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x04005E4C RID: 24140
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		// Token: 0x04005E4D RID: 24141
		public RoomRoleDef workTableRoomRole;

		// Token: 0x04005E4E RID: 24142
		public float workTableNotInRoomRoleFactor = 1f;

		// Token: 0x04005E4F RID: 24143
		public float unpoweredWorkTableWorkSpeedFactor;

		// Token: 0x04005E50 RID: 24144
		public SoundDef workTableCompleteSoundDef;

		// Token: 0x04005E51 RID: 24145
		public IntRange watchBuildingStandDistanceRange = IntRange.One;

		// Token: 0x04005E52 RID: 24146
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x04005E53 RID: 24147
		public bool watchBuildingInSameRoom;

		// Token: 0x04005E54 RID: 24148
		public JoyKindDef joyKind;

		// Token: 0x04005E55 RID: 24149
		public int haulToContainerDuration;

		// Token: 0x04005E56 RID: 24150
		public float instrumentRange;

		// Token: 0x04005E57 RID: 24151
		public int minDistanceToSameTypeOfBuilding;

		// Token: 0x04005E58 RID: 24152
		public bool artificialForMeditationPurposes = true;

		// Token: 0x04005E59 RID: 24153
		public EffecterDef effectWatching;

		// Token: 0x04005E5A RID: 24154
		public GraphicData gibbetCageTopGraphicData;

		// Token: 0x04005E5B RID: 24155
		public Vector3 gibbetCorposeDrawOffset;

		// Token: 0x04005E5C RID: 24156
		public EffecterDef gibbetCagePlaceCorpseEffecter;

		// Token: 0x04005E5D RID: 24157
		public EffecterDef openingEffect;

		// Token: 0x04005E5E RID: 24158
		public FillableBarRequestWithRotation barDrawData;

		// Token: 0x04005E5F RID: 24159
		public MoteForRotationData gestatorFormingMote;

		// Token: 0x04005E60 RID: 24160
		public MoteForRotationData gestatorCycleCompleteMote;

		// Token: 0x04005E61 RID: 24161
		public MoteForRotationData gestatorFormedMote;

		// Token: 0x04005E62 RID: 24162
		public GraphicData mechGestatorCylinderGraphic;

		// Token: 0x04005E63 RID: 24163
		public GraphicData mechGestatorTopGraphic;

		// Token: 0x04005E64 RID: 24164
		public GraphicData formingGraphicData;

		// Token: 0x04005E65 RID: 24165
		public float formingMechBobSpeed = 0.001f;

		// Token: 0x04005E66 RID: 24166
		public float formingMechYBobDistance = 0.1f;

		// Token: 0x04005E67 RID: 24167
		public List<Vector3> formingMechPerRotationOffset = new List<Vector3>();

		// Token: 0x04005E68 RID: 24168
		public Vector2 maxFormedMechDrawSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04005E69 RID: 24169
		public List<IngredientCount> subcoreScannerFixedIngredients;

		// Token: 0x04005E6A RID: 24170
		public int subcoreScannerTicks = 7500;

		// Token: 0x04005E6B RID: 24171
		public HediffDef subcoreScannerHediff;

		// Token: 0x04005E6C RID: 24172
		public bool destroyBrain;

		// Token: 0x04005E6D RID: 24173
		public ThingDef subcoreScannerOutputDef;

		// Token: 0x04005E6E RID: 24174
		public EffecterDef subcoreScannerStartEffect;

		// Token: 0x04005E6F RID: 24175
		public SoundDef subcoreScannerWorking;

		// Token: 0x04005E70 RID: 24176
		public SoundDef subcoreScannerComplete;

		// Token: 0x04005E71 RID: 24177
		public bool isInsectCocoon;

		// Token: 0x04005E72 RID: 24178
		public GraphicData wastepackAtomizerBottomGraphic;

		// Token: 0x04005E73 RID: 24179
		public GraphicData wastepackAtomizerWindowGraphic;

		// Token: 0x04005E74 RID: 24180
		public EffecterDef wastepackAtomizerOperationEffecter;

		// Token: 0x04005E75 RID: 24181
		public GraphicData bookendGraphicEast;

		// Token: 0x04005E76 RID: 24182
		public GraphicData bookendGraphicNorth;

		// Token: 0x04005E77 RID: 24183
		public bool isSupportDoor;

		// Token: 0x04005E78 RID: 24184
		public GraphicData doorTopGraphic;

		// Token: 0x04005E79 RID: 24185
		public GraphicData doorSupportGraphic;

		// Token: 0x04005E7A RID: 24186
		public GraphicData upperMoverGraphic;

		// Token: 0x04005E7B RID: 24187
		public Vector3 doorTopGraphicOffset;

		// Token: 0x04005E7C RID: 24188
		public Vector3 doorSupportGraphicOffset;

		// Token: 0x04005E7D RID: 24189
		public Vector3 doorTopHorizontalOffset = new Vector3(0f, 0f, 0.1f);

		// Token: 0x04005E7E RID: 24190
		public Vector3 doorTopVerticalOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04005E7F RID: 24191
		public ThingDef groundSpawnerThingToSpawn;

		// Token: 0x04005E80 RID: 24192
		public IntRange groundSpawnerSpawnDelay;

		// Token: 0x04005E81 RID: 24193
		public bool groundSpawnerDestroyAdjacent;

		// Token: 0x04005E82 RID: 24194
		public SoundDef groundSpawnerSustainerSound;

		// Token: 0x04005E83 RID: 24195
		public EffecterDef groundSpawnerSustainedEffecter;

		// Token: 0x04005E84 RID: 24196
		public EffecterDef groundSpawnerCompleteEffecter;

		// Token: 0x04005E85 RID: 24197
		public string groundSpawnerLetterLabel;

		// Token: 0x04005E86 RID: 24198
		public string groundSpawnerLetterText;

		// Token: 0x04005E87 RID: 24199
		private static List<string> tmpFenceBlockedAnimals = new List<string>();
	}
}
