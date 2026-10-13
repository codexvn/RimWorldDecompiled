using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF6 RID: 8182
	public class GenStep_ScatterAncientTurret : GenStep_Scatterer
	{
		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600B44A RID: 46154 RVA: 0x0033AA77 File Offset: 0x00338C77
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x0600B44B RID: 46155 RVA: 0x0033B6E8 File Offset: 0x003398E8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient turret"))
			{
				return;
			}
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			this.count = 1;
			this.warnOnFail = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B44C RID: 46156 RVA: 0x0033B720 File Offset: 0x00339920
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map) || !loc.InBounds(map))
			{
				return false;
			}
			if (loc.GetTerrain(map).IsWater)
			{
				return false;
			}
			RoofDef roof = loc.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				return false;
			}
			if (loc.GetEdifice(map) != null)
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(loc, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(loc, 2f, false))
			{
				if (intVec.InBounds(map) && !intVec.Roofed(map) && intVec.GetEdifice(map) == null && GenSight.LineOfSight(loc, intVec, map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B44D RID: 46157 RVA: 0x0033B7FC File Offset: 0x003399FC
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientSecurityTurret, null), loc, map, Rot4.North, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_DriedBlood, 0.5f, 1, 3, null);
		}

		// Token: 0x04007BA7 RID: 31655
		private const int ShellScatterRadius = 10;

		// Token: 0x04007BA8 RID: 31656
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
