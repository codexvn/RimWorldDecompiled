using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000254 RID: 596
	public class RaceProperties
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000467FF File Offset: 0x000449FF
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0004680D File Offset: 0x00044A0D
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0004681B File Offset: 0x00044A1B
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh && !this.IsAnomalyEntity;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00046838 File Offset: 0x00044A38
		public bool Insect
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Insectoid;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00046847 File Offset: 0x00044A47
		public bool Dryad
		{
			get
			{
				return this.animalType == AnimalType.Dryad;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00046852 File Offset: 0x00044A52
		public bool ShouldHaveAbilityTracker
		{
			get
			{
				return this.Humanlike || this.IsMechanoid;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00046864 File Offset: 0x00044A64
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00046870 File Offset: 0x00044A70
		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x000468D0 File Offset: 0x00044AD0
		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00046923 File Offset: 0x00044B23
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				return this.deathAction.Worker;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00046930 File Offset: 0x00044B30
		public FleshTypeDef FleshType
		{
			get
			{
				return this.fleshType ?? FleshTypeDefOf.Normal;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00046941 File Offset: 0x00044B41
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00046950 File Offset: 0x00044B50
		public bool IsFlesh
		{
			get
			{
				return this.FleshType.isOrganic;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0004695D File Offset: 0x00044B5D
		public bool IsAnomalyEntity
		{
			get
			{
				return ModsConfig.AnomalyActive && (this.FleshType == FleshTypeDefOf.EntityMechanical || this.FleshType == FleshTypeDefOf.EntityFlesh || this.FleshType == FleshTypeDefOf.Fleshbeast);
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00046991 File Offset: 0x00044B91
		public bool IsDrone
		{
			get
			{
				return ModsConfig.OdysseyActive && this.FleshType == FleshTypeDefOf.Drone;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x000469A9 File Offset: 0x00044BA9
		public ThingDef BloodDef
		{
			get
			{
				return this.bloodDef;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x000469B1 File Offset: 0x00044BB1
		public ThingDef BloodSmearDef
		{
			get
			{
				return this.bloodSmearDef;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000469B9 File Offset: 0x00044BB9
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000469CB File Offset: 0x00044BCB
		public bool CanPassFences
		{
			get
			{
				return !this.FenceBlocked;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x000469D6 File Offset: 0x00044BD6
		public bool FenceBlocked
		{
			get
			{
				return this.Roamer;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x000469DE File Offset: 0x00044BDE
		public bool Roamer
		{
			get
			{
				return this.roamMtbDays != null;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x000469EB File Offset: 0x00044BEB
		public bool IsWorkMech
		{
			get
			{
				return !this.mechEnabledWorkTypes.NullOrEmpty<WorkTypeDef>();
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x000469FC File Offset: 0x00044BFC
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00046A51 File Offset: 0x00044C51
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00046A6C File Offset: 0x00044C6C
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00046A7C File Offset: 0x00044C7C
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00046AD5 File Offset: 0x00044CD5
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00046AEC File Offset: 0x00044CEC
		public void ResolveReferencesSpecial()
		{
			if (this.specificMeatDef != null)
			{
				this.meatDef = this.specificMeatDef;
			}
			else if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00046B4B File Offset: 0x00044D4B
		public IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (thingDef.IsCorpse)
			{
				yield break;
			}
			if (this.predator && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "predator but doesn't eat meat";
			}
			if (this.canFishForFood && !this.Eats(FoodTypeFlags.Meat))
			{
				yield return "canFishForFood but doesn't eat meat";
			}
			int num;
			for (int i = 0; i < this.lifeStageAges.Count; i = num + 1)
			{
				for (int j = 0; j < i; j = num + 1)
				{
					if (this.lifeStageAges[j].minAge > this.lifeStageAges[i].minAge)
					{
						yield return "lifeStages minAges are not in ascending order";
					}
					num = j;
				}
				num = i;
			}
			if (thingDef.IsCaravanRideable())
			{
				if (!this.lifeStageAges.Any((LifeStageAge s) => s.def.caravanRideable))
				{
					yield return "must contain at least one lifeStage with caravanRideable when CaravanRidingSpeedFactor is defined";
				}
			}
			IEnumerator<string> enumerator;
			if (this.litterSizeCurve != null)
			{
				foreach (string text in this.litterSizeCurve.ConfigErrors("litterSizeCurve"))
				{
					yield return text;
				}
				enumerator = null;
			}
			if (this.nameOnTameChance > 0f && this.nameGenerator == null)
			{
				yield return "can be named, but has no nameGenerator";
			}
			if (this.specificMeatDef != null && this.useMeatFrom != null)
			{
				yield return "specificMeatDef and useMeatFrom are both non-null. specificMeatDef will be chosen.";
			}
			if (this.useMeatFrom != null && this.useMeatFrom.category != ThingCategory.Pawn)
			{
				string str = "tries to use meat from non-pawn ";
				ThingDef thingDef2 = this.useMeatFrom;
				yield return str + ((thingDef2 != null) ? thingDef2.ToString() : null);
			}
			ThingDef thingDef3 = this.useMeatFrom;
			if (((thingDef3 != null) ? thingDef3.race.useMeatFrom : null) != null)
			{
				string str2 = "tries to use meat from ";
				ThingDef thingDef4 = this.useMeatFrom;
				string str3 = (thingDef4 != null) ? thingDef4.ToString() : null;
				string str4 = " which uses meat from ";
				ThingDef thingDef5 = this.useMeatFrom.race.useMeatFrom;
				yield return str2 + str3 + str4 + ((thingDef5 != null) ? thingDef5.ToString() : null);
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.category != ThingCategory.Pawn)
			{
				string str5 = "tries to use leather from non-pawn ";
				ThingDef thingDef6 = this.useLeatherFrom;
				yield return str5 + ((thingDef6 != null) ? thingDef6.ToString() : null);
			}
			if (this.useLeatherFrom != null && this.useLeatherFrom.race.useLeatherFrom != null)
			{
				string str6 = "tries to use leather from ";
				ThingDef thingDef7 = this.useLeatherFrom;
				string str7 = (thingDef7 != null) ? thingDef7.ToString() : null;
				string str8 = " which uses leather from ";
				ThingDef thingDef8 = this.useLeatherFrom.race.useLeatherFrom;
				yield return str6 + str7 + str8 + ((thingDef8 != null) ? thingDef8.ToString() : null);
			}
			if (this.Animal && this.trainability == null)
			{
				yield return "animal has trainability = null";
			}
			if (this.fleshType == FleshTypeDefOf.Normal && this.gestationPeriodDays < 0f)
			{
				yield return "normal flesh but gestationPeriodDays not configured.";
			}
			if (this.IsMechanoid && thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
			{
				yield return thingDef.label + " mech has no butcher products set";
			}
			foreach (string text2 in this.deathAction.ConfigErrors())
			{
				yield return text2;
			}
			enumerator = null;
			if (this.renderTree == null)
			{
				yield return "renderTree is null";
			}
			if (this.canCrossBreedWith != null)
			{
				foreach (ThingDef thingDef9 in this.canCrossBreedWith)
				{
					if (thingDef9.race == null || !thingDef9.race.Animal)
					{
						yield return "tries to set canCrossBreedWith " + thingDef9.defName + " but that is not an animal";
					}
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00046B62 File Offset: 0x00044D62
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			Pawn pawnThing = req.Pawn ?? (req.Thing as Pawn);
			if (ModsConfig.BiotechActive && this.Humanlike)
			{
				Pawn pawnThing4 = pawnThing;
				if (((pawnThing4 != null) ? pawnThing4.genes : null) != null && pawnThing.genes.Xenotype != XenotypeDefOf.Baseliner)
				{
					goto IL_15E;
				}
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), parentDef.LabelCap, parentDef.description, 4205, null, null, false, true);
			IL_15E:
			if (pawnThing != null && pawnThing.IsMutant && pawnThing.mutant.Def.overrideFoodType)
			{
				string text = pawnThing.mutant.Def.foodType.ToHumanString().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.PawnFood, "Diet".Translate(), text, "Stat_Race_Diet_Desc".Translate(text), 1500, null, null, false, false);
			}
			else if (!parentDef.race.IsMechanoid && !parentDef.race.IsAnomalyEntity && !parentDef.race.IsDrone)
			{
				string text2 = this.foodType.ToHumanString().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.PawnFood, "Diet".Translate(), text2, "Stat_Race_Diet_Desc".Translate(text2), 1500, null, null, false, false);
			}
			if (pawnThing != null)
			{
				Pawn_NeedsTracker needs = pawnThing.needs;
				if (((needs != null) ? needs.food : null) != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.PawnFood, "FoodConsumption".Translate(), RaceProperties.NutritionEatenPerDay(pawnThing), RaceProperties.NutritionEatenPerDayExplanation(pawnThing, false, false, true), 1600, null, null, false, false);
				}
			}
			if (parentDef.race.leatherDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "LeatherType".Translate(), parentDef.race.leatherDef.LabelCap, "Stat_Race_LeatherType_Desc".Translate(), 3550, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(parentDef.race.leatherDef, -1)
				}, false, false);
			}
			if (!parentDef.race.Animal)
			{
				Pawn pawnThing2 = pawnThing;
				if (pawnThing2 == null || !pawnThing2.IsWildMan() || parentDef.GetStatValueAbstract(StatDefOf.Wildness, null) <= 0f)
				{
					goto IL_581;
				}
			}
			float f = (pawnThing == null) ? PawnUtility.GetManhunterOnDamageChance(parentDef) : PawnUtility.GetManhunterOnDamageChance(pawnThing, null, -1f);
			yield return new StatDrawEntry(StatCategoryDefOf.Animals, "HarmedRevengeChance".Translate(), f.ToStringPercent(), PawnUtility.GetManhunterOnDamageChanceExplanation(parentDef, pawnThing), 510, null, null, false, false);
			float f2 = (pawnThing == null) ? PawnUtility.GetManhunterOnTameFailChance(parentDef) : PawnUtility.GetManhunterOnTameFailChance(pawnThing);
			yield return new StatDrawEntry(StatCategoryDefOf.Animals, "TameFailedRevengeChance".Translate(), f2.ToStringPercent(), PawnUtility.GetManhunterOnTameFailChanceExplanation(parentDef, pawnThing), 511, null, null, false, false);
			IL_581:
			TrainabilityDef trainabilityDef = TrainableUtility.GetTrainability(pawnThing) ?? this.trainability;
			if (this.intelligence < Intelligence.Humanlike && trainabilityDef != null && !parentDef.race.IsAnomalyEntity && !parentDef.race.IsDrone)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Stat_Race_Trainability_Desc".Translate());
				if (ModsConfig.OdysseyActive)
				{
					Pawn pawnThing3 = pawnThing;
					if (pawnThing3 != null && pawnThing3.health.hediffSet.HasHediff(HediffDefOf.SentienceCatalyst, false))
					{
						sb.AppendLine();
						sb.AppendLine("StatsReport_SentienceCatalystInstalled".Translate());
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Animals, "Trainability".Translate(), trainabilityDef.LabelCap, sb.ToString(), 2500, null, null, false, false);
				sb.Clear();
				sb.AppendLine("StatsReport_AvailableTraining_Desc".Translate());
				IOrderedEnumerable<TrainableDef> orderedEnumerable = from td in DefDatabase<TrainableDef>.AllDefsListForReading.Where(delegate(TrainableDef td)
				{
					bool flag;
					return Pawn_TrainingTracker.CanAssignToTrain(td, parentDef, out flag, pawnThing);
				})
				orderby td.listPriority descending
				select td;
				foreach (TrainableDef trainableDef in orderedEnumerable)
				{
					sb.AppendLine();
					sb.AppendLine(trainableDef.LabelCap.Colorize(ColorLibrary.Yellow).ResolveTags());
					sb.AppendLine(trainableDef.description);
				}
				if (sb.Length != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_AvailableTraining".Translate(), (from td in orderedEnumerable
					select td.label).ToCommaList(false, false).CapitalizeFirst(), sb.ToString(), 2499, null, null, false, false);
				}
				sb = null;
			}
			if (!parentDef.race.IsDrone)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PawnHealth, "StatsReport_LifeExpectancy".Translate(), this.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute), "Stat_Race_LifeExpectancy_Desc".Translate(), 4300, null, null, false, false);
			}
			if (parentDef.race.Animal || this.FenceBlocked)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_BlockedByFences".Translate(), this.FenceBlocked ? "Yes".Translate() : "No".Translate(), "Stat_Race_BlockedByFences_Desc".Translate(), 2040, null, null, false, false);
			}
			if (parentDef.race.Animal)
			{
				StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Animals, "PackAnimal".Translate(), this.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202, null, null, false, false);
				yield return statDrawEntry;
				if (pawnThing != null && pawnThing.gender != Gender.None)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Sex".Translate(), pawnThing.gender.GetLabel(true).CapitalizeFirst(), pawnThing.gender.GetLabel(true).CapitalizeFirst(), 4204, null, null, false, false);
				}
				if (parentDef.race.nuzzleMtbHours > 0f)
				{
					float num = (pawnThing != null) ? NuzzleUtility.GetNuzzleMTBHours(pawnThing) : parentDef.race.nuzzleMtbHours;
					StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.Animals, "NuzzleInterval".Translate(), Mathf.RoundToInt(num * 2500f).ToStringTicksToPeriod(true, false, true, true, false), "NuzzleIntervalExplanation".Translate(), 500, null, null, false, false);
					yield return statDrawEntry2;
				}
				if (parentDef.race.roamMtbDays != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_RoamInterval".Translate(), Mathf.RoundToInt(parentDef.race.roamMtbDays.Value * 60000f).ToStringTicksToPeriod(true, false, true, true, false), "Stat_Race_RoamInterval_Desc".Translate(), 2030, null, null, false, false);
				}
				foreach (StatDrawEntry statDrawEntry3 in AnimalProductionUtility.AnimalProductionStats(parentDef))
				{
					yield return statDrawEntry3;
				}
				IEnumerator<StatDrawEntry> enumerator2 = null;
			}
			if (ModsConfig.BiotechActive && this.IsMechanoid)
			{
				if (this.mechWeightClass != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Mechanoid, "MechWeightClass".Translate(), this.mechWeightClass.LabelCap, "MechWeightClassExplanation".Translate(), 500, null, null, false, false);
				}
				ThingDef thingDef = MechanitorUtility.RechargerForMech(parentDef);
				if (thingDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Mechanoid, "StatsReport_RechargerNeeded".Translate(), thingDef.LabelCap, "StatsReport_RechargerNeeded_Desc".Translate(), 503, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(thingDef, -1)
					}, false, false);
				}
				foreach (StatDrawEntry statDrawEntry4 in MechWorkUtility.SpecialDisplayStats(parentDef, req))
				{
					yield return statDrawEntry4;
				}
				IEnumerator<StatDrawEntry> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00046B80 File Offset: 0x00044D80
		public static string NutritionEatenPerDay(Pawn p)
		{
			return (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToString("0.##");
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00046BB4 File Offset: 0x00044DB4
		public static string NutritionEatenPerDayExplanation(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##")));
			stringBuilder.AppendLine();
			if (showDiet)
			{
				stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
				stringBuilder.AppendLine();
			}
			if (showLegend)
			{
				stringBuilder.AppendLine("Legend".Translate() + ":");
				stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				stringBuilder.AppendLine();
			}
			if (showCalculations)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.6666667E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute));
				if (p.health.hediffSet.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Hediff hediff in p.health.hediffSet.hediffs)
					{
						if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
					foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
					{
						if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
						{
							stringBuilder.AppendLine("    " + hediff2.LabelCap + ": " + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Offset));
						}
					}
				}
				Pawn_StoryTracker story = p.story;
				if (((story != null) ? story.traits : null) != null && p.story.traits.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Trait trait in p.story.traits.allTraits)
					{
						if (!trait.Suppressed && trait.CurrentData.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + trait.LabelCap + ": " + trait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
				}
				Building_Bed building_Bed = p.CurrentBed() ?? p.CurrentCaravanBed();
				if (building_Bed != null)
				{
					float statValue = building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor, true, -1);
					if (statValue != 1f)
					{
						stringBuilder.AppendLine().AppendLine("StatsReport_InBed".Translate() + ": x" + statValue.ToStringPercent());
					}
				}
				if (ModsConfig.BiotechActive)
				{
					Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
					if (firstHediffOfDef != null)
					{
						HediffComp_Lactating hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>();
						if (hediffComp_Lactating != null)
						{
							float f = hediffComp_Lactating.AddedNutritionPerDay();
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(firstHediffOfDef.LabelBaseCap + ": " + f.ToStringWithSign("0.##"));
						}
					}
				}
				if (p.genes != null)
				{
					int num = 0;
					foreach (Gene gene in p.genes.GenesListForReading)
					{
						if (!gene.Overridden)
						{
							num += gene.def.biostatMet;
						}
					}
					float num2 = GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num);
					if (num2 != 1f)
					{
						stringBuilder.AppendLine().AppendLine("FactorForMetabolism".Translate() + ": x" + num2.ToStringPercent());
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + RaceProperties.NutritionEatenPerDay(p));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000A8C RID: 2700
		public Intelligence intelligence;

		// Token: 0x04000A8D RID: 2701
		private FleshTypeDef fleshType;

		// Token: 0x04000A8E RID: 2702
		private ThingDef bloodDef;

		// Token: 0x04000A8F RID: 2703
		private ThingDef bloodSmearDef;

		// Token: 0x04000A90 RID: 2704
		public bool hasGenders = true;

		// Token: 0x04000A91 RID: 2705
		public Gender forceGender;

		// Token: 0x04000A92 RID: 2706
		public bool needsRest = true;

		// Token: 0x04000A93 RID: 2707
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x04000A94 RID: 2708
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04000A95 RID: 2709
		public DutyDef dutyBoss;

		// Token: 0x04000A96 RID: 2710
		public PawnNameCategory nameCategory;

		// Token: 0x04000A97 RID: 2711
		public FoodTypeFlags foodType;

		// Token: 0x04000A98 RID: 2712
		public BodyDef body;

		// Token: 0x04000A99 RID: 2713
		public DeathActionProperties deathAction = new DeathActionProperties();

		// Token: 0x04000A9A RID: 2714
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x04000A9B RID: 2715
		public SimpleCurve ageGenerationCurve;

		// Token: 0x04000A9C RID: 2716
		public bool makesFootprints;

		// Token: 0x04000A9D RID: 2717
		public int executionRange = 2;

		// Token: 0x04000A9E RID: 2718
		public float lifeExpectancy = 10f;

		// Token: 0x04000A9F RID: 2719
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x04000AA0 RID: 2720
		public float? roamMtbDays;

		// Token: 0x04000AA1 RID: 2721
		public bool allowedOnCaravan = true;

		// Token: 0x04000AA2 RID: 2722
		public bool canReleaseToWild = true;

		// Token: 0x04000AA3 RID: 2723
		public bool playerCanChangeMaster = true;

		// Token: 0x04000AA4 RID: 2724
		public bool showTrainables = true;

		// Token: 0x04000AA5 RID: 2725
		public bool hideTrainingTab;

		// Token: 0x04000AA6 RID: 2726
		public bool doesntMove;

		// Token: 0x04000AA7 RID: 2727
		public PawnRenderTreeDef renderTree;

		// Token: 0x04000AA8 RID: 2728
		public AnimationDef startingAnimation;

		// Token: 0x04000AA9 RID: 2729
		public ThingDef linkedCorpseKind;

		// Token: 0x04000AAA RID: 2730
		public bool canOpenFactionlessDoors = true;

		// Token: 0x04000AAB RID: 2731
		public bool alwaysAwake;

		// Token: 0x04000AAC RID: 2732
		public bool alwaysViolent;

		// Token: 0x04000AAD RID: 2733
		public bool isImmuneToInfections;

		// Token: 0x04000AAE RID: 2734
		public float bleedRateFactor = 1f;

		// Token: 0x04000AAF RID: 2735
		public bool canBecomeShambler;

		// Token: 0x04000AB0 RID: 2736
		public bool neverIncludeInQuests;

		// Token: 0x04000AB1 RID: 2737
		public bool canBeVacuumBurnt = true;

		// Token: 0x04000AB2 RID: 2738
		public bool herdAnimal;

		// Token: 0x04000AB3 RID: 2739
		public bool packAnimal;

		// Token: 0x04000AB4 RID: 2740
		public bool predator;

		// Token: 0x04000AB5 RID: 2741
		public float maxPreyBodySize = 99999f;

		// Token: 0x04000AB6 RID: 2742
		public float petness;

		// Token: 0x04000AB7 RID: 2743
		public float nuzzleMtbHours = -1f;

		// Token: 0x04000AB8 RID: 2744
		public float manhunterOnDamageChance;

		// Token: 0x04000AB9 RID: 2745
		public float manhunterOnTameFailChance;

		// Token: 0x04000ABA RID: 2746
		public bool canBePredatorPrey = true;

		// Token: 0x04000ABB RID: 2747
		public bool herdMigrationAllowed = true;

		// Token: 0x04000ABC RID: 2748
		public AnimalType animalType;

		// Token: 0x04000ABD RID: 2749
		public List<ThingDef> willNeverEat;

		// Token: 0x04000ABE RID: 2750
		public bool giveNonToolUserBeatFireVerb;

		// Token: 0x04000ABF RID: 2751
		public bool disableIgniteVerb;

		// Token: 0x04000AC0 RID: 2752
		public bool disableAreaControl;

		// Token: 0x04000AC1 RID: 2753
		public bool waterSeeker;

		// Token: 0x04000AC2 RID: 2754
		public int? waterCellCost;

		// Token: 0x04000AC3 RID: 2755
		public bool disableMating;

		// Token: 0x04000AC4 RID: 2756
		public List<ThingDef> canCrossBreedWith;

		// Token: 0x04000AC5 RID: 2757
		public List<ThingDef> crossAggroWith;

		// Token: 0x04000AC6 RID: 2758
		public bool canFishForFood;

		// Token: 0x04000AC7 RID: 2759
		public bool canFlyInVacuum;

		// Token: 0x04000AC8 RID: 2760
		public PawnKindDef manhunterPackUseLabelFrom;

		// Token: 0x04000AC9 RID: 2761
		public float flightStartChanceOnJobStart;

		// Token: 0x04000ACA RID: 2762
		public float flightSpeedFactor = 2.8f;

		// Token: 0x04000ACB RID: 2763
		public bool canFlyIntoMap;

		// Token: 0x04000ACC RID: 2764
		public bool canLeaveMapFlying;

		// Token: 0x04000ACD RID: 2765
		public float leaveMapOnFleeChance;

		// Token: 0x04000ACE RID: 2766
		public int maxMechEnergy = 100;

		// Token: 0x04000ACF RID: 2767
		public List<WorkTypeDef> mechEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04000AD0 RID: 2768
		public int mechFixedSkillLevel = 10;

		// Token: 0x04000AD1 RID: 2769
		public List<MechWorkTypePriority> mechWorkTypePriorities;

		// Token: 0x04000AD2 RID: 2770
		public int? bulletStaggerDelayTicks;

		// Token: 0x04000AD3 RID: 2771
		public float? bulletStaggerSpeedFactor;

		// Token: 0x04000AD4 RID: 2772
		public EffecterDef bulletStaggerEffecterDef;

		// Token: 0x04000AD5 RID: 2773
		public bool bulletStaggerIgnoreBodySize;

		// Token: 0x04000AD6 RID: 2774
		public MechWeightClassDef mechWeightClass;

		// Token: 0x04000AD7 RID: 2775
		public List<DetritusLeavingType> detritusLeavings = new List<DetritusLeavingType>();

		// Token: 0x04000AD8 RID: 2776
		public float gestationPeriodDays = -1f;

		// Token: 0x04000AD9 RID: 2777
		public SimpleCurve litterSizeCurve;

		// Token: 0x04000ADA RID: 2778
		public float mateMtbHours = 12f;

		// Token: 0x04000ADB RID: 2779
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x04000ADC RID: 2780
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x04000ADD RID: 2781
		public TrainabilityDef trainability;

		// Token: 0x04000ADE RID: 2782
		public List<TrainableDef> specialTrainables;

		// Token: 0x04000ADF RID: 2783
		private RulePackDef nameGenerator;

		// Token: 0x04000AE0 RID: 2784
		private RulePackDef nameGeneratorFemale;

		// Token: 0x04000AE1 RID: 2785
		public float nameOnTameChance;

		// Token: 0x04000AE2 RID: 2786
		public float baseBodySize = 1f;

		// Token: 0x04000AE3 RID: 2787
		public float baseHealthScale = 1f;

		// Token: 0x04000AE4 RID: 2788
		public float baseHungerRate = 1f;

		// Token: 0x04000AE5 RID: 2789
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x04000AE6 RID: 2790
		public List<LifeStageWorkSettings> lifeStageWorkSettings = new List<LifeStageWorkSettings>();

		// Token: 0x04000AE7 RID: 2791
		public bool hasMeat = true;

		// Token: 0x04000AE8 RID: 2792
		[MustTranslate]
		public string meatLabel;

		// Token: 0x04000AE9 RID: 2793
		public Color meatColor = Color.white;

		// Token: 0x04000AEA RID: 2794
		public float meatMarketValue = 2f;

		// Token: 0x04000AEB RID: 2795
		public ThingDef specificMeatDef;

		// Token: 0x04000AEC RID: 2796
		public ThingDef useMeatFrom;

		// Token: 0x04000AED RID: 2797
		public ThingDef useLeatherFrom;

		// Token: 0x04000AEE RID: 2798
		public bool hasCorpse = true;

		// Token: 0x04000AEF RID: 2799
		public bool hasUnnaturalCorpse;

		// Token: 0x04000AF0 RID: 2800
		public bool corpseHiddenWhileUndiscovered;

		// Token: 0x04000AF1 RID: 2801
		public ThingDef leatherDef;

		// Token: 0x04000AF2 RID: 2802
		public ShadowData specialShadowData;

		// Token: 0x04000AF3 RID: 2803
		public List<Vector3> headPosPerRotation;

		// Token: 0x04000AF4 RID: 2804
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x04000AF5 RID: 2805
		public float soundCallIntervalFriendlyFactor = 1f;

		// Token: 0x04000AF6 RID: 2806
		public float soundCallIntervalAggressiveFactor = 0.25f;

		// Token: 0x04000AF7 RID: 2807
		public SoundDef soundMeleeHitPawn;

		// Token: 0x04000AF8 RID: 2808
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x04000AF9 RID: 2809
		public SoundDef soundMeleeMiss;

		// Token: 0x04000AFA RID: 2810
		public SoundDef soundMeleeDodge;

		// Token: 0x04000AFB RID: 2811
		public SoundDef soundAmbience;

		// Token: 0x04000AFC RID: 2812
		public SoundDef soundMoving;

		// Token: 0x04000AFD RID: 2813
		public SoundDef soundEating;

		// Token: 0x04000AFE RID: 2814
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x04000AFF RID: 2815
		public int anomalyKnowledge;

		// Token: 0x04000B00 RID: 2816
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x04000B01 RID: 2817
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x04000B02 RID: 2818
		[Unsaved(false)]
		public ThingDef unnaturalCorpseDef;

		// Token: 0x04000B03 RID: 2819
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
