using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001DF4 RID: 7668
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		// Token: 0x0600AACD RID: 43725 RVA: 0x0030D330 File Offset: 0x0030B530
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			List<Faction> list = (from x in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x, false)
			select x into p
			select ((Thing)p).Faction).Distinct<Faction>().ToList<Faction>();
			Faction faction = parms.faction;
			parms.faction = f;
			if (!RaidStrategyDefOf.ImmediateAttackFriendly.Worker.CanUseWith(parms, null))
			{
				parms.faction = faction;
				return false;
			}
			parms.faction = faction;
			return base.FactionCanBeGroupSource(f, parms, desperate) && !f.Hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!list.Any<Faction>() || list.Any((Faction hf) => hf.HostileTo(f)));
		}

		// Token: 0x0600AACE RID: 43726 RVA: 0x0030D434 File Offset: 0x0030B634
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			return (from p in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(p, false)
			select p).Sum(delegate(IAttackTarget p)
			{
				Pawn pawn = p as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				return 0f;
			}) > 120f;
		}

		// Token: 0x0600AACF RID: 43727 RVA: 0x0030D4B0 File Offset: 0x0030B6B0
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			if (parms.faction != null)
			{
				return true;
			}
			if (!base.CandidateFactions(parms, false).Any<Faction>())
			{
				return false;
			}
			parms.faction = base.CandidateFactions(parms, false).RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.00001f);
			return true;
		}

		// Token: 0x0600AAD0 RID: 43728 RVA: 0x0030D50B File Offset: 0x0030B70B
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			parms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		}

		// Token: 0x0600AAD1 RID: 43729 RVA: 0x0030D521 File Offset: 0x0030B721
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x0600AAD2 RID: 43730 RVA: 0x0030D541 File Offset: 0x0030B741
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly + ": " + parms.faction.Name;
		}

		// Token: 0x0600AAD3 RID: 43731 RVA: 0x0030D564 File Offset: 0x0030B764
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textFriendly, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction));
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "FriendlyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		// Token: 0x0600AAD4 RID: 43732 RVA: 0x0030D642 File Offset: 0x0030B842
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		// Token: 0x0600AAD5 RID: 43733 RVA: 0x0030D649 File Offset: 0x0030B849
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
