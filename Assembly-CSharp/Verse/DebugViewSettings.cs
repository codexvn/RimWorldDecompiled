using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CA1 RID: 3233
	public static class DebugViewSettings
	{
		// Token: 0x060052F6 RID: 21238 RVA: 0x001ABD5D File Offset: 0x001A9F5D
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Terrain);
			}
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001ABD7F File Offset: 0x001A9F7F
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(~MapMeshFlagDefOf.None);
			}
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x00140DBB File Offset: 0x0013EFBB
		public static void drawIndoorMaskToggled()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x001ABDA2 File Offset: 0x001A9FA2
		public static void showMemoryInfoToggled()
		{
			MemoryUsageUtility.SetShouldRecord(DebugViewSettings.showMemoryInfo);
		}

		// Token: 0x04003B6C RID: 15212
		public static bool drawFog = true;

		// Token: 0x04003B6D RID: 15213
		public static bool drawSnow = true;

		// Token: 0x04003B6E RID: 15214
		public static bool drawSand = true;

		// Token: 0x04003B6F RID: 15215
		public static bool drawTerrain = true;

		// Token: 0x04003B70 RID: 15216
		public static bool drawTerrainWater = true;

		// Token: 0x04003B71 RID: 15217
		public static bool drawThingsDynamic = true;

		// Token: 0x04003B72 RID: 15218
		public static bool drawThingsPrinted = true;

		// Token: 0x04003B73 RID: 15219
		public static bool drawShadows = true;

		// Token: 0x04003B74 RID: 15220
		public static bool drawLightingOverlay = true;

		// Token: 0x04003B75 RID: 15221
		public static bool drawWorldOverlays = true;

		// Token: 0x04003B76 RID: 15222
		public static bool drawGas = true;

		// Token: 0x04003B77 RID: 15223
		public static bool singleThreadedDrawing = false;

		// Token: 0x04003B78 RID: 15224
		public static bool playGravshipCutscenes = true;

		// Token: 0x04003B79 RID: 15225
		public static bool drawWorldObjects = true;

		// Token: 0x04003B7A RID: 15226
		public static bool drawPaths = false;

		// Token: 0x04003B7B RID: 15227
		public static bool drawPatherState = false;

		// Token: 0x04003B7C RID: 15228
		public static bool drawCastPositionSearch = false;

		// Token: 0x04003B7D RID: 15229
		public static bool drawDestSearch = false;

		// Token: 0x04003B7E RID: 15230
		public static bool drawStyleSearch = false;

		// Token: 0x04003B7F RID: 15231
		public static bool drawSectionEdges = false;

		// Token: 0x04003B80 RID: 15232
		public static bool drawRiverDebug = false;

		// Token: 0x04003B81 RID: 15233
		public static bool drawRiverFlowDebug = false;

		// Token: 0x04003B82 RID: 15234
		public static bool drawPawnDebug = false;

		// Token: 0x04003B83 RID: 15235
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x04003B84 RID: 15236
		public static bool drawRegions = false;

		// Token: 0x04003B85 RID: 15237
		public static bool drawRegionLinks = false;

		// Token: 0x04003B86 RID: 15238
		public static bool drawRegionDirties = false;

		// Token: 0x04003B87 RID: 15239
		public static bool drawRegionTraversal = false;

		// Token: 0x04003B88 RID: 15240
		public static bool drawRegionThings = false;

		// Token: 0x04003B89 RID: 15241
		public static bool drawDistricts = false;

		// Token: 0x04003B8A RID: 15242
		public static bool drawRooms = false;

		// Token: 0x04003B8B RID: 15243
		public static bool drawPower = false;

		// Token: 0x04003B8C RID: 15244
		public static bool drawPowerNetGrid = false;

		// Token: 0x04003B8D RID: 15245
		public static bool drawOpportunisticJobs = false;

		// Token: 0x04003B8E RID: 15246
		public static bool drawTooltipEdges = false;

		// Token: 0x04003B8F RID: 15247
		public static bool drawRecordedNoise = false;

		// Token: 0x04003B90 RID: 15248
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x04003B91 RID: 15249
		public static bool drawPreyInfo = false;

		// Token: 0x04003B92 RID: 15250
		public static bool drawGlow = false;

		// Token: 0x04003B93 RID: 15251
		public static bool drawAvoidGrid = false;

		// Token: 0x04003B94 RID: 15252
		public static bool drawBreachingGrid = false;

		// Token: 0x04003B95 RID: 15253
		public static bool drawBreachingNoise = false;

		// Token: 0x04003B96 RID: 15254
		public static bool drawLords = false;

		// Token: 0x04003B97 RID: 15255
		public static bool drawDuties = false;

		// Token: 0x04003B98 RID: 15256
		public static bool drawShooting = false;

		// Token: 0x04003B99 RID: 15257
		public static bool drawInfestationChance = false;

		// Token: 0x04003B9A RID: 15258
		public static bool drawFleshmassHeartChance = false;

		// Token: 0x04003B9B RID: 15259
		public static bool drawStealDebug = false;

		// Token: 0x04003B9C RID: 15260
		public static bool drawDeepResources = false;

		// Token: 0x04003B9D RID: 15261
		public static bool drawAttackTargetScores = false;

		// Token: 0x04003B9E RID: 15262
		public static bool drawFOVSymmetry = false;

		// Token: 0x04003B9F RID: 15263
		public static bool drawNonCombatantTimer = false;

		// Token: 0x04003BA0 RID: 15264
		public static bool drawInteractionCells = false;

		// Token: 0x04003BA1 RID: 15265
		public static bool drawDoorsDebug = false;

		// Token: 0x04003BA2 RID: 15266
		public static bool drawDestReservations = false;

		// Token: 0x04003BA3 RID: 15267
		public static bool drawDamageRects = false;

		// Token: 0x04003BA4 RID: 15268
		public static bool drawDissolutionCells = false;

		// Token: 0x04003BA5 RID: 15269
		public static bool drawUnpollutionCells = false;

		// Token: 0x04003BA6 RID: 15270
		public static bool drawHateChanterPositions = false;

		// Token: 0x04003BA7 RID: 15271
		public static bool drawDarknessOverlay = true;

		// Token: 0x04003BA8 RID: 15272
		public static bool drawWoundAnchorsOnHover = false;

		// Token: 0x04003BA9 RID: 15273
		public static bool drawMapGraphs = false;

		// Token: 0x04003BAA RID: 15274
		public static bool drawMapRooms = false;

		// Token: 0x04003BAB RID: 15275
		public static bool drawIndoorMask = false;

		// Token: 0x04003BAC RID: 15276
		public static bool drawOutdoorMask = false;

		// Token: 0x04003BAD RID: 15277
		public static bool drawShamblerAlertMote = true;

		// Token: 0x04003BAE RID: 15278
		public static bool drawWaterBodies = false;

		// Token: 0x04003BAF RID: 15279
		public static bool drawMeltingIce = false;

		// Token: 0x04003BB0 RID: 15280
		public static bool drawUsedRects = false;

		// Token: 0x04003BB1 RID: 15281
		public static bool drawRoadPaths = false;

		// Token: 0x04003BB2 RID: 15282
		public static bool drawGravshipMask = false;

		// Token: 0x04003BB3 RID: 15283
		public static bool drawTerrainCurtain = true;

		// Token: 0x04003BB4 RID: 15284
		public static bool writeGame = false;

		// Token: 0x04003BB5 RID: 15285
		public static bool writeSteamItems = false;

		// Token: 0x04003BB6 RID: 15286
		public static bool writeConcepts = false;

		// Token: 0x04003BB7 RID: 15287
		public static bool writeReservations = false;

		// Token: 0x04003BB8 RID: 15288
		public static bool writePathCosts = false;

		// Token: 0x04003BB9 RID: 15289
		public static bool writeFertility = false;

		// Token: 0x04003BBA RID: 15290
		public static bool writeLinkFlags = false;

		// Token: 0x04003BBB RID: 15291
		public static bool writeCover = false;

		// Token: 0x04003BBC RID: 15292
		public static bool writeCellContents = false;

		// Token: 0x04003BBD RID: 15293
		public static bool writeMusicManagerPlay = false;

		// Token: 0x04003BBE RID: 15294
		public static bool writeStoryteller = false;

		// Token: 0x04003BBF RID: 15295
		public static bool writePlayingSounds = false;

		// Token: 0x04003BC0 RID: 15296
		public static bool writeSoundEventsRecord = false;

		// Token: 0x04003BC1 RID: 15297
		public static bool writeMoteSaturation = false;

		// Token: 0x04003BC2 RID: 15298
		public static bool writeSnowDepth = false;

		// Token: 0x04003BC3 RID: 15299
		public static bool writeSandDepth = false;

		// Token: 0x04003BC4 RID: 15300
		public static bool writeEcosystem = false;

		// Token: 0x04003BC5 RID: 15301
		public static bool writeRecentStrikes = false;

		// Token: 0x04003BC6 RID: 15302
		public static bool writeBeauty = false;

		// Token: 0x04003BC7 RID: 15303
		public static bool writeListRepairableBldgs = false;

		// Token: 0x04003BC8 RID: 15304
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x04003BC9 RID: 15305
		public static bool writeListHaulables = false;

		// Token: 0x04003BCA RID: 15306
		public static bool writeListMergeables = false;

		// Token: 0x04003BCB RID: 15307
		public static bool writeTotalSnowDepth = false;

		// Token: 0x04003BCC RID: 15308
		public static bool writeCanReachColony = false;

		// Token: 0x04003BCD RID: 15309
		public static bool writeMentalStateCalcs = false;

		// Token: 0x04003BCE RID: 15310
		public static bool writeWind = false;

		// Token: 0x04003BCF RID: 15311
		public static bool writeTerrain = false;

		// Token: 0x04003BD0 RID: 15312
		public static bool writeApparelScore = false;

		// Token: 0x04003BD1 RID: 15313
		public static bool writeWorkSettings = false;

		// Token: 0x04003BD2 RID: 15314
		public static bool writeSkyManager = false;

		// Token: 0x04003BD3 RID: 15315
		public static bool writeMemoryUsage = false;

		// Token: 0x04003BD4 RID: 15316
		public static bool writeMapGameConditions = false;

		// Token: 0x04003BD5 RID: 15317
		public static bool writeAttackTargets = false;

		// Token: 0x04003BD6 RID: 15318
		public static bool writeRopesAndPens = false;

		// Token: 0x04003BD7 RID: 15319
		public static bool writeRoomRoles = false;

		// Token: 0x04003BD8 RID: 15320
		public static bool logIncapChance = false;

		// Token: 0x04003BD9 RID: 15321
		public static bool logInput = false;

		// Token: 0x04003BDA RID: 15322
		public static bool logApparelGeneration = false;

		// Token: 0x04003BDB RID: 15323
		public static bool logLordToilTransitions = false;

		// Token: 0x04003BDC RID: 15324
		public static bool logGrammarResolution = false;

		// Token: 0x04003BDD RID: 15325
		public static bool logCombatLogMouseover = false;

		// Token: 0x04003BDE RID: 15326
		public static bool logCauseOfDeath = false;

		// Token: 0x04003BDF RID: 15327
		public static bool logMapLoad = false;

		// Token: 0x04003BE0 RID: 15328
		public static bool logTutor = false;

		// Token: 0x04003BE1 RID: 15329
		public static bool logSignals = false;

		// Token: 0x04003BE2 RID: 15330
		public static bool logWorldPawnGC = false;

		// Token: 0x04003BE3 RID: 15331
		public static bool logTaleRecording = false;

		// Token: 0x04003BE4 RID: 15332
		public static bool logHourlyScreenshot = false;

		// Token: 0x04003BE5 RID: 15333
		public static bool logFilthSummary = false;

		// Token: 0x04003BE6 RID: 15334
		public static bool logCarriedBetweenJobs = false;

		// Token: 0x04003BE7 RID: 15335
		public static bool logComplexGenPoints = false;

		// Token: 0x04003BE8 RID: 15336
		public static bool saveGravshipRenders = false;

		// Token: 0x04003BE9 RID: 15337
		public static bool debugApparelOptimize = false;

		// Token: 0x04003BEA RID: 15338
		public static bool disableGravshipRenderShader = false;

		// Token: 0x04003BEB RID: 15339
		public static bool showAllRoomStats = false;

		// Token: 0x04003BEC RID: 15340
		public static bool showFloatMenuWorkGivers = false;

		// Token: 0x04003BED RID: 15341
		public static bool neverForceNormalSpeed = false;

		// Token: 0x04003BEE RID: 15342
		public static bool showArchitectMenuOrder = false;

		// Token: 0x04003BEF RID: 15343
		public static bool showTpsCounter = false;

		// Token: 0x04003BF0 RID: 15344
		public static bool showFpsCounter = false;

		// Token: 0x04003BF1 RID: 15345
		public static bool showMemoryInfo = false;
	}
}
