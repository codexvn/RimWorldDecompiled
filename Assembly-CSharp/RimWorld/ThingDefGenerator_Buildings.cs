using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001912 RID: 6418
	public static class ThingDefGenerator_Buildings
	{
		// Token: 0x060091D9 RID: 37337 RVA: 0x002AF177 File Offset: 0x002AD377
		public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs(bool hotReload = false)
		{
			foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>())
			{
				ThingDef blueprint = null;
				if (def.BuildableByPlayer)
				{
					blueprint = ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, false, null, hotReload);
					yield return blueprint;
					yield return ThingDefGenerator_Buildings.NewFrameDef_Thing(def, hotReload);
				}
				if (def.Minifiable)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, true, blueprint, hotReload);
				}
				blueprint = null;
				def = null;
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			foreach (TerrainDef terrDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (terrDef.BuildableByPlayer)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Terrain(terrDef, hotReload);
					yield return ThingDefGenerator_Buildings.NewFrameDef_Terrain(terrDef, hotReload);
					terrDef = null;
				}
			}
			IEnumerator<TerrainDef> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x060091DA RID: 37338 RVA: 0x002AF188 File Offset: 0x002AD388
		private static ThingDef BaseBlueprintDef()
		{
			return new ThingDef
			{
				category = ThingCategory.Ethereal,
				label = "Unspecified blueprint",
				altitudeLayer = AltitudeLayer.Blueprint,
				useHitPoints = false,
				selectable = true,
				seeThroughFog = true,
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				drawerType = DrawerType.MapMeshOnly
			};
		}

		// Token: 0x060091DB RID: 37339 RVA: 0x002AF1F4 File Offset: 0x002AD3F4
		private static ThingDef BaseFrameDef()
		{
			return new ThingDef
			{
				isFrameInt = true,
				category = ThingCategory.Building,
				label = "Unspecified building frame",
				thingClass = typeof(Frame),
				altitudeLayer = AltitudeLayer.BuildingOnTop,
				useHitPoints = true,
				selectable = true,
				drawerType = DrawerType.RealtimeOnly,
				building = new BuildingProperties(),
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				scatterableOnMapGen = false,
				leaveResourcesWhenKilled = true
			};
		}

		// Token: 0x060091DC RID: 37340 RVA: 0x002AF288 File Offset: 0x002AD488
		private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + def.defName;
			if (isInstallBlueprint)
			{
				defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + ThingDefGenerator_Buildings.InstallBlueprintDefNamePrefix + def.defName;
			}
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseBlueprintDef()) : ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.defName = defName;
			thingDef.label = def.label + "BlueprintLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			thingDef.rotatable = def.rotatable;
			thingDef.replaceTags = def.replaceTags;
			if (!isInstallBlueprint)
			{
				thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
				thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			}
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (isInstallBlueprint && normalBlueprint != null)
			{
				thingDef.graphicData = normalBlueprint.graphicData;
			}
			else
			{
				thingDef.graphicData = new GraphicData();
				if (def.building != null && def.building.blueprintGraphicData != null)
				{
					thingDef.graphicData.CopyFrom(def.building.blueprintGraphicData);
					if (thingDef.graphicData.graphicClass == null)
					{
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					}
					if (thingDef.graphicData.shaderType == null)
					{
						thingDef.graphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (def.graphicData != null)
					{
						if (def.building.blueprintGraphicData.drawSize == Vector2.one)
						{
							thingDef.graphicData.drawSize = def.graphicData.drawSize;
						}
						thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
						thingDef.graphicData.linkType = def.graphicData.linkType;
						thingDef.graphicData.asymmetricLink = def.graphicData.asymmetricLink;
					}
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
				}
				else
				{
					thingDef.graphicData.CopyFrom(def.graphicData);
					thingDef.graphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					thingDef.graphicData.colorTwo = Color.white;
					thingDef.graphicData.shadowData = null;
				}
			}
			thingDef.graphicData.renderQueue = 2950;
			thingDef.defaultPlacingRot = def.defaultPlacingRot;
			if (thingDef.graphicData.shadowData != null)
			{
				Log.Error("Blueprint has shadow: " + ((def != null) ? def.ToString() : null));
			}
			if (isInstallBlueprint)
			{
				thingDef.thingClass = typeof(Blueprint_Install);
			}
			else if (def.building != null)
			{
				thingDef.thingClass = def.building.blueprintClass;
			}
			else
			{
				Log.Error("Tried creating build blueprint for thing that has no blueprint class assigned!");
			}
			if (def.thingClass == typeof(Building_Door))
			{
				thingDef.drawerType = DrawerType.RealtimeOnly;
			}
			else
			{
				thingDef.drawerType = def.drawerType;
			}
			thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
			thingDef.entityDefToBuild = def;
			if (isInstallBlueprint)
			{
				def.installBlueprintDef = thingDef;
			}
			else
			{
				def.blueprintDef = thingDef;
			}
			return thingDef;
		}

		// Token: 0x060091DD RID: 37341 RVA: 0x002AF5C4 File Offset: 0x002AD7C4
		private static ThingDef NewFrameDef_Thing(ThingDef def, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + def.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseFrameDef()) : ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = defName;
			thingDef.label = def.label + "FrameLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)def.BaseMaxHitPoints * 0.25f);
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
			thingDef.SetStatBaseValue(StatDefOf.Flammability, def.BaseFlammability);
			thingDef.fillPercent = 0.2f;
			thingDef.pathCost = 14;
			thingDef.description = def.description;
			thingDef.passability = def.passability;
			thingDef.altitudeLayer = def.altitudeLayer;
			if (thingDef.passability > Traversability.PassThroughOnly)
			{
				thingDef.passability = Traversability.PassThroughOnly;
			}
			thingDef.selectable = def.selectable;
			thingDef.constructEffect = def.constructEffect;
			thingDef.building.isEdifice = def.building.isEdifice;
			thingDef.building.watchBuildingInSameRoom = def.building.watchBuildingInSameRoom;
			thingDef.building.watchBuildingStandDistanceRange = def.building.watchBuildingStandDistanceRange;
			thingDef.building.watchBuildingStandRectWidth = def.building.watchBuildingStandRectWidth;
			thingDef.building.artificialForMeditationPurposes = def.building.artificialForMeditationPurposes;
			thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = def.modContentPack;
			thingDef.blocksAltitudes = def.blocksAltitudes;
			thingDef.replaceTags = def.replaceTags;
			thingDef.rotatable = def.rotatable;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (def.BuildableByPlayer)
			{
				thingDef.stuffCategories = def.stuffCategories;
				thingDef.costListForDifficulty = def.costListForDifficulty;
			}
			thingDef.entityDefToBuild = def;
			def.frameDef = thingDef;
			return thingDef;
		}

		// Token: 0x060091DE RID: 37342 RVA: 0x002AF7EC File Offset: 0x002AD9EC
		private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + terrDef.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseBlueprintDef()) : ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.thingClass = typeof(Blueprint_Build);
			thingDef.defName = defName;
			thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.graphicData = new GraphicData();
			thingDef.graphicData.shaderType = ShaderTypeDefOf.MetaOverlay;
			thingDef.graphicData.texPath = ThingDefGenerator_Buildings.TerrainBlueprintGraphicPath;
			thingDef.graphicData.renderQueue = 2950;
			thingDef.graphicData.graphicClass = typeof(Graphic_Single);
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.entityDefToBuild = terrDef;
			terrDef.blueprintDef = thingDef;
			return thingDef;
		}

		// Token: 0x060091DF RID: 37343 RVA: 0x002AF8FC File Offset: 0x002ADAFC
		private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef, bool hotReload = false)
		{
			string defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + terrDef.defName;
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? ThingDefGenerator_Buildings.BaseFrameDef()) : ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.building.artificialForMeditationPurposes = false;
			thingDef.defName = defName;
			thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.useHitPoints = false;
			thingDef.fillPercent = 0f;
			thingDef.description = "Terrain building in progress.";
			thingDef.passability = Traversability.Standable;
			thingDef.selectable = true;
			thingDef.constructEffect = terrDef.constructEffect;
			thingDef.building.isEdifice = false;
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.category = ThingCategory.Ethereal;
			thingDef.entityDefToBuild = terrDef;
			terrDef.frameDef = thingDef;
			if (!thingDef.IsFrame)
			{
				string str = "Framedef is not frame: ";
				ThingDef thingDef2 = thingDef;
				Log.Error(str + ((thingDef2 != null) ? thingDef2.ToString() : null));
			}
			return thingDef;
		}

		// Token: 0x04005CF8 RID: 23800
		public static readonly string BlueprintDefNamePrefix = "Blueprint_";

		// Token: 0x04005CF9 RID: 23801
		public static readonly string InstallBlueprintDefNamePrefix = "Install_";

		// Token: 0x04005CFA RID: 23802
		public static readonly string BuildingFrameDefNamePrefix = "Frame_";

		// Token: 0x04005CFB RID: 23803
		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		// Token: 0x04005CFC RID: 23804
		public static readonly Color BlueprintColor = new Color(0.8235294f, 0.92156863f, 1f, 0.6f);
	}
}
