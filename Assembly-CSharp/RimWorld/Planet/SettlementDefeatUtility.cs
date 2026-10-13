using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DD1 RID: 15825
	public static class SettlementDefeatUtility
	{
		// Token: 0x06015951 RID: 88401 RVA: 0x0067605C File Offset: 0x0067425C
		public static void CheckDefeated(Settlement factionBase)
		{
			if (factionBase.Faction == Faction.OfPlayer)
			{
				return;
			}
			Map map = factionBase.Map;
			if (map == null || !SettlementDefeatUtility.IsDefeated(map, factionBase.Faction))
			{
				return;
			}
			IdeoUtility.Notify_PlayerRaidedSomeone(map.mapPawns.FreeColonistsSpawned);
			DestroyedSettlement destroyedSettlement = (DestroyedSettlement)WorldObjectMaker.MakeWorldObject(factionBase.Tile.LayerDef.DestroyedSettlementWorldObjectDef);
			destroyedSettlement.Tile = factionBase.Tile;
			destroyedSettlement.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(destroyedSettlement);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = SettlementDefeatUtility.HasAnyOtherBase(factionBase);
			TimedDetectionRaids timedDetectionRaids;
			if (flag && destroyedSettlement.TryGetComponent<TimedDetectionRaids>(out timedDetectionRaids))
			{
				timedDetectionRaids.CopyFrom(factionBase.GetComponent<TimedDetectionRaids>());
				timedDetectionRaids.SetNotifiedSilently();
				if (!string.IsNullOrEmpty(timedDetectionRaids.DetectionCountdownTimeLeftString))
				{
					stringBuilder.Append("LetterFactionBaseDefeated".Translate(factionBase.Label, timedDetectionRaids.DetectionCountdownTimeLeftString));
				}
				else
				{
					stringBuilder.Append("LetterFactionBaseDefeatedNoRaids".Translate(factionBase.Label));
				}
			}
			else
			{
				stringBuilder.Append("LetterFactionBaseDefeatedNoRaids".Translate(factionBase.Label));
			}
			if (!flag)
			{
				factionBase.Faction.defeated = true;
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("LetterFactionBaseDefeated_FactionDestroyed".Translate(factionBase.Faction.Name));
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.Hidden && !faction.IsPlayer && faction != factionBase.Faction && faction.HostileTo(factionBase.Faction))
				{
					FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
					Faction.OfPlayer.TryAffectGoodwillWith(faction, 20, false, false, HistoryEventDefOf.DestroyedEnemyBase, null);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("RelationsWith".Translate(faction.Name) + ": " + 20.ToStringWithSign());
					faction.TryAppendRelationKindChangedInfo(stringBuilder, playerRelationKind, faction.PlayerRelationKind, null);
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseDefeated".Translate(), stringBuilder.ToString(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(factionBase.Tile), factionBase.Faction, null, null, null, 0, true);
			map.info.parent = destroyedSettlement;
			factionBase.Destroy();
			TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
			{
				map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
		}

		// Token: 0x06015952 RID: 88402 RVA: 0x00676344 File Offset: 0x00674544
		public static bool IsDefeated(Map map, Faction faction)
		{
			List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				if (pawn.RaceProps.Humanlike && GenHostility.IsActiveThreatToPlayer(pawn, map.Parent is Settlement && map.Parent.Faction == faction))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06015953 RID: 88403 RVA: 0x006763B0 File Offset: 0x006745B0
		private static bool HasAnyOtherBase(Settlement defeatedFactionBase)
		{
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction == defeatedFactionBase.Faction && settlement != defeatedFactionBase)
				{
					return true;
				}
			}
			return false;
		}
	}
}
