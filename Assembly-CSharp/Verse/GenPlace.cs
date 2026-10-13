using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000D15 RID: 3349
	public static class GenPlace
	{
		// Token: 0x06005597 RID: 21911 RVA: 0x001B9840 File Offset: 0x001B7A40
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction = null, Predicate<IntVec3> extraValidator = null, Rot4? rot = null, int squareRadius = 1)
		{
			Thing thing2;
			return GenPlace.TryPlaceThing(thing, center, map, mode, out thing2, placedAction, extraValidator, rot, 1);
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x001B9860 File Offset: 0x001B7A60
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> extraValidator = null, Rot4? rot = null, int squareRadius = 1)
		{
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = thing.def.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			lastResultingThing = null;
			if (map == null)
			{
				Log.Error("Tried to place thing " + ((thing != null) ? thing.ToString() : null) + " in a null map.");
				lastResultingThing = null;
				return false;
			}
			if (thing.def.category == ThingCategory.Filth)
			{
				mode = ThingPlaceMode.Direct;
			}
			if (mode == ThingPlaceMode.Direct)
			{
				return GenPlace.TryPlaceDirect(thing, center, rot.Value, map, out lastResultingThing, placedAction);
			}
			if (mode == ThingPlaceMode.Near)
			{
				for (;;)
				{
					int stackCount = thing.stackCount;
					IntVec3 loc;
					if (!GenPlace.TryFindPlaceSpotNear(center, rot.Value, map, thing, true, out loc, extraValidator))
					{
						break;
					}
					if (GenPlace.TryPlaceDirect(thing, loc, rot.Value, map, out lastResultingThing, placedAction))
					{
						return true;
					}
					if (thing.stackCount == stackCount)
					{
						goto Block_8;
					}
				}
				return false;
				Block_8:
				string[] array = new string[7];
				array[0] = "Failed to place ";
				array[1] = ((thing != null) ? thing.ToString() : null);
				array[2] = " at ";
				int num = 3;
				IntVec3 intVec = center;
				array[num] = intVec.ToString();
				array[4] = " in mode ";
				array[5] = mode.ToString();
				array[6] = ".";
				Log.Error(string.Concat(array));
				lastResultingThing = null;
				return false;
			}
			if (mode == ThingPlaceMode.Radius)
			{
				for (;;)
				{
					int stackCount2 = thing.stackCount;
					IntVec3 loc2;
					if (!GenPlace.TryFindPlaceSpotInRadius(center, rot.Value, map, thing, squareRadius, true, out loc2, 100, extraValidator))
					{
						break;
					}
					if (GenPlace.TryPlaceDirect(thing, loc2, rot.Value, map, out lastResultingThing, placedAction))
					{
						return true;
					}
					if (thing.stackCount == stackCount2)
					{
						goto Block_12;
					}
				}
				return false;
				Block_12:
				string[] array2 = new string[7];
				array2[0] = "Failed to place ";
				array2[1] = ((thing != null) ? thing.ToString() : null);
				array2[2] = " at ";
				int num2 = 3;
				IntVec3 intVec = center;
				array2[num2] = intVec.ToString();
				array2[4] = " in mode ";
				array2[5] = mode.ToString();
				array2[6] = ".";
				Log.Error(string.Concat(array2));
				lastResultingThing = null;
				return false;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x001B9A50 File Offset: 0x001B7C50
		private static bool TryFindPlaceSpotNear(IntVec3 center, Rot4 rot, Map map, Thing thing, bool allowStacking, out IntVec3 bestSpot, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Unusable;
			bestSpot = center;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int j = 0; j < GenPlace.PlaceNearMiddleRadialCells; j++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[j];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int k = 0; k < GenPlace.PlaceNearMaxRadialCells; k++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[k];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality > GenPlace.PlaceSpotQuality.Unusable)
			{
				return true;
			}
			bestSpot = center;
			return false;
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x001B9B54 File Offset: 0x001B7D54
		private static bool TryFindPlaceSpotInRadius(IntVec3 center, Rot4 rot, Map map, Thing thing, int radius, bool allowStacking, out IntVec3 bestSpot, int attempts = 100, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Unusable;
			bestSpot = center;
			while (attempts-- > 0)
			{
				IntVec3 intVec;
				if (CellFinder.TryRandomClosewalkCellNear(center, map, radius, out intVec, null))
				{
					GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
					if (placeSpotQuality2 > placeSpotQuality)
					{
						bestSpot = intVec;
						placeSpotQuality = placeSpotQuality2;
					}
					if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
					{
						break;
					}
				}
			}
			return placeSpotQuality > GenPlace.PlaceSpotQuality.Unusable;
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x001B9BAC File Offset: 0x001B7DAC
		private static GenPlace.PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Rot4 rot, Map map, Thing thing, IntVec3 center, bool allowStacking, Predicate<IntVec3> extraValidator = null)
		{
			if (!GenSpawn.CanSpawnAt(thing.def, c, map, new Rot4?(rot), true))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (extraValidator != null && !extraValidator(c))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			GenPlace.thingList.Clear();
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot, thing.def.Size))
			{
				GenPlace.thingList.AddRange(c2.GetThingList(map));
			}
			bool flag = false;
			for (int i = 0; i < GenPlace.thingList.Count; i++)
			{
				Thing thing2 = GenPlace.thingList[i];
				if (thing.def.saveCompressible && thing2.def.saveCompressible)
				{
					return GenPlace.PlaceSpotQuality.Unusable;
				}
				if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item && allowStacking && thing2.stackCount < thing2.def.stackLimit && thing2.CanStackWith(thing))
				{
					flag = true;
				}
			}
			if (thing.def.category == ThingCategory.Item && !flag && c.GetItemCount(map) >= c.GetMaxItemsAllowedInCell(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			IHaulDestination haulDestination = c.GetEdifice(map) as IHaulDestination;
			if (haulDestination != null && !haulDestination.Accepts(thing))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (thing is Building)
			{
				foreach (IntVec3 c3 in GenAdj.OccupiedRect(c, rot, thing.def.size))
				{
					Building edifice = c3.GetEdifice(map);
					if (edifice != null && GenSpawn.SpawningWipes(thing.def, edifice.def, false))
					{
						return GenPlace.PlaceSpotQuality.Awful;
					}
				}
			}
			if (c.GetRoom(map) == center.GetRoom(map))
			{
				if (allowStacking)
				{
					for (int j = 0; j < GenPlace.thingList.Count; j++)
					{
						Thing thing3 = GenPlace.thingList[j];
						if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing3.def.stackLimit)
						{
							return GenPlace.PlaceSpotQuality.Perfect;
						}
					}
				}
				Pawn pawn = thing as Pawn;
				bool flag2 = pawn != null && pawn.Downed;
				GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Perfect;
				for (int k = 0; k < GenPlace.thingList.Count; k++)
				{
					Thing thing4 = GenPlace.thingList[k];
					if (thing4.def.Fillage == FillCategory.Full)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4.def.preventDroppingThingsOn)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4.def.IsDoor)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4 is Building_WorkTable)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					Pawn pawn2 = thing4 as Pawn;
					if (pawn2 != null && (pawn2.Downed || flag2))
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4.def.category == ThingCategory.Plant && thing4.def.selectable && placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
					{
						placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
					}
				}
				return placeSpotQuality;
			}
			if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
			{
				return GenPlace.PlaceSpotQuality.Awful;
			}
			return GenPlace.PlaceSpotQuality.Bad;
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001B9EE4 File Offset: 0x001B80E4
		private static bool SplitAndSpawnOneStackOnCell(Thing thing, IntVec3 loc, Rot4 rot, Map map, out Thing resultingThing, Action<Thing, int> placedAction)
		{
			Thing thing2;
			if (thing.stackCount > thing.def.stackLimit)
			{
				thing2 = thing.SplitOff(thing.def.stackLimit);
			}
			else
			{
				thing2 = thing;
			}
			resultingThing = GenSpawn.Spawn(thing2, loc, map, rot, WipeMode.Vanish, false, false);
			if (placedAction != null)
			{
				placedAction(thing2, thing2.stackCount);
			}
			return thing2 == thing;
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001B9F40 File Offset: 0x001B8140
		private static bool TryPlaceDirect(Thing thing, IntVec3 loc, Rot4 rot, Map map, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			resultingThing = null;
			GenPlace.cellThings.Clear();
			GenPlace.cellThings.AddRange(loc.GetThingList(map));
			GenPlace.cellThings.Sort((Thing lhs, Thing rhs) => rhs.stackCount.CompareTo(lhs.stackCount));
			if (thing.def.stackLimit > 1)
			{
				for (int i = 0; i < GenPlace.cellThings.Count; i++)
				{
					Thing thing2 = GenPlace.cellThings[i];
					if (thing2.CanStackWith(thing))
					{
						int stackCount = thing.stackCount;
						if (thing2.TryAbsorbStack(thing, true))
						{
							resultingThing = thing2;
							if (placedAction != null)
							{
								placedAction(thing2, stackCount);
							}
							return true;
						}
						if (placedAction != null && stackCount != thing.stackCount)
						{
							placedAction(thing2, stackCount - thing.stackCount);
						}
					}
				}
			}
			int num2;
			if (thing.def.category == ThingCategory.Item)
			{
				int num = GenPlace.cellThings.Count((Thing cellThing) => cellThing.def.category == ThingCategory.Item);
				num2 = loc.GetMaxItemsAllowedInCell(map) - num;
			}
			else
			{
				num2 = thing.stackCount + 1;
			}
			if (num2 <= 0 && thing.def.stackLimit <= 1)
			{
				num2 = 1;
			}
			for (int j = 0; j < num2; j++)
			{
				if (GenPlace.SplitAndSpawnOneStackOnCell(thing, loc, rot, map, out resultingThing, placedAction))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x001BA094 File Offset: 0x001B8294
		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprintsAndFrames)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (checkBlueprintsAndFrames && (thing.def.IsBlueprint || thing.def.IsFrame))
				{
					return thing;
				}
				if ((thing.def.category != ThingCategory.Plant || thing.def.passability != Traversability.Standable) && thing.def.category != ThingCategory.Filth && (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount))
				{
					if (thing.def.EverHaulable)
					{
						return thing;
					}
					if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def, false))
					{
						return thing;
					}
					if (thing.def.passability != Traversability.Standable && thing.def.surfaceType != SurfaceType.Item)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x04003DE6 RID: 15846
		private static readonly int PlaceNearMaxRadialCells = GenRadial.NumCellsInRadius(12.9f);

		// Token: 0x04003DE7 RID: 15847
		private static readonly int PlaceNearMiddleRadialCells = GenRadial.NumCellsInRadius(3f);

		// Token: 0x04003DE8 RID: 15848
		private static List<Thing> thingList = new List<Thing>();

		// Token: 0x04003DE9 RID: 15849
		private static List<Thing> cellThings = new List<Thing>(8);

		// Token: 0x02000D16 RID: 3350
		private enum PlaceSpotQuality : byte
		{
			// Token: 0x04003DEB RID: 15851
			Unusable,
			// Token: 0x04003DEC RID: 15852
			Awful,
			// Token: 0x04003DED RID: 15853
			Bad,
			// Token: 0x04003DEE RID: 15854
			Okay,
			// Token: 0x04003DEF RID: 15855
			Perfect
		}
	}
}
