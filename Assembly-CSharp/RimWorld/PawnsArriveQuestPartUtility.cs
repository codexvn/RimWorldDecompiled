using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF6 RID: 7414
	public static class PawnsArriveQuestPartUtility
	{
		// Token: 0x0600A598 RID: 42392 RVA: 0x002F6BAF File Offset: 0x002F4DAF
		public static IEnumerable<Pawn> GetQuestLookTargets(IEnumerable<Pawn> pawns)
		{
			if (pawns.Count<Pawn>() == 1)
			{
				yield return pawns.First<Pawn>();
				yield break;
			}
			foreach (Pawn p in pawns)
			{
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					yield return p;
				}
				if (p.Faction == null && p.Downed)
				{
					yield return p;
				}
				p = null;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600A599 RID: 42393 RVA: 0x002F6BC0 File Offset: 0x002F4DC0
		public static bool IncreasesPopulation(IEnumerable<Pawn> pawns, bool joinPlayer, bool makePrisoners)
		{
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Destroyed && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || pawn.Downed || joinPlayer || makePrisoners))
				{
					return true;
				}
			}
			return false;
		}
	}
}
