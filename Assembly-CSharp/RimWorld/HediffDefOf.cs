using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003596 RID: 13718
	[DefOf]
	public static class HediffDefOf
	{
		// Token: 0x06013384 RID: 78724 RVA: 0x005B5A4F File Offset: 0x005B3C4F
		static HediffDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
		}

		// Token: 0x0400CDC6 RID: 52678
		public static HediffDef Cut;

		// Token: 0x0400CDC7 RID: 52679
		public static HediffDef SurgicalCut;

		// Token: 0x0400CDC8 RID: 52680
		public static HediffDef ExecutionCut;

		// Token: 0x0400CDC9 RID: 52681
		public static HediffDef Bite;

		// Token: 0x0400CDCA RID: 52682
		public static HediffDef MissingBodyPart;

		// Token: 0x0400CDCB RID: 52683
		public static HediffDef Misc;

		// Token: 0x0400CDCC RID: 52684
		[MayRequireBiotech]
		public static HediffDef BloodfeederMark;

		// Token: 0x0400CDCD RID: 52685
		[MayRequireAnomaly]
		public static HediffDef Decayed;

		// Token: 0x0400CDCE RID: 52686
		[MayRequireAnomaly]
		public static HediffDef Digested;

		// Token: 0x0400CDCF RID: 52687
		[MayRequireOdyssey]
		public static HediffDef PorcupineQuill;

		// Token: 0x0400CDD0 RID: 52688
		public static HediffDef BloodLoss;

		// Token: 0x0400CDD1 RID: 52689
		public static HediffDef Hypothermia;

		// Token: 0x0400CDD2 RID: 52690
		public static HediffDef Heatstroke;

		// Token: 0x0400CDD3 RID: 52691
		public static HediffDef Malnutrition;

		// Token: 0x0400CDD4 RID: 52692
		public static HediffDef ToxicBuildup;

		// Token: 0x0400CDD5 RID: 52693
		public static HediffDef PsychicShock;

		// Token: 0x0400CDD6 RID: 52694
		public static HediffDef ResurrectionSickness;

		// Token: 0x0400CDD7 RID: 52695
		public static HediffDef ResurrectionPsychosis;

		// Token: 0x0400CDD8 RID: 52696
		[MayRequireBiotech]
		public static HediffDef PollutionStimulus;

		// Token: 0x0400CDD9 RID: 52697
		[MayRequireBiotech]
		public static HediffDef ToxGasExposure;

		// Token: 0x0400CDDA RID: 52698
		[MayRequireAnomaly]
		public static HediffDef Shambler;

		// Token: 0x0400CDDB RID: 52699
		[MayRequireAnomaly]
		public static HediffDef ShamblerCorpse;

		// Token: 0x0400CDDC RID: 52700
		[MayRequireAnomaly]
		public static HediffDef Rising;

		// Token: 0x0400CDDD RID: 52701
		[MayRequireAnomaly]
		public static HediffDef DeathRefusal;

		// Token: 0x0400CDDE RID: 52702
		[MayRequireAnomaly]
		public static HediffDef DeathRefusalSickness;

		// Token: 0x0400CDDF RID: 52703
		[MayRequireAnomaly]
		public static HediffDef MetalhorrorImplant;

		// Token: 0x0400CDE0 RID: 52704
		[MayRequireAnomaly]
		public static HediffDef MetalhorrorSpeedBoost;

		// Token: 0x0400CDE1 RID: 52705
		[MayRequireAnomaly]
		public static HediffDef PsychicTrance;

		// Token: 0x0400CDE2 RID: 52706
		[MayRequireAnomaly]
		public static HediffDef DuplicateSickness;

		// Token: 0x0400CDE3 RID: 52707
		[MayRequireAnomaly]
		public static HediffDef VoidTouched;

		// Token: 0x0400CDE4 RID: 52708
		[MayRequireAnomaly]
		public static HediffDef MeatHunger;

		// Token: 0x0400CDE5 RID: 52709
		[MayRequireAnomaly]
		public static HediffDef ShardHolder;

		// Token: 0x0400CDE6 RID: 52710
		[MayRequireAnomaly]
		public static HediffDef Inhumanized;

		// Token: 0x0400CDE7 RID: 52711
		[MayRequireAnomaly]
		public static HediffDef DarknessExposure;

		// Token: 0x0400CDE8 RID: 52712
		[MayRequireAnomaly]
		public static HediffDef LightExposure;

		// Token: 0x0400CDE9 RID: 52713
		[MayRequireAnomaly]
		public static HediffDef DisruptorFlash;

		// Token: 0x0400CDEA RID: 52714
		[MayRequireOdyssey]
		public static HediffDef VacuumExposure;

		// Token: 0x0400CDEB RID: 52715
		[MayRequireOdyssey]
		public static HediffDef PsilocapHigh;

		// Token: 0x0400CDEC RID: 52716
		[MayRequireOdyssey]
		public static HediffDef GravNausea;

		// Token: 0x0400CDED RID: 52717
		[MayRequireOdyssey]
		public static HediffDef VolcanicAsh;

		// Token: 0x0400CDEE RID: 52718
		public static HediffDef Anesthetic;

		// Token: 0x0400CDEF RID: 52719
		public static HediffDef CryptosleepSickness;

		// Token: 0x0400CDF0 RID: 52720
		public static HediffDef FoodPoisoning;

		// Token: 0x0400CDF1 RID: 52721
		public static HediffDef Pregnant;

		// Token: 0x0400CDF2 RID: 52722
		[MayRequireBiotech]
		public static HediffDef PregnantHuman;

		// Token: 0x0400CDF3 RID: 52723
		[MayRequireBiotech]
		public static HediffDef MorningSickness;

		// Token: 0x0400CDF4 RID: 52724
		[MayRequireBiotech]
		public static HediffDef PregnancyMood;

		// Token: 0x0400CDF5 RID: 52725
		[MayRequireBiotech]
		public static HediffDef PregnancyLabor;

		// Token: 0x0400CDF6 RID: 52726
		[MayRequireBiotech]
		public static HediffDef PregnancyLaborPushing;

		// Token: 0x0400CDF7 RID: 52727
		[MayRequireBiotech]
		public static HediffDef OvumExtracted;

		// Token: 0x0400CDF8 RID: 52728
		[MayRequireBiotech]
		public static HediffDef VatLearning;

		// Token: 0x0400CDF9 RID: 52729
		[MayRequireBiotech]
		public static HediffDef VatGrowing;

		// Token: 0x0400CDFA RID: 52730
		public static HediffDef CatatonicBreakdown;

		// Token: 0x0400CDFB RID: 52731
		public static HediffDef Scaria;

		// Token: 0x0400CDFC RID: 52732
		[MayRequireIdeology]
		public static HediffDef Scarification;

		// Token: 0x0400CDFD RID: 52733
		[MayRequireIdeology]
		public static HediffDef NeuralSupercharge;

		// Token: 0x0400CDFE RID: 52734
		[MayRequireIdeology]
		public static HediffDef BiosculptingSickness;

		// Token: 0x0400CDFF RID: 52735
		[MayRequireRoyalty]
		public static HediffDef PsychicEntropy;

		// Token: 0x0400CE00 RID: 52736
		[MayRequireRoyalty]
		public static HediffDef PsychicHangover;

		// Token: 0x0400CE01 RID: 52737
		[MayRequireRoyalty]
		public static HediffDef PsychicSuppression;

		// Token: 0x0400CE02 RID: 52738
		[MayRequireRoyalty]
		public static HediffDef Abasia;

		// Token: 0x0400CE03 RID: 52739
		[MayRequireRoyalty]
		public static HediffDef PsychicLove;

		// Token: 0x0400CE04 RID: 52740
		public static HediffDef Sterilized;

		// Token: 0x0400CE05 RID: 52741
		[MayRequireBiotech]
		public static HediffDef Vasectomy;

		// Token: 0x0400CE06 RID: 52742
		[MayRequireBiotech]
		public static HediffDef ImplantedIUD;

		// Token: 0x0400CE07 RID: 52743
		[MayRequireIdeology]
		public static HediffDef WorkFocus;

		// Token: 0x0400CE08 RID: 52744
		[MayRequireBiotech]
		public static HediffDef ScanningSickness;

		// Token: 0x0400CE09 RID: 52745
		[MayRequireBiotech]
		public static HediffDef PsychicBond;

		// Token: 0x0400CE0A RID: 52746
		[MayRequireBiotech]
		public static HediffDef PsychicBondTorn;

		// Token: 0x0400CE0B RID: 52747
		public static HediffDef LungRot;

		// Token: 0x0400CE0C RID: 52748
		public static HediffDef LungRotExposure;

		// Token: 0x0400CE0D RID: 52749
		public static HediffDef CoveredInFirefoam;

		// Token: 0x0400CE0E RID: 52750
		[MayRequireAnomaly]
		public static HediffDef BioferriteExtracted;

		// Token: 0x0400CE0F RID: 52751
		public static HediffDef Plague;

		// Token: 0x0400CE10 RID: 52752
		public static HediffDef WoundInfection;

		// Token: 0x0400CE11 RID: 52753
		public static HediffDef ScariaInfection;

		// Token: 0x0400CE12 RID: 52754
		public static HediffDef AlcoholHigh;

		// Token: 0x0400CE13 RID: 52755
		public static HediffDef Hangover;

		// Token: 0x0400CE14 RID: 52756
		public static HediffDef DrugOverdose;

		// Token: 0x0400CE15 RID: 52757
		public static HediffDef WakeUpTolerance;

		// Token: 0x0400CE16 RID: 52758
		public static HediffDef GoJuiceTolerance;

		// Token: 0x0400CE17 RID: 52759
		public static HediffDef Blindness;

		// Token: 0x0400CE18 RID: 52760
		public static HediffDef Carcinoma;

		// Token: 0x0400CE19 RID: 52761
		public static HediffDef Dementia;

		// Token: 0x0400CE1A RID: 52762
		public static HediffDef OrganDecay;

		// Token: 0x0400CE1B RID: 52763
		[MayRequireBiotech]
		public static HediffDef DetoxifierLung;

		// Token: 0x0400CE1C RID: 52764
		[MayRequireRoyalty]
		public static HediffDef LoveEnhancer;

		// Token: 0x0400CE1D RID: 52765
		[MayRequireRoyalty]
		public static HediffDef PsychicAmplifier;

		// Token: 0x0400CE1E RID: 52766
		[MayRequireRoyalty]
		public static HediffDef PsychicHarmonizer;

		// Token: 0x0400CE1F RID: 52767
		[MayRequireBiotech]
		public static HediffDef MechlinkImplant;

		// Token: 0x0400CE20 RID: 52768
		[MayRequireBiotech]
		public static HediffDef SelfShutdown;

		// Token: 0x0400CE21 RID: 52769
		[MayRequireBiotech]
		public static HediffDef InterruptedDeathrest;

		// Token: 0x0400CE22 RID: 52770
		[MayRequireBiotech]
		public static HediffDef InfantIllness;

		// Token: 0x0400CE23 RID: 52771
		[MayRequireBiotech]
		public static HediffDef Stillborn;

		// Token: 0x0400CE24 RID: 52772
		[MayRequireBiotech]
		public static HediffDef PostpartumExhaustion;

		// Token: 0x0400CE25 RID: 52773
		[MayRequireBiotech]
		public static HediffDef Lactating;

		// Token: 0x0400CE26 RID: 52774
		[MayRequireBiotech]
		public static HediffDef RegenerationComa;

		// Token: 0x0400CE27 RID: 52775
		[MayRequireBiotech]
		public static HediffDef XenogermLossShock;

		// Token: 0x0400CE28 RID: 52776
		[MayRequireBiotech]
		public static HediffDef XenogermReplicating;

		// Token: 0x0400CE29 RID: 52777
		[MayRequireBiotech]
		public static HediffDef GeneticDrugNeed;

		// Token: 0x0400CE2A RID: 52778
		[MayRequireBiotech]
		public static HediffDef BioStarvation;

		// Token: 0x0400CE2B RID: 52779
		[MayRequireBiotech]
		public static HediffDef XenogerminationComa;

		// Token: 0x0400CE2C RID: 52780
		[MayRequireBiotech]
		public static HediffDef Deathrest;

		// Token: 0x0400CE2D RID: 52781
		[MayRequireBiotech]
		public static HediffDef DeathrestExhaustion;

		// Token: 0x0400CE2E RID: 52782
		[MayRequireBiotech]
		public static HediffDef BandNode;

		// Token: 0x0400CE2F RID: 52783
		[MayRequireBiotech]
		public static HediffDef HemogenCraving;

		// Token: 0x0400CE30 RID: 52784
		[MayRequireAnomaly]
		public static HediffDef HoraxianInvisibility;

		// Token: 0x0400CE31 RID: 52785
		[MayRequireAnomaly]
		public static HediffDef RevenantHypnosis;

		// Token: 0x0400CE32 RID: 52786
		[MayRequireAnomaly]
		public static HediffDef AwokenHypnosis;

		// Token: 0x0400CE33 RID: 52787
		[MayRequireAnomaly]
		public static HediffDef DarkPsychicShock;

		// Token: 0x0400CE34 RID: 52788
		[MayRequireAnomaly]
		public static HediffDef Tentacle;

		// Token: 0x0400CE35 RID: 52789
		[MayRequireAnomaly]
		public static HediffDef FleshWhip;

		// Token: 0x0400CE36 RID: 52790
		[MayRequireAnomaly]
		public static HediffDef OrganDecayUndiagnosedDuplicaton;

		// Token: 0x0400CE37 RID: 52791
		[MayRequireAnomaly]
		public static HediffDef CrumblingMindUndiagnosedDuplication;

		// Token: 0x0400CE38 RID: 52792
		[MayRequireAnomaly]
		public static HediffDef OrganDecayCreepjoiner;

		// Token: 0x0400CE39 RID: 52793
		[MayRequireAnomaly]
		public static HediffDef CrumblingMind;

		// Token: 0x0400CE3A RID: 52794
		[MayRequireAnomaly]
		public static HediffDef CrumbledMind;

		// Token: 0x0400CE3B RID: 52795
		[MayRequireAnomaly]
		public static HediffDef BlissLobotomy;

		// Token: 0x0400CE3C RID: 52796
		[MayRequireAnomaly]
		public static HediffDef PleasurePulse;

		// Token: 0x0400CE3D RID: 52797
		[MayRequireAnomaly]
		public static HediffDef NeurosisPulse;

		// Token: 0x0400CE3E RID: 52798
		[MayRequireAnomaly]
		public static HediffDef BloodRage;

		// Token: 0x0400CE3F RID: 52799
		[MayRequireAnomaly]
		public static HediffDef FrenzyField;

		// Token: 0x0400CE40 RID: 52800
		[MayRequireAnomaly]
		public static HediffDef AgonyPulse;

		// Token: 0x0400CE41 RID: 52801
		[MayRequireAnomaly]
		public static HediffDef CubeInterest;

		// Token: 0x0400CE42 RID: 52802
		[MayRequireAnomaly]
		public static HediffDef CubeWithdrawal;

		// Token: 0x0400CE43 RID: 52803
		[MayRequireAnomaly]
		public static HediffDef CubeRage;

		// Token: 0x0400CE44 RID: 52804
		[MayRequireAnomaly]
		public static HediffDef CubeComa;

		// Token: 0x0400CE45 RID: 52805
		[MayRequireAnomaly]
		public static HediffDef BrainwipeComa;

		// Token: 0x0400CE46 RID: 52806
		[MayRequireAnomaly]
		public static HediffDef CorpseTorment;

		// Token: 0x0400CE47 RID: 52807
		[MayRequireAnomaly]
		public static HediffDef PsychicallyDead;

		// Token: 0x0400CE48 RID: 52808
		[MayRequireAnomaly]
		public static HediffDef Psychophage;

		// Token: 0x0400CE49 RID: 52809
		[MayRequireAnomaly]
		public static HediffDef SleepSuppression;

		// Token: 0x0400CE4A RID: 52810
		[MayRequireAnomaly]
		public static HediffDef RageSpeed;

		// Token: 0x0400CE4B RID: 52811
		[MayRequireAnomaly]
		public static HediffDef AwokenCorpse;

		// Token: 0x0400CE4C RID: 52812
		[MayRequireAnomaly]
		public static HediffDef RapidRegeneration;

		// Token: 0x0400CE4D RID: 52813
		[MayRequireAnomaly]
		public static HediffDef GhoulBarbs;

		// Token: 0x0400CE4E RID: 52814
		[MayRequireAnomaly]
		public static HediffDef GhoulPlating;

		// Token: 0x0400CE4F RID: 52815
		[MayRequireAnomaly]
		public static HediffDef AdrenalHeart;

		// Token: 0x0400CE50 RID: 52816
		[MayRequireAnomaly]
		public static HediffDef FleshmassStomach;

		// Token: 0x0400CE51 RID: 52817
		[MayRequireAnomaly]
		public static HediffDef FleshmassLung;

		// Token: 0x0400CE52 RID: 52818
		[MayRequireAnomaly]
		public static HediffDef Metalblood;

		// Token: 0x0400CE53 RID: 52819
		[MayRequireOdyssey]
		public static HediffDef SentienceCatalyst;
	}
}
