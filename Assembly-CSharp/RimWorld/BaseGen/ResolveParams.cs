using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen;

public struct ResolveParams
{
	public CellRect rect;

	public Faction faction;

	public float? threatPoints;

	private Dictionary<string, object> custom;

	public PawnGroupMakerParms pawnGroupMakerParams;

	public PawnGroupKindDef pawnGroupKindDef;

	public RoofDef roofDef;

	public bool? noRoof;

	public bool? addRoomCenterToRootsToUnfog;

	public Thing singleThingToSpawn;

	public ThingDef singleThingDef;

	public ThingDef singleThingStuff;

	public int? singleThingStackCount;

	public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

	public bool? spawnBridgeIfTerrainCantSupportThing;

	public bool? spawnOutside;

	public List<Thing> singleThingInnerThings;

	public Pawn singlePawnToSpawn;

	public PawnKindDef singlePawnKindDef;

	public bool? disableSinglePawn;

	public Lord singlePawnLord;

	public Lord settlementLord;

	public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

	public PawnGenerationRequest? singlePawnGenerationRequest;

	public Action<Thing> postThingSpawn;

	public Action<Thing> postThingGenerate;

	public int? mechanoidsCount;

	public bool? wakeSleepingMechanoidsOnUnfogged;

	public int? fleshbeastsCount;

	public int? hivesCount;

	public bool? disableHives;

	public Rot4? thingRot;

	public ThingDef wallStuff;

	public float? chanceToSkipWallBlock;

	public ThingDef wallThingDef;

	public TerrainDef floorDef;

	public float? chanceToSkipFloor;

	public ThingDef filthDef;

	public FloatRange? filthDensity;

	public bool? floorOnlyIfTerrainSupports;

	public bool? allowBridgeOnAnyImpassableTerrain;

	public bool? clearEdificeOnly;

	public bool? clearFillageOnly;

	public bool? clearRoof;

	public int? ancientCryptosleepCasketGroupID;

	public PodContentsType? podContentsType;

	public string ancientCryptosleepCasketOpenSignalTag;

	public ThingSetMakerDef thingSetMakerDef;

	public ThingSetMakerParams? thingSetMakerParams;

	public IList<Thing> stockpileConcreteContents;

	public float? stockpileMarketValue;

	public int? innerStockpileSize;

	public int? edgeDefenseWidth;

	public int? edgeDefenseTurretsCount;

	public int? edgeDefenseMortarsCount;

	public int? edgeDefenseGuardsCount;

	public ThingDef mortarDef;

	public TerrainDef pathwayFloorDef;

	public ThingDef cultivatedPlantDef;

	public float? fixedCulativedPlantGrowth;

	public int? fillWithThingsPadding;

	public float? settlementPawnGroupPoints;

	public int? settlementPawnGroupSeed;

	public bool? settlementDontGeneratePawns;

	public bool? attackWhenPlayerBecameEnemy;

	public bool? streetHorizontal;

	public bool? edgeThingAvoidOtherEdgeThings;

	public bool? edgeThingMustReachMapEdge;

	public bool? allowPlacementOffEdge;

	public Rot4? thrustAxis;

	public FloatRange? hpPercentRange;

	public Thing conditionCauser;

	public bool? makeWarningLetter;

	public ThingFilter allowedMonumentThings;

	public bool? allowGeneratingThronerooms;

	public int? bedCount;

	public float workSitePoints;

	public IList<Thing> lootConcereteContents;

	public float? lootMarketValue;

	public Rot4? extraDoorEdge;

	public string sleepingInsectsWakeupSignalTag;

	public string sleepingMechanoidsWakeupSignalTag;

	public string rectTriggerSignalTag;

	public bool? destroyIfUnfogged;

	public string messageSignalTag;

	public string message;

	public MessageTypeDef messageType;

	public LookTargets lookTargets;

	public bool? spawnAnywhereIfNoGoodCell;

	public bool? ignoreRoofedRequirement;

	public IntVec3? overrideLoc;

	public bool? sendStandardLetter;

	public string infestationSignalTag;

	public float? insectsPoints;

	public string ambushSignalTag;

	public SignalActionAmbushType? ambushType;

	public IntVec3? spawnNear;

	public CellRect? spawnAround;

	public bool? spawnPawnsOnEdge;

	public float? ambushPoints;

	public int? cornerRadius;

	public SoundDef sound;

	public string soundOneShotActionSignalTag;

	public string unfoggedSignalTag;

	public Thing triggerContainerEmptiedThing;

	public string triggerContainerEmptiedSignalTag;

	public Building_Door openDoorActionDoor;

	public string openDoorActionSignalTag;

	public string triggerSecuritySignal;

	public Thing relicThing;

	public float? exteriorThreatPoints;

	public float? interiorThreatPoints;

	public SitePart sitePart;

	public int? minLengthAfterSplit;

	public bool? sendWokenUpMessage;

	public LayoutStructureSketch ancientLayoutStructureSketch;

	public bool? ignoreDoorways;

	public int? minorBuildingCount;

	public int? minorBuildingRadialDistance;

	public ThingDef centralBuilding;

	public PawnKindDef desiccatedCorpsePawnKind;

	public IntRange? desiccatedCorpseRandomAgeRange;

	public FloatRange? dessicatedCorpseDensityRange;

	public float edgeUnpolluteChance;

	public void SetCustom<T>(string name, T obj, bool inherit = false)
	{
		ResolveParamsUtility.SetCustom(ref custom, name, obj, inherit);
	}

	public void RemoveCustom(string name)
	{
		ResolveParamsUtility.RemoveCustom(ref custom, name);
	}

	public bool TryGetCustom<T>(string name, out T obj)
	{
		return ResolveParamsUtility.TryGetCustom<T>(custom, name, out obj);
	}

	public T GetCustom<T>(string name)
	{
		return ResolveParamsUtility.GetCustom<T>(custom, name);
	}

	public override string ToString()
	{
		string[] array = new string[144];
		array[0] = "rect=";
		CellRect cellRect = rect;
		array[1] = cellRect.ToString();
		array[2] = ", faction=";
		array[3] = ((faction != null) ? faction.ToString() : "null");
		array[4] = ", custom=";
		array[5] = ((custom != null) ? custom.Count.ToString() : "null");
		array[6] = ", combatPoints=";
		array[7] = (threatPoints.HasValue ? threatPoints.ToString() : "null");
		array[8] = ", pawnGroupMakerParams=";
		array[9] = ((pawnGroupMakerParams != null) ? pawnGroupMakerParams.ToString() : "null");
		array[10] = ", pawnGroupKindDef=";
		array[11] = ((pawnGroupKindDef != null) ? pawnGroupKindDef.ToString() : "null");
		array[12] = ", roofDef=";
		array[13] = ((roofDef != null) ? roofDef.ToString() : "null");
		array[14] = ", noRoof=";
		array[15] = (noRoof.HasValue ? noRoof.ToString() : "null");
		array[16] = ", addRoomCenterToRootsToUnfog=";
		array[17] = (addRoomCenterToRootsToUnfog.HasValue ? addRoomCenterToRootsToUnfog.ToString() : "null");
		array[18] = ", singleThingToSpawn=";
		array[19] = ((singleThingToSpawn != null) ? singleThingToSpawn.ToString() : "null");
		array[20] = ", singleThingDef=";
		array[21] = ((singleThingDef != null) ? singleThingDef.ToString() : "null");
		array[22] = ", singleThingStuff=";
		array[23] = ((singleThingStuff != null) ? singleThingStuff.ToString() : "null");
		array[24] = ", singleThingStackCount=";
		array[25] = (singleThingStackCount.HasValue ? singleThingStackCount.ToString() : "null");
		array[26] = ", skipSingleThingIfHasToWipeBuildingOrDoesntFit=";
		array[27] = (skipSingleThingIfHasToWipeBuildingOrDoesntFit.HasValue ? skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString() : "null");
		array[28] = ", spawnBridgeIfTerrainCantSupportThing=";
		array[29] = (spawnBridgeIfTerrainCantSupportThing.HasValue ? spawnBridgeIfTerrainCantSupportThing.ToString() : "null");
		array[30] = ", singleThingInnerThings=";
		array[31] = ((singleThingInnerThings != null) ? singleThingInnerThings.ToString() : "null");
		array[32] = ", singlePawnToSpawn=";
		array[33] = ((singlePawnToSpawn != null) ? singlePawnToSpawn.ToString() : "null");
		array[34] = ", singlePawnKindDef=";
		array[35] = ((singlePawnKindDef != null) ? singlePawnKindDef.ToString() : "null");
		array[36] = ", disableSinglePawn=";
		array[37] = (disableSinglePawn.HasValue ? disableSinglePawn.ToString() : "null");
		array[38] = ", singlePawnLord=";
		array[39] = ((singlePawnLord != null) ? singlePawnLord.ToString() : "null");
		array[40] = ", settlementLord=";
		array[41] = ((settlementLord != null) ? settlementLord.ToString() : "null");
		array[42] = ", singlePawnSpawnCellExtraPredicate=";
		array[43] = ((singlePawnSpawnCellExtraPredicate != null) ? singlePawnSpawnCellExtraPredicate.ToString() : "null");
		array[44] = ", singlePawnGenerationRequest=";
		array[45] = (singlePawnGenerationRequest.HasValue ? singlePawnGenerationRequest.ToString() : "null");
		array[46] = ", postThingSpawn=";
		array[47] = ((postThingSpawn != null) ? postThingSpawn.ToString() : "null");
		array[48] = ", postThingGenerate=";
		array[49] = ((postThingGenerate != null) ? postThingGenerate.ToString() : "null");
		array[50] = ", mechanoidsCount=";
		array[51] = (mechanoidsCount.HasValue ? mechanoidsCount.ToString() : "null");
		array[52] = ", wakeSleepingMechanoidsOnUnfogged=";
		array[53] = (wakeSleepingMechanoidsOnUnfogged.HasValue ? wakeSleepingMechanoidsOnUnfogged.ToString() : "null");
		array[54] = ", hivesCount=";
		array[55] = (hivesCount.HasValue ? hivesCount.ToString() : "null");
		array[56] = ", disableHives=";
		array[57] = (disableHives.HasValue ? disableHives.ToString() : "null");
		array[58] = ", thingRot=";
		array[59] = (thingRot.HasValue ? thingRot.ToString() : "null");
		array[60] = ", wallStuff=";
		array[61] = ((wallStuff != null) ? wallStuff.ToString() : "null");
		array[62] = ", chanceToSkipWallBlock=";
		array[63] = (chanceToSkipWallBlock.HasValue ? chanceToSkipWallBlock.ToString() : "null");
		array[64] = ", floorDef=";
		array[65] = ((floorDef != null) ? floorDef.ToString() : "null");
		array[66] = ", chanceToSkipFloor=";
		array[67] = (chanceToSkipFloor.HasValue ? chanceToSkipFloor.ToString() : "null");
		array[68] = ", filthDef=";
		array[69] = ((filthDef != null) ? filthDef.ToString() : "null");
		array[70] = ", filthDensity=";
		array[71] = (filthDensity.HasValue ? filthDensity.ToString() : "null");
		array[72] = ", floorOnlyIfTerrainSupports=";
		array[73] = (floorOnlyIfTerrainSupports.HasValue ? floorOnlyIfTerrainSupports.ToString() : "null");
		array[74] = ", allowBridgeOnAnyImpassableTerrain=";
		array[75] = (allowBridgeOnAnyImpassableTerrain.HasValue ? allowBridgeOnAnyImpassableTerrain.ToString() : "null");
		array[76] = ", clearEdificeOnly=";
		array[77] = (clearEdificeOnly.HasValue ? clearEdificeOnly.ToString() : "null");
		array[78] = ", clearFillageOnly=";
		array[79] = (clearFillageOnly.HasValue ? clearFillageOnly.ToString() : "null");
		array[80] = ", clearRoof=";
		array[81] = (clearRoof.HasValue ? clearRoof.ToString() : "null");
		array[82] = ", ancientCryptosleepCasketGroupID=";
		array[83] = (ancientCryptosleepCasketGroupID.HasValue ? ancientCryptosleepCasketGroupID.ToString() : "null");
		array[84] = ", podContentsType=";
		array[85] = (podContentsType.HasValue ? podContentsType.ToString() : "null");
		array[86] = ", ancientCryptosleepCasketOpenSignalTag=";
		array[87] = ((ancientCryptosleepCasketOpenSignalTag != null) ? ancientCryptosleepCasketOpenSignalTag.ToString() : "null");
		array[88] = ", thingSetMakerDef=";
		array[89] = ((thingSetMakerDef != null) ? thingSetMakerDef.ToString() : "null");
		array[90] = ", thingSetMakerParams=";
		array[91] = (thingSetMakerParams.HasValue ? thingSetMakerParams.ToString() : "null");
		array[92] = ", stockpileConcreteContents=";
		array[93] = ((stockpileConcreteContents != null) ? stockpileConcreteContents.Count.ToString() : "null");
		array[94] = ", stockpileMarketValue=";
		array[95] = (stockpileMarketValue.HasValue ? stockpileMarketValue.ToString() : "null");
		array[96] = ", innerStockpileSize=";
		array[97] = (innerStockpileSize.HasValue ? innerStockpileSize.ToString() : "null");
		array[98] = ", edgeDefenseWidth=";
		array[99] = (edgeDefenseWidth.HasValue ? edgeDefenseWidth.ToString() : "null");
		array[100] = ", edgeDefenseTurretsCount=";
		array[101] = (edgeDefenseTurretsCount.HasValue ? edgeDefenseTurretsCount.ToString() : "null");
		array[102] = ", edgeDefenseMortarsCount=";
		array[103] = (edgeDefenseMortarsCount.HasValue ? edgeDefenseMortarsCount.ToString() : "null");
		array[104] = ", edgeDefenseGuardsCount=";
		array[105] = (edgeDefenseGuardsCount.HasValue ? edgeDefenseGuardsCount.ToString() : "null");
		array[106] = ", mortarDef=";
		array[107] = ((mortarDef != null) ? mortarDef.ToString() : "null");
		array[108] = ", pathwayFloorDef=";
		array[109] = ((pathwayFloorDef != null) ? pathwayFloorDef.ToString() : "null");
		array[110] = ", cultivatedPlantDef=";
		array[111] = ((cultivatedPlantDef != null) ? cultivatedPlantDef.ToString() : "null");
		array[112] = ", fixedCulativedPlantGrowth=";
		array[113] = (fixedCulativedPlantGrowth.HasValue ? fixedCulativedPlantGrowth.ToString() : "null");
		array[114] = ", fillWithThingsPadding=";
		array[115] = (fillWithThingsPadding.HasValue ? fillWithThingsPadding.ToString() : "null");
		array[116] = ", settlementPawnGroupPoints=";
		array[117] = (settlementPawnGroupPoints.HasValue ? settlementPawnGroupPoints.ToString() : "null");
		array[118] = ", settlementPawnGroupSeed=";
		array[119] = (settlementPawnGroupSeed.HasValue ? settlementPawnGroupSeed.ToString() : "null");
		array[120] = ", settlementDontGeneratePawns=";
		array[121] = (settlementDontGeneratePawns.HasValue ? settlementDontGeneratePawns.ToString() : "null");
		array[122] = ", attackWhenPlayerBecameEnemy=";
		array[123] = (attackWhenPlayerBecameEnemy.HasValue ? attackWhenPlayerBecameEnemy.ToString() : "null");
		array[124] = ", streetHorizontal=";
		array[125] = (streetHorizontal.HasValue ? streetHorizontal.ToString() : "null");
		array[126] = ", edgeThingAvoidOtherEdgeThings=";
		array[127] = (edgeThingAvoidOtherEdgeThings.HasValue ? edgeThingAvoidOtherEdgeThings.ToString() : "null");
		array[128] = ", edgeThingMustReachMapEdge=";
		array[129] = (edgeThingMustReachMapEdge.HasValue ? edgeThingMustReachMapEdge.ToString() : "null");
		array[130] = ", allowPlacementOffEdge=";
		array[131] = (allowPlacementOffEdge.HasValue ? allowPlacementOffEdge.ToString() : "null");
		array[132] = ", thrustAxis=";
		array[133] = (thrustAxis.HasValue ? thrustAxis.ToString() : "null");
		array[134] = ", makeWarningLetter=";
		array[135] = (makeWarningLetter.HasValue ? makeWarningLetter.ToString() : "null");
		array[136] = ", allowedMonumentThings=";
		array[137] = ((allowedMonumentThings != null) ? allowedMonumentThings.ToString() : "null");
		array[138] = ", bedCount=";
		array[139] = (bedCount.HasValue ? bedCount.ToString() : ("null, workSitePoints=" + ((workSitePoints != 0f) ? workSitePoints.ToString() : "null") + ", lootConcereteContents=" + ((lootConcereteContents != null) ? lootConcereteContents.ToString() : "null") + ", lootMarketValue=" + (lootMarketValue.HasValue ? lootMarketValue.ToString() : "null")));
		array[140] = ", extraDoorEdge=";
		array[141] = (extraDoorEdge.HasValue ? extraDoorEdge.ToString() : "null");
		array[142] = ", minLengthAfterSplit=";
		array[143] = (minLengthAfterSplit.HasValue ? minLengthAfterSplit.ToString() : "null");
		return string.Concat(array);
	}
}
