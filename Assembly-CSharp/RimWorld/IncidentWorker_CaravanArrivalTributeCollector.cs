using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E10 RID: 7696
	public class IncidentWorker_CaravanArrivalTributeCollector : IncidentWorker_TraderCaravanArrival
	{
		// Token: 0x0600AB51 RID: 43857 RVA: 0x0030FBB0 File Offset: 0x0030DDB0
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			if (Faction.OfEmpire == null)
			{
				return false;
			}
			Map map = (Map)parms.target;
			parms.faction = Faction.OfEmpire;
			parms.traderKind = (from t in DefDatabase<TraderKindDef>.AllDefsListForReading
			where t.category == "TributeCollector"
			select t).RandomElementByWeight((TraderKindDef t) => this.TraderKindCommonality(t, map, parms.faction));
			return true;
		}

		// Token: 0x0600AB52 RID: 43858 RVA: 0x0030FC56 File Offset: 0x0030DE56
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Faction.OfEmpire != null && this.FactionCanBeGroupSource(Faction.OfEmpire, parms, false);
		}

		// Token: 0x0600AB53 RID: 43859 RVA: 0x0030A408 File Offset: 0x00308608
		protected override float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x0600AB54 RID: 43860 RVA: 0x0030FC78 File Offset: 0x0030DE78
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTributeCollectorArrival".Translate().CapitalizeFirst();
			TaggedString taggedString = "LetterTributeCollectorArrival".Translate(parms.faction.Named("FACTION")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x040075DC RID: 30172
		public const string TributeCollectorTraderKindCategory = "TributeCollector";
	}
}
