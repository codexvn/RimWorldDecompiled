using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E01 RID: 7681
	public class IncidentWorker_PsychicRitualSiege : IncidentWorker_RaidEnemy
	{
		// Token: 0x0600AB03 RID: 43779 RVA: 0x0030E394 File Offset: 0x0030C594
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (parms.psychicRitualDef == null && !(from x in DefDatabase<PsychicRitualDef>.AllDefs
			where x.aiCastable && x.minThreatPoints <= parms.points
			select x).TryRandomElement(out parms.psychicRitualDef))
			{
				return false;
			}
			parms.pawnGroupKind = PawnGroupKindDefOf.PsychicRitualSiege;
			return base.TryExecuteWorker(parms);
		}

		// Token: 0x0600AB04 RID: 43780 RVA: 0x0030E40A File Offset: 0x0030C60A
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			if (Faction.OfHoraxCult != null)
			{
				parms.faction = Faction.OfHoraxCult;
				return true;
			}
			return false;
		}

		// Token: 0x0600AB05 RID: 43781 RVA: 0x0030E421 File Offset: 0x0030C621
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			parms.raidStrategy = RaidStrategyDefOf.PsychicRitualSiege;
		}

		// Token: 0x0600AB06 RID: 43782 RVA: 0x0030E42E File Offset: 0x0030C62E
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		// Token: 0x0600AB07 RID: 43783 RVA: 0x0030E43C File Offset: 0x0030C63C
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			TaggedString t = parms.raidArrivalMode.textEnemy.Formatted(parms.faction.def.pawnsPlural, parms.faction.NameColored).CapitalizeFirst();
			t += "\n\n";
			if (!parms.psychicRitualDef.letterAIArrivedText.NullOrEmpty())
			{
				t += parms.psychicRitualDef.letterAIArrivedText;
			}
			else
			{
				t += parms.raidStrategy.arrivalTextEnemy.Formatted(parms.psychicRitualDef.label.Named("RITUAL"));
			}
			t += "\n\n" + "AttackersMayCastRitualMultipleTimes".Translate();
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				t += "\n\n";
				t += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER")).Resolve();
			}
			if (parms.raidAgeRestriction != null && !parms.raidAgeRestriction.arrivalTextExtra.NullOrEmpty())
			{
				t += "\n\n";
				t += parms.raidAgeRestriction.arrivalTextExtra.Formatted(parms.faction.def.pawnsPlural.Named("PAWNSPLURAL")).Resolve();
			}
			return t.Resolve();
		}
	}
}
