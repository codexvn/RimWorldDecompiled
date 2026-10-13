using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001F9C RID: 8092
	public class GenStep_AsteroidItemStash : GenStep
	{
		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x0600B2B2 RID: 45746 RVA: 0x00332EE0 File Offset: 0x003310E0
		public override int SeedPart
		{
			get
			{
				return 234098237;
			}
		}

		// Token: 0x0600B2B3 RID: 45747 RVA: 0x00332EE8 File Offset: 0x003310E8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Asteroid Item Stash"))
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			IntVec2 size = new IntVec2(GenStep_AsteroidItemStash.StructureSizeRange.RandomInRange, GenStep_AsteroidItemStash.StructureSizeRange.RandomInRange);
			CellRect item;
			if (!this.FindStashRect(map, size, out item))
			{
				return;
			}
			ModuleBase moduleBase = new DistFromPointRects(new List<CellRect>
			{
				item.ExpandedBy(3)
			});
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, -1);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (moduleBase.GetValue(intVec) > 0f)
				{
					Building edifice = intVec.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			StructureGenParams parms2 = new StructureGenParams
			{
				size = size
			};
			LayoutWorker worker = LayoutDefOf.OrbitalItemStash.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
			List<Thing> list = new List<Thing>();
			LayoutStructureSketch layoutStructureSketch2 = layoutStructureSketch;
			IntVec3 min = item.Min;
			List<Thing> allSpawnedThings = list;
			worker.Spawn(layoutStructureSketch2, map, min, null, allSpawnedThings, true, false, null);
			orGenerateVar.Add(item);
			foreach (Thing thing in list)
			{
				if (thing.def.IsDoor)
				{
					this.ClearAreaAroundDoor(thing.Position, map);
				}
			}
			map.OrbitalDebris = OrbitalDebrisDefOf.Asteroid;
		}

		// Token: 0x0600B2B4 RID: 45748 RVA: 0x00333080 File Offset: 0x00331280
		private void ClearAreaAroundDoor(IntVec3 thingPosition, Map map)
		{
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(thingPosition, map, 100, null))
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.building.isNaturalRock)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B2B5 RID: 45749 RVA: 0x003330F4 File Offset: 0x003312F4
		private bool FindStashRect(Map map, IntVec2 size, out CellRect rect)
		{
			rect = CellRect.Empty;
			int num = 0;
			Predicate<IntVec3> <>9__0;
			Func<IntVec3, bool> <>9__1;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 center = map.Center;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.GetEdifice(map) != null));
				}
				IntVec3 center2;
				if (RCellFinder.TryFindRandomCellNearWith(center, validator, map, out center2, 5, 2147483647))
				{
					CellRect cellRect = CellRect.CenteredOn(center2, size);
					IEnumerable<IntVec3> cells = cellRect.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((IntVec3 c) => c.GetEdifice(map) != null));
					}
					int num2 = cells.Count(predicate);
					if (num2 > num)
					{
						num = num2;
						rect = cellRect;
					}
				}
			}
			return (float)num > (float)rect.Area / 2f;
		}

		// Token: 0x04007A6A RID: 31338
		private const float ClearNoiseFreq = 0.03f;

		// Token: 0x04007A6B RID: 31339
		private const float ClearNoiseStrength = 5f;

		// Token: 0x04007A6C RID: 31340
		private static readonly IntRange StructureSizeRange = new IntRange(30, 40);

		// Token: 0x04007A6D RID: 31341
		private const int DoorClearArea = 100;
	}
}
