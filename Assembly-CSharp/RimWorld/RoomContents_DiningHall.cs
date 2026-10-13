using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED6 RID: 7894
	public class RoomContents_DiningHall : RoomContentsWorker
	{
		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600AEE8 RID: 44776 RVA: 0x0031E75A File Offset: 0x0031C95A
		protected virtual ThingDef TableDef
		{
			get
			{
				return ThingDefOf.Table2x4c;
			}
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600AEE9 RID: 44777 RVA: 0x0031E761 File Offset: 0x0031C961
		protected virtual ThingDef TableStuffDef
		{
			get
			{
				return ThingDefOf.Steel;
			}
		}

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600AEEA RID: 44778 RVA: 0x0031E768 File Offset: 0x0031C968
		protected virtual ThingDef ChairDef
		{
			get
			{
				return ThingDefOf.DiningChair;
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x0600AEEB RID: 44779 RVA: 0x0031E761 File Offset: 0x0031C961
		protected virtual ThingDef ChairStuffDef
		{
			get
			{
				return ThingDefOf.Steel;
			}
		}

		// Token: 0x0600AEEC RID: 44780 RVA: 0x0031E770 File Offset: 0x0031C970
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			List<Thing> spawned = new List<Thing>();
			RoomGenUtility.FillWithPadding(this.TableDef, this.GetCount(room), room, map, new Rot4?(value), null, spawned, 1, this.TableStuffDef, false, false, null, null);
			this.SpawnChairs(map, spawned);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AEED RID: 44781 RVA: 0x0031E7E0 File Offset: 0x0031C9E0
		private void SpawnChairs(Map map, List<Thing> spawned)
		{
			foreach (Thing thing in spawned)
			{
				CellRect cellRect = thing.OccupiedRect().ExpandedBy(1);
				float randomInRange = RoomContents_DiningHall.ChairsPerTableRange.RandomInRange;
				int num = 0;
				foreach (IntVec3 intVec in cellRect.EdgeCellsNoCorners.InRandomOrder(null))
				{
					if ((float)num < randomInRange)
					{
						Rot4 rot = Rot4.FromAngleFlat((thing.Position - intVec).AngleFlat);
						if (GenSpawn.CanSpawnAt(this.ChairDef, intVec, map, new Rot4?(rot), true))
						{
							GenSpawn.Spawn(ThingMaker.MakeThing(this.ChairDef, this.ChairStuffDef), intVec, map, rot, WipeMode.Vanish, false, false);
							num++;
						}
					}
				}
			}
		}

		// Token: 0x0600AEEE RID: 44782 RVA: 0x0031E8F4 File Offset: 0x0031CAF4
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			LayoutRoomDef roomDef = base.RoomDef;
			return Mathf.Max(Mathf.RoundToInt(((roomDef.itemsPer100CellsRange != null) ? roomDef.itemsPer100CellsRange.GetValueOrDefault().RandomInRange : 1f) * num), 1);
		}

		// Token: 0x04007836 RID: 30774
		private static readonly FloatRange ChairsPerTableRange = new FloatRange(-1f, 2f);
	}
}
