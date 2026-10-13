using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A4D RID: 14925
	public class QuestNode_GetMap : QuestNode
	{
		// Token: 0x060146A6 RID: 83622 RVA: 0x00619014 File Offset: 0x00617214
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return true;
			}
			if (this.TryFindMap(slate, out map))
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
				return true;
			}
			return false;
		}

		// Token: 0x060146A7 RID: 83623 RVA: 0x00619068 File Offset: 0x00617268
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map;
			if (QuestGen.slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return;
			}
			if (this.TryFindMap(slate, out map))
			{
				QuestGen.slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
			}
		}

		// Token: 0x060146A8 RID: 83624 RVA: 0x006190C4 File Offset: 0x006172C4
		private bool TryFindMap(Slate slate, out Map map)
		{
			int minCount;
			if (!this.preferMapWithMinFreeColonists.TryGetValue(slate, out minCount) || minCount < 1)
			{
				minCount = 1;
			}
			IEnumerable<Map> source = from x in Find.Maps
			where x.IsPlayerHome && this.IsAcceptableMap(x, slate)
			select x;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= minCount
			select x).TryRandomElement(out map))
			{
				return (from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out map);
			}
			return true;
		}

		// Token: 0x060146A9 RID: 83625 RVA: 0x0061916C File Offset: 0x0061736C
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			if (map == null)
			{
				return false;
			}
			if (map.IsPocketMap)
			{
				return false;
			}
			if (!this.canBeSpace.GetValue(slate) && map.Tile.Valid && map.Tile.LayerDef.isSpace)
			{
				return false;
			}
			IntVec3 intVec;
			if (this.mustBeInfestable.GetValue(slate) && !InfestationCellFinder.TryFindCell(out intVec, map))
			{
				return false;
			}
			List<PlanetLayerDef> value = this.layerWhitelist.GetValue(slate);
			List<PlanetLayerDef> value2 = this.layerBlacklist.GetValue(slate);
			return (value.NullOrEmpty<PlanetLayerDef>() || !map.Tile.Valid || value.Contains(map.Tile.LayerDef)) && (value2.NullOrEmpty<PlanetLayerDef>() || !map.Tile.Valid || !value2.Contains(map.Tile.LayerDef));
		}

		// Token: 0x0400E1B8 RID: 57784
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x0400E1B9 RID: 57785
		public SlateRef<bool> mustBeInfestable;

		// Token: 0x0400E1BA RID: 57786
		public SlateRef<bool> canBeSpace;

		// Token: 0x0400E1BB RID: 57787
		public SlateRef<int> preferMapWithMinFreeColonists;

		// Token: 0x0400E1BC RID: 57788
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x0400E1BD RID: 57789
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
