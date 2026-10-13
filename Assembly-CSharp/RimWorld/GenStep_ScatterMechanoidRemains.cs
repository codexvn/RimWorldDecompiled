using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFF RID: 8191
	[Obsolete]
	public class GenStep_ScatterMechanoidRemains : GenStep_Scatterer
	{
		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x0600B470 RID: 46192 RVA: 0x0033AA77 File Offset: 0x00338C77
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x0600B471 RID: 46193 RVA: 0x0033C3CD File Offset: 0x0033A5CD
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter mechanoid remains"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B472 RID: 46194 RVA: 0x0033C3F2 File Offset: 0x0033A5F2
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			return base.CanScatterAt(loc, map) && this.CanPlaceDebrisAt(loc, map);
		}

		// Token: 0x0600B473 RID: 46195 RVA: 0x0033C40D File Offset: 0x0033A60D
		private bool CanPlaceDebrisAt(IntVec3 loc, Map map)
		{
			return loc.InBounds(map) && !loc.Roofed(map) && loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && !map.terrainGrid.TerrainAt(loc).IsWater;
		}

		// Token: 0x0600B474 RID: 46196 RVA: 0x0033C44C File Offset: 0x0033A64C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			foreach (IntVec3 c in GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientMechDropBeacon, null), loc, map, Rot4.North, WipeMode.Vanish, false, false).OccupiedRect())
			{
				if (Rand.Bool && c.InBounds(map) && !c.Impassable(map))
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_MachineBits, 1, FilthSourceFlags.None, true);
				}
			}
			FilthMaker.TryMakeFilth(loc, map, ThingDefOf.Filth_RubbleBuilding, 1, FilthSourceFlags.None, true);
			CellRect cellRect = CellRect.CenteredOn(loc, 10);
			int randomInRange = GenStep_ScatterMechanoidRemains.MaxShellsRange.RandomInRange;
			int num = 0;
			foreach (IntVec3 intVec in cellRect.InRandomOrder(null))
			{
				if (GenSight.LineOfSight(intVec, loc, map, true, null, 0, 0))
				{
					ThingDef chunkMechanoidSlag = ThingDefOf.ChunkMechanoidSlag;
					Rot4 rot = chunkMechanoidSlag.rotatable ? Rot4.Random : Rot4.North;
					bool flag = true;
					foreach (IntVec3 loc2 in GenAdj.OccupiedRect(intVec, rot, chunkMechanoidSlag.size))
					{
						if (!this.CanPlaceDebrisAt(loc2, map))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(chunkMechanoidSlag, null), intVec, map, rot, WipeMode.Vanish, false, false);
						FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
						num++;
						if (num >= randomInRange)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x04007BB7 RID: 31671
		private const int ShellScatterRadius = 10;

		// Token: 0x04007BB8 RID: 31672
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
