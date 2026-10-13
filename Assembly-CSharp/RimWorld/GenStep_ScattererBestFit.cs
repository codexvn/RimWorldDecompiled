using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E0 RID: 14304
	public abstract class GenStep_ScattererBestFit : GenStep_Scatterer
	{
		// Token: 0x1700329E RID: 12958
		// (get) Token: 0x06013D39 RID: 81209
		protected abstract IntVec2 Size { get; }

		// Token: 0x06013D3A RID: 81210
		public abstract bool CollisionAt(IntVec3 cell, Map map);

		// Token: 0x06013D3B RID: 81211 RVA: 0x005E3080 File Offset: 0x005E1280
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect cellRect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			if (!cellRect.FullyContainedWithin(CellRect.WholeMap(map)))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.InBounds(map))
				{
					return false;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (this.CollisionAt(intVec, map))
				{
					num++;
				}
			}
			if (num < GenStep_ScattererBestFit.bestOccupiedScore)
			{
				GenStep_ScattererBestFit.bestOccupiedScore = num;
				GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = new IntVec3?(c);
			}
			return num == 0;
		}

		// Token: 0x06013D3C RID: 81212 RVA: 0x005E31AC File Offset: 0x005E13AC
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			GenStep_ScattererBestFit.bestOccupiedScore = int.MaxValue;
			GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = null;
			if (this.CanScatterAt(map.Center, map))
			{
				result = map.Center;
				return true;
			}
			int num = Mathf.FloorToInt((float)Mathf.Min(map.Size.x, map.Size.z) / 2f);
			Vector3 v = IntVec3.North.ToVector3();
			for (int i = 5; i <= num; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					float angle = Rand.Range(0f, 360f);
					Vector3 a = v.RotatedBy(angle);
					int num2 = Rand.Range(i - 5, i);
					IntVec3 intVec = map.Center + (a * (float)num2).ToIntVec3();
					if (this.CanScatterAt(intVec, map))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions != null)
			{
				result = GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions.Value;
				return true;
			}
			if (this.warnOnFail)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Scatterer ",
					this.ToString(),
					" from def ",
					this.def.defName,
					" could not find cell to generate at."
				}));
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0400DB9A RID: 56218
		private const int DistanceStep = 5;

		// Token: 0x0400DB9B RID: 56219
		private const int AttemptsPerStep = 5;

		// Token: 0x0400DB9C RID: 56220
		private static IntVec3? bestCellWithLeastOccupiedCollisions;

		// Token: 0x0400DB9D RID: 56221
		private static int bestOccupiedScore;
	}
}
