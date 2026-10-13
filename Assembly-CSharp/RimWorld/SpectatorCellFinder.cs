using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200355E RID: 13662
	public static class SpectatorCellFinder
	{
		// Token: 0x06013276 RID: 78454 RVA: 0x005B0E24 File Offset: 0x005AF024
		public static bool TryFindSpectatorCellFor(Pawn p, CellRect spectateRect, Map map, out IntVec3 cell, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1, List<IntVec3> extraDisallowedCells = null, Precept_Ritual ritual = null, Func<IntVec3, Pawn, Map, bool> extraValidator = null)
		{
			spectateRect.ClipInsideMap(map);
			if (spectateRect.Area == 0 || allowedSides == SpectateRectSide.None)
			{
				cell = IntVec3.Invalid;
				return false;
			}
			if (ritual != null && ritual.ideo.RitualSeatDef != null)
			{
				IEnumerable<Thing> source = p.Map.listerThings.ThingsOfDef(ritual.ideo.RitualSeatDef);
				Func<Thing, float> <>9__1;
				Func<Thing, float> keySelector;
				if ((keySelector = <>9__1) == null)
				{
					keySelector = (<>9__1 = ((Thing t) => t.Position.DistanceTo(spectateRect.CenterCell)));
				}
				foreach (Thing thing in source.OrderBy(keySelector))
				{
					if (GatheringsUtility.InGatheringArea(thing.Position, spectateRect.CenterCell, p.Map) && SpectatorCellFinder.IsCorrectlyRotatedChair(thing.Position, thing.Rotation, thing.def, spectateRect))
					{
						foreach (IntVec3 intVec in thing.OccupiedRect())
						{
							IntVec3 intVec2 = spectateRect.ClosestCellTo(intVec);
							if ((float)intVec2.DistanceToSquared(intVec) <= 210.25f)
							{
								PawnDuty duty = p.mindState.duty;
								Building building = ((duty != null) ? duty.focusThird.Thing : null) as Building;
								if ((building == null || !(intVec == building.InteractionCell)) && p.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && p.CanReserveSittableOrSpot(intVec, false) && GenSight.LineOfSight(intVec2, intVec, map, true, null, 0, 0))
								{
									cell = intVec;
									return true;
								}
							}
						}
					}
				}
			}
			CellRect rectWithMargin = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
			Predicate<Thing> <>9__2;
			Predicate<IntVec3> predicate = delegate(IntVec3 x)
			{
				if (!x.InBounds(map))
				{
					return false;
				}
				if (!x.Standable(map))
				{
					return false;
				}
				if (x.Fogged(map))
				{
					return false;
				}
				if (rectWithMargin.Contains(x))
				{
					return false;
				}
				if ((x.z <= rectWithMargin.maxZ || (allowedSides & SpectateRectSide.Up) != SpectateRectSide.Up) && (x.x <= rectWithMargin.maxX || (allowedSides & SpectateRectSide.Right) != SpectateRectSide.Right) && (x.z >= rectWithMargin.minZ || (allowedSides & SpectateRectSide.Down) != SpectateRectSide.Down) && (x.x >= rectWithMargin.minX || (allowedSides & SpectateRectSide.Left) != SpectateRectSide.Left))
				{
					return false;
				}
				IntVec3 intVec6 = spectateRect.ClosestCellTo(x);
				if ((float)intVec6.DistanceToSquared(x) > 210.25f)
				{
					return false;
				}
				if (!GenSight.LineOfSight(intVec6, x, map, true, null, 0, 0))
				{
					return false;
				}
				List<Thing> thingList = x.GetThingList(map);
				Predicate<Thing> match;
				if ((match = <>9__2) == null)
				{
					match = (<>9__2 = ((Thing y) => y is Pawn && y != p));
				}
				if (thingList.Find(match) != null)
				{
					return false;
				}
				if (p != null && !SpectatorCellFinder.CellAccessibleForPawn(p, x, map))
				{
					return false;
				}
				if (extraDisallowedCells != null && extraDisallowedCells.Contains(x))
				{
					return false;
				}
				if (!SpectatorCellFinder.CorrectlyRotatedChairAt(x, map, spectateRect) && !SpectatorCellFinder.GoodCellToSpectateStanding(x, map, spectateRect))
				{
					return false;
				}
				Pawn p2 = p;
				object obj;
				if (p2 == null)
				{
					obj = null;
				}
				else
				{
					Pawn_MindState mindState = p2.mindState;
					if (mindState == null)
					{
						obj = null;
					}
					else
					{
						PawnDuty duty2 = mindState.duty;
						obj = ((duty2 != null) ? duty2.focusThird.Thing : null);
					}
				}
				Building building2 = obj as Building;
				return (building2 == null || !(x == building2.InteractionCell)) && (extraValidator == null || extraValidator(x, p, map));
			};
			if (p != null && predicate(p.Position) && SpectatorCellFinder.CorrectlyRotatedChairAt(p.Position, map, spectateRect))
			{
				cell = p.Position;
				return true;
			}
			IntVec2 intVec3 = new IntVec2((rectWithMargin.Width % 2 == 0) ? -1 : 0, (rectWithMargin.Height % 2 == 0) ? -1 : 0);
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec4 = rectWithMargin.CenterCell + GenRadial.RadialPattern[i];
				if (intVec4.x > rectWithMargin.CenterCell.x)
				{
					intVec4.x += intVec3.x;
				}
				if (intVec4.z > rectWithMargin.CenterCell.z)
				{
					intVec4.z += intVec3.z;
				}
				if (predicate(intVec4))
				{
					if (!SpectatorCellFinder.CorrectlyRotatedChairAt(intVec4, map, spectateRect))
					{
						for (int j = 0; j < 90; j++)
						{
							IntVec3 intVec5 = intVec4 + GenRadial.RadialPattern[j];
							if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec5, map, spectateRect) && predicate(intVec5))
							{
								cell = intVec5;
								return true;
							}
						}
					}
					cell = intVec4;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06013277 RID: 78455 RVA: 0x005B1248 File Offset: 0x005AF448
		public static bool CorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			return SpectatorCellFinder.GetCorrectlyRotatedChairAt(x, map, spectateRect) != null;
		}

		// Token: 0x06013278 RID: 78456 RVA: 0x005B1258 File Offset: 0x005AF458
		public static Building GetCorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			if (!x.InBounds(map))
			{
				return null;
			}
			Building edifice = x.GetEdifice(map);
			if (edifice == null || edifice.def.category != ThingCategory.Building || !edifice.def.building.isSittable)
			{
				return null;
			}
			if (SpectatorCellFinder.IsCorrectlyRotatedChair(edifice.Position, edifice.Rotation, edifice.def, spectateRect))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06013279 RID: 78457 RVA: 0x005B12BC File Offset: 0x005AF4BC
		public static bool IsCorrectlyRotatedChair(IntVec3 chairPos, Rot4 chairRot, ThingDef chairDef, CellRect spectateRect)
		{
			return chairDef.building.sitIgnoreOrientation || GenGeo.AngleDifferenceBetween(chairRot.AsAngle, (spectateRect.ClosestCellTo(chairPos) - chairPos).AngleFlat) <= 75f;
		}

		// Token: 0x0601327A RID: 78458 RVA: 0x005B1304 File Offset: 0x005AF504
		private static int DistanceToClosestChair(IntVec3 from, IntVec3 step, Map map, int maxDist, CellRect spectateRect)
		{
			int num = 0;
			IntVec3 intVec = from;
			for (;;)
			{
				intVec += step;
				num++;
				if (!intVec.InBounds(map))
				{
					break;
				}
				if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
				{
					return num;
				}
				if (!intVec.Walkable(map))
				{
					return -1;
				}
				if (num >= maxDist)
				{
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x0601327B RID: 78459 RVA: 0x005B134C File Offset: 0x005AF54C
		public static void DebugFlashPotentialSpectatorCells(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			List<IntVec3> list = new List<IntVec3>();
			int num = 50;
			int num2 = 0;
			IntVec3 intVec;
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, list, null, null))
			{
				list.Add(intVec);
				float a = Mathf.Lerp(1f, 0.08f, (float)num2 / (float)num);
				Material mat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0.8f, 0f, a), false);
				map.debugDrawer.FlashCell(intVec, mat, (num2 + 1).ToString(), 50);
				num2++;
			}
			SpectateRectSide spectateRectSide = SpectatorCellFinder.FindSingleBestSide(spectateRect, map, allowedSides, margin, null);
			IntVec3 centerCell = spectateRect.CenterCell;
			switch (spectateRectSide)
			{
			case SpectateRectSide.Up:
				centerCell.z += spectateRect.Height / 2 + 10;
				break;
			case SpectateRectSide.Right:
				centerCell.x += spectateRect.Width / 2 + 10;
				break;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				centerCell.z -= spectateRect.Height / 2 + 10;
				break;
			default:
				if (spectateRectSide == SpectateRectSide.Left)
				{
					centerCell.x -= spectateRect.Width / 2 + 10;
				}
				break;
			}
			map.debugDrawer.FlashLine(spectateRect.CenterCell, centerCell, 50, SimpleColor.White);
		}

		// Token: 0x0601327C RID: 78460 RVA: 0x005B1488 File Offset: 0x005AF688
		private static bool GoodCellToSpectateStanding(IntVec3 cell, Map map, CellRect spectateRect)
		{
			if (CommonRitualCellPredicates.InDoor(map, cell) || CommonRitualCellPredicates.OnBed(map, cell))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < GenAdj.AdjacentCells.Length; i++)
			{
				if (SpectatorCellFinder.CorrectlyRotatedChairAt(cell + GenAdj.AdjacentCells[i], map, spectateRect))
				{
					num++;
				}
			}
			if (num >= 3)
			{
				return false;
			}
			int num2 = SpectatorCellFinder.DistanceToClosestChair(cell, new IntVec3(-1, 0, 0), map, 4, spectateRect);
			if (num2 >= 0)
			{
				int num3 = SpectatorCellFinder.DistanceToClosestChair(cell, new IntVec3(1, 0, 0), map, 4, spectateRect);
				if (num3 >= 0 && Mathf.Abs(num2 - num3) <= 1)
				{
					return false;
				}
			}
			int num4 = SpectatorCellFinder.DistanceToClosestChair(cell, new IntVec3(0, 0, 1), map, 4, spectateRect);
			if (num4 >= 0)
			{
				int num5 = SpectatorCellFinder.DistanceToClosestChair(cell, new IntVec3(0, 0, -1), map, 4, spectateRect);
				if (num5 >= 0 && Mathf.Abs(num4 - num5) <= 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0601327D RID: 78461 RVA: 0x005B1564 File Offset: 0x005AF764
		private static bool CellAccessibleForPawn(Pawn p, IntVec3 cell, Map map)
		{
			return p.CanReserveSittableOrSpot(cell, false) && p.CanReach(cell, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn) && !cell.IsForbidden(p) && cell.GetDangerFor(p, map) == Danger.None;
		}

		// Token: 0x0601327E RID: 78462 RVA: 0x005B15A0 File Offset: 0x005AF7A0
		public static SpectateRectSide FindSingleBestSide(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1, Func<IntVec3, SpectateRectSide, int, float> scoreOffset = null)
		{
			for (int i = 0; i < SpectatorCellFinder.scorePerSide.Length; i++)
			{
				SpectatorCellFinder.scorePerSide[i] = 0f;
			}
			SpectatorCellFinder.usedCells.Clear();
			int num = 30;
			CellRect cellRect = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
			int num2 = 0;
			IntVec3 intVec;
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, SpectatorCellFinder.usedCells, null, null))
			{
				SpectatorCellFinder.usedCells.Add(intVec);
				SpectateRectSide spectateRectSide = SpectateRectSide.None;
				if (intVec.z > cellRect.maxZ)
				{
					spectateRectSide |= SpectateRectSide.Up;
				}
				if (intVec.x > cellRect.maxX)
				{
					spectateRectSide |= SpectateRectSide.Right;
				}
				if (intVec.z < cellRect.minZ)
				{
					spectateRectSide |= SpectateRectSide.Down;
				}
				if (intVec.x < cellRect.minX)
				{
					spectateRectSide |= SpectateRectSide.Left;
				}
				float num3 = Mathf.Lerp(1f, 0.35f, (float)num2 / (float)num);
				float num4 = num3 + ((scoreOffset != null) ? scoreOffset(intVec, spectateRectSide, num2) : 0f);
				Building correctlyRotatedChairAt = SpectatorCellFinder.GetCorrectlyRotatedChairAt(intVec, map, spectateRect);
				foreach (SpectateRectSide spectateRectSide2 in spectateRectSide.GetAllSelectedItems<SpectateRectSide>())
				{
					if (spectateRectSide2.ValidSingleSide() && allowedSides.HasFlag(spectateRectSide2))
					{
						SpectatorCellFinder.scorePerSide[spectateRectSide2.AsInt()] += num4;
						if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == spectateRectSide2.AsRot4())
						{
							SpectatorCellFinder.scorePerSide[spectateRectSide2.AsInt()] += 1.2f * num3;
						}
					}
				}
				num2++;
			}
			float num5 = 0f;
			int num6 = -1;
			for (int j = 0; j < SpectatorCellFinder.scorePerSide.Length; j++)
			{
				if (SpectatorCellFinder.scorePerSide[j] != 0f && (num6 < 0 || SpectatorCellFinder.scorePerSide[j] > num5))
				{
					num6 = j;
					num5 = SpectatorCellFinder.scorePerSide[j];
				}
			}
			SpectatorCellFinder.usedCells.Clear();
			return num6.ToSpectatorSide();
		}

		// Token: 0x0601327F RID: 78463 RVA: 0x005B17C8 File Offset: 0x005AF9C8
		public static bool ValidSingleSide(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return true;
			case SpectateRectSide.Right:
				return true;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return true;
			default:
				if (side == SpectateRectSide.Left)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x06013280 RID: 78464 RVA: 0x005B17F1 File Offset: 0x005AF9F1
		public static Rot4 AsRot4(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return Rot4.North;
			case SpectateRectSide.Right:
				return Rot4.East;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return Rot4.South;
			default:
				if (side == SpectateRectSide.Left)
				{
					return Rot4.West;
				}
				break;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06013281 RID: 78465 RVA: 0x005B182E File Offset: 0x005AFA2E
		public static int AsInt(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return 0;
			case SpectateRectSide.Right:
				return 1;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return 2;
			default:
				if (side == SpectateRectSide.Left)
				{
					return 3;
				}
				break;
			}
			return 0;
		}

		// Token: 0x06013282 RID: 78466 RVA: 0x005B1857 File Offset: 0x005AFA57
		public static SpectateRectSide ToSpectatorSide(this int side)
		{
			switch (side)
			{
			case 0:
				return SpectateRectSide.Up;
			case 1:
				return SpectateRectSide.Right;
			case 2:
				return SpectateRectSide.Down;
			case 3:
				return SpectateRectSide.Left;
			default:
				return SpectateRectSide.None;
			}
		}

		// Token: 0x06013283 RID: 78467 RVA: 0x005B187C File Offset: 0x005AFA7C
		public static SpectateRectSide Opposite(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.None:
				return SpectateRectSide.All;
			case SpectateRectSide.Up:
				return SpectateRectSide.Down;
			case SpectateRectSide.Right:
				return SpectateRectSide.Left;
			case SpectateRectSide.Up | SpectateRectSide.Right:
			case SpectateRectSide.Right | SpectateRectSide.Down:
			case SpectateRectSide.Up | SpectateRectSide.Right | SpectateRectSide.Down:
			case SpectateRectSide.Up | SpectateRectSide.Left:
				break;
			case SpectateRectSide.Down:
				return SpectateRectSide.Up;
			case SpectateRectSide.Vertical:
				return SpectateRectSide.Horizontal;
			case SpectateRectSide.Left:
				return SpectateRectSide.Right;
			case SpectateRectSide.Horizontal:
				return SpectateRectSide.Vertical;
			default:
				if (side == SpectateRectSide.All)
				{
					return SpectateRectSide.None;
				}
				break;
			}
			return side;
		}

		// Token: 0x06013284 RID: 78468 RVA: 0x005B18D8 File Offset: 0x005AFAD8
		public static SpectateRectSide NextRotationClockwise(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return SpectateRectSide.Right;
			case SpectateRectSide.Right:
				return SpectateRectSide.Down;
			case SpectateRectSide.Down:
				return SpectateRectSide.Left;
			case SpectateRectSide.Vertical:
				return SpectateRectSide.Horizontal;
			case SpectateRectSide.Left:
				return SpectateRectSide.Up;
			case SpectateRectSide.Horizontal:
				return SpectateRectSide.Vertical;
			}
			return side;
		}

		// Token: 0x06013285 RID: 78469 RVA: 0x005B1928 File Offset: 0x005AFB28
		public static SpectateRectSide Rotated(this SpectateRectSide side, Rot4 rot)
		{
			SpectateRectSide spectateRectSide = side;
			if (rot.IsValid && rot != Rot4.North)
			{
				for (int i = 0; i < rot.AsInt; i++)
				{
					spectateRectSide = spectateRectSide.NextRotationClockwise();
				}
			}
			return spectateRectSide;
		}

		// Token: 0x06013286 RID: 78470 RVA: 0x005B1968 File Offset: 0x005AFB68
		public static Vector3 GraphicOffsetForRect(this SpectateRectSide side, IntVec3 center, CellRect rect, Rot4 rotation, Vector2 additionalOffset)
		{
			SpectateRectSide spectateRectSide = side.Rotated(rotation);
			Vector2 asVector = spectateRectSide.AsRot4().AsVector2;
			Vector2 vector = asVector;
			vector.Scale(new Vector3((float)rect.Width / 2f, (float)rect.Height / 2f));
			Vector2 vector2 = new Vector2((rect.Width % 2 == 0) ? 0.5f : 0f, (rect.Height % 2 == 0) ? 0.5f : 0f);
			if (rotation.AsInt > 1)
			{
				vector2 = -vector2;
			}
			if (rect.Height % 2 == 0 && rotation.IsHorizontal)
			{
				vector2.y = -vector2.y;
			}
			vector2 += (((spectateRectSide & SpectateRectSide.Horizontal) != SpectateRectSide.Horizontal) ? asVector : (-asVector)) * additionalOffset;
			vector2 += new Vector2(((rect.Width % 2 != 0) ? 0.75f : 0.5f) * asVector.x, ((rect.Height % 2 != 0) ? 0.75f : 0.5f) * asVector.y);
			return center.ToVector3Shifted() + new Vector3(vector.x, 0f, vector.y) + new Vector3(vector2.x, 0f, vector2.y);
		}

		// Token: 0x06013287 RID: 78471 RVA: 0x005B1AC8 File Offset: 0x005AFCC8
		public static bool TryFindCircleSpectatorCellFor(Pawn p, CellRect spectateRect, float minDistance, float maxDistance, Map map, out IntVec3 cell, List<IntVec3> extraDisallowedCells = null, Func<IntVec3, Pawn, Map, bool> extraValidator = null)
		{
			float minDistanceSquared = minDistance * minDistance;
			float maxDistanceSquared = maxDistance * maxDistance;
			Predicate<Thing> <>9__1;
			Predicate<IntVec3> predicate = delegate(IntVec3 x)
			{
				if (!x.InBounds(map))
				{
					return false;
				}
				if (spectateRect.Contains(x))
				{
					return false;
				}
				if (!x.Standable(map))
				{
					return false;
				}
				if (x.Fogged(map))
				{
					return false;
				}
				IntVec3 intVec3 = spectateRect.ClosestCellTo(x);
				if (!WanderUtility.InSameRoom(x, intVec3, map))
				{
					return false;
				}
				if ((float)intVec3.DistanceToSquared(x) > maxDistanceSquared)
				{
					return false;
				}
				if ((float)intVec3.DistanceToSquared(x) < minDistanceSquared)
				{
					return false;
				}
				if (!GenSight.LineOfSight(intVec3, x, map, true, null, 0, 0))
				{
					return false;
				}
				List<Thing> thingList = x.GetThingList(map);
				Predicate<Thing> match;
				if ((match = <>9__1) == null)
				{
					match = (<>9__1 = ((Thing y) => y is Pawn && y != p));
				}
				return thingList.Find(match) == null && (p == null || SpectatorCellFinder.CellAccessibleForPawn(p, x, map)) && (extraDisallowedCells == null || !extraDisallowedCells.Contains(x)) && (SpectatorCellFinder.CorrectlyRotatedChairAt(x, map, spectateRect) || SpectatorCellFinder.GoodCellToSpectateStanding(x, map, spectateRect)) && (extraValidator == null || extraValidator(x, p, map));
			};
			if (p != null && predicate(p.Position) && SpectatorCellFinder.CorrectlyRotatedChairAt(p.Position, map, spectateRect))
			{
				cell = p.Position;
				return true;
			}
			for (int i = 0; i < GenRadial.NumCellsInRadius(maxDistance); i++)
			{
				IntVec3 intVec = spectateRect.CenterCell + GenRadial.RadialPattern[i];
				if (predicate(intVec))
				{
					if (!SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
					{
						for (int j = 0; j < 90; j++)
						{
							IntVec3 intVec2 = intVec + GenRadial.RadialPattern[j];
							if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec2, map, spectateRect) && predicate(intVec2))
							{
								cell = intVec2;
								return true;
							}
						}
					}
					cell = intVec;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0400C6D2 RID: 50898
		private const float MaxDistanceToSpectateRect = 14.5f;

		// Token: 0x0400C6D3 RID: 50899
		private static float[] scorePerSide = new float[4];

		// Token: 0x0400C6D4 RID: 50900
		private static List<IntVec3> usedCells = new List<IntVec3>();
	}
}
