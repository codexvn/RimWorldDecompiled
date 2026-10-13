using System;

namespace RimWorld
{
	// Token: 0x020035B9 RID: 13753
	[DefOf]
	public static class InteractionDefOf
	{
		// Token: 0x060133A7 RID: 78759 RVA: 0x005B5CA2 File Offset: 0x005B3EA2
		static InteractionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InteractionDefOf));
		}

		// Token: 0x0400D17A RID: 53626
		public static InteractionDef Chitchat;

		// Token: 0x0400D17B RID: 53627
		public static InteractionDef DeepTalk;

		// Token: 0x0400D17C RID: 53628
		public static InteractionDef Insult;

		// Token: 0x0400D17D RID: 53629
		public static InteractionDef RomanceAttempt;

		// Token: 0x0400D17E RID: 53630
		public static InteractionDef MarriageProposal;

		// Token: 0x0400D17F RID: 53631
		public static InteractionDef BuildRapport;

		// Token: 0x0400D180 RID: 53632
		public static InteractionDef RecruitAttempt;

		// Token: 0x0400D181 RID: 53633
		public static InteractionDef SparkJailbreak;

		// Token: 0x0400D182 RID: 53634
		[MayRequireIdeology]
		public static InteractionDef ReduceWill;

		// Token: 0x0400D183 RID: 53635
		[MayRequireIdeology]
		public static InteractionDef EnslaveAttempt;

		// Token: 0x0400D184 RID: 53636
		[MayRequireIdeology]
		public static InteractionDef ConvertIdeoAttempt;

		// Token: 0x0400D185 RID: 53637
		[MayRequireAnomaly]
		public static InteractionDef PrisonerStudyAnomaly;

		// Token: 0x0400D186 RID: 53638
		[MayRequireAnomaly]
		public static InteractionDef InterrogateIdentity;

		// Token: 0x0400D187 RID: 53639
		[MayRequireIdeology]
		public static InteractionDef Suppress;

		// Token: 0x0400D188 RID: 53640
		[MayRequireIdeology]
		public static InteractionDef SparkSlaveRebellion;

		// Token: 0x0400D189 RID: 53641
		public static InteractionDef AnimalChat;

		// Token: 0x0400D18A RID: 53642
		public static InteractionDef TrainAttempt;

		// Token: 0x0400D18B RID: 53643
		public static InteractionDef TameAttempt;

		// Token: 0x0400D18C RID: 53644
		public static InteractionDef Nuzzle;

		// Token: 0x0400D18D RID: 53645
		public static InteractionDef ReleaseToWild;

		// Token: 0x0400D18E RID: 53646
		[MayRequireIdeology]
		public static InteractionDef Counsel_Success;

		// Token: 0x0400D18F RID: 53647
		[MayRequireIdeology]
		public static InteractionDef Counsel_Failure;

		// Token: 0x0400D190 RID: 53648
		[MayRequireIdeology]
		public static InteractionDef Convert_Success;

		// Token: 0x0400D191 RID: 53649
		[MayRequireIdeology]
		public static InteractionDef Convert_Failure;

		// Token: 0x0400D192 RID: 53650
		[MayRequireIdeology]
		public static InteractionDef Reassure;

		// Token: 0x0400D193 RID: 53651
		[MayRequireIdeology]
		public static InteractionDef Trial_Accuse;

		// Token: 0x0400D194 RID: 53652
		[MayRequireIdeology]
		public static InteractionDef Trial_Defend;

		// Token: 0x0400D195 RID: 53653
		[MayRequireIdeology]
		public static InteractionDef Speech_AcceptRole;

		// Token: 0x0400D196 RID: 53654
		[MayRequireIdeology]
		public static InteractionDef Speech_RemoveRole;

		// Token: 0x0400D197 RID: 53655
		[MayRequireBiotech]
		public static InteractionDef BabyPlay;

		// Token: 0x0400D198 RID: 53656
		[MayRequireBiotech]
		public static InteractionDef LessonGeneric;

		// Token: 0x0400D199 RID: 53657
		[MayRequireAnomaly]
		public static InteractionDef InhumanRambling;

		// Token: 0x0400D19A RID: 53658
		[MayRequireAnomaly]
		public static InteractionDef OccultTeaching;

		// Token: 0x0400D19B RID: 53659
		[MayRequireAnomaly]
		public static InteractionDef DisturbingChat;

		// Token: 0x0400D19C RID: 53660
		[MayRequireAnomaly]
		public static InteractionDef CreepyWords;
	}
}
