using System;

namespace RimWorld
{
	// Token: 0x020035F1 RID: 13809
	[DefOf]
	public static class HistoryEventDefOf
	{
		// Token: 0x060133DE RID: 78814 RVA: 0x005B6049 File Offset: 0x005B4249
		static HistoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HistoryEventDefOf));
		}

		// Token: 0x0400D419 RID: 54297
		public static HistoryEventDef QuestGoodwillReward;

		// Token: 0x0400D41A RID: 54298
		public static HistoryEventDef DebugGoodwill;

		// Token: 0x0400D41B RID: 54299
		public static HistoryEventDef RequestedTrader;

		// Token: 0x0400D41C RID: 54300
		public static HistoryEventDef RequestedOrbitalTrader;

		// Token: 0x0400D41D RID: 54301
		public static HistoryEventDef RequestedMilitaryAid;

		// Token: 0x0400D41E RID: 54302
		public static HistoryEventDef PeaceTalksDisaster;

		// Token: 0x0400D41F RID: 54303
		public static HistoryEventDef PeaceTalksBackfire;

		// Token: 0x0400D420 RID: 54304
		public static HistoryEventDef PeaceTalksSuccess;

		// Token: 0x0400D421 RID: 54305
		public static HistoryEventDef PeaceTalksTriumph;

		// Token: 0x0400D422 RID: 54306
		public static HistoryEventDef QuestPawnLost;

		// Token: 0x0400D423 RID: 54307
		public static HistoryEventDef QuestPawnArrested;

		// Token: 0x0400D424 RID: 54308
		public static HistoryEventDef UsedHarmfulAbility;

		// Token: 0x0400D425 RID: 54309
		public static HistoryEventDef UsedHarmfulItem;

		// Token: 0x0400D426 RID: 54310
		public static HistoryEventDef GaveGift;

		// Token: 0x0400D427 RID: 54311
		public static HistoryEventDef SettlementProximity;

		// Token: 0x0400D428 RID: 54312
		public static HistoryEventDef DestroyedEnemyBase;

		// Token: 0x0400D429 RID: 54313
		public static HistoryEventDef AttackedSettlement;

		// Token: 0x0400D42A RID: 54314
		public static HistoryEventDef PerformedHarmfulSurgery;

		// Token: 0x0400D42B RID: 54315
		public static HistoryEventDef AttackedMember;

		// Token: 0x0400D42C RID: 54316
		public static HistoryEventDef AttackedBuilding;

		// Token: 0x0400D42D RID: 54317
		public static HistoryEventDef MemberCrushed;

		// Token: 0x0400D42E RID: 54318
		public static HistoryEventDef MemberNeutrallyDied;

		// Token: 0x0400D42F RID: 54319
		public static HistoryEventDef Traded;

		// Token: 0x0400D430 RID: 54320
		public static HistoryEventDef MemberStripped;

		// Token: 0x0400D431 RID: 54321
		public static HistoryEventDef MemberExitedMapHealthy;

		// Token: 0x0400D432 RID: 54322
		public static HistoryEventDef FriendlyExitedMapTended;

		// Token: 0x0400D433 RID: 54323
		public static HistoryEventDef UsedForbiddenThing;

		// Token: 0x0400D434 RID: 54324
		public static HistoryEventDef AttackedCaravan;

		// Token: 0x0400D435 RID: 54325
		public static HistoryEventDef MemberCaptured;

		// Token: 0x0400D436 RID: 54326
		public static HistoryEventDef MemberKilled;

		// Token: 0x0400D437 RID: 54327
		public static HistoryEventDef MemberSold;

		// Token: 0x0400D438 RID: 54328
		public static HistoryEventDef ShuttleDestroyed;

		// Token: 0x0400D439 RID: 54329
		public static HistoryEventDef ReachNaturalGoodwill;

		// Token: 0x0400D43A RID: 54330
		public static HistoryEventDef ShuttleGuardsMissedShuttle;

		// Token: 0x0400D43B RID: 54331
		public static HistoryEventDef ShuttleCommanderMissedShuttle;

		// Token: 0x0400D43C RID: 54332
		public static HistoryEventDef RitualDone;

		// Token: 0x0400D43D RID: 54333
		[MayRequireBiotech]
		public static HistoryEventDef PollutedNearbySite;

		// Token: 0x0400D43E RID: 54334
		[MayRequireBiotech]
		public static HistoryEventDef PollutedBase;

		// Token: 0x0400D43F RID: 54335
		[MayRequireBiotech]
		public static HistoryEventDef AbsorbedXenogerm;

		// Token: 0x0400D440 RID: 54336
		[MayRequireBiotech]
		public static HistoryEventDef ExtractedHemogenPack;

		// Token: 0x0400D441 RID: 54337
		[MayRequireBiotech]
		public static HistoryEventDef PropagateBloodfeederGene;

		// Token: 0x0400D442 RID: 54338
		[MayRequireBiotech]
		public static HistoryEventDef ToxicWasteDumping;

		// Token: 0x0400D443 RID: 54339
		[MayRequireBiotech]
		public static HistoryEventDef BecomeNonPreferredXenotype;

		// Token: 0x0400D444 RID: 54340
		[MayRequireOdyssey]
		public static HistoryEventDef OrbitalPollution;

		// Token: 0x0400D445 RID: 54341
		[MayRequireOdyssey]
		public static HistoryEventDef PawnKidnappedOnGravShip;

		// Token: 0x0400D446 RID: 54342
		public static HistoryEventDef ObservedLayingRottingCorpse;

		// Token: 0x0400D447 RID: 54343
		public static HistoryEventDef ObservedLayingCorpse;

		// Token: 0x0400D448 RID: 54344
		public static HistoryEventDef SoldSlave;

		// Token: 0x0400D449 RID: 54345
		public static HistoryEventDef ExecutedPrisoner;

		// Token: 0x0400D44A RID: 54346
		public static HistoryEventDef ExecutedGuest;

		// Token: 0x0400D44B RID: 54347
		public static HistoryEventDef ExecutedColonist;

		// Token: 0x0400D44C RID: 54348
		public static HistoryEventDef ExecutedPrisonerGuilty;

		// Token: 0x0400D44D RID: 54349
		public static HistoryEventDef ExecutedPrisonerInnocent;

		// Token: 0x0400D44E RID: 54350
		[MayRequireIdeology]
		public static HistoryEventDef HarvestedOrgan;

		// Token: 0x0400D44F RID: 54351
		public static HistoryEventDef HarvestedOrganFromColonist;

		// Token: 0x0400D450 RID: 54352
		public static HistoryEventDef HarvestedOrganFromGuest;

		// Token: 0x0400D451 RID: 54353
		[MayRequireIdeology]
		public static HistoryEventDef TradedOrgan;

		// Token: 0x0400D452 RID: 54354
		[MayRequireIdeology]
		public static HistoryEventDef SoldOrgan;

		// Token: 0x0400D453 RID: 54355
		[MayRequireIdeology]
		public static HistoryEventDef InstalledOrgan;

		// Token: 0x0400D454 RID: 54356
		public static HistoryEventDef IngestedDrug;

		// Token: 0x0400D455 RID: 54357
		public static HistoryEventDef IngestedRecreationalDrug;

		// Token: 0x0400D456 RID: 54358
		public static HistoryEventDef IngestedHardDrug;

		// Token: 0x0400D457 RID: 54359
		public static HistoryEventDef AdministeredDrug;

		// Token: 0x0400D458 RID: 54360
		public static HistoryEventDef AdministeredRecreationalDrug;

		// Token: 0x0400D459 RID: 54361
		public static HistoryEventDef AdministeredHardDrug;

		// Token: 0x0400D45A RID: 54362
		public static HistoryEventDef AteHumanMeat;

		// Token: 0x0400D45B RID: 54363
		public static HistoryEventDef AteHumanMeatDirect;

		// Token: 0x0400D45C RID: 54364
		public static HistoryEventDef AteHumanMeatAsIngredient;

		// Token: 0x0400D45D RID: 54365
		[MayRequireIdeology]
		public static HistoryEventDef AteNonCannibalFood;

		// Token: 0x0400D45E RID: 54366
		[MayRequireIdeology]
		public static HistoryEventDef AteVeneratedAnimalMeat;

		// Token: 0x0400D45F RID: 54367
		[MayRequireIdeology]
		public static HistoryEventDef BuiltAutomatedTurret;

		// Token: 0x0400D460 RID: 54368
		public static HistoryEventDef BuildSpecificDef;

		// Token: 0x0400D461 RID: 54369
		public static HistoryEventDef Researching;

		// Token: 0x0400D462 RID: 54370
		public static HistoryEventDef InstalledProsthetic;

		// Token: 0x0400D463 RID: 54371
		[MayRequireIdeology]
		public static HistoryEventDef SlaughteredAnimal;

		// Token: 0x0400D464 RID: 54372
		[MayRequireIdeology]
		public static HistoryEventDef KilledInnocentAnimal;

		// Token: 0x0400D465 RID: 54373
		[MayRequireIdeology]
		public static HistoryEventDef SlaughteredVeneratedAnimal;

		// Token: 0x0400D466 RID: 54374
		[MayRequireIdeology]
		public static HistoryEventDef HuntedVeneratedAnimal;

		// Token: 0x0400D467 RID: 54375
		public static HistoryEventDef CutTree;

		// Token: 0x0400D468 RID: 54376
		public static HistoryEventDef MinifiedTreeDied;

		// Token: 0x0400D469 RID: 54377
		public static HistoryEventDef Mined;

		// Token: 0x0400D46A RID: 54378
		[MayRequireIdeology]
		public static HistoryEventDef AteMeat;

		// Token: 0x0400D46B RID: 54379
		[MayRequireIdeology]
		public static HistoryEventDef AteNonMeat;

		// Token: 0x0400D46C RID: 54380
		public static HistoryEventDef DestroyedMineable;

		// Token: 0x0400D46D RID: 54381
		public static HistoryEventDef Bonded;

		// Token: 0x0400D46E RID: 54382
		public static HistoryEventDef InitiatedLovin;

		// Token: 0x0400D46F RID: 54383
		public static HistoryEventDef GotLovin;

		// Token: 0x0400D470 RID: 54384
		public static HistoryEventDef GotLovin_Spouse;

		// Token: 0x0400D471 RID: 54385
		public static HistoryEventDef GotLovin_NonSpouse;

		// Token: 0x0400D472 RID: 54386
		public static HistoryEventDef SharedBed;

		// Token: 0x0400D473 RID: 54387
		public static HistoryEventDef SharedBed_Spouse;

		// Token: 0x0400D474 RID: 54388
		public static HistoryEventDef SharedBed_NonSpouse;

		// Token: 0x0400D475 RID: 54389
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Beggars;

		// Token: 0x0400D476 RID: 54390
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Beggars_Betrayed;

		// Token: 0x0400D477 RID: 54391
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_WandererJoins;

		// Token: 0x0400D478 RID: 54392
		public static HistoryEventDef ButcheredHuman;

		// Token: 0x0400D479 RID: 54393
		public static HistoryEventDef SowedPlant;

		// Token: 0x0400D47A RID: 54394
		public static HistoryEventDef SowedHumanFoodPlant;

		// Token: 0x0400D47B RID: 54395
		[MayRequireIdeology]
		public static HistoryEventDef AteFungus;

		// Token: 0x0400D47C RID: 54396
		[MayRequireIdeology]
		public static HistoryEventDef AteFungusAsIngredient;

		// Token: 0x0400D47D RID: 54397
		[MayRequireIdeology]
		public static HistoryEventDef AteNonFungusPlant;

		// Token: 0x0400D47E RID: 54398
		[MayRequireIdeology]
		public static HistoryEventDef AteNonFungusMealWithPlants;

		// Token: 0x0400D47F RID: 54399
		[MayRequireIdeology]
		public static HistoryEventDef CharityFulfilled_Beggars;

		// Token: 0x0400D480 RID: 54400
		[MayRequireIdeology]
		public static HistoryEventDef CharityFulfilled_WandererJoins;

		// Token: 0x0400D481 RID: 54401
		public static HistoryEventDef InnocentPrisonerDied;

		// Token: 0x0400D482 RID: 54402
		public static HistoryEventDef GuiltyPrisonerDied;

		// Token: 0x0400D483 RID: 54403
		public static HistoryEventDef PrisonerDied;

		// Token: 0x0400D484 RID: 54404
		public static HistoryEventDef GotMarried_TookWomansName;

		// Token: 0x0400D485 RID: 54405
		public static HistoryEventDef GotMarried_TookMansName;

		// Token: 0x0400D486 RID: 54406
		public static HistoryEventDef GotMarried_KeptName;

		// Token: 0x0400D487 RID: 54407
		public static HistoryEventDef GotMarried_SpouseCount_OneOrFewer;

		// Token: 0x0400D488 RID: 54408
		public static HistoryEventDef GotMarried_SpouseCount_Two;

		// Token: 0x0400D489 RID: 54409
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_Three;

		// Token: 0x0400D48A RID: 54410
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_Four;

		// Token: 0x0400D48B RID: 54411
		[MayRequireIdeology]
		public static HistoryEventDef GotMarried_SpouseCount_FiveOrMore;

		// Token: 0x0400D48C RID: 54412
		[MayRequireIdeology]
		public static HistoryEventDef ChangedIdeo;

		// Token: 0x0400D48D RID: 54413
		[MayRequireIdeology]
		public static HistoryEventDef EnslavedPrisoner;

		// Token: 0x0400D48E RID: 54414
		[MayRequireIdeology]
		public static HistoryEventDef EnslavedPrisonerNotPreviouslyEnslaved;

		// Token: 0x0400D48F RID: 54415
		[MayRequireIdeology]
		public static HistoryEventDef Raided;

		// Token: 0x0400D490 RID: 54416
		[MayRequireIdeology]
		public static HistoryEventDef PlayerRaidedSomeone;

		// Token: 0x0400D491 RID: 54417
		[MayRequireIdeology]
		public static HistoryEventDef RelicDestroyed;

		// Token: 0x0400D492 RID: 54418
		[MayRequireIdeology]
		public static HistoryEventDef RelicLost;

		// Token: 0x0400D493 RID: 54419
		[MayRequireIdeology]
		public static HistoryEventDef KillWithNobleWeapon;

		// Token: 0x0400D494 RID: 54420
		[MayRequireIdeology]
		public static HistoryEventDef UsedNobleWeapon;

		// Token: 0x0400D495 RID: 54421
		[MayRequireIdeology]
		public static HistoryEventDef KillWithDespisedWeapon;

		// Token: 0x0400D496 RID: 54422
		[MayRequireIdeology]
		public static HistoryEventDef UsedDespisedWeapon;

		// Token: 0x0400D497 RID: 54423
		public static HistoryEventDef AteInsectMeatDirect;

		// Token: 0x0400D498 RID: 54424
		public static HistoryEventDef AteInsectMeatAsIngredient;

		// Token: 0x0400D499 RID: 54425
		[MayRequireIdeology]
		public static HistoryEventDef GotScarified;

		// Token: 0x0400D49A RID: 54426
		[MayRequireIdeology]
		public static HistoryEventDef GotBlinded;

		// Token: 0x0400D49B RID: 54427
		[MayRequireIdeology]
		public static HistoryEventDef UsedBiosculpterPod;

		// Token: 0x0400D49C RID: 54428
		[MayRequireIdeology]
		public static HistoryEventDef PlantedGauranlenTree;

		// Token: 0x0400D49D RID: 54429
		[MayRequireIdeology]
		public static HistoryEventDef ConvertedNewMember;

		// Token: 0x0400D49E RID: 54430
		[MayRequireIdeology]
		public static HistoryEventDef ParticipatedInOthersRitual;

		// Token: 0x0400D49F RID: 54431
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Pilgrims;

		// Token: 0x0400D4A0 RID: 54432
		[MayRequireIdeology]
		public static HistoryEventDef CharityRefused_Pilgrims_Betrayed;

		// Token: 0x0400D4A1 RID: 54433
		[MayRequireAnomaly]
		public static HistoryEventDef PsychicRitualPerformed;

		// Token: 0x0400D4A2 RID: 54434
		[MayRequireAnomaly]
		public static HistoryEventDef InvolvedInPsychicRitual;

		// Token: 0x0400D4A3 RID: 54435
		[MayRequireAnomaly]
		public static HistoryEventDef WasPsychicRitualTarget;

		// Token: 0x0400D4A4 RID: 54436
		[MayRequireAnomaly]
		public static HistoryEventDef GhoulInfusionPerformed;

		// Token: 0x0400D4A5 RID: 54437
		[MayRequireAnomaly]
		public static HistoryEventDef DestroyedVoidMonolith;

		// Token: 0x0400D4A6 RID: 54438
		[MayRequireOdyssey]
		public static HistoryEventDef SlaughteredFish;

		// Token: 0x0400D4A7 RID: 54439
		public static HistoryEventDef CharityRefused_ThreatReward_Joiner;
	}
}
