using System;

namespace RimWorld
{
	// Token: 0x020035B7 RID: 13751
	[DefOf]
	public static class RecordDefOf
	{
		// Token: 0x060133A5 RID: 78757 RVA: 0x005B5C80 File Offset: 0x005B3E80
		static RecordDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
		}

		// Token: 0x0400D134 RID: 53556
		public static RecordDef Kills;

		// Token: 0x0400D135 RID: 53557
		public static RecordDef KillsHumanlikes;

		// Token: 0x0400D136 RID: 53558
		public static RecordDef KillsAnimals;

		// Token: 0x0400D137 RID: 53559
		public static RecordDef KillsMechanoids;

		// Token: 0x0400D138 RID: 53560
		[MayRequireAnomaly]
		public static RecordDef KillsEntities;

		// Token: 0x0400D139 RID: 53561
		public static RecordDef PawnsDowned;

		// Token: 0x0400D13A RID: 53562
		public static RecordDef PawnsDownedHumanlikes;

		// Token: 0x0400D13B RID: 53563
		[MayRequireAnomaly]
		public static RecordDef PawnsDownedEntities;

		// Token: 0x0400D13C RID: 53564
		public static RecordDef PawnsDownedAnimals;

		// Token: 0x0400D13D RID: 53565
		public static RecordDef PawnsDownedMechanoids;

		// Token: 0x0400D13E RID: 53566
		public static RecordDef ShotsFired;

		// Token: 0x0400D13F RID: 53567
		public static RecordDef Headshots;

		// Token: 0x0400D140 RID: 53568
		public static RecordDef DamageDealt;

		// Token: 0x0400D141 RID: 53569
		public static RecordDef DamageTaken;

		// Token: 0x0400D142 RID: 53570
		public static RecordDef TimesInMentalState;

		// Token: 0x0400D143 RID: 53571
		public static RecordDef TimesOnFire;

		// Token: 0x0400D144 RID: 53572
		public static RecordDef FiresExtinguished;

		// Token: 0x0400D145 RID: 53573
		public static RecordDef OperationsReceived;

		// Token: 0x0400D146 RID: 53574
		public static RecordDef OperationsPerformed;

		// Token: 0x0400D147 RID: 53575
		public static RecordDef TimesTendedTo;

		// Token: 0x0400D148 RID: 53576
		public static RecordDef TimesTendedOther;

		// Token: 0x0400D149 RID: 53577
		public static RecordDef PeopleCaptured;

		// Token: 0x0400D14A RID: 53578
		public static RecordDef PrisonersRecruited;

		// Token: 0x0400D14B RID: 53579
		public static RecordDef PrisonersChatted;

		// Token: 0x0400D14C RID: 53580
		public static RecordDef AnimalsTamed;

		// Token: 0x0400D14D RID: 53581
		public static RecordDef AnimalsSlaughtered;

		// Token: 0x0400D14E RID: 53582
		public static RecordDef MealsCooked;

		// Token: 0x0400D14F RID: 53583
		public static RecordDef ThingsConstructed;

		// Token: 0x0400D150 RID: 53584
		public static RecordDef ThingsInstalled;

		// Token: 0x0400D151 RID: 53585
		public static RecordDef ThingsRepaired;

		// Token: 0x0400D152 RID: 53586
		public static RecordDef ThingsCrafted;

		// Token: 0x0400D153 RID: 53587
		public static RecordDef ThingsHauled;

		// Token: 0x0400D154 RID: 53588
		public static RecordDef PlantsSown;

		// Token: 0x0400D155 RID: 53589
		public static RecordDef PlantsHarvested;

		// Token: 0x0400D156 RID: 53590
		public static RecordDef CellsMined;

		// Token: 0x0400D157 RID: 53591
		public static RecordDef MessesCleaned;

		// Token: 0x0400D158 RID: 53592
		public static RecordDef ResearchPointsResearched;

		// Token: 0x0400D159 RID: 53593
		public static RecordDef CorpsesBuried;

		// Token: 0x0400D15A RID: 53594
		public static RecordDef NutritionEaten;

		// Token: 0x0400D15B RID: 53595
		public static RecordDef BodiesStripped;

		// Token: 0x0400D15C RID: 53596
		public static RecordDef ThingsUninstalled;

		// Token: 0x0400D15D RID: 53597
		public static RecordDef ThingsDeconstructed;

		// Token: 0x0400D15E RID: 53598
		public static RecordDef ArtifactsActivated;

		// Token: 0x0400D15F RID: 53599
		public static RecordDef ContainersOpened;

		// Token: 0x0400D160 RID: 53600
		public static RecordDef SwitchesFlicked;

		// Token: 0x0400D161 RID: 53601
		public static RecordDef TimeAsColonistOrColonyAnimal;

		// Token: 0x0400D162 RID: 53602
		public static RecordDef TimeAsPrisoner;

		// Token: 0x0400D163 RID: 53603
		public static RecordDef TimeAsQuestLodger;

		// Token: 0x0400D164 RID: 53604
		[MayRequireBiotech]
		public static RecordDef TimeAsChildInColony;
	}
}
