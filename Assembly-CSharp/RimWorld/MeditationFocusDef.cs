using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8B RID: 6795
	public class MeditationFocusDef : Def
	{
		// Token: 0x0600985A RID: 39002 RVA: 0x002C714D File Offset: 0x002C534D
		public bool CanPawnUse(Pawn p)
		{
			return MeditationFocusTypeAvailabilityCache.PawnCanUse(p, this);
		}

		// Token: 0x0600985B RID: 39003 RVA: 0x002C7158 File Offset: 0x002C5358
		public string EnablingThingsExplanation(Pawn pawn)
		{
			MeditationFocusDef.tmpReasons.Clear();
			if (this.requiresRoyalTitle && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				RoyalTitle royalTitle = pawn.royalty.AllTitlesInEffectForReading.MaxBy((RoyalTitle t) => t.def.seniority);
				MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByTitle".Translate(royalTitle.def.GetLabelCapFor(pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).Resolve());
			}
			if (pawn.story != null)
			{
				BackstoryDef adulthood = pawn.story.Adulthood;
				BackstoryDef childhood = pawn.story.Childhood;
				if (!this.requiresRoyalTitle && this.requiredBackstoriesAny.Count == 0)
				{
					for (int i = 0; i < this.incompatibleBackstoriesAny.Count; i++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = this.incompatibleBackstoriesAny[i];
						BackstoryDef backstoryDef = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
						if (!backstoryDef.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName))
						{
							MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|5_0(backstoryCategoryAndSlot.slot, backstoryDef);
						}
					}
					for (int j = 0; j < DefDatabase<TraitDef>.AllDefsListForReading.Count; j++)
					{
						TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[j];
						List<MeditationFocusDef> disallowedMeditationFocusTypes = traitDef.degreeDatas[0].disallowedMeditationFocusTypes;
						if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(this))
						{
							MeditationFocusDef.tmpReasons.Add("MeditationFocusDisabledByTrait".Translate() + ": " + traitDef.degreeDatas[0].GetLabelCapFor(pawn) + ".");
						}
					}
				}
				for (int k = 0; k < this.requiredBackstoriesAny.Count; k++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = this.requiredBackstoriesAny[k];
					BackstoryDef backstoryDef2 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
					if (backstoryDef2.spawnCategories.Contains(backstoryCategoryAndSlot2.categoryName))
					{
						MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|5_0(backstoryCategoryAndSlot2.slot, backstoryDef2);
					}
				}
				for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
				{
					Trait trait = pawn.story.traits.allTraits[l];
					if (!trait.Suppressed)
					{
						List<MeditationFocusDef> allowedMeditationFocusTypes = trait.CurrentData.allowedMeditationFocusTypes;
						if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(this))
						{
							MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByTrait".Translate() + ": " + trait.LabelCap + ".");
						}
					}
				}
			}
			for (int m = 0; m < pawn.health.hediffSet.hediffs.Count; m++)
			{
				HediffDef def = pawn.health.hediffSet.hediffs[m].def;
				if (def.allowedMeditationFocusTypes.NotNullAndContains(this))
				{
					MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByHediff".Translate() + ": " + def.LabelCap + ".");
				}
			}
			return MeditationFocusDef.tmpReasons.ToLineList("  - ", true);
		}

		// Token: 0x0600985E RID: 39006 RVA: 0x002C74F8 File Offset: 0x002C56F8
		[CompilerGenerated]
		internal static void <EnablingThingsExplanation>g__AddBackstoryReason|5_0(BackstorySlot slot, BackstoryDef backstory)
		{
			if (slot == BackstorySlot.Adulthood)
			{
				MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByAdulthood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
				return;
			}
			MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByChildhood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
		}

		// Token: 0x0400665E RID: 26206
		public bool requiresRoyalTitle;

		// Token: 0x0400665F RID: 26207
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x04006660 RID: 26208
		public List<BackstoryCategoryAndSlot> incompatibleBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x04006661 RID: 26209
		private static List<string> tmpReasons = new List<string>();
	}
}
