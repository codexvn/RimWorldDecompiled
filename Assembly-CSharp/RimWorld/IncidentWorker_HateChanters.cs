using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001DFB RID: 7675
	public class IncidentWorker_HateChanters : IncidentWorker
	{
		// Token: 0x0600AAE9 RID: 43753 RVA: 0x0030DB38 File Offset: 0x0030BD38
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfHoraxCult;
			parms.sendLetter = false;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkInHateChanters;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms.pawnGroupKind ?? PawnGroupKindDefOf.PsychicRitualSiege, parms, false);
			float num = Faction.OfHoraxCult.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			LordJob_HateChant lordJob_HateChant = new LordJob_HateChant();
			lordJob_HateChant.smallGroup = (list.Count < 20);
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			Lord lord = LordMaker.MakeNewLord(Faction.OfHoraxCult, lordJob_HateChant, (Map)parms.target, list);
			TaggedString text = "HateChantersText".Translate();
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			TaggedString label = "HateChantersLabel".Translate();
			TaggedString taggedString = "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, taggedString, true, true);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, lord.ownedPawns, null, null, null, null, 0, true);
			return true;
		}

		// Token: 0x040075B3 RID: 30131
		public const int SmallGroupSize = 20;
	}
}
