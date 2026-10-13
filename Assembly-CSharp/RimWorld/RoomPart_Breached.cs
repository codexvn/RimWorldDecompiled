using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4E RID: 8014
	public class RoomPart_Breached : RoomPartWorker
	{
		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600B132 RID: 45362 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool FillOnPost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B133 RID: 45363 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_Breached(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B134 RID: 45364 RVA: 0x0032A028 File Offset: 0x00328228
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (!RoomPart_Breached.CanRemoveWalls(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				int num = Rand.Range(0, 4);
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i + num);
					CellRect edgeRect = cellRect.GetEdgeRect(rot);
					bool flag = true;
					foreach (IntVec3 a in edgeRect.Cells)
					{
						for (int j = 0; j < 4; j++)
						{
							IntVec3 intVec = a + GenAdj.CardinalDirections[j];
							LayoutRoom room2;
							if (!room.Contains(intVec, 0) && room.sketch.structureLayout.TryGetRoom(intVec, out room2) && !RoomPart_Breached.CanRemoveWalls(room2))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						this.DestroyWall(map, edgeRect, rot);
						return;
					}
				}
			}
		}

		// Token: 0x0600B135 RID: 45365 RVA: 0x0032A158 File Offset: 0x00328358
		private static bool CanRemoveWalls(LayoutRoom room)
		{
			if (room.requiredDef != null)
			{
				return false;
			}
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				if (layoutRoomDef.canRemoveBorderDoors || layoutRoomDef.canRemoveBorderWalls)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B136 RID: 45366 RVA: 0x0032A1C8 File Offset: 0x003283C8
		private void DestroyWall(Map map, CellRect edge, Rot4 rot)
		{
			int num = edge.GetSideLength(rot) / 2 - 2;
			if (num <= 2)
			{
				return;
			}
			int i = Mathf.Min(RoomPart_Breached.CellsToDestroy.RandomInRange, num);
			int i2 = Mathf.Min(RoomPart_Breached.CellsToDestroy.RandomInRange, num);
			IntVec3 first = edge.CenterCell - rot.RighthandCell * i;
			IntVec3 second = edge.CenterCell + rot.RighthandCell * i2;
			CellRect cellRect = CellRect.FromLimits(first, second);
			foreach (IntVec3 c in cellRect.Cells)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null)
				{
					if (cellRect.IsCorner(c))
					{
						edifice.HitPoints = (int)((float)edifice.MaxHitPoints * RoomPart_Breached.DamageRangePercent.RandomInRange);
					}
					else
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			int randomInRange = RoomPart_Breached.RadialDamageRange.RandomInRange;
			CellRect cellRect2 = cellRect.CenterCell.RectAbout(randomInRange, randomInRange);
			foreach (IntVec3 c2 in cellRect2.Cells)
			{
				if (!cellRect2.IsCorner(c2))
				{
					Building edifice2 = c2.GetEdifice(map);
					if (edifice2 != null)
					{
						edifice2.HitPoints = (int)((float)edifice2.MaxHitPoints * RoomPart_Breached.DamageRangePercent.RandomInRange);
					}
				}
			}
			GenSpawn.SpawnIrregularLump(ThingDefOf.Filth_RubbleBuilding, cellRect.CenterCell, map, RoomPart_Breached.FilthGroupRange, RoomPart_Breached.FilthDistanceRange, WipeMode.Vanish, null, null, null, null, null);
			GenSpawn.SpawnIrregularLump(ThingDefOf.Filth_RubbleBuilding, cellRect.RandomCell, map, RoomPart_Breached.FilthGroupRange, RoomPart_Breached.FilthDistanceRange, WipeMode.Vanish, null, null, null, null, null);
			FilthMaker.TryMakeFilth(cellRect.CenterCell, map, ThingDefOf.Filth_BlastMark, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x04007951 RID: 31057
		private static readonly IntRange CellsToDestroy = new IntRange(2, 4);

		// Token: 0x04007952 RID: 31058
		private static readonly IntRange RadialDamageRange = new IntRange(2, 4);

		// Token: 0x04007953 RID: 31059
		private static readonly FloatRange DamageRangePercent = new FloatRange(0.4f, 0.8f);

		// Token: 0x04007954 RID: 31060
		private static readonly IntRange FilthGroupRange = new IntRange(3, 6);

		// Token: 0x04007955 RID: 31061
		private static readonly IntRange FilthDistanceRange = new IntRange(3, 6);
	}
}
