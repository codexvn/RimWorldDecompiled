using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038BA RID: 14522
	public class SketchResolver_AddCornerThings : SketchResolver
	{
		// Token: 0x060140E3 RID: 82147 RVA: 0x005F50B4 File Offset: 0x005F32B4
		protected override void ResolveInt(SketchResolveParams parms)
		{
			this.wallPositions.Clear();
			for (int i = 0; i < parms.sketch.Things.Count; i++)
			{
				if (parms.sketch.Things[i].def == ThingDefOf.Wall)
				{
					this.wallPositions.Add(parms.sketch.Things[i].pos);
				}
			}
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.cornerThing, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.cornerThing));
			bool valueOrDefault = parms.requireFloor.GetValueOrDefault();
			try
			{
				foreach (IntVec3 intVec in this.wallPositions)
				{
					if (Rand.Chance(0.09f))
					{
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z - 1)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z + 1)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, stuff, 1, null, null, false, 2f);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!valueOrDefault || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false, 2f);
						}
					}
				}
			}
			finally
			{
				this.wallPositions.Clear();
			}
		}

		// Token: 0x060140E4 RID: 82148 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0400DD7F RID: 56703
		private HashSet<IntVec3> wallPositions = new HashSet<IntVec3>();

		// Token: 0x0400DD80 RID: 56704
		private const float Chance = 0.09f;
	}
}
