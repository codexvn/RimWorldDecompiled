using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DD2 RID: 15826
	public class SettlementUtility
	{
		// Token: 0x06015954 RID: 88404 RVA: 0x006763F8 File Offset: 0x006745F8
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			if (faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Settlement settlement = maps[i].info.parent as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015955 RID: 88405 RVA: 0x0067645C File Offset: 0x0067465C
		public static void Attack(Caravan caravan, Settlement settlement)
		{
			if (!settlement.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					SettlementUtility.AttackNow(caravan, settlement);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			SettlementUtility.AttackNow(caravan, settlement);
		}

		// Token: 0x06015956 RID: 88406 RVA: 0x006764B8 File Offset: 0x006746B8
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterCaravanEnteredEnemyBase".Translate(caravan.Label, settlement.Label.ApplyTag(TagType.Settlement, settlement.Faction.GetUniqueLoadID())).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, caravan.PawnsListForReading, settlement.Faction, null, null, null, 0, true);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
			Find.GoodwillSituationManager.RecalculateAll(true);
		}

		// Token: 0x06015957 RID: 88407 RVA: 0x006765AC File Offset: 0x006747AC
		public static void AffectRelationsOnAttacked(MapParent mapParent, ref TaggedString letterText)
		{
			if (mapParent.Faction != null && mapParent.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = mapParent.Faction.PlayerRelationKind;
				Faction.OfPlayer.TryAffectGoodwillWith(mapParent.Faction, Faction.OfPlayer.GoodwillToMakeHostile(mapParent.Faction), false, false, HistoryEventDefOf.AttackedSettlement, null);
				mapParent.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, mapParent.Faction.PlayerRelationKind, null);
			}
		}
	}
}
