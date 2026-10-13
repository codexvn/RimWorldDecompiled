using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002595 RID: 9621
	public static class FactionUtility
	{
		// Token: 0x0600D2DE RID: 53982 RVA: 0x003E1989 File Offset: 0x003DFB89
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x0600D2DF RID: 53983 RVA: 0x003E19A7 File Offset: 0x003DFBA7
		public static bool AllyOrNeutralTo(this Faction fac, Faction other)
		{
			return !fac.HostileTo(other);
		}

		// Token: 0x0600D2E0 RID: 53984 RVA: 0x003E19B4 File Offset: 0x003DFBB4
		public static AcceptanceReport CanTradeWith(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			if (p.skills == null || p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return new AcceptanceReport("IncapableOfCapacity".Translate(SkillDefOf.Social.label));
			}
			if (faction != null)
			{
				if (faction.HostileTo(p.Faction))
				{
					return AcceptanceReport.WasRejected;
				}
				if (traderKind == null || traderKind.permitRequiredForTrading == null)
				{
					return AcceptanceReport.WasAccepted;
				}
				if (p.royalty == null || !p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction))
				{
					return new AcceptanceReport("MessageNeedRoyalTitleToCallWithShip".Translate(traderKind.TitleRequiredToTrade));
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600D2E1 RID: 53985 RVA: 0x003E1A70 File Offset: 0x003DFC70
		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			if (ft == null)
			{
				return null;
			}
			if (ft.isPlayer)
			{
				return Faction.OfPlayer;
			}
			Faction result;
			if (!(from x in Find.FactionManager.AllFactions
			where x.def == ft
			select x).TryRandomElement(out result) && !(from x in Find.FactionManager.AllFactions
			where x.def.replacesFaction != null && x.def.replacesFaction == ft
			select x).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600D2E2 RID: 53986 RVA: 0x003E1AF2 File Offset: 0x003DFCF2
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}

		// Token: 0x0600D2E3 RID: 53987 RVA: 0x003E1B29 File Offset: 0x003DFD29
		public static bool IsPlayerSafe(this Faction faction)
		{
			return faction != null && faction.IsPlayer;
		}

		// Token: 0x0600D2E4 RID: 53988 RVA: 0x003E1B38 File Offset: 0x003DFD38
		public static void ResetAllFactionRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				faction.RemoveAllRelations();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (faction != faction2)
					{
						faction.TryMakeInitialRelationsWith(faction2);
					}
				}
			}
		}

		// Token: 0x0600D2E5 RID: 53989 RVA: 0x003E1BD8 File Offset: 0x003DFDD8
		public static int GetSlavesInFactionCount(Faction faction)
		{
			if (faction == null)
			{
				return 0;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_SpawnedPawnsInFaction(faction).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSlave)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
