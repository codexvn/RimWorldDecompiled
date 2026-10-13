using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x02003DF1 RID: 15857
	public class SitePartWorker_DownedRefugee : SitePartWorker
	{
		// Token: 0x06015A26 RID: 88614 RVA: 0x00679D44 File Offset: 0x00677F44
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			PawnKindDef pawnKind = slate.Get<PawnKindDef>("refugeeKind", null, false);
			float chanceForFaction = slate.Exists("refugeeFactionChance", false) ? 0.6f : slate.Get<float>("refugeeFactionChance", 0f, false);
			Pawn pawn = DownedRefugeeQuestUtility.GenerateRefugee(part.site.Tile, pawnKind, chanceForFaction);
			part.things = new ThingOwner<Pawn>(part, true, LookMode.Deep, true);
			part.things.TryAdd(pawn, true);
			if (pawn.relations != null)
			{
				pawn.relations.everSeenByPlayer = true;
			}
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative != null)
			{
				PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
				TaggedString taggedString = "";
				if (mostImportantRelation != null && mostImportantRelation.opinionOffset > 0)
				{
					pawn.relations.relativeInvolvedInRescueQuest = mostImportantColonyRelative;
					taggedString = "\n\n" + "RelatedPawnInvolvedInQuest".Translate(mostImportantColonyRelative.LabelShort, mostImportantRelation.GetGenderSpecificLabel(pawn), mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
				}
				else
				{
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
				}
				outExtraDescriptionRules.Add(new Rule_String("pawnInvolvedInQuestInfo", taggedString));
			}
			slate.Set<Pawn>("refugee", pawn, false);
		}

		// Token: 0x06015A27 RID: 88615 RVA: 0x00679E94 File Offset: 0x00678094
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			string text = base.GetPostProcessedThreatLabel(site, sitePart);
			if (sitePart.things != null && sitePart.things.Any)
			{
				text = text + ": " + sitePart.things[0].LabelShortCap;
			}
			if (site.HasWorldObjectTimeout)
			{
				text += " (" + "DurationLeft".Translate(site.WorldObjectTimeoutTicksLeft.ToStringTicksToPeriod(true, false, true, true, false)) + ")";
			}
			return text;
		}

		// Token: 0x06015A28 RID: 88616 RVA: 0x00679F24 File Offset: 0x00678124
		public override void PostDestroy(SitePart sitePart)
		{
			base.PostDestroy(sitePart);
			if (sitePart.things != null && sitePart.things.Any)
			{
				Pawn pawn = (Pawn)sitePart.things[0];
				if (!pawn.Dead)
				{
					if (pawn.relations != null)
					{
						pawn.relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(pawn);
				}
			}
		}
	}
}
