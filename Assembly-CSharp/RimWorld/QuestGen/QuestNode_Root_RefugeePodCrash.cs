using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B82 RID: 15234
	public class QuestNode_Root_RefugeePodCrash : QuestNode_Root_WandererJoin
	{
		// Token: 0x06014B1D RID: 84765 RVA: 0x0062FEAC File Offset: 0x0062E0AC
		public override Pawn GeneratePawn()
		{
			Pawn pawn = ThingUtility.FindPawn(ThingSetMakerDefOf.RefugeePod.root.Generate());
			pawn.guest.Recruitable = true;
			return pawn;
		}

		// Token: 0x06014B1E RID: 84766 RVA: 0x0062FED0 File Offset: 0x0062E0D0
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelRefugeePodCrash".Translate();
			TaggedString taggedString = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			taggedString += "\n\n";
			if (pawn.Faction == null)
			{
				taggedString += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				taggedString += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else
			{
				taggedString += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			if (pawn.DevelopmentalStage.Juvenile())
			{
				string arg = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod(true, false, true, true, false);
				taggedString += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), arg.Named("AGE"));
			}
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, new TargetInfo(pawn), null, null, null, null, 0, true);
		}
	}
}
