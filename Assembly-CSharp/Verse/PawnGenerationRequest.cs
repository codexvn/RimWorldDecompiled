using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020007C5 RID: 1989
	public struct PawnGenerationRequest
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060031D9 RID: 12761 RVA: 0x001069D8 File Offset: 0x00104BD8
		// (set) Token: 0x060031DA RID: 12762 RVA: 0x001069FA File Offset: 0x00104BFA
		public PawnKindDef KindDef
		{
			get
			{
				if (this.PawnKindDefGetter != null)
				{
					return this.PawnKindDefGetter(this.ForcedXenotype);
				}
				return this.kindDefInner;
			}
			set
			{
				this.kindDefInner = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x00106A03 File Offset: 0x00104C03
		// (set) Token: 0x060031DC RID: 12764 RVA: 0x00106A0B File Offset: 0x00104C0B
		public PawnGenerationContext Context { readonly get; set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x00106A14 File Offset: 0x00104C14
		// (set) Token: 0x060031DE RID: 12766 RVA: 0x00106A1C File Offset: 0x00104C1C
		public Faction Faction { readonly get; set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x00106A25 File Offset: 0x00104C25
		// (set) Token: 0x060031E0 RID: 12768 RVA: 0x00106A2D File Offset: 0x00104C2D
		public PlanetTile Tile { readonly get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x00106A36 File Offset: 0x00104C36
		// (set) Token: 0x060031E2 RID: 12770 RVA: 0x00106A3E File Offset: 0x00104C3E
		public bool ForceGenerateNewPawn { readonly get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x00106A47 File Offset: 0x00104C47
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x00106A4F File Offset: 0x00104C4F
		public BodyTypeDef ForceBodyType { readonly get; set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x00106A58 File Offset: 0x00104C58
		// (set) Token: 0x060031E6 RID: 12774 RVA: 0x00106A60 File Offset: 0x00104C60
		public bool AllowDead { readonly get; set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00106A69 File Offset: 0x00104C69
		// (set) Token: 0x060031E8 RID: 12776 RVA: 0x00106A71 File Offset: 0x00104C71
		public bool AllowDowned { readonly get; set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x00106A7A File Offset: 0x00104C7A
		// (set) Token: 0x060031EA RID: 12778 RVA: 0x00106A82 File Offset: 0x00104C82
		public bool CanGeneratePawnRelations { readonly get; set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x00106A8B File Offset: 0x00104C8B
		// (set) Token: 0x060031EC RID: 12780 RVA: 0x00106A93 File Offset: 0x00104C93
		public bool MustBeCapableOfViolence { readonly get; set; }

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x00106A9C File Offset: 0x00104C9C
		// (set) Token: 0x060031EE RID: 12782 RVA: 0x00106AA4 File Offset: 0x00104CA4
		public float ColonistRelationChanceFactor { readonly get; set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x00106AAD File Offset: 0x00104CAD
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x00106AB5 File Offset: 0x00104CB5
		public bool ForceAddFreeWarmLayerIfNeeded { readonly get; set; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00106ABE File Offset: 0x00104CBE
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x00106AC6 File Offset: 0x00104CC6
		public bool AllowGay { readonly get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x00106ACF File Offset: 0x00104CCF
		// (set) Token: 0x060031F4 RID: 12788 RVA: 0x00106AD7 File Offset: 0x00104CD7
		public bool AllowPregnant { readonly get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x00106AE0 File Offset: 0x00104CE0
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x00106AE8 File Offset: 0x00104CE8
		public bool AllowFood { readonly get; set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x00106AF1 File Offset: 0x00104CF1
		// (set) Token: 0x060031F8 RID: 12792 RVA: 0x00106AF9 File Offset: 0x00104CF9
		public bool AllowAddictions { readonly get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x00106B02 File Offset: 0x00104D02
		// (set) Token: 0x060031FA RID: 12794 RVA: 0x00106B0A File Offset: 0x00104D0A
		public IEnumerable<TraitDef> ForcedTraits { readonly get; set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x00106B13 File Offset: 0x00104D13
		// (set) Token: 0x060031FC RID: 12796 RVA: 0x00106B1B File Offset: 0x00104D1B
		public IEnumerable<TraitDef> ProhibitedTraits { readonly get; set; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x00106B24 File Offset: 0x00104D24
		// (set) Token: 0x060031FE RID: 12798 RVA: 0x00106B2C File Offset: 0x00104D2C
		public bool Inhabitant { readonly get; set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x00106B35 File Offset: 0x00104D35
		// (set) Token: 0x06003200 RID: 12800 RVA: 0x00106B3D File Offset: 0x00104D3D
		public bool CertainlyBeenInCryptosleep { readonly get; set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x00106B46 File Offset: 0x00104D46
		// (set) Token: 0x06003202 RID: 12802 RVA: 0x00106B4E File Offset: 0x00104D4E
		public bool ForceRedressWorldPawnIfFormerColonist { readonly get; set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x00106B57 File Offset: 0x00104D57
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x00106B5F File Offset: 0x00104D5F
		public bool WorldPawnFactionDoesntMatter { readonly get; set; }

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00106B68 File Offset: 0x00104D68
		// (set) Token: 0x06003206 RID: 12806 RVA: 0x00106B70 File Offset: 0x00104D70
		public float BiocodeWeaponChance { readonly get; set; }

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x00106B79 File Offset: 0x00104D79
		// (set) Token: 0x06003208 RID: 12808 RVA: 0x00106B81 File Offset: 0x00104D81
		public float BiocodeApparelChance { readonly get; set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x00106B8A File Offset: 0x00104D8A
		// (set) Token: 0x0600320A RID: 12810 RVA: 0x00106B92 File Offset: 0x00104D92
		public Pawn ExtraPawnForExtraRelationChance { readonly get; set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x00106B9B File Offset: 0x00104D9B
		// (set) Token: 0x0600320C RID: 12812 RVA: 0x00106BA3 File Offset: 0x00104DA3
		public float RelationWithExtraPawnChanceFactor { readonly get; set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x00106BAC File Offset: 0x00104DAC
		// (set) Token: 0x0600320E RID: 12814 RVA: 0x00106BB4 File Offset: 0x00104DB4
		public Predicate<Pawn> RedressValidator { readonly get; set; }

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600320F RID: 12815 RVA: 0x00106BBD File Offset: 0x00104DBD
		// (set) Token: 0x06003210 RID: 12816 RVA: 0x00106BC5 File Offset: 0x00104DC5
		public bool DontGiveWeapon { readonly get; set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x00106BCE File Offset: 0x00104DCE
		// (set) Token: 0x06003212 RID: 12818 RVA: 0x00106BD6 File Offset: 0x00104DD6
		public bool OnlyUseForcedBackstories { readonly get; set; }

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003213 RID: 12819 RVA: 0x00106BDF File Offset: 0x00104DDF
		// (set) Token: 0x06003214 RID: 12820 RVA: 0x00106BE7 File Offset: 0x00104DE7
		public Predicate<Pawn> ValidatorPreGear { readonly get; set; }

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003215 RID: 12821 RVA: 0x00106BF0 File Offset: 0x00104DF0
		// (set) Token: 0x06003216 RID: 12822 RVA: 0x00106BF8 File Offset: 0x00104DF8
		public Predicate<Pawn> ValidatorPostGear { readonly get; set; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x00106C01 File Offset: 0x00104E01
		// (set) Token: 0x06003218 RID: 12824 RVA: 0x00106C09 File Offset: 0x00104E09
		public float? MinChanceToRedressWorldPawn { readonly get; set; }

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x00106C12 File Offset: 0x00104E12
		// (set) Token: 0x0600321A RID: 12826 RVA: 0x00106C1A File Offset: 0x00104E1A
		public float? FixedBiologicalAge { readonly get; set; }

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x00106C23 File Offset: 0x00104E23
		// (set) Token: 0x0600321C RID: 12828 RVA: 0x00106C2B File Offset: 0x00104E2B
		public float? FixedChronologicalAge { readonly get; set; }

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x00106C34 File Offset: 0x00104E34
		// (set) Token: 0x0600321E RID: 12830 RVA: 0x00106C3C File Offset: 0x00104E3C
		public Gender? FixedGender { readonly get; set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x00106C45 File Offset: 0x00104E45
		// (set) Token: 0x06003220 RID: 12832 RVA: 0x00106C4D File Offset: 0x00104E4D
		public string FixedLastName { readonly get; set; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x00106C56 File Offset: 0x00104E56
		// (set) Token: 0x06003222 RID: 12834 RVA: 0x00106C5E File Offset: 0x00104E5E
		public string FixedBirthName { readonly get; set; }

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003223 RID: 12835 RVA: 0x00106C67 File Offset: 0x00104E67
		// (set) Token: 0x06003224 RID: 12836 RVA: 0x00106C6F File Offset: 0x00104E6F
		public RoyalTitleDef FixedTitle { readonly get; set; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x00106C78 File Offset: 0x00104E78
		// (set) Token: 0x06003226 RID: 12838 RVA: 0x00106C80 File Offset: 0x00104E80
		public bool ForbidAnyTitle { readonly get; set; }

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x00106C89 File Offset: 0x00104E89
		// (set) Token: 0x06003228 RID: 12840 RVA: 0x00106C91 File Offset: 0x00104E91
		public Ideo FixedIdeo { readonly get; set; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x00106C9A File Offset: 0x00104E9A
		// (set) Token: 0x0600322A RID: 12842 RVA: 0x00106CA2 File Offset: 0x00104EA2
		public bool ForceNoIdeo { readonly get; set; }

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x00106CAB File Offset: 0x00104EAB
		// (set) Token: 0x0600322C RID: 12844 RVA: 0x00106CB3 File Offset: 0x00104EB3
		public bool ForceNoBackstory { readonly get; set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x00106CBC File Offset: 0x00104EBC
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x00106CC4 File Offset: 0x00104EC4
		public bool ForceDead { readonly get; set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x00106CCD File Offset: 0x00104ECD
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x00106CD5 File Offset: 0x00104ED5
		public List<GeneDef> ForcedXenogenes { readonly get; set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x00106CDE File Offset: 0x00104EDE
		// (set) Token: 0x06003232 RID: 12850 RVA: 0x00106CE6 File Offset: 0x00104EE6
		public List<GeneDef> ForcedEndogenes { readonly get; set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x00106CEF File Offset: 0x00104EEF
		// (set) Token: 0x06003234 RID: 12852 RVA: 0x00106CF7 File Offset: 0x00104EF7
		public XenotypeDef ForcedXenotype { readonly get; set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x00106D00 File Offset: 0x00104F00
		// (set) Token: 0x06003236 RID: 12854 RVA: 0x00106D08 File Offset: 0x00104F08
		public List<XenotypeDef> AllowedXenotypes { readonly get; set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x00106D11 File Offset: 0x00104F11
		// (set) Token: 0x06003238 RID: 12856 RVA: 0x00106D19 File Offset: 0x00104F19
		public float ForceBaselinerChance { readonly get; set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x00106D22 File Offset: 0x00104F22
		// (set) Token: 0x0600323A RID: 12858 RVA: 0x00106D2A File Offset: 0x00104F2A
		public CustomXenotype ForcedCustomXenotype { readonly get; set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x00106D33 File Offset: 0x00104F33
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x00106D3B File Offset: 0x00104F3B
		public DevelopmentalStage AllowedDevelopmentalStages { readonly get; set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x00106D44 File Offset: 0x00104F44
		// (set) Token: 0x0600323E RID: 12862 RVA: 0x00106D4C File Offset: 0x00104F4C
		public Func<XenotypeDef, PawnKindDef> PawnKindDefGetter { readonly get; set; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x00106D55 File Offset: 0x00104F55
		// (set) Token: 0x06003240 RID: 12864 RVA: 0x00106D5D File Offset: 0x00104F5D
		public FloatRange? ExcludeBiologicalAgeRange { readonly get; set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x00106D66 File Offset: 0x00104F66
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x00106D6E File Offset: 0x00104F6E
		public FloatRange? BiologicalAgeRange { readonly get; set; }

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x00106D77 File Offset: 0x00104F77
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x00106D7F File Offset: 0x00104F7F
		public bool ForceRecruitable { readonly get; set; }

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x00106D88 File Offset: 0x00104F88
		// (set) Token: 0x06003246 RID: 12870 RVA: 0x00106D90 File Offset: 0x00104F90
		public int MinimumAgeTraits { readonly get; set; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x00106D99 File Offset: 0x00104F99
		// (set) Token: 0x06003248 RID: 12872 RVA: 0x00106DA1 File Offset: 0x00104FA1
		public int MaximumAgeTraits { readonly get; set; }

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x00106DAA File Offset: 0x00104FAA
		// (set) Token: 0x0600324A RID: 12874 RVA: 0x00106DB2 File Offset: 0x00104FB2
		public bool ForceNoGear { readonly get; set; }

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x0600324B RID: 12875 RVA: 0x00106DBB File Offset: 0x00104FBB
		// (set) Token: 0x0600324C RID: 12876 RVA: 0x00106DC3 File Offset: 0x00104FC3
		public MutantDef ForcedMutant { readonly get; set; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x00106DCC File Offset: 0x00104FCC
		// (set) Token: 0x0600324E RID: 12878 RVA: 0x00106DD4 File Offset: 0x00104FD4
		public bool IsCreepJoiner { readonly get; set; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x00106DDD File Offset: 0x00104FDD
		// (set) Token: 0x06003250 RID: 12880 RVA: 0x00106DE5 File Offset: 0x00104FE5
		public bool DontGivePreArrivalPathway { readonly get; set; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x00106DEE File Offset: 0x00104FEE
		// (set) Token: 0x06003252 RID: 12882 RVA: 0x00106DF6 File Offset: 0x00104FF6
		public bool ForceNoIdeoGear { readonly get; set; }

		// Token: 0x06003253 RID: 12883 RVA: 0x00106E00 File Offset: 0x00105000
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, PlanetTile? tile = null, bool forceGenerateNewPawn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowPregnant = false, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, float biocodeApparelChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null, Ideo fixedIdeo = null, bool forceNoIdeo = false, bool forceNoBackstory = false, bool forbidAnyTitle = false, bool forceDead = false, List<GeneDef> forcedXenogenes = null, List<GeneDef> forcedEndogenes = null, XenotypeDef forcedXenotype = null, CustomXenotype forcedCustomXenotype = null, List<XenotypeDef> allowedXenotypes = null, float forceBaselinerChance = 0f, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, Func<XenotypeDef, PawnKindDef> pawnKindDefGetter = null, FloatRange? excludeBiologicalAgeRange = null, FloatRange? biologicalAgeRange = null, bool forceRecruitable = false, bool dontGiveWeapon = false, bool onlyUseForcedBackstories = false, int maximumAgeTraits = -1, int minimumAgeTraits = 0, bool forceNoGear = false)
		{
			this = default(PawnGenerationRequest);
			this._calledTheCorrectConstructor = true;
			this.KindDef = kind;
			this.Context = context;
			this.Tile = (tile ?? PlanetTile.Invalid);
			this.Faction = faction;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowPregnant = allowPregnant;
			this.AllowFood = allowFood;
			this.AllowAddictions = allowAddictions;
			this.ForcedTraits = forcedTraits;
			this.ProhibitedTraits = prohibitedTraits;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ExtraPawnForExtraRelationChance = extraPawnForExtraRelationChance;
			this.RelationWithExtraPawnChanceFactor = relationWithExtraPawnChanceFactor;
			this.BiocodeWeaponChance = biocodeWeaponChance;
			this.BiocodeApparelChance = biocodeApparelChance;
			this.ForceNoIdeo = forceNoIdeo;
			this.ForceNoBackstory = forceNoBackstory;
			this.ForbidAnyTitle = forbidAnyTitle;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedLastName = fixedLastName;
			this.FixedBirthName = fixedBirthName;
			this.FixedTitle = fixedTitle;
			this.FixedIdeo = fixedIdeo;
			this.ForceDead = forceDead;
			this.ForcedXenotype = forcedXenotype;
			this.ForcedCustomXenotype = forcedCustomXenotype;
			this.AllowedXenotypes = allowedXenotypes;
			this.ForceBaselinerChance = forceBaselinerChance;
			this.AllowedDevelopmentalStages = developmentalStages;
			this.PawnKindDefGetter = pawnKindDefGetter;
			this.ExcludeBiologicalAgeRange = excludeBiologicalAgeRange;
			this.BiologicalAgeRange = biologicalAgeRange;
			this.ForceRecruitable = forceRecruitable;
			this.DontGiveWeapon = dontGiveWeapon;
			this.OnlyUseForcedBackstories = onlyUseForcedBackstories;
			this.MaximumAgeTraits = maximumAgeTraits;
			this.MinimumAgeTraits = minimumAgeTraits;
			this.ForceNoGear = forceNoGear;
			if (forcedXenogenes != null)
			{
				foreach (GeneDef gene in forcedXenogenes)
				{
					this.AddForcedGene(gene, true);
				}
			}
			if (forcedEndogenes != null)
			{
				foreach (GeneDef gene2 in forcedEndogenes)
				{
					this.AddForcedGene(gene2, false);
				}
			}
			this.ValidateAndFix();
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x00107068 File Offset: 0x00105268
		public void ValidateAndFix()
		{
			if (!this._calledTheCorrectConstructor)
			{
				Log.Error("This PawnGenerationRequest was not created through the correct constructor.");
				this._calledTheCorrectConstructor = true;
			}
			if (this.Context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'");
				this.Context = PawnGenerationContext.NonPlayer;
			}
			if (this.ForceNoIdeo && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate a pawn with no ideo and a fixed ideo.");
				this.ForceNoIdeo = false;
			}
			if ((this.AllowedDevelopmentalStages.Newborn() || this.AllowedDevelopmentalStages.Baby()) && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate baby with specific ideology (babies have no ideology).");
				this.FixedIdeo = null;
			}
			string arg;
			if (!this.AllowDowned && this.AlwaysDownedLifeStages(out arg))
			{
				Log.Error(string.Format("Trying to generate a non-downed {0} {1} pawn but that would include these downed lifestages: {2}", this.AllowedDevelopmentalStages, this.KindDef.label, arg));
				this.AllowDowned = true;
			}
			if (this.AllowedDevelopmentalStages.Newborn() && this.AllowedDevelopmentalStages != DevelopmentalStage.Newborn)
			{
				Log.Error("Trying to generate a newborn and other developmental stages simultaneously.");
				this.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
			}
			if (this.ExcludeBiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and an excluded biological age range.");
				this.ExcludeBiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and a biological age range.");
				this.BiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.ExcludeBiologicalAgeRange != null)
			{
				Log.Error("Trying to generate a pawn with both a include and exclude age range");
				this.BiologicalAgeRange = null;
				this.ExcludeBiologicalAgeRange = null;
			}
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0010721C File Offset: 0x0010541C
		[Obsolete("Will be removed in 1.5; use the constructor instead.")]
		public static PawnGenerationRequest MakeDefault()
		{
			return new PawnGenerationRequest(null, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x001072B1 File Offset: 0x001054B1
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".");
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x001072DD File Offset: 0x001054DD
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".");
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0010730C File Offset: 0x0010550C
		public void AddForcedGene(GeneDef gene, bool xenogene)
		{
			if (xenogene)
			{
				if (this.ForcedXenogenes == null)
				{
					this.ForcedXenogenes = new List<GeneDef>();
				}
				this.ForcedXenogenes.Add(gene);
				return;
			}
			if (this.ForcedEndogenes == null)
			{
				this.ForcedEndogenes = new List<GeneDef>();
			}
			this.ForcedEndogenes.Add(gene);
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x0010735C File Offset: 0x0010555C
		public override string ToString()
		{
			string[] array = new string[52];
			array[0] = "kindDef=";
			int num = 1;
			PawnKindDef kindDef = this.KindDef;
			array[num] = ((kindDef != null) ? kindDef.ToString() : null);
			array[2] = ", context=";
			array[3] = this.Context.ToString();
			array[4] = ", faction=";
			int num2 = 5;
			Faction faction = this.Faction;
			array[num2] = ((faction != null) ? faction.ToString() : null);
			array[6] = ", tile=";
			array[7] = this.Tile.ToString();
			array[8] = ", forceGenerateNewPawn=";
			array[9] = this.ForceGenerateNewPawn.ToString();
			array[10] = ", allowedDevelopmentalStages=";
			array[11] = this.AllowedDevelopmentalStages.ToString();
			array[12] = ", allowDead=";
			array[13] = this.AllowDead.ToString();
			array[14] = ", allowDowned=";
			array[15] = this.AllowDowned.ToString();
			array[16] = ", canGeneratePawnRelations=";
			array[17] = this.CanGeneratePawnRelations.ToString();
			array[18] = ", mustBeCapableOfViolence=";
			array[19] = this.MustBeCapableOfViolence.ToString();
			array[20] = ", colonistRelationChanceFactor=";
			array[21] = this.ColonistRelationChanceFactor.ToString();
			array[22] = ", forceAddFreeWarmLayerIfNeeded=";
			array[23] = this.ForceAddFreeWarmLayerIfNeeded.ToString();
			array[24] = ", allowGay=";
			array[25] = this.AllowGay.ToString();
			array[26] = ", prohibitedTraits=";
			int num3 = 27;
			IEnumerable<TraitDef> prohibitedTraits = this.ProhibitedTraits;
			array[num3] = ((prohibitedTraits != null) ? prohibitedTraits.ToString() : null);
			array[28] = ", allowFood=";
			array[29] = this.AllowFood.ToString();
			array[30] = ", allowAddictions=";
			array[31] = this.AllowAddictions.ToString();
			array[32] = ", inhabitant=";
			array[33] = this.Inhabitant.ToString();
			array[34] = ", certainlyBeenInCryptosleep=";
			array[35] = this.CertainlyBeenInCryptosleep.ToString();
			array[36] = ", biocodeWeaponChance=";
			array[37] = this.BiocodeWeaponChance.ToString();
			array[38] = ", validatorPreGear=";
			int num4 = 39;
			Predicate<Pawn> validatorPreGear = this.ValidatorPreGear;
			array[num4] = ((validatorPreGear != null) ? validatorPreGear.ToString() : null);
			array[40] = ", validatorPostGear=";
			int num5 = 41;
			Predicate<Pawn> validatorPostGear = this.ValidatorPostGear;
			array[num5] = ((validatorPostGear != null) ? validatorPostGear.ToString() : null);
			array[42] = ", fixedBiologicalAge=";
			array[43] = this.FixedBiologicalAge.ToString();
			array[44] = ", fixedChronologicalAge=";
			array[45] = this.FixedChronologicalAge.ToString();
			array[46] = ", fixedGender=";
			array[47] = this.FixedGender.ToString();
			array[48] = ", fixedLastName=";
			array[49] = this.FixedLastName;
			array[50] = ", fixedBirthName=";
			array[51] = this.FixedBirthName;
			return string.Concat(array);
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x00107658 File Offset: 0x00105858
		private bool AlwaysDownedLifeStages(out string stages)
		{
			DevelopmentalStage developmentalStage = this.AllowedDevelopmentalStages;
			if (this.AllowedDevelopmentalStages.Newborn())
			{
				developmentalStage |= DevelopmentalStage.Baby;
			}
			StringBuilder stringBuilder = null;
			foreach (LifeStageAge lifeStageAge in this.KindDef.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def.alwaysDowned && developmentalStage.Has(lifeStageAge.def.developmentalStage))
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(lifeStageAge.def.label, 64);
					}
					else
					{
						stringBuilder.Append(", ");
						stringBuilder.Append(lifeStageAge.def.label);
					}
				}
			}
			if (stringBuilder == null)
			{
				stages = null;
				return false;
			}
			stages = stringBuilder.ToString();
			return true;
		}

		// Token: 0x04002616 RID: 9750
		private PawnKindDef kindDefInner;

		// Token: 0x04002653 RID: 9811
		private bool _calledTheCorrectConstructor;
	}
}
