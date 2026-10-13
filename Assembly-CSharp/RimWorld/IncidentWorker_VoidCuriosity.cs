using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE1 RID: 7649
	public class IncidentWorker_VoidCuriosity : IncidentWorker
	{
		// Token: 0x0600AA84 RID: 43652 RVA: 0x0030C38C File Offset: 0x0030A58C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (Find.Anomaly.hasPerformedVoidProvocation)
			{
				return false;
			}
			return PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn x) => !x.DevelopmentalStage.Baby());
		}

		// Token: 0x0600AA85 RID: 43653 RVA: 0x0030C3DC File Offset: 0x0030A5DC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Pawn pawn;
			if (!PawnsFinder.HomeMaps_FreeColonistsSpawned.TryRandomElement((Pawn x) => !x.DevelopmentalStage.Baby(), out pawn))
			{
				return false;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.VoidCuriosity, null, null);
			TaggedString baseLetterText = ResearchProjectDefOf.BasicPsychicRituals.IsFinished ? "VoidCuriosityText_Researched".Translate(pawn.Named("PAWN")) : "VoidCuriosityText_NotResearched".Translate(pawn.Named("PAWN"));
			base.SendStandardLetter("VoidCuriosityLabel".Translate(), baseLetterText, LetterDefOf.NeutralEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
