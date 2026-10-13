using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020038E7 RID: 14567
	public struct ResolveParams
	{
		// Token: 0x060141A4 RID: 82340 RVA: 0x005FB51F File Offset: 0x005F971F
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x060141A5 RID: 82341 RVA: 0x005FB52F File Offset: 0x005F972F
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x060141A6 RID: 82342 RVA: 0x005FB53D File Offset: 0x005F973D
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x060141A7 RID: 82343 RVA: 0x005FB54C File Offset: 0x005F974C
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x060141A8 RID: 82344 RVA: 0x005FB55C File Offset: 0x005F975C
		public override string ToString()
		{
			string[] array = new string[142];
			array[0] = "rect=";
			int num = 1;
			CellRect cellRect = this.rect;
			array[num] = cellRect.ToString();
			array[2] = ", faction=";
			array[3] = ((this.faction != null) ? this.faction.ToString() : "null");
			array[4] = ", custom=";
			array[5] = ((this.custom != null) ? this.custom.Count.ToString() : "null");
			array[6] = ", combatPoints=";
			array[7] = ((this.threatPoints != null) ? this.threatPoints.ToString() : "null");
			array[8] = ", pawnGroupMakerParams=";
			array[9] = ((this.pawnGroupMakerParams != null) ? this.pawnGroupMakerParams.ToString() : "null");
			array[10] = ", pawnGroupKindDef=";
			array[11] = ((this.pawnGroupKindDef != null) ? this.pawnGroupKindDef.ToString() : "null");
			array[12] = ", roofDef=";
			array[13] = ((this.roofDef != null) ? this.roofDef.ToString() : "null");
			array[14] = ", noRoof=";
			array[15] = ((this.noRoof != null) ? this.noRoof.ToString() : "null");
			array[16] = ", addRoomCenterToRootsToUnfog=";
			array[17] = ((this.addRoomCenterToRootsToUnfog != null) ? this.addRoomCenterToRootsToUnfog.ToString() : "null");
			array[18] = ", singleThingToSpawn=";
			array[19] = ((this.singleThingToSpawn != null) ? this.singleThingToSpawn.ToString() : "null");
			array[20] = ", singleThingDef=";
			array[21] = ((this.singleThingDef != null) ? this.singleThingDef.ToString() : "null");
			array[22] = ", singleThingStuff=";
			array[23] = ((this.singleThingStuff != null) ? this.singleThingStuff.ToString() : "null");
			array[24] = ", singleThingStackCount=";
			array[25] = ((this.singleThingStackCount != null) ? this.singleThingStackCount.ToString() : "null");
			array[26] = ", skipSingleThingIfHasToWipeBuildingOrDoesntFit=";
			array[27] = ((this.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null) ? this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString() : "null");
			array[28] = ", spawnBridgeIfTerrainCantSupportThing=";
			array[29] = ((this.spawnBridgeIfTerrainCantSupportThing != null) ? this.spawnBridgeIfTerrainCantSupportThing.ToString() : "null");
			array[30] = ", singleThingInnerThings=";
			array[31] = ((this.singleThingInnerThings != null) ? this.singleThingInnerThings.ToString() : "null");
			array[32] = ", singlePawnToSpawn=";
			array[33] = ((this.singlePawnToSpawn != null) ? this.singlePawnToSpawn.ToString() : "null");
			array[34] = ", singlePawnKindDef=";
			array[35] = ((this.singlePawnKindDef != null) ? this.singlePawnKindDef.ToString() : "null");
			array[36] = ", disableSinglePawn=";
			array[37] = ((this.disableSinglePawn != null) ? this.disableSinglePawn.ToString() : "null");
			array[38] = ", singlePawnLord=";
			array[39] = ((this.singlePawnLord != null) ? this.singlePawnLord.ToString() : "null");
			array[40] = ", settlementLord=";
			array[41] = ((this.settlementLord != null) ? this.settlementLord.ToString() : "null");
			array[42] = ", singlePawnSpawnCellExtraPredicate=";
			array[43] = ((this.singlePawnSpawnCellExtraPredicate != null) ? this.singlePawnSpawnCellExtraPredicate.ToString() : "null");
			array[44] = ", singlePawnGenerationRequest=";
			array[45] = ((this.singlePawnGenerationRequest != null) ? this.singlePawnGenerationRequest.ToString() : "null");
			array[46] = ", postThingSpawn=";
			array[47] = ((this.postThingSpawn != null) ? this.postThingSpawn.ToString() : "null");
			array[48] = ", postThingGenerate=";
			array[49] = ((this.postThingGenerate != null) ? this.postThingGenerate.ToString() : "null");
			array[50] = ", mechanoidsCount=";
			array[51] = ((this.mechanoidsCount != null) ? this.mechanoidsCount.ToString() : "null");
			array[52] = ", hivesCount=";
			array[53] = ((this.hivesCount != null) ? this.hivesCount.ToString() : "null");
			array[54] = ", disableHives=";
			array[55] = ((this.disableHives != null) ? this.disableHives.ToString() : "null");
			array[56] = ", thingRot=";
			array[57] = ((this.thingRot != null) ? this.thingRot.ToString() : "null");
			array[58] = ", wallStuff=";
			array[59] = ((this.wallStuff != null) ? this.wallStuff.ToString() : "null");
			array[60] = ", chanceToSkipWallBlock=";
			array[61] = ((this.chanceToSkipWallBlock != null) ? this.chanceToSkipWallBlock.ToString() : "null");
			array[62] = ", floorDef=";
			array[63] = ((this.floorDef != null) ? this.floorDef.ToString() : "null");
			array[64] = ", chanceToSkipFloor=";
			array[65] = ((this.chanceToSkipFloor != null) ? this.chanceToSkipFloor.ToString() : "null");
			array[66] = ", filthDef=";
			array[67] = ((this.filthDef != null) ? this.filthDef.ToString() : "null");
			array[68] = ", filthDensity=";
			array[69] = ((this.filthDensity != null) ? this.filthDensity.ToString() : "null");
			array[70] = ", floorOnlyIfTerrainSupports=";
			array[71] = ((this.floorOnlyIfTerrainSupports != null) ? this.floorOnlyIfTerrainSupports.ToString() : "null");
			array[72] = ", allowBridgeOnAnyImpassableTerrain=";
			array[73] = ((this.allowBridgeOnAnyImpassableTerrain != null) ? this.allowBridgeOnAnyImpassableTerrain.ToString() : "null");
			array[74] = ", clearEdificeOnly=";
			array[75] = ((this.clearEdificeOnly != null) ? this.clearEdificeOnly.ToString() : "null");
			array[76] = ", clearFillageOnly=";
			array[77] = ((this.clearFillageOnly != null) ? this.clearFillageOnly.ToString() : "null");
			array[78] = ", clearRoof=";
			array[79] = ((this.clearRoof != null) ? this.clearRoof.ToString() : "null");
			array[80] = ", ancientCryptosleepCasketGroupID=";
			array[81] = ((this.ancientCryptosleepCasketGroupID != null) ? this.ancientCryptosleepCasketGroupID.ToString() : "null");
			array[82] = ", podContentsType=";
			array[83] = ((this.podContentsType != null) ? this.podContentsType.ToString() : "null");
			array[84] = ", ancientCryptosleepCasketOpenSignalTag=";
			array[85] = ((this.ancientCryptosleepCasketOpenSignalTag != null) ? this.ancientCryptosleepCasketOpenSignalTag.ToString() : "null");
			array[86] = ", thingSetMakerDef=";
			array[87] = ((this.thingSetMakerDef != null) ? this.thingSetMakerDef.ToString() : "null");
			array[88] = ", thingSetMakerParams=";
			array[89] = ((this.thingSetMakerParams != null) ? this.thingSetMakerParams.ToString() : "null");
			array[90] = ", stockpileConcreteContents=";
			array[91] = ((this.stockpileConcreteContents != null) ? this.stockpileConcreteContents.Count.ToString() : "null");
			array[92] = ", stockpileMarketValue=";
			array[93] = ((this.stockpileMarketValue != null) ? this.stockpileMarketValue.ToString() : "null");
			array[94] = ", innerStockpileSize=";
			array[95] = ((this.innerStockpileSize != null) ? this.innerStockpileSize.ToString() : "null");
			array[96] = ", edgeDefenseWidth=";
			array[97] = ((this.edgeDefenseWidth != null) ? this.edgeDefenseWidth.ToString() : "null");
			array[98] = ", edgeDefenseTurretsCount=";
			array[99] = ((this.edgeDefenseTurretsCount != null) ? this.edgeDefenseTurretsCount.ToString() : "null");
			array[100] = ", edgeDefenseMortarsCount=";
			array[101] = ((this.edgeDefenseMortarsCount != null) ? this.edgeDefenseMortarsCount.ToString() : "null");
			array[102] = ", edgeDefenseGuardsCount=";
			array[103] = ((this.edgeDefenseGuardsCount != null) ? this.edgeDefenseGuardsCount.ToString() : "null");
			array[104] = ", mortarDef=";
			array[105] = ((this.mortarDef != null) ? this.mortarDef.ToString() : "null");
			array[106] = ", pathwayFloorDef=";
			array[107] = ((this.pathwayFloorDef != null) ? this.pathwayFloorDef.ToString() : "null");
			array[108] = ", cultivatedPlantDef=";
			array[109] = ((this.cultivatedPlantDef != null) ? this.cultivatedPlantDef.ToString() : "null");
			array[110] = ", fixedCulativedPlantGrowth=";
			array[111] = ((this.fixedCulativedPlantGrowth != null) ? this.fixedCulativedPlantGrowth.ToString() : "null");
			array[112] = ", fillWithThingsPadding=";
			array[113] = ((this.fillWithThingsPadding != null) ? this.fillWithThingsPadding.ToString() : "null");
			array[114] = ", settlementPawnGroupPoints=";
			array[115] = ((this.settlementPawnGroupPoints != null) ? this.settlementPawnGroupPoints.ToString() : "null");
			array[116] = ", settlementPawnGroupSeed=";
			array[117] = ((this.settlementPawnGroupSeed != null) ? this.settlementPawnGroupSeed.ToString() : "null");
			array[118] = ", settlementDontGeneratePawns=";
			array[119] = ((this.settlementDontGeneratePawns != null) ? this.settlementDontGeneratePawns.ToString() : "null");
			array[120] = ", attackWhenPlayerBecameEnemy=";
			array[121] = ((this.attackWhenPlayerBecameEnemy != null) ? this.attackWhenPlayerBecameEnemy.ToString() : "null");
			array[122] = ", streetHorizontal=";
			array[123] = ((this.streetHorizontal != null) ? this.streetHorizontal.ToString() : "null");
			array[124] = ", edgeThingAvoidOtherEdgeThings=";
			array[125] = ((this.edgeThingAvoidOtherEdgeThings != null) ? this.edgeThingAvoidOtherEdgeThings.ToString() : "null");
			array[126] = ", edgeThingMustReachMapEdge=";
			array[127] = ((this.edgeThingMustReachMapEdge != null) ? this.edgeThingMustReachMapEdge.ToString() : "null");
			array[128] = ", allowPlacementOffEdge=";
			array[129] = ((this.allowPlacementOffEdge != null) ? this.allowPlacementOffEdge.ToString() : "null");
			array[130] = ", thrustAxis=";
			array[131] = ((this.thrustAxis != null) ? this.thrustAxis.ToString() : "null");
			array[132] = ", makeWarningLetter=";
			array[133] = ((this.makeWarningLetter != null) ? this.makeWarningLetter.ToString() : "null");
			array[134] = ", allowedMonumentThings=";
			array[135] = ((this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null");
			array[136] = ", bedCount=";
			array[137] = ((this.bedCount != null) ? this.bedCount.ToString() : string.Concat(new string[]
			{
				"null, workSitePoints=",
				(this.workSitePoints != 0f) ? this.workSitePoints.ToString() : "null",
				", lootConcereteContents=",
				(this.lootConcereteContents != null) ? this.lootConcereteContents.ToString() : "null",
				", lootMarketValue=",
				(this.lootMarketValue != null) ? this.lootMarketValue.ToString() : "null"
			}));
			array[138] = ", extraDoorEdge=";
			array[139] = ((this.extraDoorEdge != null) ? this.extraDoorEdge.ToString() : "null");
			array[140] = ", minLengthAfterSplit=";
			array[141] = ((this.minLengthAfterSplit != null) ? this.minLengthAfterSplit.ToString() : "null");
			return string.Concat(array);
		}

		// Token: 0x0400DE0B RID: 56843
		public CellRect rect;

		// Token: 0x0400DE0C RID: 56844
		public Faction faction;

		// Token: 0x0400DE0D RID: 56845
		public float? threatPoints;

		// Token: 0x0400DE0E RID: 56846
		private Dictionary<string, object> custom;

		// Token: 0x0400DE0F RID: 56847
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x0400DE10 RID: 56848
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x0400DE11 RID: 56849
		public RoofDef roofDef;

		// Token: 0x0400DE12 RID: 56850
		public bool? noRoof;

		// Token: 0x0400DE13 RID: 56851
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x0400DE14 RID: 56852
		public Thing singleThingToSpawn;

		// Token: 0x0400DE15 RID: 56853
		public ThingDef singleThingDef;

		// Token: 0x0400DE16 RID: 56854
		public ThingDef singleThingStuff;

		// Token: 0x0400DE17 RID: 56855
		public int? singleThingStackCount;

		// Token: 0x0400DE18 RID: 56856
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x0400DE19 RID: 56857
		public bool? spawnBridgeIfTerrainCantSupportThing;

		// Token: 0x0400DE1A RID: 56858
		public bool? spawnOutside;

		// Token: 0x0400DE1B RID: 56859
		public List<Thing> singleThingInnerThings;

		// Token: 0x0400DE1C RID: 56860
		public Pawn singlePawnToSpawn;

		// Token: 0x0400DE1D RID: 56861
		public PawnKindDef singlePawnKindDef;

		// Token: 0x0400DE1E RID: 56862
		public bool? disableSinglePawn;

		// Token: 0x0400DE1F RID: 56863
		public Lord singlePawnLord;

		// Token: 0x0400DE20 RID: 56864
		public Lord settlementLord;

		// Token: 0x0400DE21 RID: 56865
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x0400DE22 RID: 56866
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x0400DE23 RID: 56867
		public Action<Thing> postThingSpawn;

		// Token: 0x0400DE24 RID: 56868
		public Action<Thing> postThingGenerate;

		// Token: 0x0400DE25 RID: 56869
		public int? mechanoidsCount;

		// Token: 0x0400DE26 RID: 56870
		public int? fleshbeastsCount;

		// Token: 0x0400DE27 RID: 56871
		public int? hivesCount;

		// Token: 0x0400DE28 RID: 56872
		public bool? disableHives;

		// Token: 0x0400DE29 RID: 56873
		public Rot4? thingRot;

		// Token: 0x0400DE2A RID: 56874
		public ThingDef wallStuff;

		// Token: 0x0400DE2B RID: 56875
		public float? chanceToSkipWallBlock;

		// Token: 0x0400DE2C RID: 56876
		public ThingDef wallThingDef;

		// Token: 0x0400DE2D RID: 56877
		public TerrainDef floorDef;

		// Token: 0x0400DE2E RID: 56878
		public float? chanceToSkipFloor;

		// Token: 0x0400DE2F RID: 56879
		public ThingDef filthDef;

		// Token: 0x0400DE30 RID: 56880
		public FloatRange? filthDensity;

		// Token: 0x0400DE31 RID: 56881
		public bool? floorOnlyIfTerrainSupports;

		// Token: 0x0400DE32 RID: 56882
		public bool? allowBridgeOnAnyImpassableTerrain;

		// Token: 0x0400DE33 RID: 56883
		public bool? clearEdificeOnly;

		// Token: 0x0400DE34 RID: 56884
		public bool? clearFillageOnly;

		// Token: 0x0400DE35 RID: 56885
		public bool? clearRoof;

		// Token: 0x0400DE36 RID: 56886
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x0400DE37 RID: 56887
		public PodContentsType? podContentsType;

		// Token: 0x0400DE38 RID: 56888
		public string ancientCryptosleepCasketOpenSignalTag;

		// Token: 0x0400DE39 RID: 56889
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400DE3A RID: 56890
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x0400DE3B RID: 56891
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x0400DE3C RID: 56892
		public float? stockpileMarketValue;

		// Token: 0x0400DE3D RID: 56893
		public int? innerStockpileSize;

		// Token: 0x0400DE3E RID: 56894
		public int? edgeDefenseWidth;

		// Token: 0x0400DE3F RID: 56895
		public int? edgeDefenseTurretsCount;

		// Token: 0x0400DE40 RID: 56896
		public int? edgeDefenseMortarsCount;

		// Token: 0x0400DE41 RID: 56897
		public int? edgeDefenseGuardsCount;

		// Token: 0x0400DE42 RID: 56898
		public ThingDef mortarDef;

		// Token: 0x0400DE43 RID: 56899
		public TerrainDef pathwayFloorDef;

		// Token: 0x0400DE44 RID: 56900
		public ThingDef cultivatedPlantDef;

		// Token: 0x0400DE45 RID: 56901
		public float? fixedCulativedPlantGrowth;

		// Token: 0x0400DE46 RID: 56902
		public int? fillWithThingsPadding;

		// Token: 0x0400DE47 RID: 56903
		public float? settlementPawnGroupPoints;

		// Token: 0x0400DE48 RID: 56904
		public int? settlementPawnGroupSeed;

		// Token: 0x0400DE49 RID: 56905
		public bool? settlementDontGeneratePawns;

		// Token: 0x0400DE4A RID: 56906
		public bool? attackWhenPlayerBecameEnemy;

		// Token: 0x0400DE4B RID: 56907
		public bool? streetHorizontal;

		// Token: 0x0400DE4C RID: 56908
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x0400DE4D RID: 56909
		public bool? edgeThingMustReachMapEdge;

		// Token: 0x0400DE4E RID: 56910
		public bool? allowPlacementOffEdge;

		// Token: 0x0400DE4F RID: 56911
		public Rot4? thrustAxis;

		// Token: 0x0400DE50 RID: 56912
		public FloatRange? hpPercentRange;

		// Token: 0x0400DE51 RID: 56913
		public Thing conditionCauser;

		// Token: 0x0400DE52 RID: 56914
		public bool? makeWarningLetter;

		// Token: 0x0400DE53 RID: 56915
		public ThingFilter allowedMonumentThings;

		// Token: 0x0400DE54 RID: 56916
		public bool? allowGeneratingThronerooms;

		// Token: 0x0400DE55 RID: 56917
		public int? bedCount;

		// Token: 0x0400DE56 RID: 56918
		public float workSitePoints;

		// Token: 0x0400DE57 RID: 56919
		public IList<Thing> lootConcereteContents;

		// Token: 0x0400DE58 RID: 56920
		public float? lootMarketValue;

		// Token: 0x0400DE59 RID: 56921
		public Rot4? extraDoorEdge;

		// Token: 0x0400DE5A RID: 56922
		public string sleepingInsectsWakeupSignalTag;

		// Token: 0x0400DE5B RID: 56923
		public string sleepingMechanoidsWakeupSignalTag;

		// Token: 0x0400DE5C RID: 56924
		public string rectTriggerSignalTag;

		// Token: 0x0400DE5D RID: 56925
		public bool? destroyIfUnfogged;

		// Token: 0x0400DE5E RID: 56926
		public string messageSignalTag;

		// Token: 0x0400DE5F RID: 56927
		public string message;

		// Token: 0x0400DE60 RID: 56928
		public MessageTypeDef messageType;

		// Token: 0x0400DE61 RID: 56929
		public LookTargets lookTargets;

		// Token: 0x0400DE62 RID: 56930
		public bool? spawnAnywhereIfNoGoodCell;

		// Token: 0x0400DE63 RID: 56931
		public bool? ignoreRoofedRequirement;

		// Token: 0x0400DE64 RID: 56932
		public IntVec3? overrideLoc;

		// Token: 0x0400DE65 RID: 56933
		public bool? sendStandardLetter;

		// Token: 0x0400DE66 RID: 56934
		public string infestationSignalTag;

		// Token: 0x0400DE67 RID: 56935
		public float? insectsPoints;

		// Token: 0x0400DE68 RID: 56936
		public string ambushSignalTag;

		// Token: 0x0400DE69 RID: 56937
		public SignalActionAmbushType? ambushType;

		// Token: 0x0400DE6A RID: 56938
		public IntVec3? spawnNear;

		// Token: 0x0400DE6B RID: 56939
		public CellRect? spawnAround;

		// Token: 0x0400DE6C RID: 56940
		public bool? spawnPawnsOnEdge;

		// Token: 0x0400DE6D RID: 56941
		public float? ambushPoints;

		// Token: 0x0400DE6E RID: 56942
		public int? cornerRadius;

		// Token: 0x0400DE6F RID: 56943
		public SoundDef sound;

		// Token: 0x0400DE70 RID: 56944
		public string soundOneShotActionSignalTag;

		// Token: 0x0400DE71 RID: 56945
		public string unfoggedSignalTag;

		// Token: 0x0400DE72 RID: 56946
		public Thing triggerContainerEmptiedThing;

		// Token: 0x0400DE73 RID: 56947
		public string triggerContainerEmptiedSignalTag;

		// Token: 0x0400DE74 RID: 56948
		public Building_Door openDoorActionDoor;

		// Token: 0x0400DE75 RID: 56949
		public string openDoorActionSignalTag;

		// Token: 0x0400DE76 RID: 56950
		public string triggerSecuritySignal;

		// Token: 0x0400DE77 RID: 56951
		public Thing relicThing;

		// Token: 0x0400DE78 RID: 56952
		public float? exteriorThreatPoints;

		// Token: 0x0400DE79 RID: 56953
		public float? interiorThreatPoints;

		// Token: 0x0400DE7A RID: 56954
		public SitePart sitePart;

		// Token: 0x0400DE7B RID: 56955
		public int? minLengthAfterSplit;

		// Token: 0x0400DE7C RID: 56956
		public bool? sendWokenUpMessage;

		// Token: 0x0400DE7D RID: 56957
		public LayoutStructureSketch ancientLayoutStructureSketch;

		// Token: 0x0400DE7E RID: 56958
		public bool? ignoreDoorways;

		// Token: 0x0400DE7F RID: 56959
		public int? minorBuildingCount;

		// Token: 0x0400DE80 RID: 56960
		public int? minorBuildingRadialDistance;

		// Token: 0x0400DE81 RID: 56961
		public ThingDef centralBuilding;

		// Token: 0x0400DE82 RID: 56962
		public PawnKindDef desiccatedCorpsePawnKind;

		// Token: 0x0400DE83 RID: 56963
		public IntRange? desiccatedCorpseRandomAgeRange;

		// Token: 0x0400DE84 RID: 56964
		public FloatRange? dessicatedCorpseDensityRange;

		// Token: 0x0400DE85 RID: 56965
		public float edgeUnpolluteChance;
	}
}
