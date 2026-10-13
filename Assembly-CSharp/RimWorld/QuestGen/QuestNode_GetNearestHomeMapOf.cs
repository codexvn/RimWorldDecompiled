using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A5B RID: 14939
	public class QuestNode_GetNearestHomeMapOf : QuestNode
	{
		// Token: 0x060146DB RID: 83675 RVA: 0x0061A380 File Offset: 0x00618580
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060146DC RID: 83676 RVA: 0x0061A38A File Offset: 0x0061858A
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060146DD RID: 83677 RVA: 0x0061A398 File Offset: 0x00618598
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				Map mapHeld = this.mapOf.GetValue(slate).MapHeld;
				if (mapHeld != null && mapHeld.IsPlayerHome)
				{
					slate.Set<Map>(this.storeAs.GetValue(slate), mapHeld, false);
					return;
				}
				PlanetTile tile = this.mapOf.GetValue(slate).Tile;
				if (tile.Valid)
				{
					Map map = null;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						if (maps[i].IsPlayerHome && (map == null || Find.WorldGrid.ApproxDistanceInTiles(tile, maps[i].Tile) < Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile)))
						{
							map = maps[i];
						}
					}
					if (map != null)
					{
						slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
					}
				}
			}
		}

		// Token: 0x0400E1E9 RID: 57833
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x0400E1EA RID: 57834
		public SlateRef<Thing> mapOf;
	}
}
