using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F69 RID: 3945
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06006185 RID: 24965 RVA: 0x001EDCFC File Offset: 0x001EBEFC
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled && !pawn.Downed && pawn.Spawned && !pawn.IsQuestLodger() && (pawn.guest == null || pawn.guest.Recruitable) && base.BreakCanOccur(pawn) && !pawn.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && (!ModsConfig.BiotechActive || !pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)) && pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def, 7f);
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x001EDDA0 File Offset: 0x001EBFA0
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "RanWild", pawn.Named("SUBJECT"));
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false, true);
			}
			return true;
		}
	}
}
