using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035BF RID: 13759
	[DefOf]
	public static class RulePackDefOf
	{
		// Token: 0x060133AD RID: 78765 RVA: 0x005B5D08 File Offset: 0x005B3F08
		static RulePackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RulePackDefOf));
		}

		// Token: 0x0400D23A RID: 53818
		public static RulePackDef Sentence_SocialFightStarted;

		// Token: 0x0400D23B RID: 53819
		public static RulePackDef Sentence_RomanceAttemptAccepted;

		// Token: 0x0400D23C RID: 53820
		public static RulePackDef Sentence_RomanceAttemptRejected;

		// Token: 0x0400D23D RID: 53821
		public static RulePackDef Sentence_MarriageProposalAccepted;

		// Token: 0x0400D23E RID: 53822
		public static RulePackDef Sentence_MarriageProposalRejected;

		// Token: 0x0400D23F RID: 53823
		public static RulePackDef Sentence_MarriageProposalRejectedBrokeUp;

		// Token: 0x0400D240 RID: 53824
		public static RulePackDef Sentence_RecruitAttemptAccepted;

		// Token: 0x0400D241 RID: 53825
		public static RulePackDef Sentence_RecruitAttemptRejected;

		// Token: 0x0400D242 RID: 53826
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptSuccess;

		// Token: 0x0400D243 RID: 53827
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFail;

		// Token: 0x0400D244 RID: 53828
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailSocialFight;

		// Token: 0x0400D245 RID: 53829
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailResentment;

		// Token: 0x0400D246 RID: 53830
		[MayRequireBiotech]
		public static RulePackDef NamerGenepack;

		// Token: 0x0400D247 RID: 53831
		[MayRequireBiotech]
		public static RulePackDef NamerXenotype;

		// Token: 0x0400D248 RID: 53832
		[MayRequireBiotech]
		public static RulePackDef GrowthMomentFlavor;

		// Token: 0x0400D249 RID: 53833
		public static RulePackDef ArtDescriptionRoot_HasTale;

		// Token: 0x0400D24A RID: 53834
		public static RulePackDef ArtDescriptionRoot_Taleless;

		// Token: 0x0400D24B RID: 53835
		public static RulePackDef ArtDescriptionUtility_Global;

		// Token: 0x0400D24C RID: 53836
		public static RulePackDef GlobalUtility;

		// Token: 0x0400D24D RID: 53837
		public static RulePackDef TalelessImages;

		// Token: 0x0400D24E RID: 53838
		public static RulePackDef NamerWorld;

		// Token: 0x0400D24F RID: 53839
		public static RulePackDef NamerTraderGeneral;

		// Token: 0x0400D250 RID: 53840
		public static RulePackDef NamerScenario;

		// Token: 0x0400D251 RID: 53841
		public static RulePackDef NamerQuestDefault;

		// Token: 0x0400D252 RID: 53842
		public static RulePackDef NamerArtSculpture;

		// Token: 0x0400D253 RID: 53843
		public static RulePackDef ArtDescription_Sculpture;

		// Token: 0x0400D254 RID: 53844
		public static RulePackDef NamerArtWeaponMelee;

		// Token: 0x0400D255 RID: 53845
		public static RulePackDef ArtDescription_WeaponMelee;

		// Token: 0x0400D256 RID: 53846
		public static RulePackDef NamerArtWeaponGun;

		// Token: 0x0400D257 RID: 53847
		public static RulePackDef ArtDescription_WeaponGun;

		// Token: 0x0400D258 RID: 53848
		public static RulePackDef NamerArtFurniture;

		// Token: 0x0400D259 RID: 53849
		public static RulePackDef ArtDescription_Furniture;

		// Token: 0x0400D25A RID: 53850
		public static RulePackDef NamerArtSarcophagusPlate;

		// Token: 0x0400D25B RID: 53851
		public static RulePackDef ArtDescription_SarcophagusPlate;

		// Token: 0x0400D25C RID: 53852
		public static RulePackDef SeedGenerator;

		// Token: 0x0400D25D RID: 53853
		public static RulePackDef Combat_RangedFire;

		// Token: 0x0400D25E RID: 53854
		public static RulePackDef Combat_RangedDamage;

		// Token: 0x0400D25F RID: 53855
		public static RulePackDef Combat_RangedDeflect;

		// Token: 0x0400D260 RID: 53856
		public static RulePackDef Combat_RangedMiss;

		// Token: 0x0400D261 RID: 53857
		public static RulePackDef Combat_ExplosionImpact;

		// Token: 0x0400D262 RID: 53858
		public static RulePackDef Transition_Downed;

		// Token: 0x0400D263 RID: 53859
		public static RulePackDef Transition_Died;

		// Token: 0x0400D264 RID: 53860
		public static RulePackDef Transition_DiedExplosive;

		// Token: 0x0400D265 RID: 53861
		public static RulePackDef DamageEvent_Ceiling;

		// Token: 0x0400D266 RID: 53862
		public static RulePackDef DamageEvent_Fire;

		// Token: 0x0400D267 RID: 53863
		public static RulePackDef DamageEvent_PowerBeam;

		// Token: 0x0400D268 RID: 53864
		public static RulePackDef DamageEvent_Tornado;

		// Token: 0x0400D269 RID: 53865
		public static RulePackDef DamageEvent_TrapSpike;

		// Token: 0x0400D26A RID: 53866
		[MayRequireAnomaly]
		public static RulePackDef DamageEvent_UnnaturalDarkness;

		// Token: 0x0400D26B RID: 53867
		public static RulePackDef Event_Stun;

		// Token: 0x0400D26C RID: 53868
		public static RulePackDef Event_AbilityUsed;

		// Token: 0x0400D26D RID: 53869
		public static RulePackDef Event_ItemUsed;

		// Token: 0x0400D26E RID: 53870
		public static RulePackDef Battle_Solo;

		// Token: 0x0400D26F RID: 53871
		public static RulePackDef Battle_Duel;

		// Token: 0x0400D270 RID: 53872
		public static RulePackDef Battle_Internal;

		// Token: 0x0400D271 RID: 53873
		public static RulePackDef Battle_War;

		// Token: 0x0400D272 RID: 53874
		public static RulePackDef Battle_Brawl;

		// Token: 0x0400D273 RID: 53875
		public static RulePackDef DynamicWrapper;

		// Token: 0x0400D274 RID: 53876
		[MayRequireAnomaly]
		public static RulePackDef LabyrinthRamblings;

		// Token: 0x0400D275 RID: 53877
		[MayRequireAnomaly]
		public static RulePackDef NamerBiosignature;

		// Token: 0x0400D276 RID: 53878
		[MayRequireAnomaly]
		public static RulePackDef NamerArtCubeSculpture;

		// Token: 0x0400D277 RID: 53879
		[MayRequireAnomaly]
		public static RulePackDef ArtDescription_CubeSculpture;

		// Token: 0x0400D278 RID: 53880
		[MayRequireAnomaly]
		public static RulePackDef NamerArtVoidSculpture;

		// Token: 0x0400D279 RID: 53881
		[MayRequireAnomaly]
		public static RulePackDef ArtDescription_VoidSculpture;

		// Token: 0x0400D27A RID: 53882
		[MayRequireAnomaly]
		public static RulePackDef RevenantNoises;

		// Token: 0x0400D27B RID: 53883
		[MayRequireAnomaly]
		public static RulePackDef NamerPersonCreepjoiner;

		// Token: 0x0400D27C RID: 53884
		[MayRequireAnomaly]
		public static RulePackDef Event_UnnaturalCorpseAttack;

		// Token: 0x0400D27D RID: 53885
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerConsumeLeap;

		// Token: 0x0400D27E RID: 53886
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerDigestionAborted;

		// Token: 0x0400D27F RID: 53887
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerDigestionCompleted;

		// Token: 0x0400D280 RID: 53888
		[MayRequireAnomaly]
		public static RulePackDef Event_MetalhorrorEmerged;

		// Token: 0x0400D281 RID: 53889
		[MayRequireAnomaly]
		public static RulePackDef Event_Hypnotized;

		// Token: 0x0400D282 RID: 53890
		[MayRequireOdyssey]
		public static RulePackDef NamerGravship;

		// Token: 0x0400D283 RID: 53891
		[MayRequireOdyssey]
		public static RulePackDef NamerUniqueWeapon;
	}
}
