using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECB RID: 7883
	public class RoomContents_ChemfuelStorage : RoomContentsWorker
	{
		// Token: 0x0600AEC8 RID: 44744 RVA: 0x0031E18C File Offset: 0x0031C38C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.GenerateRows(ThingDefOf.Shelf, room, map, ThingDefOf.Steel);
			int num = RoomContents_ChemfuelStorage.ChemfuelCountRange.RandomInRange;
			int num2 = 99;
			Func<IntVec3, bool> <>9__0;
			while (num > 0 && num2-- > 0)
			{
				Map map2 = map;
				int contractedBy = 0;
				int padding = 0;
				Func<IntVec3, bool> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ShelfValidator(map, c, ThingDefOf.Chemfuel)));
				}
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map2, out loc, contractedBy, padding, validator, true))
				{
					break;
				}
				int num3 = Rand.Range(25, 50);
				num3 = Mathf.Min(num3, num);
				Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Chemfuel, null), loc, map, WipeMode.Vanish);
				thing.stackCount = num3;
				thing.SetForbidden(true, true);
				num -= num3;
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AEC9 RID: 44745 RVA: 0x0031E26C File Offset: 0x0031C46C
		private bool ShelfValidator(Map map, IntVec3 c, ThingDef itemDef)
		{
			Building_Storage building_Storage = c.GetFirstThing(map, ThingDefOf.Shelf) as Building_Storage;
			return building_Storage != null && building_Storage.SpaceRemainingFor(itemDef) != 0;
		}

		// Token: 0x04007822 RID: 30754
		private static readonly IntRange ChemfuelCountRange = new IntRange(300, 500);
	}
}
