using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200362B RID: 13867
	public static class DoorUtility
	{
		// Token: 0x06013418 RID: 78872 RVA: 0x005B6596 File Offset: 0x005B4796
		public static IEnumerable<IntVec3> WallRequirementCells(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			if (def.size == IntVec2.One)
			{
				yield break;
			}
			CellRect rect = GenAdj.OccupiedRect(IntVec3.Zero, def.defaultPlacingRot, def.size);
			int max = def.defaultPlacingRot.IsHorizontal ? rect.Width : rect.Height;
			int num;
			for (int i = 0; i < max; i = num + 1)
			{
				yield return pos + new IntVec3(rect.minX - 1, 0, rect.minZ + i).RotatedBy(rot);
				yield return pos + new IntVec3(rect.maxX + 1, 0, rect.minZ + i).RotatedBy(rot);
				num = i;
			}
			yield break;
		}

		// Token: 0x06013419 RID: 78873 RVA: 0x005B65B4 File Offset: 0x005B47B4
		public static bool EncapsulatingWallAt(IntVec3 cell, Map map, bool includeUnbuilt = false)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			if (includeUnbuilt)
			{
				List<Thing> thingList = cell.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Blueprint blueprint = thingList[i] as Blueprint;
					if (blueprint != null)
					{
						ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.Fillage == FillCategory.Full && !thingDef.IsDoor)
						{
							return true;
						}
					}
					Frame frame = thingList[i] as Frame;
					if (frame != null && frame.BuildDef.Fillage == FillCategory.Full && !frame.BuildDef.IsDoor)
					{
						return true;
					}
				}
			}
			Building edifice = cell.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full && !edifice.def.IsDoor;
		}

		// Token: 0x0601341A RID: 78874 RVA: 0x005B6680 File Offset: 0x005B4880
		public static Rot4 DoorRotationAt(IntVec3 loc, Map map, bool preferFences)
		{
			int num = 0;
			int num2 = 0;
			int num3 = num + DoorUtility.AlignQualityAgainst(loc, IntVec3.East, map, preferFences) + DoorUtility.AlignQualityAgainst(loc, IntVec3.West, map, preferFences);
			num2 += DoorUtility.AlignQualityAgainst(loc, IntVec3.North, map, preferFences);
			num2 += DoorUtility.AlignQualityAgainst(loc, IntVec3.South, map, preferFences);
			if (num3 >= num2)
			{
				return Rot4.North;
			}
			return Rot4.East;
		}

		// Token: 0x0601341B RID: 78875 RVA: 0x005B66DC File Offset: 0x005B48DC
		private static int AlignQualityAgainst(IntVec3 c, IntVec3 offset, Map map, bool preferFences)
		{
			IntVec3 c2 = c + offset;
			if (!c2.InBounds(map))
			{
				return 0;
			}
			if (!c2.WalkableByNormal(map))
			{
				return 9;
			}
			List<Thing> thingList = c2.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				Thing thing = thingList[i];
				if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
				{
					if ((c - offset).GetDoor(map) == null)
					{
						return 1;
					}
					return 5;
				}
				else if (thing.def.IsFence)
				{
					if (!preferFences)
					{
						return 1;
					}
					return 10;
				}
				else
				{
					Thing thing2 = thing as Blueprint;
					if (thing2 != null)
					{
						if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
						{
							return 9;
						}
						ThingDef thingDef = thing2.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.IsFence)
						{
							if (!preferFences)
							{
								return 1;
							}
							return 10;
						}
						else if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
						{
							return 1;
						}
					}
					i++;
				}
			}
			return 0;
		}
	}
}
