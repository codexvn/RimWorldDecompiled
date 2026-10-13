using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033DE RID: 13278
	public static class ThingSelectionUtility
	{
		// Token: 0x06012A88 RID: 76424 RVA: 0x005859FC File Offset: 0x00583BFC
		public static bool SelectableByMapClick(Thing t)
		{
			if (!t.def.selectable)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.IsHiddenFromPlayer())
			{
				return false;
			}
			Thing spawnedParentOrMe = t.SpawnedParentOrMe;
			if (spawnedParentOrMe == null)
			{
				return false;
			}
			if (spawnedParentOrMe.def.size.x == 1 && spawnedParentOrMe.def.size.z == 1)
			{
				return !spawnedParentOrMe.Position.Fogged(spawnedParentOrMe.Map);
			}
			using (CellRect.Enumerator enumerator = spawnedParentOrMe.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Fogged(spawnedParentOrMe.Map))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06012A89 RID: 76425 RVA: 0x00585ACC File Offset: 0x00583CCC
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x06012A8A RID: 76426 RVA: 0x00585AE3 File Offset: 0x00583CE3
		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						List<Thing> cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
						if (cellThings != null)
						{
							int num;
							for (int i = 0; i < cellThings.Count; i = num + 1)
							{
								Thing t = cellThings[i];
								if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
								{
									yield return t;
									ThingSelectionUtility.yieldedThings.Add(t);
								}
								t = null;
								num = i;
							}
						}
						cellThings = null;
					}
				}
				Rect rectInWorldSpace = ThingSelectionUtility.GetRectInWorldSpace(rect);
				foreach (IntVec3 c2 in mapRect.ExpandedBy(1).EdgeCells)
				{
					if (c2.InBounds(Find.CurrentMap) && c2.GetItemCount(Find.CurrentMap) > 1)
					{
						foreach (Thing t in Find.CurrentMap.thingGrid.ThingsAt(c2))
						{
							if (t.def.category == ThingCategory.Item && ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
							{
								Vector3 vector = t.TrueCenter();
								Rect rect2 = new Rect(vector.x - 0.5f, vector.z - 0.5f, 1f, 1f);
								if (rect2.Overlaps(rectInWorldSpace))
								{
									yield return t;
									ThingSelectionUtility.yieldedThings.Add(t);
								}
							}
							t = null;
						}
						IEnumerator<Thing> enumerator3 = null;
					}
				}
				IEnumerator<IntVec3> enumerator2 = null;
				rectInWorldSpace = default(Rect);
			}
			finally
			{
				ThingSelectionUtility.yieldedThings.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06012A8B RID: 76427 RVA: 0x00585AF4 File Offset: 0x00583CF4
		private static Rect GetRectInWorldSpace(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new Rect(vector.x, vector2.z, vector2.x - vector.x, vector.z - vector2.z);
		}

		// Token: 0x06012A8C RID: 76428 RVA: 0x00585B7F File Offset: 0x00583D7F
		public static IEnumerable<Plan> MultiSelectablePlansInScreenRectDistinct(Rect rect, ColorDef match = null)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedPlans.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						Plan plan = c.GetPlan(Find.CurrentMap);
						if (plan != null && (match == null || plan.Color == match))
						{
							if (!ThingSelectionUtility.yieldedPlans.Contains(plan))
							{
								yield return plan;
								ThingSelectionUtility.yieldedPlans.Add(plan);
							}
							plan = null;
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedPlans.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06012A8D RID: 76429 RVA: 0x00585B96 File Offset: 0x00583D96
		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect, Zone matchType = null)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						Zone zone = c.GetZone(Find.CurrentMap);
						if (zone != null && zone.IsMultiselectable && (matchType == null || !(zone.GetType() != matchType.GetType())))
						{
							if (!ThingSelectionUtility.yieldedZones.Contains(zone))
							{
								yield return zone;
								ThingSelectionUtility.yieldedZones.Add(zone);
							}
							zone = null;
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedZones.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06012A8E RID: 76430 RVA: 0x00585BB0 File Offset: 0x00583DB0
		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		// Token: 0x06012A8F RID: 76431 RVA: 0x00585C64 File Offset: 0x00583E64
		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldSelected = WorldRendererUtility.WorldSelected;
			int num = -1;
			for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
			{
				if ((!worldSelected && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldSelected && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0], CameraJumper.MovementMode.Pan);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count], CameraJumper.MovementMode.Pan);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x06012A90 RID: 76432 RVA: 0x00585D64 File Offset: 0x00583F64
		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldSelected = WorldRendererUtility.WorldSelected;
			int num = -1;
			for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
			{
				if ((!worldSelected && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldSelected && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1], CameraJumper.MovementMode.Pan);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)], CameraJumper.MovementMode.Pan);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x0400C1C6 RID: 49606
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x0400C1C7 RID: 49607
		private static readonly HashSet<Plan> yieldedPlans = new HashSet<Plan>();

		// Token: 0x0400C1C8 RID: 49608
		private static readonly HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x0400C1C9 RID: 49609
		private static readonly List<Pawn> tmpColonists = new List<Pawn>();
	}
}
