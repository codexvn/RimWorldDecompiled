using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003769 RID: 14185
	public static class ThoughtUtility
	{
		// Token: 0x06013A89 RID: 80521 RVA: 0x005D732C File Offset: 0x005D552C
		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList<ThoughtDef>();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList<ThoughtDef>();
		}

		// Token: 0x06013A8A RID: 80522 RVA: 0x005D73A0 File Offset: 0x005D55A0
		public static void GiveThoughtsForPawnExecuted(Pawn victim, Pawn executioner, PawnExecutionKind kind)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			int num = 0;
			if (victim.guilt.IsGuilty)
			{
				num = 0;
			}
			else
			{
				switch (kind)
				{
				case PawnExecutionKind.GenericBrutal:
					num = 2;
					break;
				case PawnExecutionKind.GenericHumane:
					num = 1;
					break;
				case PawnExecutionKind.OrganHarvesting:
					num = 3;
					break;
				case PawnExecutionKind.Ripscanned:
					num = (ModsConfig.BiotechActive ? 4 : 3);
					break;
				}
			}
			if (victim.IsPrisoner)
			{
				if (((executioner != null) ? executioner.Faction : null) != null)
				{
					executioner.Faction.lastExecutionTick = Find.TickManager.TicksGame;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
				if (victim.guilt.IsGuilty)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerGuilty, executioner.Named(HistoryEventArgsNames.Doer)), true);
					return;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerInnocent, executioner.Named(HistoryEventArgsNames.Doer)), true);
				return;
			}
			else
			{
				if (victim.HostFaction != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedGuest, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
					return;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedColonist, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
				return;
			}
		}

		// Token: 0x06013A8B RID: 80523 RVA: 0x005D7514 File Offset: 0x005D5714
		public static void GiveThoughtsForPawnOrganHarvested(Pawn victim, Pawn billDoer)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			if (victim.needs.mood != null)
			{
				victim.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrgan, billDoer.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (billDoer.needs.mood != null)
			{
				Pawn_StoryTracker story = billDoer.story;
				if (((story != null) ? story.traits : null) != null && billDoer.story.traits.HasTrait(TraitDefOf.Bloodlust))
				{
					billDoer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarvestedOrgan_Bloodlust, null, null);
				}
			}
			if (victim.IsColonist)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromColonist, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				return;
			}
			if (victim.HostFaction == Faction.OfPlayer)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromGuest, billDoer.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x06013A8C RID: 80524 RVA: 0x005D7634 File Offset: 0x005D5834
		public static Gene NullifyingGene(ThoughtDef def, Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (def.nullifyingGenes != null && pawn.genes != null)
			{
				for (int i = 0; i < def.nullifyingGenes.Count; i++)
				{
					Gene gene = pawn.genes.GetGene(def.nullifyingGenes[i]);
					if (gene != null)
					{
						return gene;
					}
				}
			}
			return null;
		}

		// Token: 0x06013A8D RID: 80525 RVA: 0x005D768E File Offset: 0x005D588E
		public static Hediff NullifyingHediff(ThoughtDef def, Pawn pawn)
		{
			return pawn.health.hediffSet.ThoughtNullifyingHediff(def);
		}

		// Token: 0x06013A8E RID: 80526 RVA: 0x005D76A4 File Offset: 0x005D58A4
		public static bool NeverNullified(ThoughtDef def, Pawn pawn)
		{
			if (!def.neverNullifyIfAnyTrait.NullOrEmpty<TraitDef>())
			{
				for (int i = 0; i < def.neverNullifyIfAnyTrait.Count; i++)
				{
					if (pawn.story.traits.GetTrait(def.neverNullifyIfAnyTrait[i]) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06013A8F RID: 80527 RVA: 0x005D76F8 File Offset: 0x005D58F8
		public static Trait NullifyingTrait(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingTraits != null)
			{
				for (int i = 0; i < def.nullifyingTraits.Count; i++)
				{
					Trait trait = pawn.story.traits.GetTrait(def.nullifyingTraits[i]);
					if (trait != null)
					{
						return trait;
					}
				}
			}
			if (def.nullifyingTraitDegrees != null)
			{
				for (int j = 0; j < def.nullifyingTraitDegrees.Count; j++)
				{
					Trait trait2 = def.nullifyingTraitDegrees[j].GetTrait(pawn);
					if (trait2 != null)
					{
						return trait2;
					}
				}
			}
			return null;
		}

		// Token: 0x06013A90 RID: 80528 RVA: 0x005D777C File Offset: 0x005D597C
		public static TaleDef NullifyingTale(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingOwnTales != null)
			{
				for (int i = 0; i < def.nullifyingOwnTales.Count; i++)
				{
					if (Find.TaleManager.GetLatestTale(def.nullifyingOwnTales[i], pawn) != null)
					{
						return def.nullifyingOwnTales[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06013A91 RID: 80529 RVA: 0x005D77D0 File Offset: 0x005D59D0
		public static PreceptDef NullifyingPrecept(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingPrecepts != null)
			{
				for (int i = 0; i < def.nullifyingPrecepts.Count; i++)
				{
					if (pawn.Ideo != null && pawn.Ideo.HasPrecept(def.nullifyingPrecepts[i]))
					{
						return def.nullifyingPrecepts[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06013A92 RID: 80530 RVA: 0x005D782C File Offset: 0x005D5A2C
		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				flag = (((needs != null) ? needs.mood : null) != null);
			}
			if (!flag)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Thought_Memory thought) => thought.MoodOffset() > 0f);
		}

		// Token: 0x06013A93 RID: 80531 RVA: 0x005D78D0 File Offset: 0x005D5AD0
		public static bool CanGetThought(Pawn pawn, ThoughtDef def, bool checkIfNullified = false)
		{
			try
			{
				if (!def.developmentalStageFilter.Has(pawn.DevelopmentalStage))
				{
					return false;
				}
				if (def.gender != Gender.None && pawn.gender != def.gender && !def.IsSocial)
				{
					return false;
				}
				if (def.doNotApplyToQuestLodgers && pawn.IsQuestLodger())
				{
					return false;
				}
				if (def.minExpectation != null)
				{
					if (!pawn.Spawned)
					{
						return false;
					}
					ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(pawn.MapHeld);
					if (expectationDef != null && expectationDef.order < def.minExpectation.order)
					{
						return false;
					}
				}
				if (!def.validWhileDespawned && !pawn.Spawned && !def.IsMemory)
				{
					return false;
				}
				if (pawn.story.traits.IsThoughtDisallowed(def))
				{
					return false;
				}
				bool flag = false;
				bool flag2 = false;
				if (!def.requiredHediffs.NullOrEmpty<HediffDef>())
				{
					flag = true;
					for (int i = 0; i < def.requiredHediffs.Count; i++)
					{
						if (pawn.health.hediffSet.HasHediff(def.requiredHediffs[i], false))
						{
							flag2 = true;
							break;
						}
					}
				}
				if (!def.requiredTraits.NullOrEmpty<TraitDef>() && !flag2)
				{
					flag = true;
					for (int j = 0; j < def.requiredTraits.Count; j++)
					{
						if (pawn.story.traits.HasTrait(def.requiredTraits[j]) && (!def.RequiresSpecificTraitsDegree || def.requiredTraitsDegree == pawn.story.traits.DegreeOfTrait(def.requiredTraits[j])))
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag && !flag2)
				{
					return false;
				}
				if (ModsConfig.BiotechActive && !def.requiredGenes.NullOrEmpty<GeneDef>())
				{
					if (pawn.genes == null)
					{
						return false;
					}
					for (int k = 0; k < def.requiredGenes.Count; k++)
					{
						Gene gene = pawn.genes.GetGene(def.requiredGenes[k]);
						if (gene == null || !gene.Active)
						{
							return false;
						}
					}
				}
				if (def.nullifiedIfNotColonist && !pawn.IsColonist)
				{
					return false;
				}
				if (checkIfNullified && ThoughtUtility.ThoughtNullified(pawn, def))
				{
					return false;
				}
			}
			finally
			{
			}
			return true;
		}

		// Token: 0x06013A94 RID: 80532 RVA: 0x005D7B34 File Offset: 0x005D5D34
		public static bool ThoughtNullified(Pawn pawn, ThoughtDef def)
		{
			return !ThoughtUtility.NeverNullified(def, pawn) && (ThoughtUtility.NullifyingTrait(def, pawn) != null || ThoughtUtility.NullifyingHediff(def, pawn) != null || ThoughtUtility.NullifyingTale(def, pawn) != null || ThoughtUtility.NullifyingPrecept(def, pawn) != null || ThoughtUtility.NullifyingGene(def, pawn) != null);
		}

		// Token: 0x06013A95 RID: 80533 RVA: 0x005D7B84 File Offset: 0x005D5D84
		public static string ThoughtNullifiedMessage(Pawn pawn, ThoughtDef def)
		{
			if (ThoughtUtility.NeverNullified(def, pawn))
			{
				return "";
			}
			Trait trait = ThoughtUtility.NullifyingTrait(def, pawn);
			if (trait != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + trait.LabelCap;
			}
			Hediff hediff = ThoughtUtility.NullifyingHediff(def, pawn);
			if (hediff != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + hediff.def.LabelCap;
			}
			TaleDef taleDef = ThoughtUtility.NullifyingTale(def, pawn);
			if (taleDef != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + taleDef.LabelCap;
			}
			PreceptDef preceptDef = ThoughtUtility.NullifyingPrecept(def, pawn);
			if (preceptDef != null)
			{
				return "DisabledByPrecept".Translate(preceptDef.issue.LabelCap) + ": " + preceptDef.LabelCap;
			}
			Gene gene = ThoughtUtility.NullifyingGene(def, pawn);
			if (gene != null)
			{
				return "DisabledByGene".Translate() + ": " + gene.LabelCap;
			}
			return "";
		}

		// Token: 0x06013A96 RID: 80534 RVA: 0x005D7CC8 File Offset: 0x005D5EC8
		public static bool Witnessed(Pawn p, Pawn victim)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			if (victim.IsCaravanMember())
			{
				return victim.GetCaravan() == p.GetCaravan();
			}
			return victim.Spawned && p.Spawned && p.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, p.Position, victim.Map);
		}

		// Token: 0x06013A97 RID: 80535 RVA: 0x005D7D49 File Offset: 0x005D5F49
		public static IEnumerable<TraitRequirement> GetNullifyingTraits(ThoughtDef thoughtDef)
		{
			if (thoughtDef.nullifyingTraits != null)
			{
				int num;
				for (int i = 0; i < thoughtDef.nullifyingTraits.Count; i = num + 1)
				{
					yield return new TraitRequirement
					{
						def = thoughtDef.nullifyingTraits[i]
					};
					num = i;
				}
			}
			if (thoughtDef.nullifyingTraitDegrees != null)
			{
				int num;
				for (int i = 0; i < thoughtDef.nullifyingTraitDegrees.Count; i = num + 1)
				{
					yield return thoughtDef.nullifyingTraitDegrees[i];
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06013A98 RID: 80536 RVA: 0x005D7D5C File Offset: 0x005D5F5C
		public static string GenerateBabyTalk(string str)
		{
			string[] syllables = "BabyTalk".Translate().ToString().Split(',', StringSplitOptions.None);
			return GenText.CapitalizeSentences(Regex.Replace(str, "(\\p{L}|')+", (Match word) => string.Join("", Enumerable.Repeat<string>(syllables[Rand.RangeSeeded(0, syllables.Length, GenText.StableStringHash(word.ToString()))], (word.Length > 6) ? 2 : 1))), true);
		}

		// Token: 0x0400D9CD RID: 55757
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		// Token: 0x0400D9CE RID: 55758
		public static List<ThoughtDef> situationalNonSocialThoughtDefs;
	}
}
