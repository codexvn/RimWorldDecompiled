using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B44 RID: 6980
	public class FoodRestrictionDatabase : IExposable
	{
		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x06009BDD RID: 39901 RVA: 0x002D2E78 File Offset: 0x002D1078
		public List<FoodPolicy> AllFoodRestrictions
		{
			get
			{
				return this.foodRestrictions;
			}
		}

		// Token: 0x06009BDE RID: 39902 RVA: 0x002D2E80 File Offset: 0x002D1080
		public FoodRestrictionDatabase()
		{
			this.GenerateStartingFoodRestrictions();
		}

		// Token: 0x06009BDF RID: 39903 RVA: 0x002D2E99 File Offset: 0x002D1099
		public void ExposeData()
		{
			Scribe_Collections.Look<FoodPolicy>(ref this.foodRestrictions, "foodRestrictions", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06009BE0 RID: 39904 RVA: 0x002D2EB7 File Offset: 0x002D10B7
		public FoodPolicy DefaultFoodRestriction()
		{
			if (this.foodRestrictions.Count == 0)
			{
				this.MakeNewFoodRestriction();
			}
			return this.foodRestrictions[0];
		}

		// Token: 0x06009BE1 RID: 39905 RVA: 0x002D2EDC File Offset: 0x002D10DC
		public void SetDefault(FoodPolicy policy)
		{
			int index = this.foodRestrictions.IndexOf(policy);
			FoodPolicy value = this.foodRestrictions[0];
			this.foodRestrictions[0] = policy;
			this.foodRestrictions[index] = value;
		}

		// Token: 0x06009BE2 RID: 39906 RVA: 0x002D2F20 File Offset: 0x002D1120
		public AcceptanceReport TryDelete(FoodPolicy foodPolicy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodPolicy == foodPolicy)
				{
					return new AcceptanceReport("FoodRestrictionInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.foodRestriction != null && pawn2.foodRestriction.CurrentFoodPolicy == foodPolicy)
				{
					pawn2.foodRestriction.CurrentFoodPolicy = null;
				}
			}
			this.foodRestrictions.Remove(foodPolicy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009BE3 RID: 39907 RVA: 0x002D3010 File Offset: 0x002D1210
		public FoodPolicy MakeNewFoodRestriction()
		{
			int num;
			if (!this.foodRestrictions.Any<FoodPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.foodRestrictions.Max((FoodPolicy o) => o.id) + 1;
			}
			int id = num;
			FoodPolicy foodPolicy = new FoodPolicy(id, "FoodPolicy".Translate() + " " + id.ToString());
			foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
			where x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f
			select x)
			{
				foodPolicy.filter.SetAllow(thingDef, true);
			}
			this.foodRestrictions.Add(foodPolicy);
			if (ModsConfig.IdeologyActive)
			{
				foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, true);
				foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, true);
				foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, true);
				foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, true);
			}
			if (ModsConfig.BiotechActive)
			{
				foodPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
			}
			return foodPolicy;
		}

		// Token: 0x06009BE4 RID: 39908 RVA: 0x002D3160 File Offset: 0x002D1360
		private void GenerateStartingFoodRestrictions()
		{
			this.MakeNewFoodRestriction().label = "FoodRestrictionLavish".Translate();
			FoodPolicy foodPolicy = this.MakeNewFoodRestriction();
			foodPolicy.label = "FoodRestrictionFine".Translate();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealLavish && thingDef != ThingDefOf.InsectJelly)
				{
					foodPolicy.filter.SetAllow(thingDef, false);
				}
			}
			if (ModsConfig.BiotechActive)
			{
				foodPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
			}
			FoodPolicy foodPolicy2 = this.MakeNewFoodRestriction();
			foodPolicy2.label = "FoodRestrictionSimple".Translate();
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.ingestible != null && thingDef2.ingestible.preferability >= FoodPreferability.MealFine && thingDef2 != ThingDefOf.InsectJelly)
				{
					foodPolicy2.filter.SetAllow(thingDef2, false);
				}
			}
			foodPolicy2.filter.SetAllow(ThingDefOf.MealSurvivalPack, false);
			if (ModsConfig.BiotechActive)
			{
				foodPolicy2.filter.SetAllow(ThingDefOf.HemogenPack, false);
			}
			FoodPolicy foodPolicy3 = this.MakeNewFoodRestriction();
			foodPolicy3.label = "FoodRestrictionPaste".Translate();
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.ingestible != null && thingDef3.ingestible.preferability >= FoodPreferability.MealSimple && thingDef3 != ThingDefOf.MealNutrientPaste && thingDef3 != ThingDefOf.InsectJelly)
				{
					foodPolicy3.filter.SetAllow(thingDef3, false);
				}
			}
			if (ModsConfig.BiotechActive)
			{
				foodPolicy3.filter.SetAllow(ThingDefOf.HemogenPack, false);
			}
			FoodPolicy foodPolicy4 = this.MakeNewFoodRestriction();
			foodPolicy4.label = "FoodRestrictionRaw".Translate();
			foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef4.ingestible != null && thingDef4.ingestible.preferability >= FoodPreferability.MealAwful)
				{
					foodPolicy4.filter.SetAllow(thingDef4, false);
				}
			}
			foodPolicy4.filter.SetAllow(ThingDefOf.Chocolate, false);
			if (ModsConfig.BiotechActive)
			{
				foodPolicy4.filter.SetAllow(ThingDefOf.HemogenPack, false);
			}
			FoodPolicy foodPolicy5 = this.MakeNewFoodRestriction();
			foodPolicy5.label = "FoodRestrictionNothing".Translate();
			foodPolicy5.filter.SetDisallowAll(null, null);
			this.CreateIdeologyFoodRestrictions();
		}

		// Token: 0x06009BE5 RID: 39909 RVA: 0x002D3440 File Offset: 0x002D1640
		public void CreateIdeologyFoodRestrictions()
		{
			if (ModsConfig.IdeologyActive)
			{
				TaggedString vegLabel = "FoodRestrictionVegetarian".Translate();
				if (this.foodRestrictions.FirstOrDefault((FoodPolicy fr) => fr.label == vegLabel) == null)
				{
					FoodPolicy foodPolicy = this.MakeNewFoodRestriction();
					foodPolicy.label = vegLabel;
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
					{
						if (FoodUtility.UnacceptableVegetarian(thingDef))
						{
							foodPolicy.filter.SetAllow(thingDef, false);
						}
					}
					foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
					foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
					foodPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
					if (ModsConfig.BiotechActive)
					{
						foodPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
					}
				}
				TaggedString carnivoreLabel = "FoodRestrictionCarnivore".Translate();
				if (this.foodRestrictions.FirstOrDefault((FoodPolicy fr) => fr.label == carnivoreLabel) == null)
				{
					FoodPolicy foodPolicy2 = this.MakeNewFoodRestriction();
					foodPolicy2.label = carnivoreLabel;
					foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
					{
						if (!FoodUtility.UnacceptableCarnivore(thingDef2) && FoodUtility.GetMeatSourceCategory(thingDef2) != MeatSourceCategory.Humanlike)
						{
							if (!thingDef2.IsCorpse)
							{
								continue;
							}
							ThingDef sourceDef = thingDef2.ingestible.sourceDef;
							bool flag;
							if (sourceDef == null)
							{
								flag = false;
							}
							else
							{
								RaceProperties race = sourceDef.race;
								bool? flag2 = (race != null) ? new bool?(race.Humanlike) : null;
								bool flag3 = true;
								flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
							}
							if (!flag)
							{
								continue;
							}
						}
						foodPolicy2.filter.SetAllow(thingDef2, false);
					}
					foodPolicy2.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
					foodPolicy2.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
					foodPolicy2.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
					if (ModsConfig.BiotechActive)
					{
						foodPolicy2.filter.SetAllow(ThingDefOf.HemogenPack, false);
					}
				}
				TaggedString cannibalLabel = "FoodRestrictionCannibal".Translate();
				if (this.foodRestrictions.FirstOrDefault((FoodPolicy fr) => fr.label == cannibalLabel) == null)
				{
					FoodPolicy foodPolicy3 = this.MakeNewFoodRestriction();
					foodPolicy3.label = cannibalLabel;
					foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
					{
						if (!FoodUtility.MaybeAcceptableCannibalDef(thingDef3))
						{
							foodPolicy3.filter.SetAllow(thingDef3, false);
						}
					}
					foodPolicy3.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
					foodPolicy3.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
					foodPolicy3.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
					if (ModsConfig.BiotechActive)
					{
						foodPolicy3.filter.SetAllow(ThingDefOf.HemogenPack, false);
					}
				}
				TaggedString insectMeatLabel = "FoodRestrictionInsectMeat".Translate();
				if (this.foodRestrictions.FirstOrDefault((FoodPolicy fr) => fr.label == insectMeatLabel) == null)
				{
					FoodPolicy foodPolicy4 = this.MakeNewFoodRestriction();
					foodPolicy4.label = insectMeatLabel;
					foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
					{
						if (!FoodUtility.MaybeAcceptableInsectMeatEatersDef(thingDef4))
						{
							foodPolicy4.filter.SetAllow(thingDef4, false);
						}
					}
					foodPolicy4.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
					foodPolicy4.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
					foodPolicy4.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
					if (ModsConfig.BiotechActive)
					{
						foodPolicy4.filter.SetAllow(ThingDefOf.HemogenPack, false);
					}
				}
			}
		}

		// Token: 0x04006C31 RID: 27697
		private List<FoodPolicy> foodRestrictions = new List<FoodPolicy>();
	}
}
