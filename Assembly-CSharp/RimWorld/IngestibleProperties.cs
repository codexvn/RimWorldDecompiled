using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A0 RID: 6560
	public class IngestibleProperties
	{
		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06009469 RID: 37993 RVA: 0x002B973A File Offset: 0x002B793A
		public JoyKindDef JoyKind
		{
			get
			{
				return this.joyKind ?? JoyKindDefOf.Gluttonous;
			}
		}

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x0600946A RID: 37994 RVA: 0x002B974B File Offset: 0x002B794B
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x0600946B RID: 37995 RVA: 0x002B975C File Offset: 0x002B795C
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x0600946C RID: 37996 RVA: 0x002B9776 File Offset: 0x002B7976
		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		// Token: 0x0600946D RID: 37997 RVA: 0x002B97A2 File Offset: 0x002B79A2
		public IEnumerable<string> ConfigErrors()
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return "Nutrition == 0 but preferability is " + this.preferability.ToString() + " instead of " + FoodPreferability.NeverForNutrition.ToString();
			}
			if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0f && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
			if (this.joy == 0f && this.joyKind != null)
			{
				string str = "joy is 0 but joyKind is ";
				JoyKindDef joyKindDef = this.joyKind;
				yield return str + ((joyKindDef != null) ? joyKindDef.ToString() : null);
			}
			yield break;
		}

		// Token: 0x0600946E RID: 37998 RVA: 0x002B97B4 File Offset: 0x002B79B4
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		// Token: 0x0600946F RID: 37999 RVA: 0x002B9824 File Offset: 0x002B7A24
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.joy > 0f)
			{
				StatCategoryDef category = (this.drugCategory != DrugCategory.None) ? StatCategoryDefOf.Drug : StatCategoryDefOf.Basics;
				yield return new StatDrawEntry(category, "Joy".Translate(), this.joy.ToStringPercent("F0") + " (" + this.JoyKind.label + ")", "Stat_Thing_Ingestible_Joy_Desc".Translate(), 4751, null, null, false, false);
			}
			if (this.HumanEdible && this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f)
			{
				RoyalTitleDef royalTitleDef = this.MaxSatisfiedTitle();
				if (royalTitleDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Ingestible_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Ingestible_MaxSatisfiedTitle_Desc".Translate(), 4752, null, null, false, false);
				}
			}
			if (this.drugCategory != DrugCategory.None)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "DrugCategory".Translate().CapitalizeFirst(), this.drugCategory.GetLabel().CapitalizeFirst(), "Stat_Thing_Drug_Category_Desc".Translate(), 2485, null, null, false, false);
			}
			if (this.outcomeDoers != null)
			{
				int num;
				for (int i = 0; i < this.outcomeDoers.Count; i = num + 1)
				{
					foreach (StatDrawEntry statDrawEntry in this.outcomeDoers[i].SpecialDisplayStats(this.parent))
					{
						yield return statDrawEntry;
					}
					IEnumerator<StatDrawEntry> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06009470 RID: 38000 RVA: 0x002B9834 File Offset: 0x002B7A34
		private bool TryGetOutcomeDoer<T>(out T doer) where T : IngestionOutcomeDoer
		{
			doer = default(T);
			if (this.outcomeDoers == null)
			{
				return false;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in this.outcomeDoers)
			{
				T t = ingestionOutcomeDoer as T;
				if (t != null)
				{
					doer = t;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006046 RID: 24646
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04006047 RID: 24647
		public int maxNumToIngestAtOnce;

		// Token: 0x04006048 RID: 24648
		public int defaultNumToIngestAtOnce = 20;

		// Token: 0x04006049 RID: 24649
		public List<IngestionOutcomeDoer> outcomeDoers;

		// Token: 0x0400604A RID: 24650
		public int baseIngestTicks = 500;

		// Token: 0x0400604B RID: 24651
		public float chairSearchRadius = 32f;

		// Token: 0x0400604C RID: 24652
		public bool useEatingSpeedStat = true;

		// Token: 0x0400604D RID: 24653
		public bool babiesCanIngest;

		// Token: 0x0400604E RID: 24654
		public bool humanlikeOnly;

		// Token: 0x0400604F RID: 24655
		public ThoughtDef tasteThought;

		// Token: 0x04006050 RID: 24656
		public ThoughtDef specialThoughtDirect;

		// Token: 0x04006051 RID: 24657
		public ThoughtDef specialThoughtAsIngredient;

		// Token: 0x04006052 RID: 24658
		public HistoryEventDef ateEvent;

		// Token: 0x04006053 RID: 24659
		public EffecterDef ingestEffect;

		// Token: 0x04006054 RID: 24660
		public EffecterDef ingestEffectEat;

		// Token: 0x04006055 RID: 24661
		public SoundDef ingestSound;

		// Token: 0x04006056 RID: 24662
		[MustTranslate]
		public string ingestCommandString;

		// Token: 0x04006057 RID: 24663
		[MustTranslate]
		public string ingestReportString;

		// Token: 0x04006058 RID: 24664
		[MustTranslate]
		public string ingestReportStringEat;

		// Token: 0x04006059 RID: 24665
		public HoldOffsetSet ingestHoldOffsetStanding;

		// Token: 0x0400605A RID: 24666
		public bool ingestHoldUsesTable = true;

		// Token: 0x0400605B RID: 24667
		public bool tableDesired = true;

		// Token: 0x0400605C RID: 24668
		public bool showIngestFloatOption = true;

		// Token: 0x0400605D RID: 24669
		public FoodTypeFlags foodType;

		// Token: 0x0400605E RID: 24670
		public float joy;

		// Token: 0x0400605F RID: 24671
		public JoyKindDef joyKind;

		// Token: 0x04006060 RID: 24672
		public ThingDef sourceDef;

		// Token: 0x04006061 RID: 24673
		public FoodPreferability preferability;

		// Token: 0x04006062 RID: 24674
		public bool nurseable;

		// Token: 0x04006063 RID: 24675
		public float optimalityOffsetHumanlikes;

		// Token: 0x04006064 RID: 24676
		public float optimalityOffsetFeedingAnimals;

		// Token: 0x04006065 RID: 24677
		public DrugCategory drugCategory;

		// Token: 0x04006066 RID: 24678
		public bool canAutoSelectAsFoodForCaravan = true;

		// Token: 0x04006067 RID: 24679
		public bool lowPriorityCaravanFood;

		// Token: 0x04006068 RID: 24680
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
