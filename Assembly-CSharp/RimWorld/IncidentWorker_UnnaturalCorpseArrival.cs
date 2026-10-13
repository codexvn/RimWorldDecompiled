using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE0 RID: 7648
	public class IncidentWorker_UnnaturalCorpseArrival : IncidentWorker
	{
		// Token: 0x0600AA81 RID: 43649 RVA: 0x0030C2AC File Offset: 0x0030A4AC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map == null)
			{
				return false;
			}
			Pawn pawn;
			if (!QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				return false;
			}
			UnnaturalCorpse unnaturalCorpse = AnomalyUtility.MakeUnnaturalCorpse(pawn);
			Find.Anomaly.RegisterUnnaturalCorpse(pawn, unnaturalCorpse);
			IntVec3 loc;
			RCellFinder.TryFindRandomSpotJustOutsideColony(IntVec3.Invalid, map, out loc);
			GenSpawn.Spawn(unnaturalCorpse, loc, map, WipeMode.Vanish);
			EffecterDefOf.Skip_EntryNoDelay.Spawn(unnaturalCorpse, map, 1f).Cleanup();
			base.SendStandardLetter("UnnaturalCorpseArrivalLabel".Translate(), "UnnaturalCorpseArrivalText".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatSmall, parms, unnaturalCorpse, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600AA82 RID: 43650 RVA: 0x0030C35A File Offset: 0x0030A55A
		protected bool ValidatePawn(Pawn pawn)
		{
			return (pawn.IsColonist || pawn.IsSlaveOfColony) && pawn.RaceProps.unnaturalCorpseDef != null && !Find.Anomaly.PawnHasUnnaturalCorpse(pawn);
		}
	}
}
