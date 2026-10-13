using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200373D RID: 14141
	public class StatWorker_ContainmentStrength : StatWorker
	{
		// Token: 0x060139C0 RID: 80320 RVA: 0x005D29E0 File Offset: 0x005D0BE0
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return (thingDef != null && thingDef.HasComp<CompEntityHolder>()) || req.StatBases.GetStatValueFromList(StatDefOf.ContainmentStrength, 0f) != 0f;
		}

		// Token: 0x060139C1 RID: 80321 RVA: 0x005D2A28 File Offset: 0x005D0C28
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return base.GetValueUnfinalized(req, applyPostProcess) + this.CalculateValues(req).Sum;
		}

		// Token: 0x060139C2 RID: 80322 RVA: 0x005D2A50 File Offset: 0x005D0C50
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StatWorker_ContainmentStrength.explanationSB.Clear();
			if (req.HasThing)
			{
				StatWorker_ContainmentStrength.ContainmentStrengthValues containmentStrengthValues = this.CalculateValues(req);
				if (containmentStrengthValues.lighting > 0f)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_AverageLighting".Translate(), containmentStrengthValues.lighting));
				}
				if (containmentStrengthValues.fullyRoofed < 0f)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_NotFullyRoofed".Translate(), containmentStrengthValues.fullyRoofed));
				}
				if (containmentStrengthValues.wallHp > 0f)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_AverageWallHp".Translate(), containmentStrengthValues.wallHp));
				}
				if (containmentStrengthValues.doorHp > 0f || containmentStrengthValues.doorBreach)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_AverageDoorHp".Translate(), containmentStrengthValues.doorHp) + (containmentStrengthValues.doorBreach ? string.Format(" ({0})", "Stat_ContainmentStrength_DoorForcedOpen".Translate()) : string.Empty));
				}
				if (containmentStrengthValues.floor > 0f)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_Flooring".Translate(), containmentStrengthValues.floor));
				}
				if (containmentStrengthValues.otherHoldingPlatformsCount > 0)
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_OtherHoldingPlatforms".Translate(containmentStrengthValues.otherHoldingPlatformsCount + 1), containmentStrengthValues.otherHoldingPlatformsOffset));
				}
				if (!Mathf.Approximately(containmentStrengthValues.globalOffset, 0f))
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: {1:F2}", "Stat_ContainmentStrength_MapwideOffset".Translate(), containmentStrengthValues.globalOffset));
				}
				if (!Mathf.Approximately(containmentStrengthValues.holderFactor, 1f))
				{
					StatWorker_ContainmentStrength.explanationSB.AppendLine(string.Format("{0}: x{1:F2}", "Stat_ContainmentStrength_Platform".Translate(), containmentStrengthValues.holderFactor));
				}
			}
			StatWorker_ContainmentStrength.explanationSB.Append(base.GetExplanationUnfinalized(req, numberSense));
			return StatWorker_ContainmentStrength.explanationSB.ToString();
		}

		// Token: 0x060139C3 RID: 80323 RVA: 0x005D2CC4 File Offset: 0x005D0EC4
		private StatWorker_ContainmentStrength.ContainmentStrengthValues CalculateValues(StatRequest req)
		{
			StatWorker_ContainmentStrength.ContainmentStrengthValues result;
			if (!req.HasThing)
			{
				result = default(StatWorker_ContainmentStrength.ContainmentStrengthValues);
				return result;
			}
			Map mapHeld = req.Thing.MapHeld;
			Room room = req.Thing.GetRoom(RegionType.Set_All);
			if (room == null || mapHeld == null || room.TouchesMapEdge)
			{
				result = default(StatWorker_ContainmentStrength.ContainmentStrengthValues);
				return result;
			}
			bool psychologicallyOutdoors = room.PsychologicallyOutdoors;
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (!psychologicallyOutdoors)
				{
					num += mapHeld.glowGrid.GroundGlowAt(c, false, false);
				}
			}
			num = num / (float)room.CellCount * 10f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			int num5 = 0;
			int num6 = 0;
			float globalOffset = 0f;
			bool doorBreach = false;
			float num7 = 0f;
			int num8 = 0;
			float holderFactor = 1f;
			if (!psychologicallyOutdoors)
			{
				int num9;
				try
				{
					this.CalculateDoorStats(room, out num9, out num3, out doorBreach);
				}
				finally
				{
					StatWorker_ContainmentStrength.tmpRegions.Clear();
					StatWorker_ContainmentStrength.tmpDoors.Clear();
				}
				foreach (Thing thing in room.ContainedAndAdjacentThings)
				{
					if (ThingRequestGroup.EntityHolder.Includes(thing.def) && thing != req.Thing)
					{
						num6++;
					}
				}
				CompEntityHolder compEntityHolder;
				if (req.Thing.TryGetComp(out compEntityHolder))
				{
					holderFactor = compEntityHolder.Props.containmentFactor;
				}
				foreach (IntVec3 c2 in room.Cells)
				{
					if (c2.InBounds(mapHeld) && !c2.Filled(mapHeld))
					{
						num7 += c2.GetTerrain(mapHeld).statBases.GetStatValueFromList(StatDefOf.ContainmentStrength, 0f);
						num8++;
					}
				}
				foreach (IntVec3 c3 in room.BorderCellsCached.Distinct<IntVec3>())
				{
					if (c3.InBounds(mapHeld))
					{
						Building edifice = c3.GetEdifice(mapHeld);
						if (edifice != null && !(edifice is Building_Door))
						{
							num2 += (float)edifice.HitPoints;
							num5++;
						}
					}
				}
				num4 = ((num8 == 0) ? 0f : (num7 / (float)num8));
				num2 = ((num5 == 0) ? 0f : StatWorker_ContainmentStrength.WallContainmentStrengthFromHPCurve.Evaluate(num2 / (float)num5));
				num3 = ((num9 == 0) ? 0f : (num3 / (float)num9 / 5f));
			}
			float num10 = num + num2 + num3 + num4;
			float num11 = num10;
			for (int i = 0; i < num6; i++)
			{
				num10 *= 0.9f;
			}
			num11 = num10 - num11;
			result = new StatWorker_ContainmentStrength.ContainmentStrengthValues
			{
				lighting = num,
				wallHp = num2,
				doorHp = num3,
				otherHoldingPlatformsCount = num6,
				otherHoldingPlatformsOffset = num11,
				globalOffset = globalOffset,
				floor = num4,
				fullyRoofed = ((!psychologicallyOutdoors && room.OpenRoofCount > 0) ? -30f : 0f),
				holderFactor = holderFactor,
				doorBreach = doorBreach
			};
			return result;
		}

		// Token: 0x060139C4 RID: 80324 RVA: 0x005D3054 File Offset: 0x005D1254
		private void CalculateDoorStats(Room room, out int doorCount, out float avgDoorHp, out bool doorBreach)
		{
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.type == RegionType.Portal && StatWorker_ContainmentStrength.tmpRegions.Add(otherRegion))
					{
						Building_Door door = otherRegion.door;
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							Region regionA = otherRegion.links[i].RegionA;
							Region regionB = otherRegion.links[i].RegionB;
							if ((regionA.Room != room && regionA != otherRegion && regionA.door != door) || (regionB.Room != room && regionB != otherRegion && regionB.door != door))
							{
								StatWorker_ContainmentStrength.tmpDoors.Add(door);
								break;
							}
						}
					}
				}
			}
			doorCount = StatWorker_ContainmentStrength.tmpDoors.Count;
			avgDoorHp = 0f;
			doorBreach = false;
			foreach (Building_Door building_Door in StatWorker_ContainmentStrength.tmpDoors)
			{
				if (building_Door.ContainmentBreached)
				{
					doorBreach = true;
					avgDoorHp = 0f;
				}
				else if (!doorBreach)
				{
					avgDoorHp += (float)building_Door.HitPoints;
				}
			}
		}

		// Token: 0x060139C5 RID: 80325 RVA: 0x005D3214 File Offset: 0x005D1414
		public static bool AnyDoorForcedOpen(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return false;
			}
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null && building_Door.ContainmentBreached)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400D940 RID: 55616
		private static HashSet<Region> tmpRegions = new HashSet<Region>();

		// Token: 0x0400D941 RID: 55617
		private static HashSet<Building_Door> tmpDoors = new HashSet<Building_Door>();

		// Token: 0x0400D942 RID: 55618
		private const float LightingMultiplier = 10f;

		// Token: 0x0400D943 RID: 55619
		private const float DoorHpDivisor = 5f;

		// Token: 0x0400D944 RID: 55620
		private const float OtherHoldingPlatformsFactor = 0.9f;

		// Token: 0x0400D945 RID: 55621
		private const float NotFullyRoofed = -30f;

		// Token: 0x0400D946 RID: 55622
		private static readonly SimpleCurve WallContainmentStrengthFromHPCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 100f),
				true
			},
			{
				new CurvePoint(10000f, 150f),
				true
			}
		};

		// Token: 0x0400D947 RID: 55623
		private static StringBuilder explanationSB = new StringBuilder();

		// Token: 0x0200373E RID: 14142
		private struct ContainmentStrengthValues
		{
			// Token: 0x17003240 RID: 12864
			// (get) Token: 0x060139C8 RID: 80328 RVA: 0x005D32FB File Offset: 0x005D14FB
			public float Sum
			{
				get
				{
					return (this.lighting + this.wallHp + this.doorHp + this.otherHoldingPlatformsOffset + this.globalOffset + this.fullyRoofed + this.floor) * this.holderFactor;
				}
			}

			// Token: 0x0400D948 RID: 55624
			public float lighting;

			// Token: 0x0400D949 RID: 55625
			public float wallHp;

			// Token: 0x0400D94A RID: 55626
			public float doorHp;

			// Token: 0x0400D94B RID: 55627
			public float floor;

			// Token: 0x0400D94C RID: 55628
			public int otherHoldingPlatformsCount;

			// Token: 0x0400D94D RID: 55629
			public float otherHoldingPlatformsOffset;

			// Token: 0x0400D94E RID: 55630
			public float globalOffset;

			// Token: 0x0400D94F RID: 55631
			public float fullyRoofed;

			// Token: 0x0400D950 RID: 55632
			public float holderFactor;

			// Token: 0x0400D951 RID: 55633
			public bool doorBreach;
		}
	}
}
