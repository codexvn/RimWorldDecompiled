using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B84 RID: 15236
	public class QuestNode_Root_RefugeePodCrash_Ghoul : QuestNode_Root_WandererJoin
	{
		// Token: 0x06014B25 RID: 84773 RVA: 0x006303F8 File Offset: 0x0062E5F8
		public override Pawn GeneratePawn()
		{
			Pawn pawn;
			if (Find.World.worldPawns.GetPawnsBySituationCount(WorldPawnSituation.StartingPawnLeftBehind) > 0)
			{
				pawn = Find.World.worldPawns.GetPawnsBySituation(WorldPawnSituation.StartingPawnLeftBehind).RandomElement<Pawn>();
				MutantUtility.SetFreshPawnAsMutant(pawn, MutantDefOf.Ghoul);
				pawn.SetFaction(Faction.OfEntities, null);
				this.wasStartingPawn = true;
			}
			else
			{
				pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Ghoul, Faction.OfEntities, null);
			}
			pawn.health.AddHediff(HediffDefOf.DarkPsychicShock, null, null, null).TryGetComp<HediffComp_Disappears>().ticksToDisappear = QuestNode_Root_RefugeePodCrash_Ghoul.ShockTicksRange.RandomInRange;
			return pawn;
		}

		// Token: 0x06014B26 RID: 84774 RVA: 0x0063049C File Offset: 0x0062E69C
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "GhoulPodCrashLabel".Translate();
			TaggedString text = this.wasStartingPawn ? "GhoulPodCrashStartingPawnText".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true) : "GhoulPodCrashText".Translate();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatSmall, new TargetInfo(pawn), null, null, null, null, 0, true);
		}

		// Token: 0x0400E5F4 RID: 58868
		private static readonly IntRange ShockTicksRange = new IntRange(10000, 15000);

		// Token: 0x0400E5F5 RID: 58869
		private bool wasStartingPawn;
	}
}
