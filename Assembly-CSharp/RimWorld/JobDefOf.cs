using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200358F RID: 13711
	[DefOf]
	public static class JobDefOf
	{
		// Token: 0x0601337D RID: 78717 RVA: 0x005B59D8 File Offset: 0x005B3BD8
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		// Token: 0x0400CBE7 RID: 52199
		public static JobDef IdleWhileDespawned;

		// Token: 0x0400CBE8 RID: 52200
		public static JobDef Goto;

		// Token: 0x0400CBE9 RID: 52201
		public static JobDef PickupToHold;

		// Token: 0x0400CBEA RID: 52202
		public static JobDef Wait;

		// Token: 0x0400CBEB RID: 52203
		public static JobDef StandAndStare;

		// Token: 0x0400CBEC RID: 52204
		public static JobDef Wait_MaintainPosture;

		// Token: 0x0400CBED RID: 52205
		public static JobDef Wait_Asleep;

		// Token: 0x0400CBEE RID: 52206
		public static JobDef Wait_Downed;

		// Token: 0x0400CBEF RID: 52207
		public static JobDef Wait_AsleepDormancy;

		// Token: 0x0400CBF0 RID: 52208
		public static JobDef GotoWander;

		// Token: 0x0400CBF1 RID: 52209
		public static JobDef Wait_Wander;

		// Token: 0x0400CBF2 RID: 52210
		public static JobDef GotoSafeTemperature;

		// Token: 0x0400CBF3 RID: 52211
		public static JobDef Wait_SafeTemperature;

		// Token: 0x0400CBF4 RID: 52212
		public static JobDef Wait_Combat;

		// Token: 0x0400CBF5 RID: 52213
		public static JobDef Equip;

		// Token: 0x0400CBF6 RID: 52214
		public static JobDef AttackMelee;

		// Token: 0x0400CBF7 RID: 52215
		public static JobDef AttackStatic;

		// Token: 0x0400CBF8 RID: 52216
		public static JobDef UseVerbOnThing;

		// Token: 0x0400CBF9 RID: 52217
		public static JobDef UseVerbOnThingStatic;

		// Token: 0x0400CBFA RID: 52218
		public static JobDef UseVerbOnThingStaticReserve;

		// Token: 0x0400CBFB RID: 52219
		public static JobDef CastJump;

		// Token: 0x0400CBFC RID: 52220
		public static JobDef CastAbilityOnThing;

		// Token: 0x0400CBFD RID: 52221
		public static JobDef CastAbilityOnWorldTile;

		// Token: 0x0400CBFE RID: 52222
		public static JobDef TakeInventory;

		// Token: 0x0400CBFF RID: 52223
		public static JobDef Follow;

		// Token: 0x0400CC00 RID: 52224
		public static JobDef FollowClose;

		// Token: 0x0400CC01 RID: 52225
		public static JobDef Wear;

		// Token: 0x0400CC02 RID: 52226
		public static JobDef ForceTargetWear;

		// Token: 0x0400CC03 RID: 52227
		public static JobDef RemoveApparel;

		// Token: 0x0400CC04 RID: 52228
		public static JobDef DropEquipment;

		// Token: 0x0400CC05 RID: 52229
		public static JobDef Strip;

		// Token: 0x0400CC06 RID: 52230
		public static JobDef Open;

		// Token: 0x0400CC07 RID: 52231
		public static JobDef EjectFuel;

		// Token: 0x0400CC08 RID: 52232
		public static JobDef Hunt;

		// Token: 0x0400CC09 RID: 52233
		public static JobDef ManTurret;

		// Token: 0x0400CC0A RID: 52234
		public static JobDef EnterCryptosleepCasket;

		// Token: 0x0400CC0B RID: 52235
		public static JobDef UseNeurotrainer;

		// Token: 0x0400CC0C RID: 52236
		public static JobDef ClearSnow;

		// Token: 0x0400CC0D RID: 52237
		public static JobDef Vomit;

		// Token: 0x0400CC0E RID: 52238
		public static JobDef Flick;

		// Token: 0x0400CC0F RID: 52239
		public static JobDef DoBill;

		// Token: 0x0400CC10 RID: 52240
		public static JobDef Research;

		// Token: 0x0400CC11 RID: 52241
		public static JobDef Mine;

		// Token: 0x0400CC12 RID: 52242
		public static JobDef OperateDeepDrill;

		// Token: 0x0400CC13 RID: 52243
		public static JobDef OperateScanner;

		// Token: 0x0400CC14 RID: 52244
		public static JobDef Repair;

		// Token: 0x0400CC15 RID: 52245
		[MayRequireBiotech]
		public static JobDef RepairMech;

		// Token: 0x0400CC16 RID: 52246
		[MayRequireBiotech]
		public static JobDef ControlMech;

		// Token: 0x0400CC17 RID: 52247
		public static JobDef FixBrokenDownBuilding;

		// Token: 0x0400CC18 RID: 52248
		public static JobDef UseCommsConsole;

		// Token: 0x0400CC19 RID: 52249
		public static JobDef Clean;

		// Token: 0x0400CC1A RID: 52250
		public static JobDef PaintBuilding;

		// Token: 0x0400CC1B RID: 52251
		public static JobDef PaintFloor;

		// Token: 0x0400CC1C RID: 52252
		public static JobDef RemovePaintBuilding;

		// Token: 0x0400CC1D RID: 52253
		public static JobDef RemovePaintFloor;

		// Token: 0x0400CC1E RID: 52254
		public static JobDef TradeWithPawn;

		// Token: 0x0400CC1F RID: 52255
		public static JobDef DismissTrader;

		// Token: 0x0400CC20 RID: 52256
		public static JobDef Flee;

		// Token: 0x0400CC21 RID: 52257
		public static JobDef FleeAndCower;

		// Token: 0x0400CC22 RID: 52258
		[MayRequireAnomaly]
		public static JobDef FleeAndCowerShort;

		// Token: 0x0400CC23 RID: 52259
		public static JobDef Lovin;

		// Token: 0x0400CC24 RID: 52260
		public static JobDef SocialFight;

		// Token: 0x0400CC25 RID: 52261
		public static JobDef Maintain;

		// Token: 0x0400CC26 RID: 52262
		public static JobDef GiveToPackAnimal;

		// Token: 0x0400CC27 RID: 52263
		public static JobDef EnterTransporter;

		// Token: 0x0400CC28 RID: 52264
		public static JobDef Resurrect;

		// Token: 0x0400CC29 RID: 52265
		public static JobDef InstallImplant;

		// Token: 0x0400CC2A RID: 52266
		public static JobDef Insult;

		// Token: 0x0400CC2B RID: 52267
		public static JobDef HaulCorpseToPublicPlace;

		// Token: 0x0400CC2C RID: 52268
		public static JobDef InducePrisonerToEscape;

		// Token: 0x0400CC2D RID: 52269
		public static JobDef OfferHelp;

		// Token: 0x0400CC2E RID: 52270
		public static JobDef ApplyTechprint;

		// Token: 0x0400CC2F RID: 52271
		public static JobDef GotoMindControlled;

		// Token: 0x0400CC30 RID: 52272
		public static JobDef EmptyThingContainer;

		// Token: 0x0400CC31 RID: 52273
		public static JobDef InteractThing;

		// Token: 0x0400CC32 RID: 52274
		public static JobDef TakeFromOtherInventory;

		// Token: 0x0400CC33 RID: 52275
		public static JobDef Reading;

		// Token: 0x0400CC34 RID: 52276
		public static JobDef EnterPortal;

		// Token: 0x0400CC35 RID: 52277
		public static JobDef ExitMapFlying;

		// Token: 0x0400CC36 RID: 52278
		public static JobDef Hack;

		// Token: 0x0400CC37 RID: 52279
		[MayRequireIdeology]
		public static JobDef ActivateArchonexusCore;

		// Token: 0x0400CC38 RID: 52280
		[MayRequireBiotech]
		public static JobDef RemoveMechlink;

		// Token: 0x0400CC39 RID: 52281
		[MayRequireBiotech]
		public static JobDef GetReimplanted;

		// Token: 0x0400CC3A RID: 52282
		[MayRequireBiotech]
		public static JobDef ClearPollution;

		// Token: 0x0400CC3B RID: 52283
		[MayRequireBiotech]
		public static JobDef TryRomance;

		// Token: 0x0400CC3C RID: 52284
		[MayRequireAnomaly]
		public static JobDef TalkCreepJoiner;

		// Token: 0x0400CC3D RID: 52285
		[MayRequireAnomaly]
		public static JobDef AnalyzeItem;

		// Token: 0x0400CC3E RID: 52286
		[MayRequireOdyssey]
		public static JobDef InspectGravEngine;

		// Token: 0x0400CC3F RID: 52287
		[MayRequireOdyssey]
		public static JobDef GotoOxygenatedArea;

		// Token: 0x0400CC40 RID: 52288
		[MayRequireOdyssey]
		public static JobDef GotoPatrolDest;

		// Token: 0x0400CC41 RID: 52289
		[MayRequireOdyssey]
		public static JobDef UseOutfitStand;

		// Token: 0x0400CC42 RID: 52290
		[MayRequireOdyssey]
		public static JobDef Forage;

		// Token: 0x0400CC43 RID: 52291
		[MayRequireOdyssey]
		public static JobDef GoSwimming;

		// Token: 0x0400CC44 RID: 52292
		[MayRequireOdyssey]
		public static JobDef Seal;

		// Token: 0x0400CC45 RID: 52293
		public static JobDef MarryAdjacentPawn;

		// Token: 0x0400CC46 RID: 52294
		public static JobDef SpectateCeremony;

		// Token: 0x0400CC47 RID: 52295
		public static JobDef StandAndBeSociallyActive;

		// Token: 0x0400CC48 RID: 52296
		public static JobDef GiveSpeech;

		// Token: 0x0400CC49 RID: 52297
		[MayRequireIdeology]
		public static JobDef AcceptRole;

		// Token: 0x0400CC4A RID: 52298
		[MayRequireIdeology]
		public static JobDef Dance;

		// Token: 0x0400CC4B RID: 52299
		[MayRequireIdeology]
		public static JobDef EatAtCannibalPlatter;

		// Token: 0x0400CC4C RID: 52300
		[MayRequireAnomaly]
		public static JobDef HateChanting;

		// Token: 0x0400CC4D RID: 52301
		[MayRequireOdyssey]
		public static JobDef GotoShip;

		// Token: 0x0400CC4E RID: 52302
		[MayRequireOdyssey]
		public static JobDef PilotConsole;

		// Token: 0x0400CC4F RID: 52303
		public static JobDef PrepareCaravan_GatherItems;

		// Token: 0x0400CC50 RID: 52304
		public static JobDef PrepareCaravan_GatherAnimals;

		// Token: 0x0400CC51 RID: 52305
		public static JobDef PrepareCaravan_CollectAnimals;

		// Token: 0x0400CC52 RID: 52306
		public static JobDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x0400CC53 RID: 52307
		public static JobDef ReturnedCaravan_PenAnimals;

		// Token: 0x0400CC54 RID: 52308
		public static JobDef Ignite;

		// Token: 0x0400CC55 RID: 52309
		public static JobDef BeatFire;

		// Token: 0x0400CC56 RID: 52310
		public static JobDef ExtinguishSelf;

		// Token: 0x0400CC57 RID: 52311
		public static JobDef ExtinguishFiresNearby;

		// Token: 0x0400CC58 RID: 52312
		public static JobDef LayDown;

		// Token: 0x0400CC59 RID: 52313
		public static JobDef LayDownAwake;

		// Token: 0x0400CC5A RID: 52314
		public static JobDef LayDownResting;

		// Token: 0x0400CC5B RID: 52315
		public static JobDef Ingest;

		// Token: 0x0400CC5C RID: 52316
		[MayRequireBiotech]
		public static JobDef SelfShutdown;

		// Token: 0x0400CC5D RID: 52317
		[MayRequireAnomaly]
		public static JobDef ActivityDormant;

		// Token: 0x0400CC5E RID: 52318
		[MayRequireAnomaly]
		public static JobDef EntityGoPassive;

		// Token: 0x0400CC5F RID: 52319
		public static JobDef HaulToCell;

		// Token: 0x0400CC60 RID: 52320
		public static JobDef HaulToContainer;

		// Token: 0x0400CC61 RID: 52321
		public static JobDef Steal;

		// Token: 0x0400CC62 RID: 52322
		public static JobDef Reload;

		// Token: 0x0400CC63 RID: 52323
		public static JobDef Refuel;

		// Token: 0x0400CC64 RID: 52324
		public static JobDef RefuelAtomic;

		// Token: 0x0400CC65 RID: 52325
		public static JobDef RearmTurret;

		// Token: 0x0400CC66 RID: 52326
		public static JobDef RearmTurretAtomic;

		// Token: 0x0400CC67 RID: 52327
		public static JobDef FillFermentingBarrel;

		// Token: 0x0400CC68 RID: 52328
		public static JobDef TakeBeerOutOfFermentingBarrel;

		// Token: 0x0400CC69 RID: 52329
		public static JobDef UnloadInventory;

		// Token: 0x0400CC6A RID: 52330
		public static JobDef UnloadYourInventory;

		// Token: 0x0400CC6B RID: 52331
		public static JobDef HaulToTransporter;

		// Token: 0x0400CC6C RID: 52332
		public static JobDef HaulToPortal;

		// Token: 0x0400CC6D RID: 52333
		[MayRequireIdeology]
		public static JobDef GiveToPawn;

		// Token: 0x0400CC6E RID: 52334
		[MayRequireIdeology]
		public static JobDef ExtractRelic;

		// Token: 0x0400CC6F RID: 52335
		[MayRequireIdeology]
		public static JobDef InstallRelic;

		// Token: 0x0400CC70 RID: 52336
		[MayRequireIdeology]
		public static JobDef ExtractToInventory;

		// Token: 0x0400CC71 RID: 52337
		[MayRequireBiotech]
		public static JobDef EmptyWasteContainer;

		// Token: 0x0400CC72 RID: 52338
		[MayRequireBiotech]
		public static JobDef CarryGenepackToContainer;

		// Token: 0x0400CC73 RID: 52339
		[MayRequireAnomaly]
		public static JobDef TakeBioferriteOutOfHarvester;

		// Token: 0x0400CC74 RID: 52340
		public static JobDef Carried;

		// Token: 0x0400CC75 RID: 52341
		public static JobDef Rescue;

		// Token: 0x0400CC76 RID: 52342
		public static JobDef Arrest;

		// Token: 0x0400CC77 RID: 52343
		public static JobDef Capture;

		// Token: 0x0400CC78 RID: 52344
		public static JobDef TakeWoundedPrisonerToBed;

		// Token: 0x0400CC79 RID: 52345
		public static JobDef TakeToBedToOperate;

		// Token: 0x0400CC7A RID: 52346
		public static JobDef TakeDownedPawnToBedDrafted;

		// Token: 0x0400CC7B RID: 52347
		public static JobDef EscortPrisonerToBed;

		// Token: 0x0400CC7C RID: 52348
		public static JobDef CarryToCryptosleepCasket;

		// Token: 0x0400CC7D RID: 52349
		public static JobDef CarryToCryptosleepCasketDrafted;

		// Token: 0x0400CC7E RID: 52350
		public static JobDef ReleasePrisoner;

		// Token: 0x0400CC7F RID: 52351
		public static JobDef Kidnap;

		// Token: 0x0400CC80 RID: 52352
		public static JobDef CarryDownedPawnToExit;

		// Token: 0x0400CC81 RID: 52353
		public static JobDef CarryDownedPawnToPortal;

		// Token: 0x0400CC82 RID: 52354
		public static JobDef CarryDownedPawnDrafted;

		// Token: 0x0400CC83 RID: 52355
		public static JobDef CarryToPrisonerBedDrafted;

		// Token: 0x0400CC84 RID: 52356
		public static JobDef DeliverToCell;

		// Token: 0x0400CC85 RID: 52357
		public static JobDef DeliverToBed;

		// Token: 0x0400CC86 RID: 52358
		[MayRequireIdeology]
		public static JobDef DeliverToAltar;

		// Token: 0x0400CC87 RID: 52359
		[MayRequireIdeology]
		public static JobDef Sacrifice;

		// Token: 0x0400CC88 RID: 52360
		[MayRequireIdeology]
		public static JobDef Scarify;

		// Token: 0x0400CC89 RID: 52361
		[MayRequireIdeology]
		public static JobDef Blind;

		// Token: 0x0400CC8A RID: 52362
		[MayRequireBiotech]
		public static JobDef HaulMechToCharger;

		// Token: 0x0400CC8B RID: 52363
		[MayRequireBiotech]
		public static JobDef HaulToAtomizer;

		// Token: 0x0400CC8C RID: 52364
		[MayRequireAnomaly]
		public static JobDef CarryToEntityHolder;

		// Token: 0x0400CC8D RID: 52365
		[MayRequireAnomaly]
		public static JobDef TransferBetweenEntityHolders;

		// Token: 0x0400CC8E RID: 52366
		[MayRequireAnomaly]
		public static JobDef CarryToEntityHolderAlreadyHolding;

		// Token: 0x0400CC8F RID: 52367
		public static JobDef PlaceNoCostFrame;

		// Token: 0x0400CC90 RID: 52368
		public static JobDef Replant;

		// Token: 0x0400CC91 RID: 52369
		public static JobDef FinishFrame;

		// Token: 0x0400CC92 RID: 52370
		public static JobDef Deconstruct;

		// Token: 0x0400CC93 RID: 52371
		public static JobDef DeconstructForBlueprint;

		// Token: 0x0400CC94 RID: 52372
		public static JobDef Uninstall;

		// Token: 0x0400CC95 RID: 52373
		public static JobDef ExtractTree;

		// Token: 0x0400CC96 RID: 52374
		public static JobDef SmoothFloor;

		// Token: 0x0400CC97 RID: 52375
		public static JobDef RemoveFloor;

		// Token: 0x0400CC98 RID: 52376
		public static JobDef RemoveFoundation;

		// Token: 0x0400CC99 RID: 52377
		public static JobDef BuildRoof;

		// Token: 0x0400CC9A RID: 52378
		public static JobDef RemoveRoof;

		// Token: 0x0400CC9B RID: 52379
		public static JobDef SmoothWall;

		// Token: 0x0400CC9C RID: 52380
		public static JobDef PrisonerAttemptRecruit;

		// Token: 0x0400CC9D RID: 52381
		public static JobDef PrisonerExecution;

		// Token: 0x0400CC9E RID: 52382
		public static JobDef GuiltyColonistExecution;

		// Token: 0x0400CC9F RID: 52383
		public static JobDef DeliverFood;

		// Token: 0x0400CCA0 RID: 52384
		[MayRequireIdeology]
		public static JobDef PrisonerEnslave;

		// Token: 0x0400CCA1 RID: 52385
		[MayRequireIdeology]
		public static JobDef PrisonerReduceWill;

		// Token: 0x0400CCA2 RID: 52386
		[MayRequireIdeology]
		public static JobDef PrisonerConvert;

		// Token: 0x0400CCA3 RID: 52387
		[MayRequireBiotech]
		public static JobDef PrisonerBloodfeed;

		// Token: 0x0400CCA4 RID: 52388
		[MayRequireAnomaly]
		public static JobDef PrisonerInterrogateIdentity;

		// Token: 0x0400CCA5 RID: 52389
		[MayRequireIdeology]
		public static JobDef SlaveSuppress;

		// Token: 0x0400CCA6 RID: 52390
		[MayRequireIdeology]
		public static JobDef SlaveEmancipation;

		// Token: 0x0400CCA7 RID: 52391
		[MayRequireIdeology]
		public static JobDef SlaveExecution;

		// Token: 0x0400CCA8 RID: 52392
		public static JobDef FeedPatient;

		// Token: 0x0400CCA9 RID: 52393
		public static JobDef TendPatient;

		// Token: 0x0400CCAA RID: 52394
		public static JobDef VisitSickPawn;

		// Token: 0x0400CCAB RID: 52395
		public static JobDef Sow;

		// Token: 0x0400CCAC RID: 52396
		public static JobDef Harvest;

		// Token: 0x0400CCAD RID: 52397
		public static JobDef CutPlant;

		// Token: 0x0400CCAE RID: 52398
		public static JobDef HarvestDesignated;

		// Token: 0x0400CCAF RID: 52399
		public static JobDef CutPlantDesignated;

		// Token: 0x0400CCB0 RID: 52400
		public static JobDef PlantSeed;

		// Token: 0x0400CCB1 RID: 52401
		[MayRequireIdeology]
		public static JobDef PruneGauranlenTree;

		// Token: 0x0400CCB2 RID: 52402
		public static JobDef Slaughter;

		// Token: 0x0400CCB3 RID: 52403
		public static JobDef Milk;

		// Token: 0x0400CCB4 RID: 52404
		public static JobDef Shear;

		// Token: 0x0400CCB5 RID: 52405
		public static JobDef Tame;

		// Token: 0x0400CCB6 RID: 52406
		public static JobDef Train;

		// Token: 0x0400CCB7 RID: 52407
		public static JobDef RopeToPen;

		// Token: 0x0400CCB8 RID: 52408
		public static JobDef RopeRoamerToUnenclosedPen;

		// Token: 0x0400CCB9 RID: 52409
		public static JobDef RopeRoamerToHitchingPost;

		// Token: 0x0400CCBA RID: 52410
		public static JobDef Unrope;

		// Token: 0x0400CCBB RID: 52411
		public static JobDef ReleaseAnimalToWild;

		// Token: 0x0400CCBC RID: 52412
		public static JobDef ExtractSkull;

		// Token: 0x0400CCBD RID: 52413
		[MayRequireOdyssey]
		public static JobDef Fish;

		// Token: 0x0400CCBE RID: 52414
		[MayRequireOdyssey]
		public static JobDef FishAnimal;

		// Token: 0x0400CCBF RID: 52415
		public static JobDef Nuzzle;

		// Token: 0x0400CCC0 RID: 52416
		public static JobDef Mate;

		// Token: 0x0400CCC1 RID: 52417
		public static JobDef LayEgg;

		// Token: 0x0400CCC2 RID: 52418
		public static JobDef PredatorHunt;

		// Token: 0x0400CCC3 RID: 52419
		public static JobDef FollowRoper;

		// Token: 0x0400CCC4 RID: 52420
		[MayRequireRoyalty]
		public static JobDef Reign;

		// Token: 0x0400CCC5 RID: 52421
		[MayRequireRoyalty]
		public static JobDef Meditate;

		// Token: 0x0400CCC6 RID: 52422
		[MayRequireRoyalty]
		public static JobDef Play_MusicalInstrument;

		// Token: 0x0400CCC7 RID: 52423
		[MayRequireRoyalty]
		public static JobDef LinkPsylinkable;

		// Token: 0x0400CCC8 RID: 52424
		[MayRequireRoyalty]
		public static JobDef BestowingCeremony;

		// Token: 0x0400CCC9 RID: 52425
		[MayRequireIdeology]
		public static JobDef InduceSlaveToRebel;

		// Token: 0x0400CCCA RID: 52426
		[MayRequireIdeology]
		public static JobDef OpenStylingStationDialog;

		// Token: 0x0400CCCB RID: 52427
		[MayRequireIdeology]
		public static JobDef UseStylingStation;

		// Token: 0x0400CCCC RID: 52428
		[MayRequireIdeology]
		public static JobDef UseStylingStationAutomatic;

		// Token: 0x0400CCCD RID: 52429
		[MayRequireIdeology]
		public static JobDef DyeHair;

		// Token: 0x0400CCCE RID: 52430
		[MayRequireIdeology]
		public static JobDef TakeCountToInventory;

		// Token: 0x0400CCCF RID: 52431
		[MayRequireIdeology]
		public static JobDef GotoAndBeSociallyActive;

		// Token: 0x0400CCD0 RID: 52432
		[MayRequireIdeology]
		public static JobDef PrepareSkylantern;

		// Token: 0x0400CCD1 RID: 52433
		[MayRequireIdeology]
		public static JobDef MeditatePray;

		// Token: 0x0400CCD2 RID: 52434
		[MayRequireIdeology]
		public static JobDef GetNeuralSupercharge;

		// Token: 0x0400CCD3 RID: 52435
		[MayRequireIdeology]
		public static JobDef CreateAndEnterCocoon;

		// Token: 0x0400CCD4 RID: 52436
		[MayRequireIdeology]
		public static JobDef CreateAndEnterHealingPod;

		// Token: 0x0400CCD5 RID: 52437
		[MayRequireIdeology]
		public static JobDef ReturnToGauranlenTree;

		// Token: 0x0400CCD6 RID: 52438
		[MayRequireIdeology]
		public static JobDef MergeIntoGaumakerPod;

		// Token: 0x0400CCD7 RID: 52439
		[MayRequireIdeology]
		public static JobDef ChangeTreeMode;

		// Token: 0x0400CCD8 RID: 52440
		[MayRequireIdeology]
		public static JobDef RecolorApparel;

		// Token: 0x0400CCD9 RID: 52441
		[MayRequireIdeology]
		public static JobDef EnterBiosculpterPod;

		// Token: 0x0400CCDA RID: 52442
		[MayRequireIdeology]
		public static JobDef CarryToBiosculpterPod;

		// Token: 0x0400CCDB RID: 52443
		[MayRequireBiotech]
		public static JobDef MechCharge;

		// Token: 0x0400CCDC RID: 52444
		[MayRequireBiotech]
		public static JobDef Deathrest;

		// Token: 0x0400CCDD RID: 52445
		[MayRequireBiotech]
		public static JobDef CreateXenogerm;

		// Token: 0x0400CCDE RID: 52446
		[MayRequireBiotech]
		public static JobDef EnterBuilding;

		// Token: 0x0400CCDF RID: 52447
		[MayRequireBiotech]
		public static JobDef CarryToBuilding;

		// Token: 0x0400CCE0 RID: 52448
		[MayRequireBiotech]
		public static JobDef AbsorbXenogerm;

		// Token: 0x0400CCE1 RID: 52449
		[MayRequireBiotech]
		public static JobDef Breastfeed;

		// Token: 0x0400CCE2 RID: 52450
		[MayRequireBiotech]
		public static JobDef BreastfeedCarryToMom;

		// Token: 0x0400CCE3 RID: 52451
		[MayRequireBiotech]
		public static JobDef BringBabyToSafety;

		// Token: 0x0400CCE4 RID: 52452
		[MayRequireBiotech]
		public static JobDef BringBabyToSafetyUnforced;

		// Token: 0x0400CCE5 RID: 52453
		[MayRequireBiotech]
		public static JobDef BottleFeedBaby;

		// Token: 0x0400CCE6 RID: 52454
		[MayRequireBiotech]
		public static JobDef CarryToMomAfterBirth;

		// Token: 0x0400CCE7 RID: 52455
		[MayRequireBiotech]
		public static JobDef Wait_WithSleeping;

		// Token: 0x0400CCE8 RID: 52456
		[MayRequireBiotech]
		public static JobDef ReleaseMechs;

		// Token: 0x0400CCE9 RID: 52457
		[MayRequireBiotech]
		public static JobDef Lessontaking;

		// Token: 0x0400CCEA RID: 52458
		[MayRequireBiotech]
		public static JobDef Lessongiving;

		// Token: 0x0400CCEB RID: 52459
		[MayRequireBiotech]
		public static JobDef FertilizeOvum;

		// Token: 0x0400CCEC RID: 52460
		[MayRequireBiotech]
		public static JobDef DisassembleMech;

		// Token: 0x0400CCED RID: 52461
		[MayRequireBiotech]
		public static JobDef BabySuckle;

		// Token: 0x0400CCEE RID: 52462
		[MayRequireBiotech]
		public static JobDef BabyPlay;

		// Token: 0x0400CCEF RID: 52463
		[MayRequireAnomaly]
		public static JobDef RevenantWander;

		// Token: 0x0400CCF0 RID: 52464
		[MayRequireAnomaly]
		public static JobDef RevenantAttack;

		// Token: 0x0400CCF1 RID: 52465
		[MayRequireAnomaly]
		public static JobDef RevenantEscape;

		// Token: 0x0400CCF2 RID: 52466
		[MayRequireAnomaly]
		public static JobDef RevenantSleep;

		// Token: 0x0400CCF3 RID: 52467
		[MayRequireAnomaly]
		public static JobDef StudyInteract;

		// Token: 0x0400CCF4 RID: 52468
		[MayRequireAnomaly]
		public static JobDef ReleaseEntity;

		// Token: 0x0400CCF5 RID: 52469
		[MayRequireAnomaly]
		public static JobDef TendEntity;

		// Token: 0x0400CCF6 RID: 52470
		[MayRequireAnomaly]
		public static JobDef ExecuteEntity;

		// Token: 0x0400CCF7 RID: 52471
		[MayRequireAnomaly]
		public static JobDef ExtractBioferrite;

		// Token: 0x0400CCF8 RID: 52472
		[MayRequireAnomaly]
		public static JobDef ActivitySuppression;

		// Token: 0x0400CCF9 RID: 52473
		[MayRequireAnomaly]
		public static JobDef NociosphereDepart;

		// Token: 0x0400CCFA RID: 52474
		[MayRequireAnomaly]
		public static JobDef GoldenCubePlay;

		// Token: 0x0400CCFB RID: 52475
		[MayRequireAnomaly]
		public static JobDef BuildCubeSculpture;

		// Token: 0x0400CCFC RID: 52476
		[MayRequireAnomaly]
		public static JobDef InvestigateMonolith;

		// Token: 0x0400CCFD RID: 52477
		[MayRequireAnomaly]
		public static JobDef ActivateMonolith;

		// Token: 0x0400CCFE RID: 52478
		[MayRequireAnomaly]
		public static JobDef FillIn;

		// Token: 0x0400CCFF RID: 52479
		[MayRequireAnomaly]
		public static JobDef ChimeraSwitchToAttackMode;

		// Token: 0x0400CD00 RID: 52480
		[MayRequireAnomaly]
		public static JobDef DevourerDigest;

		// Token: 0x0400CD01 RID: 52481
		[MayRequireAnomaly]
		public static JobDef UnnaturalCorpseAttack;

		// Token: 0x0400CD02 RID: 52482
		[MayRequireOdyssey]
		public static JobDef Deactivated;

		// Token: 0x0400CD03 RID: 52483
		[MayRequireOdyssey]
		public static JobDef SelfDetonate;

		// Token: 0x0400CD04 RID: 52484
		[MayRequireOdyssey]
		public static JobDef PutApparelOnOutfitStand;
	}
}
