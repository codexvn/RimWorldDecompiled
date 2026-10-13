using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020025A3 RID: 9635
	public static class NamePlayerFactionAndSettlementUtility
	{
		// Token: 0x0600D321 RID: 54049 RVA: 0x003E2F11 File Offset: 0x003E1111
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x0600D322 RID: 54050 RVA: 0x003E2F22 File Offset: 0x003E1122
		public static bool CanNameSettlementNow(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x0600D323 RID: 54051 RVA: 0x003E2F3B File Offset: 0x003E113B
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x0600D324 RID: 54052 RVA: 0x003E2F52 File Offset: 0x003E1152
		public static bool CanNameSettlementSoon(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x0600D325 RID: 54053 RVA: 0x003E2F71 File Offset: 0x003E1171
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 4.3f && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x0600D326 RID: 54054 RVA: 0x003E2F98 File Offset: 0x003E1198
		private static bool CanNameSettlement(Settlement factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 4.3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x0600D327 RID: 54055 RVA: 0x003E3000 File Offset: 0x003E1200
		private static bool CanNameAnythingNow()
		{
			if (Find.AnyPlayerHomeMap == null || Find.CurrentMap == null || !Find.CurrentMap.IsPlayerHome || Find.GameEnder.gameEnding)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					if (maps[i].mapPawns.FreeColonistsSpawnedCount >= 2)
					{
						flag = true;
					}
					if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x, false)))
					{
						flag2 = true;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x04008F43 RID: 36675
		private const float MinDaysPassedToNameFaction = 4.3f;

		// Token: 0x04008F44 RID: 36676
		private const float MinDaysPassedToNameSettlement = 4.3f;

		// Token: 0x04008F45 RID: 36677
		private const int SoonTicks = 30000;
	}
}
