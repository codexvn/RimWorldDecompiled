using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A59 RID: 14937
	public class QuestNode_GetNearbySettlement : QuestNode
	{
		// Token: 0x060146D5 RID: 83669 RVA: 0x00619F0C File Offset: 0x0061810C
		private Settlement RandomNearbyTradeableSettlement(PlanetTile originTile, Slate slate)
		{
			QuestNode_GetNearbySettlement.<>c__DisplayClass0_0 CS$<>8__locals1 = new QuestNode_GetNearbySettlement.<>c__DisplayClass0_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slate = slate;
			CS$<>8__locals1.originTile = originTile;
			return Find.WorldObjects.SettlementBases.Where(new Func<Settlement, bool>(CS$<>8__locals1.<RandomNearbyTradeableSettlement>g__Validator|0)).RandomElementWithFallback(null);
		}

		// Token: 0x060146D6 RID: 83670 RVA: 0x00619F58 File Offset: 0x00618158
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			QuestGen.slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
			if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
			{
				QuestGen.slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
			}
			if (!this.storeFactionLeaderAs.GetValue(slate).NullOrEmpty())
			{
				QuestGen.slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
			}
			if (!this.storeCanCaravanAs.GetValue(slate).NullOrEmpty())
			{
				bool var = settlement.Tile.Valid && map.Tile.Valid && settlement.Tile.Layer == map.Tile.Layer && settlement.Tile.LayerDef.SurfaceTiles;
				QuestGen.slate.Set<bool>(this.storeCanCaravanAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x060146D7 RID: 83671 RVA: 0x0061A090 File Offset: 0x00618290
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			if (settlement != null)
			{
				slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
				if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
				{
					slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
				}
				if (!string.IsNullOrEmpty(this.storeFactionLeaderAs.GetValue(slate)))
				{
					slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400E1DC RID: 57820
		public SlateRef<bool> allowActiveTradeRequest = true;

		// Token: 0x0400E1DD RID: 57821
		public SlateRef<bool> canBeSpace;

		// Token: 0x0400E1DE RID: 57822
		public SlateRef<bool> requireSameOrAdjacentLayer;

		// Token: 0x0400E1DF RID: 57823
		public SlateRef<float> maxTileDistance;

		// Token: 0x0400E1E0 RID: 57824
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1E1 RID: 57825
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x0400E1E2 RID: 57826
		[NoTranslate]
		public SlateRef<string> storeFactionLeaderAs;

		// Token: 0x0400E1E3 RID: 57827
		[NoTranslate]
		public SlateRef<string> storeCanCaravanAs;

		// Token: 0x0400E1E4 RID: 57828
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x0400E1E5 RID: 57829
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
