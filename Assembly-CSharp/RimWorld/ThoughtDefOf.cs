using System;

namespace RimWorld
{
	// Token: 0x020035B2 RID: 13746
	[DefOf]
	public static class ThoughtDefOf
	{
		// Token: 0x060133A0 RID: 78752 RVA: 0x005B5C2B File Offset: 0x005B3E2B
		static ThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
		}

		// Token: 0x0400CF94 RID: 53140
		public static ThoughtDef AteRawFood;

		// Token: 0x0400CF95 RID: 53141
		public static ThoughtDef AteCorpse;

		// Token: 0x0400CF96 RID: 53142
		public static ThoughtDef AteRottenFood;

		// Token: 0x0400CF97 RID: 53143
		[MayRequireRoyalty]
		public static ThoughtDef AteFoodInappropriateForTitle;

		// Token: 0x0400CF98 RID: 53144
		public static ThoughtDef SleptInBedroom;

		// Token: 0x0400CF99 RID: 53145
		public static ThoughtDef SleptInBarracks;

		// Token: 0x0400CF9A RID: 53146
		public static ThoughtDef SleptOutside;

		// Token: 0x0400CF9B RID: 53147
		public static ThoughtDef SleptOnGround;

		// Token: 0x0400CF9C RID: 53148
		public static ThoughtDef SleptInCold;

		// Token: 0x0400CF9D RID: 53149
		public static ThoughtDef SleptInHeat;

		// Token: 0x0400CF9E RID: 53150
		public static ThoughtDef SleepDisturbed;

		// Token: 0x0400CF9F RID: 53151
		[MayRequireIdeology]
		public static ThoughtDef SleptInRoomWithSlave;

		// Token: 0x0400CFA0 RID: 53152
		[MayRequireRoyalty]
		public static ThoughtDef ReignedInThroneroom;

		// Token: 0x0400CFA1 RID: 53153
		[MayRequireIdeology]
		public static ThoughtDef ObservedTerror;

		// Token: 0x0400CFA2 RID: 53154
		public static ThoughtDef WasImprisoned;

		// Token: 0x0400CFA3 RID: 53155
		public static ThoughtDef NewColonyOptimism;

		// Token: 0x0400CFA4 RID: 53156
		public static ThoughtDef NewColonyHope;

		// Token: 0x0400CFA5 RID: 53157
		public static ThoughtDef AteWithoutTable;

		// Token: 0x0400CFA6 RID: 53158
		public static ThoughtDef AteInImpressiveDiningRoom;

		// Token: 0x0400CFA7 RID: 53159
		public static ThoughtDef JoyActivityInImpressiveRecRoom;

		// Token: 0x0400CFA8 RID: 53160
		public static ThoughtDef Catharsis;

		// Token: 0x0400CFA9 RID: 53161
		public static ThoughtDef MyOrganHarvested;

		// Token: 0x0400CFAA RID: 53162
		public static ThoughtDef HarvestedOrgan_Bloodlust;

		// Token: 0x0400CFAB RID: 53163
		public static ThoughtDef KnowColonistDied;

		// Token: 0x0400CFAC RID: 53164
		public static ThoughtDef KnowPrisonerDiedInnocent;

		// Token: 0x0400CFAD RID: 53165
		public static ThoughtDef FreedFromSlavery;

		// Token: 0x0400CFAE RID: 53166
		public static ThoughtDef ReleasedHealthyPrisoner;

		// Token: 0x0400CFAF RID: 53167
		public static ThoughtDef SoakingWet;

		// Token: 0x0400CFB0 RID: 53168
		public static ThoughtDef SoldMyBondedAnimalMood;

		// Token: 0x0400CFB1 RID: 53169
		public static ThoughtDef WitnessedDeathNonAlly;

		// Token: 0x0400CFB2 RID: 53170
		public static ThoughtDef WitnessedDeathAlly;

		// Token: 0x0400CFB3 RID: 53171
		public static ThoughtDef WitnessedDeathBloodlust;

		// Token: 0x0400CFB4 RID: 53172
		public static ThoughtDef WitnessedDeathFamily;

		// Token: 0x0400CFB5 RID: 53173
		public static ThoughtDef KilledHumanlikeBloodlust;

		// Token: 0x0400CFB6 RID: 53174
		public static ThoughtDef Nuzzled;

		// Token: 0x0400CFB7 RID: 53175
		public static ThoughtDef ArtifactMoodBoost;

		// Token: 0x0400CFB8 RID: 53176
		public static ThoughtDef KnowBuriedInSarcophagus;

		// Token: 0x0400CFB9 RID: 53177
		public static ThoughtDef DebugGood;

		// Token: 0x0400CFBA RID: 53178
		public static ThoughtDef DebugBad;

		// Token: 0x0400CFBB RID: 53179
		public static ThoughtDef DeniedJoining;

		// Token: 0x0400CFBC RID: 53180
		public static ThoughtDef ColonistBanished;

		// Token: 0x0400CFBD RID: 53181
		public static ThoughtDef ColonistBanishedToDie;

		// Token: 0x0400CFBE RID: 53182
		public static ThoughtDef PrisonerBanishedToDie;

		// Token: 0x0400CFBF RID: 53183
		public static ThoughtDef ColonyPrisonerEscaped;

		// Token: 0x0400CFC0 RID: 53184
		public static ThoughtDef BondedAnimalBanished;

		// Token: 0x0400CFC1 RID: 53185
		public static ThoughtDef BondedAnimalReleased;

		// Token: 0x0400CFC2 RID: 53186
		public static ThoughtDef FailedToRescueRelative;

		// Token: 0x0400CFC3 RID: 53187
		public static ThoughtDef RescuedRelative;

		// Token: 0x0400CFC4 RID: 53188
		public static ThoughtDef Rescued;

		// Token: 0x0400CFC5 RID: 53189
		public static ThoughtDef PyroUsed;

		// Token: 0x0400CFC6 RID: 53190
		[MayRequireRoyalty]
		public static ThoughtDef DecreeMet;

		// Token: 0x0400CFC7 RID: 53191
		[MayRequireRoyalty]
		public static ThoughtDef JealousRage;

		// Token: 0x0400CFC8 RID: 53192
		[MayRequireIdeology]
		public static ThoughtDef FailedConvertIdeoAttemptResentment;

		// Token: 0x0400CFC9 RID: 53193
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingMissing;

		// Token: 0x0400CFCA RID: 53194
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingDisrespected;

		// Token: 0x0400CFCB RID: 53195
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleLost;

		// Token: 0x0400CFCC RID: 53196
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleEmpty;

		// Token: 0x0400CFCD RID: 53197
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleApparelRequirementNotMet;

		// Token: 0x0400CFCE RID: 53198
		[MayRequireIdeology]
		public static ThoughtDef Counselled;

		// Token: 0x0400CFCF RID: 53199
		[MayRequireIdeology]
		public static ThoughtDef Counselled_MoodBoost;

		// Token: 0x0400CFD0 RID: 53200
		[MayRequireIdeology]
		public static ThoughtDef ObservedGibbetCage;

		// Token: 0x0400CFD1 RID: 53201
		[MayRequireIdeology]
		public static ThoughtDef ObservedSkullspike;

		// Token: 0x0400CFD2 RID: 53202
		[MayRequireIdeology]
		public static ThoughtDef RelicDestroyed;

		// Token: 0x0400CFD3 RID: 53203
		[MayRequireIdeology]
		public static ThoughtDef RelicLost;

		// Token: 0x0400CFD4 RID: 53204
		[MayRequireIdeology]
		public static ThoughtDef RelicAtRitual;

		// Token: 0x0400CFD5 RID: 53205
		[MayRequireIdeology]
		public static ThoughtDef WasEnslaved;

		// Token: 0x0400CFD6 RID: 53206
		[MayRequireIdeology]
		public static ThoughtDef TameVeneratedAnimalDied;

		// Token: 0x0400CFD7 RID: 53207
		[MayRequireIdeology]
		public static ThoughtDef ConnectedTreeDied;

		// Token: 0x0400CFD8 RID: 53208
		[MayRequireIdeology]
		public static ThoughtDef RelicsCollected;

		// Token: 0x0400CFD9 RID: 53209
		[MayRequireIdeology]
		public static ThoughtDef BiosculpterPleasure;

		// Token: 0x0400CFDA RID: 53210
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalDemanded;

		// Token: 0x0400CFDB RID: 53211
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalReceived;

		// Token: 0x0400CFDC RID: 53212
		[MayRequireIdeology]
		public static ThoughtDef DryadDied;

		// Token: 0x0400CFDD RID: 53213
		[MayRequireBiotech]
		public static ThoughtDef PregnancyTerminated;

		// Token: 0x0400CFDE RID: 53214
		[MayRequireBiotech]
		public static ThoughtDef PregnancyEnded;

		// Token: 0x0400CFDF RID: 53215
		[MayRequireBiotech]
		public static ThoughtDef Stillbirth;

		// Token: 0x0400CFE0 RID: 53216
		[MayRequireBiotech]
		public static ThoughtDef Miscarried;

		// Token: 0x0400CFE1 RID: 53217
		[MayRequireBiotech]
		public static ThoughtDef PartnerMiscarried;

		// Token: 0x0400CFE2 RID: 53218
		[MayRequireBiotech]
		public static ThoughtDef BabyBorn;

		// Token: 0x0400CFE3 RID: 53219
		[MayRequireBiotech]
		public static ThoughtDef CryingBaby;

		// Token: 0x0400CFE4 RID: 53220
		[MayRequireBiotech]
		public static ThoughtDef MyCryingBaby;

		// Token: 0x0400CFE5 RID: 53221
		[MayRequireBiotech]
		public static ThoughtDef GigglingBaby;

		// Token: 0x0400CFE6 RID: 53222
		[MayRequireBiotech]
		public static ThoughtDef MyGigglingBaby;

		// Token: 0x0400CFE7 RID: 53223
		[MayRequireBiotech]
		public static ThoughtDef BabyGiggledSocial;

		// Token: 0x0400CFE8 RID: 53224
		[MayRequireBiotech]
		public static ThoughtDef BabyCriedSocial;

		// Token: 0x0400CFE9 RID: 53225
		[MayRequireBiotech]
		public static ThoughtDef BreastfedMe;

		// Token: 0x0400CFEA RID: 53226
		[MayRequireBiotech]
		public static ThoughtDef BreastfedBaby;

		// Token: 0x0400CFEB RID: 53227
		[MayRequireBiotech]
		public static ThoughtDef FedMe;

		// Token: 0x0400CFEC RID: 53228
		[MayRequireBiotech]
		public static ThoughtDef FedBaby;

		// Token: 0x0400CFED RID: 53229
		[MayRequireBiotech]
		public static ThoughtDef PlayedWithMe;

		// Token: 0x0400CFEE RID: 53230
		[MayRequireBiotech]
		public static ThoughtDef PsychicBondTorn;

		// Token: 0x0400CFEF RID: 53231
		[MayRequireBiotech]
		public static ThoughtDef XenogermHarvested_Prisoner;

		// Token: 0x0400CFF0 RID: 53232
		[MayRequireAnomaly]
		public static ThoughtDef PsychicRitualVictim;

		// Token: 0x0400CFF1 RID: 53233
		[MayRequireAnomaly]
		public static ThoughtDef DarknessLifted;

		// Token: 0x0400CFF2 RID: 53234
		[MayRequireAnomaly]
		public static ThoughtDef PlayedWithCube;

		// Token: 0x0400CFF3 RID: 53235
		[MayRequireAnomaly]
		public static ThoughtDef DestroyedCubeSculpture;

		// Token: 0x0400CFF4 RID: 53236
		[MayRequireAnomaly]
		public static ThoughtDef UnnaturalCorpseDestroyed;

		// Token: 0x0400CFF5 RID: 53237
		[MayRequireAnomaly]
		public static ThoughtDef ObeliskAbduction;

		// Token: 0x0400CFF6 RID: 53238
		[MayRequireAnomaly]
		public static ThoughtDef EmbracedTheVoid;

		// Token: 0x0400CFF7 RID: 53239
		[MayRequireAnomaly]
		public static ThoughtDef ClosedTheVoid;

		// Token: 0x0400CFF8 RID: 53240
		[MayRequireAnomaly]
		public static ThoughtDef VoidClosed;

		// Token: 0x0400CFF9 RID: 53241
		[MayRequireAnomaly]
		public static ThoughtDef ClosedTheVoidOpinion;

		// Token: 0x0400CFFA RID: 53242
		[MayRequireAnomaly]
		public static ThoughtDef PsychicRitualGuilt;

		// Token: 0x0400CFFB RID: 53243
		[MayRequireAnomaly]
		public static ThoughtDef VoidCuriosity;

		// Token: 0x0400CFFC RID: 53244
		[MayRequireAnomaly]
		public static ThoughtDef VoidCuriositySatisfied;

		// Token: 0x0400CFFD RID: 53245
		[MayRequireOdyssey]
		public static ThoughtDef DestroyedMechhive;

		// Token: 0x0400CFFE RID: 53246
		public static ThoughtDef ColonistLost;

		// Token: 0x0400CFFF RID: 53247
		public static ThoughtDef PawnWithGoodOpinionLost;

		// Token: 0x0400D000 RID: 53248
		public static ThoughtDef PawnWithBadOpinionLost;

		// Token: 0x0400D001 RID: 53249
		public static ThoughtDef ApparelDamaged;

		// Token: 0x0400D002 RID: 53250
		public static ThoughtDef Naked;

		// Token: 0x0400D003 RID: 53251
		public static ThoughtDef ClothedNudist;

		// Token: 0x0400D004 RID: 53252
		public static ThoughtDef DeadMansApparel;

		// Token: 0x0400D005 RID: 53253
		public static ThoughtDef HumanLeatherApparelSad;

		// Token: 0x0400D006 RID: 53254
		public static ThoughtDef HumanLeatherApparelHappy;

		// Token: 0x0400D007 RID: 53255
		[MayRequireRoyalty]
		public static ThoughtDef DecreeUnmet;

		// Token: 0x0400D008 RID: 53256
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredRavenous;

		// Token: 0x0400D009 RID: 53257
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredStrong;

		// Token: 0x0400D00A RID: 53258
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_Preferred;

		// Token: 0x0400D00B RID: 53259
		[MayRequireIdeology]
		public static ThoughtDef TreesDesired;

		// Token: 0x0400D00C RID: 53260
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Desired;

		// Token: 0x0400D00D RID: 53261
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Disapproved;

		// Token: 0x0400D00E RID: 53262
		[MayRequireAnomaly]
		public static ThoughtDef HeardInhumanRambling;

		// Token: 0x0400D00F RID: 53263
		[MayRequireAnomaly]
		public static ThoughtDef Chimera;

		// Token: 0x0400D010 RID: 53264
		public static ThoughtDef PawnWithGoodOpinionDied;

		// Token: 0x0400D011 RID: 53265
		public static ThoughtDef PawnWithBadOpinionDied;

		// Token: 0x0400D012 RID: 53266
		public static ThoughtDef HadAngeringFight;

		// Token: 0x0400D013 RID: 53267
		public static ThoughtDef HadCatharticFight;

		// Token: 0x0400D014 RID: 53268
		public static ThoughtDef HarmedMe;

		// Token: 0x0400D015 RID: 53269
		public static ThoughtDef BotchedMySurgery;

		// Token: 0x0400D016 RID: 53270
		public static ThoughtDef CheatedOnMe;

		// Token: 0x0400D017 RID: 53271
		public static ThoughtDef RebuffedMyRomanceAttempt;

		// Token: 0x0400D018 RID: 53272
		public static ThoughtDef FailedRomanceAttemptOnMe;

		// Token: 0x0400D019 RID: 53273
		public static ThoughtDef FailedRomanceAttemptOnMeLowOpinionMood;

		// Token: 0x0400D01A RID: 53274
		public static ThoughtDef BrokeUpWithMe;

		// Token: 0x0400D01B RID: 53275
		public static ThoughtDef DivorcedMe;

		// Token: 0x0400D01C RID: 53276
		public static ThoughtDef RejectedMyProposal;

		// Token: 0x0400D01D RID: 53277
		public static ThoughtDef RejectedMyProposalMood;

		// Token: 0x0400D01E RID: 53278
		public static ThoughtDef IRejectedTheirProposal;

		// Token: 0x0400D01F RID: 53279
		public static ThoughtDef GotMarried;

		// Token: 0x0400D020 RID: 53280
		public static ThoughtDef HoneymoonPhase;

		// Token: 0x0400D021 RID: 53281
		public static ThoughtDef RescuedMe;

		// Token: 0x0400D022 RID: 53282
		public static ThoughtDef RescuedMeByOfferingHelp;

		// Token: 0x0400D023 RID: 53283
		public static ThoughtDef RecruitedMe;

		// Token: 0x0400D024 RID: 53284
		public static ThoughtDef AttendedWedding;

		// Token: 0x0400D025 RID: 53285
		public static ThoughtDef AttendedParty;

		// Token: 0x0400D026 RID: 53286
		public static ThoughtDef AttendedConcert;

		// Token: 0x0400D027 RID: 53287
		public static ThoughtDef HeldConcert;

		// Token: 0x0400D028 RID: 53288
		public static ThoughtDef EncouragingSpeech;

		// Token: 0x0400D029 RID: 53289
		public static ThoughtDef InspirationalSpeech;

		// Token: 0x0400D02A RID: 53290
		public static ThoughtDef CrashedTogether;

		// Token: 0x0400D02B RID: 53291
		public static ThoughtDef GotSomeLovin;

		// Token: 0x0400D02C RID: 53292
		public static ThoughtDef KilledMyFriend;

		// Token: 0x0400D02D RID: 53293
		public static ThoughtDef KilledMyRival;

		// Token: 0x0400D02E RID: 53294
		public static ThoughtDef DefeatedHostileFactionLeader;

		// Token: 0x0400D02F RID: 53295
		public static ThoughtDef DefeatedMechCluster;

		// Token: 0x0400D030 RID: 53296
		public static ThoughtDef DefeatedInsectHive;

		// Token: 0x0400D031 RID: 53297
		public static ThoughtDef ForcedMeToTakeDrugs;

		// Token: 0x0400D032 RID: 53298
		public static ThoughtDef ForcedMeToTakeLuciferium;

		// Token: 0x0400D033 RID: 53299
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerDied;

		// Token: 0x0400D034 RID: 53300
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerArrested;

		// Token: 0x0400D035 RID: 53301
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerSurgicallyViolated;

		// Token: 0x0400D036 RID: 53302
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerLeftBehind;

		// Token: 0x0400D037 RID: 53303
		[MayRequireRoyalty]
		public static ThoughtDef NeuroquakeEcho;

		// Token: 0x0400D038 RID: 53304
		[MayRequireIdeology]
		public static ThoughtDef TrialExonerated;

		// Token: 0x0400D039 RID: 53305
		[MayRequireIdeology]
		public static ThoughtDef TrialFailed;

		// Token: 0x0400D03A RID: 53306
		[MayRequireIdeology]
		public static ThoughtDef TrialConvicted;

		// Token: 0x0400D03B RID: 53307
		[MayRequireBiotech]
		public static ThoughtDef FedOn;

		// Token: 0x0400D03C RID: 53308
		[MayRequireBiotech]
		public static ThoughtDef FedOn_Social;

		// Token: 0x0400D03D RID: 53309
		[MayRequireBiotech]
		public static ThoughtDef WasTaught;

		// Token: 0x0400D03E RID: 53310
		[MayRequireBiotech]
		public static ThoughtDef GaveLesson;

		// Token: 0x0400D03F RID: 53311
		[MayRequireBiotech]
		public static ThoughtDef KilledChild;

		// Token: 0x0400D040 RID: 53312
		[MayRequireAnomaly]
		public static ThoughtDef DrainedMySkills;

		// Token: 0x0400D041 RID: 53313
		[MayRequireAnomaly]
		public static ThoughtDef UsedMeForPsychicRitual;

		// Token: 0x0400D042 RID: 53314
		[MayRequireOdyssey]
		public static ThoughtDef MadeStatueOfMe;
	}
}
