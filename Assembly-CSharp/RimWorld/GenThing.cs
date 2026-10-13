using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003655 RID: 13909
	public static class GenThing
	{
		// Token: 0x06013502 RID: 79106 RVA: 0x005BD1B0 File Offset: 0x005BB3B0
		public static Vector3 TrueCenter(this Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				return pawn.Drawer.DrawPos;
			}
			if (t.def.category == ThingCategory.Item && t.Spawned)
			{
				return GenThing.ItemCenterAt(t);
			}
			return GenThing.TrueCenter(t.Position, t.Rotation, t.def.size, t.def.Altitude);
		}

		// Token: 0x06013503 RID: 79107 RVA: 0x005BD218 File Offset: 0x005BB418
		public static Vector3 TrueCenter(IntVec3 loc, Rot4 rotation, IntVec2 thingSize, float altitude)
		{
			Vector3 result = loc.ToVector3ShiftedWithAltitude(altitude);
			if (thingSize.x != 1 || thingSize.z != 1)
			{
				if (rotation.IsHorizontal)
				{
					ref int ptr = ref thingSize.x;
					int z = thingSize.z;
					int x = thingSize.x;
					ptr = z;
					thingSize.z = x;
				}
				switch (rotation.AsInt)
				{
				case 0:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				case 1:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 2:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 3:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x06013504 RID: 79108 RVA: 0x005BD370 File Offset: 0x005BB570
		private static Vector3 ItemCenterAt(Thing thing)
		{
			GenThing.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			IntVec3 position = CS$<>8__locals1.thing.Position;
			int num = 0;
			CS$<>8__locals1.itemsWithLowerID = 0;
			bool flag = false;
			bool flag2 = true;
			ThingDef thingDef = null;
			List<Thing> thingList = position.GetThingList(CS$<>8__locals1.thing.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing2 = thingList[i];
				if (thing2.def.category == ThingCategory.Item)
				{
					if (thingDef == null)
					{
						thingDef = thing2.def;
					}
					num++;
					if (thing2.def.IsWeapon && thing2.def != ThingDefOf.WoodLog)
					{
						flag = true;
					}
					if (thing2.thingIDNumber < CS$<>8__locals1.thing.thingIDNumber)
					{
						int itemsWithLowerID = CS$<>8__locals1.itemsWithLowerID;
						CS$<>8__locals1.itemsWithLowerID = itemsWithLowerID + 1;
					}
					if (thing2.def != thingDef)
					{
						flag2 = false;
					}
				}
			}
			float num2 = (float)CS$<>8__locals1.itemsWithLowerID * 0.03658537f / 10f;
			if (num <= 1)
			{
				Vector3 vector = position.ToVector3Shifted();
				return new Vector3(vector.x, CS$<>8__locals1.thing.def.Altitude, vector.z);
			}
			if (flag)
			{
				Vector3 vector2 = position.ToVector3Shifted();
				float num3 = 1f / (float)num;
				int num4 = GenThing.<ItemCenterAt>g__GetAdjacencyOffset|2_0(position + IntVec3.West, ref CS$<>8__locals1);
				return new Vector3(vector2.x - 0.5f + num3 * ((float)CS$<>8__locals1.itemsWithLowerID + 0.5f), CS$<>8__locals1.thing.def.Altitude + num2, vector2.z + ((num4 % 2 == 0) ? -0.02f : 0.2f));
			}
			if (flag2)
			{
				Vector3 vector3 = position.ToVector3Shifted();
				return new Vector3(vector3.x + (float)CS$<>8__locals1.itemsWithLowerID * 0.11f - 0.08f, CS$<>8__locals1.thing.def.Altitude + num2, vector3.z + (float)CS$<>8__locals1.itemsWithLowerID * 0.24f - 0.05f);
			}
			Vector3 vector4 = position.ToVector3Shifted();
			Vector2 vector5 = GenGeo.RegularPolygonVertexPosition(num, CS$<>8__locals1.itemsWithLowerID, ((position.x + position.z) % 2 == 0) ? 0f : 60f) * 0.3f;
			return new Vector3(vector5.x + vector4.x, CS$<>8__locals1.thing.def.Altitude + num2, vector5.y + vector4.z);
		}

		// Token: 0x06013505 RID: 79109 RVA: 0x005BD5E4 File Offset: 0x005BB7E4
		public static void TryDirtyAdjacentGroupContainers(ISlotGroupParent parent, Map map)
		{
			IntVec3 intVec = parent.Position + IntVec3.East;
			SlotGroup slotGroup;
			while (intVec.InBounds(map) && intVec.TryGetSlotGroup(map, out slotGroup) && slotGroup.parent != null)
			{
				map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Things);
				intVec += IntVec3.East;
			}
		}

		// Token: 0x06013506 RID: 79110 RVA: 0x005BD642 File Offset: 0x005BB842
		public static bool TryDropAndSetForbidden(Thing th, IntVec3 pos, Map map, ThingPlaceMode mode, out Thing resultingThing, bool forbidden)
		{
			if (GenDrop.TryDropSpawn(th, pos, map, ThingPlaceMode.Near, out resultingThing, null, null, true))
			{
				if (resultingThing != null)
				{
					resultingThing.SetForbidden(forbidden, false);
				}
				return true;
			}
			resultingThing = null;
			return false;
		}

		// Token: 0x06013507 RID: 79111 RVA: 0x005BD66C File Offset: 0x005BB86C
		public static string ThingsToCommaList(IList<Thing> things, bool useAnd = false, bool aggregate = true, int maxCount = -1)
		{
			GenThing.tmpThings.Clear();
			GenThing.tmpThingLabels.Clear();
			GenThing.tmpThingCounts.Clear();
			GenThing.tmpThings.AddRange(things);
			if (GenThing.tmpThings.Count >= 2)
			{
				GenThing.tmpThings.SortByDescending((Thing x) => x is Pawn, (Thing x) => x.def.BaseMarketValue * (float)x.stackCount);
			}
			for (int i = 0; i < GenThing.tmpThings.Count; i++)
			{
				string text = (GenThing.tmpThings[i] is Pawn) ? GenThing.tmpThings[i].LabelShort : GenThing.tmpThings[i].LabelNoCount;
				bool flag = false;
				if (aggregate)
				{
					for (int j = 0; j < GenThing.tmpThingCounts.Count; j++)
					{
						if (GenThing.tmpThingCounts[j].First == text)
						{
							GenThing.tmpThingCounts[j] = new Pair<string, int>(GenThing.tmpThingCounts[j].First, GenThing.tmpThingCounts[j].Second + GenThing.tmpThings[i].stackCount);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					GenThing.tmpThingCounts.Add(new Pair<string, int>(text, GenThing.tmpThings[i].stackCount));
				}
			}
			GenThing.tmpThings.Clear();
			bool flag2 = false;
			int num = GenThing.tmpThingCounts.Count;
			if (maxCount >= 0 && num > maxCount)
			{
				num = maxCount;
				flag2 = true;
			}
			for (int k = 0; k < num; k++)
			{
				string text2 = GenThing.tmpThingCounts[k].First;
				if (GenThing.tmpThingCounts[k].Second != 1)
				{
					text2 = text2 + " x" + GenThing.tmpThingCounts[k].Second.ToString();
				}
				GenThing.tmpThingLabels.Add(text2);
			}
			string text3 = GenThing.tmpThingLabels.ToCommaList(useAnd && !flag2, false);
			if (flag2)
			{
				text3 += "...";
			}
			return text3;
		}

		// Token: 0x06013508 RID: 79112 RVA: 0x005BD8C4 File Offset: 0x005BBAC4
		public static float GetMarketValue(IList<Thing> things)
		{
			float num = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += things[i].MarketValue * (float)things[i].stackCount;
			}
			return num;
		}

		// Token: 0x06013509 RID: 79113 RVA: 0x005BD908 File Offset: 0x005BBB08
		public static bool CloserThingBetween(ThingDef thingDef, IntVec3 a, IntVec3 b, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 intVec in CellRect.FromLimits(a, b))
			{
				if (!(intVec == a) && !(intVec == b) && intVec.InBounds(map))
				{
					foreach (Thing thing in intVec.GetThingList(map))
					{
						if ((thingToIgnore == null || thingToIgnore != thing) && (thing.def == thingDef || thing.def.entityDefToBuild == thingDef))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0601350B RID: 79115 RVA: 0x005BDA00 File Offset: 0x005BBC00
		[CompilerGenerated]
		internal static int <ItemCenterAt>g__GetAdjacencyOffset|2_0(IntVec3 x, ref GenThing.<>c__DisplayClass2_0 A_1)
		{
			int num = A_1.itemsWithLowerID;
			while (x.InBounds(A_1.thing.Map))
			{
				int itemCount = x.GetItemCount(A_1.thing.Map);
				if (itemCount <= 1)
				{
					break;
				}
				if (itemCount % 2 == 1)
				{
					num++;
				}
				x += IntVec3.West;
			}
			return num;
		}

		// Token: 0x0400D657 RID: 54871
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400D658 RID: 54872
		private static List<string> tmpThingLabels = new List<string>();

		// Token: 0x0400D659 RID: 54873
		private static List<Pair<string, int>> tmpThingCounts = new List<Pair<string, int>>();
	}
}
