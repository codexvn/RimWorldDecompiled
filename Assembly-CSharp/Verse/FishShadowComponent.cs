using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000465 RID: 1125
	[StaticConstructorOnStartup]
	public class FishShadowComponent : MapComponent
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x00095CBB File Offset: 0x00093EBB
		public FishShadowComponent(Map map) : base(map)
		{
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x00095CD0 File Offset: 0x00093ED0
		public override void MapComponentTick()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			foreach (WaterBody waterBody in this.map.waterBodyTracker.Bodies)
			{
				if (waterBody.HasFish && waterBody.CellCount >= 12 && GenTicks.IsTickInterval(waterBody.GetHashCode(), Mathf.CeilToInt(450f / Mathf.Max(1f, (float)waterBody.CellCount / 550f))))
				{
					IntVec3? intVec = FishShadowComponent.FindFishFleckLocation(waterBody);
					if (intVec != null)
					{
						FishShadowComponent.SpawnFishFleck(this.map, intVec.Value.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteLow));
					}
				}
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x00095D9C File Offset: 0x00093F9C
		private static IntVec3? FindFishFleckLocation(WaterBody body)
		{
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = body.cells.RandomElement<IntVec3>();
				List<IntVec3> list = GenAdjFast.AdjacentCells8Way(intVec);
				bool flag = true;
				foreach (IntVec3 intVec2 in list)
				{
					if (!body.cells.Contains(intVec2) || !intVec2.GetTerrain(body.map).IsWater)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return new IntVec3?(intVec);
				}
			}
			return null;
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x00095E40 File Offset: 0x00094040
		private static void SpawnFishFleck(Map map, Vector3 loc)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			bool @bool = Rand.Bool;
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, @bool ? FleckDefOf.FishShadowReverse : FleckDefOf.FishShadow, 3f * Rand.Range(0.4f, 1.1f));
			dataStatic.rotation = Rand.Range(0f, 180f);
			dataStatic.rotationRate = (@bool ? -1f : 1f) * 75f * Rand.Range(0.75f, 1.25f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x040019A9 RID: 6569
		public List<IntVec3> fishWaterCells = new List<IntVec3>();

		// Token: 0x040019AA RID: 6570
		private const int SpawnFishFleckIntervalTicks = 450;

		// Token: 0x040019AB RID: 6571
		private const int CellsPerFish = 550;

		// Token: 0x040019AC RID: 6572
		private const int SpawnFishFleckMaxAttempts = 5;

		// Token: 0x040019AD RID: 6573
		private const float FishFleckRotationSpeed = 75f;

		// Token: 0x040019AE RID: 6574
		private const int MinCellCount = 12;
	}
}
