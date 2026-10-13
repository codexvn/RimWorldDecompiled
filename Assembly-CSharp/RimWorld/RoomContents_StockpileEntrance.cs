using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F25 RID: 7973
	public class RoomContents_StockpileEntrance : RoomContentsWorker
	{
		// Token: 0x0600B032 RID: 45106 RVA: 0x003234FC File Offset: 0x003216FC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnExit(map, room);
			this.SpawnTurrets(map, room, faction);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600B033 RID: 45107 RVA: 0x0032351C File Offset: 0x0032171C
		private void SpawnExit(Map map, LayoutRoom room)
		{
			List<Thing> list = new List<Thing>();
			ThingDef ancientHatchExit = ThingDefOf.AncientHatchExit;
			int count = 1;
			List<Thing> spawned = list;
			RoomGenUtility.FillWithPadding(ancientHatchExit, count, room, map, null, null, spawned, 3, null, false, false, null, null);
			MapGenerator.PlayerStartSpot = list.First<Thing>().Position;
		}

		// Token: 0x0600B034 RID: 45108 RVA: 0x00323560 File Offset: 0x00321760
		private void SpawnTurrets(Map map, LayoutRoom room, Faction faction)
		{
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientSecurityTurret, RoomContents_StockpileEntrance.TurretsRange.RandomInRange, IntRange.One, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, faction);
		}

		// Token: 0x040078C2 RID: 30914
		private static readonly IntRange TurretsRange = new IntRange(1, 2);
	}
}
