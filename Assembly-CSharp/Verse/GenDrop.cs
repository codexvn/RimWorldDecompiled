using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CE4 RID: 3300
	public static class GenDrop
	{
		// Token: 0x06005461 RID: 21601 RVA: 0x001B26D4 File Offset: 0x001B08D4
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (map == null)
			{
				Log.Error("Dropped " + ((thing != null) ? thing.ToString() : null) + " in a null map.");
				resultingThing = null;
				return false;
			}
			if (!dropCell.InBounds(map))
			{
				string str = "Dropped ";
				string str2 = (thing != null) ? thing.ToString() : null;
				string str3 = " out of bounds at ";
				IntVec3 intVec = dropCell;
				Log.Error(str + str2 + str3 + intVec.ToString());
				resultingThing = null;
				return false;
			}
			if (thing.def.destroyOnDrop)
			{
				thing.Destroy(DestroyMode.Vanish);
				resultingThing = null;
				return true;
			}
			if (GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, null, 1))
			{
				if (playDropSound && thing.def.soundDrop != null)
				{
					thing.def.soundDrop.PlayOneShot(new TargetInfo(dropCell, map, false));
				}
				return true;
			}
			return false;
		}
	}
}
