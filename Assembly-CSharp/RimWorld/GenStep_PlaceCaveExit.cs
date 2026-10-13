using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002025 RID: 8229
	public class GenStep_PlaceCaveExit : GenStep
	{
		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x0600B507 RID: 46343 RVA: 0x0033FD12 File Offset: 0x0033DF12
		public override int SeedPart
		{
			get
			{
				return 12412314;
			}
		}

		// Token: 0x0600B508 RID: 46344 RVA: 0x0033FD1C File Offset: 0x0033DF1C
		public override void Generate(Map map, GenStepParams parms)
		{
			IntVec3 intVec;
			CellFinder.TryFindRandomCell(map, (IntVec3 cell) => cell.Standable(map) && (float)cell.DistanceToEdge(map) > 5.5f, out intVec);
			foreach (IntVec3 c in GenRadial.RadialCellsAround(intVec, 4.5f, true))
			{
				foreach (Thing thing in from t in c.GetThingList(map).ToList<Thing>()
				where t.def.destroyable
				select t)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.CaveExit, null), intVec, map, WipeMode.Vanish);
			MapGenerator.PlayerStartSpot = intVec;
		}

		// Token: 0x04007C37 RID: 31799
		public const float ClearRadius = 4.5f;
	}
}
