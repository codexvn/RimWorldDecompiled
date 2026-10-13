using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200359F RID: 13727
	[DefOf]
	public static class DutyDefOf
	{
		// Token: 0x0601338D RID: 78733 RVA: 0x005B5AE8 File Offset: 0x005B3CE8
		static DutyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
		}

		// Token: 0x0400CE8C RID: 52876
		public static DutyDef TravelOrLeave;

		// Token: 0x0400CE8D RID: 52877
		public static DutyDef TravelOrWait;

		// Token: 0x0400CE8E RID: 52878
		public static DutyDef Kidnap;

		// Token: 0x0400CE8F RID: 52879
		public static DutyDef Steal;

		// Token: 0x0400CE90 RID: 52880
		public static DutyDef TakeWoundedGuest;

		// Token: 0x0400CE91 RID: 52881
		public static DutyDef Follow;

		// Token: 0x0400CE92 RID: 52882
		public static DutyDef PrisonerEscape;

		// Token: 0x0400CE93 RID: 52883
		public static DutyDef PrisonerEscapeSapper;

		// Token: 0x0400CE94 RID: 52884
		public static DutyDef DefendAndExpandHive;

		// Token: 0x0400CE95 RID: 52885
		public static DutyDef DefendHiveAggressively;

		// Token: 0x0400CE96 RID: 52886
		public static DutyDef LoadAndEnterTransporters;

		// Token: 0x0400CE97 RID: 52887
		public static DutyDef EnterTransporterAndDefendSelf;

		// Token: 0x0400CE98 RID: 52888
		public static DutyDef LoadAndEnterPortal;

		// Token: 0x0400CE99 RID: 52889
		public static DutyDef ManClosestTurret;

		// Token: 0x0400CE9A RID: 52890
		public static DutyDef SleepForever;

		// Token: 0x0400CE9B RID: 52891
		public static DutyDef Idle;

		// Token: 0x0400CE9C RID: 52892
		public static DutyDef IdleNoInteraction;

		// Token: 0x0400CE9D RID: 52893
		public static DutyDef WanderClose;

		// Token: 0x0400CE9E RID: 52894
		public static DutyDef WanderClose_NoNeeds;

		// Token: 0x0400CE9F RID: 52895
		[MayRequireAnomaly]
		public static DutyDef ShamblerSwarm;

		// Token: 0x0400CEA0 RID: 52896
		[MayRequireAnomaly]
		public static DutyDef SightstealerSwarm;

		// Token: 0x0400CEA1 RID: 52897
		[MayRequireAnomaly]
		public static DutyDef SightstealerAssault;

		// Token: 0x0400CEA2 RID: 52898
		[MayRequireAnomaly]
		public static DutyDef GorehulkAssault;

		// Token: 0x0400CEA3 RID: 52899
		[MayRequireAnomaly]
		public static DutyDef DevourerAssault;

		// Token: 0x0400CEA4 RID: 52900
		[MayRequireAnomaly]
		public static DutyDef FleshbeastAssault;

		// Token: 0x0400CEA5 RID: 52901
		[MayRequireAnomaly]
		public static DutyDef PerformHateChant;

		// Token: 0x0400CEA6 RID: 52902
		[MayRequireAnomaly]
		public static DutyDef ChimeraStalkFlee;

		// Token: 0x0400CEA7 RID: 52903
		[MayRequireAnomaly]
		public static DutyDef ChimeraStalkWander;

		// Token: 0x0400CEA8 RID: 52904
		[MayRequireAnomaly]
		public static DutyDef ChimeraAttack;

		// Token: 0x0400CEA9 RID: 52905
		[MayRequireAnomaly]
		public static DutyDef DefendFleshmassHeart;

		// Token: 0x0400CEAA RID: 52906
		[MayRequireAnomaly]
		public static DutyDef VoidAwakeningWander;

		// Token: 0x0400CEAB RID: 52907
		[MayRequireOdyssey]
		public static DutyDef WanderNest;

		// Token: 0x0400CEAC RID: 52908
		[MayRequireOdyssey]
		public static DutyDef NestAssault;

		// Token: 0x0400CEAD RID: 52909
		public static DutyDef AssaultColony;

		// Token: 0x0400CEAE RID: 52910
		public static DutyDef Breaching;

		// Token: 0x0400CEAF RID: 52911
		public static DutyDef Sapper;

		// Token: 0x0400CEB0 RID: 52912
		public static DutyDef Escort;

		// Token: 0x0400CEB1 RID: 52913
		public static DutyDef Defend;

		// Token: 0x0400CEB2 RID: 52914
		[MayRequireAnomaly]
		public static DutyDef DefendInvoker;

		// Token: 0x0400CEB3 RID: 52915
		public static DutyDef Build;

		// Token: 0x0400CEB4 RID: 52916
		public static DutyDef HuntEnemiesIndividual;

		// Token: 0x0400CEB5 RID: 52917
		public static DutyDef DefendBase;

		// Token: 0x0400CEB6 RID: 52918
		[MayRequireRoyalty]
		public static DutyDef AssaultThing;

		// Token: 0x0400CEB7 RID: 52919
		public static DutyDef PrisonerAssaultColony;

		// Token: 0x0400CEB8 RID: 52920
		public static DutyDef HuntDownColonists;

		// Token: 0x0400CEB9 RID: 52921
		public static DutyDef ExitMapRandom;

		// Token: 0x0400CEBA RID: 52922
		public static DutyDef ExitMapBest;

		// Token: 0x0400CEBB RID: 52923
		public static DutyDef ExitMapBestAndDefendSelf;

		// Token: 0x0400CEBC RID: 52924
		public static DutyDef ExitMapNearDutyTarget;

		// Token: 0x0400CEBD RID: 52925
		public static DutyDef MarryPawn;

		// Token: 0x0400CEBE RID: 52926
		public static DutyDef GiveSpeech;

		// Token: 0x0400CEBF RID: 52927
		public static DutyDef Spectate;

		// Token: 0x0400CEC0 RID: 52928
		public static DutyDef BestowingCeremony_MoveInPlace;

		// Token: 0x0400CEC1 RID: 52929
		[MayRequireRoyalty]
		public static DutyDef Bestow;

		// Token: 0x0400CEC2 RID: 52930
		[MayRequireIdeology]
		public static DutyDef Pilgrims_Spectate;

		// Token: 0x0400CEC3 RID: 52931
		[MayRequireIdeology]
		public static DutyDef PlayTargetInstrument;

		// Token: 0x0400CEC4 RID: 52932
		[MayRequireBiotech]
		public static DutyDef SocialMeeting;

		// Token: 0x0400CEC5 RID: 52933
		public static DutyDef PrepareCaravan_GatherItems;

		// Token: 0x0400CEC6 RID: 52934
		public static DutyDef PrepareCaravan_Wait;

		// Token: 0x0400CEC7 RID: 52935
		public static DutyDef PrepareCaravan_GatherAnimals;

		// Token: 0x0400CEC8 RID: 52936
		public static DutyDef PrepareCaravan_CollectAnimals;

		// Token: 0x0400CEC9 RID: 52937
		public static DutyDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x0400CECA RID: 52938
		public static DutyDef PrepareCaravan_Pause;

		// Token: 0x0400CECB RID: 52939
		public static DutyDef ReturnedCaravan_PenAnimals;

		// Token: 0x0400CECC RID: 52940
		[MayRequireAnomaly]
		public static DutyDef Goto;

		// Token: 0x0400CECD RID: 52941
		[MayRequireAnomaly]
		public static DutyDef Goto_NoZeroLengthPaths;

		// Token: 0x0400CECE RID: 52942
		[MayRequireAnomaly]
		public static DutyDef Invoke;

		// Token: 0x0400CECF RID: 52943
		[MayRequireAnomaly]
		public static DutyDef PsychicRitualDance;

		// Token: 0x0400CED0 RID: 52944
		[MayRequireAnomaly]
		public static DutyDef WaitForRitualParticipants;

		// Token: 0x0400CED1 RID: 52945
		[MayRequireAnomaly]
		public static DutyDef DeliverPawnToPsychicRitualCell;

		// Token: 0x0400CED2 RID: 52946
		[MayRequireAnomaly]
		public static DutyDef GatherOfferingsForPsychicRitual;
	}
}
