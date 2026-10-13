using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020D7 RID: 8407
	public class TileMutatorWorker_Cavern : TileMutatorWorker_Caves
	{
		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x0600B7BB RID: 47035 RVA: 0x0034EE1A File Offset: 0x0034D01A
		protected override float BranchChance
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x0600B7BC RID: 47036 RVA: 0x0034EE1A File Offset: 0x0034D01A
		protected override float WidthOffsetPerCell
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x0600B7BD RID: 47037 RVA: 0x0034EE21 File Offset: 0x0034D021
		protected override float MinTunnelWidth
		{
			get
			{
				return 1.6f;
			}
		}

		// Token: 0x0600B7BE RID: 47038 RVA: 0x0034EBAC File Offset: 0x0034CDAC
		public TileMutatorWorker_Cavern(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7BF RID: 47039 RVA: 0x0034EE28 File Offset: 0x0034D028
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.openingNoise = new DistFromPoint((float)map.Size.x * 0.4f);
			this.openingNoise = new Scale((double)TileMutatorWorker_Cavern.OpeningSquashRange.RandomInRange, 1.0, 1.0, this.openingNoise);
			this.openingNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.openingNoise);
			this.openingNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.openingNoise);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, 0.1f, map);
				this.openingNoise = new SmoothMin(this.openingNoise, rhs, 0.2);
			}
			this.openingNoise = MapNoiseUtility.AddDisplacementNoise(this.openingNoise, 0.015f, 40f, 4, -1);
			this.holesNoise = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			this.holesNoise = MapNoiseUtility.AddDisplacementNoise(this.holesNoise, 0.015f, 40f, 4, -1);
			base.Init(map);
		}

		// Token: 0x0600B7C0 RID: 47040 RVA: 0x0034EFD0 File Offset: 0x0034D1D0
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.openingNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
			this.CreateTunnels(map);
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (this.holesNoise.GetValue(intVec2) > 0.7f)
				{
					caves[intVec2] = 1f;
				}
			}
			RoofCollapseCellsFinder.CheckAndRemoveCollpsingRoofs(map);
		}

		// Token: 0x0600B7C1 RID: 47041 RVA: 0x0034F0A8 File Offset: 0x0034D2A8
		private void CreateTunnels(Map map)
		{
			TileMutatorWorker_Cavern.<>c__DisplayClass22_0 CS$<>8__locals1 = new TileMutatorWorker_Cavern.<>c__DisplayClass22_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.elevation = MapGenerator.Elevation;
			List<IntVec3> group = this.FindLargestContiguousRockFormation(CS$<>8__locals1.map);
			int randomInRange = TileMutatorWorker_Cavern.NumTunnelsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num = -1f;
				float dir = -1f;
				float num2 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = MapGenCavesUtility.FindRandomEdgeCellForTunnel(group, CS$<>8__locals1.map);
					float distToCave = MapGenCavesUtility.GetDistToCave(intVec, group, CS$<>8__locals1.map, 40f, false);
					float num4;
					float num3 = MapGenCavesUtility.FindBestInitialDir(intVec, group, out num4);
					if (!start.IsValid || distToCave > num || (Mathf.Approximately(distToCave, num) && num4 > num2))
					{
						start = intVec;
						num = distToCave;
						dir = num3;
						num2 = num4;
					}
				}
				float width = Rand.Range(5.6f, 7f);
				MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
				@default.widthOffsetPerCell = this.WidthOffsetPerCell;
				@default.minTunnelWidth = this.MinTunnelWidth;
				@default.branchChance = this.BranchChance;
				@default.allowBranchingAfterThisManyCells = 10;
				@default.widthNoiseAmplitude = 2f;
				MapGenCavesUtility.Dig(start, dir, width, group, CS$<>8__locals1.map, false, this.directionNoise, @default, new Predicate<IntVec3>(CS$<>8__locals1.<CreateTunnels>g__Rock|0), null);
			}
		}

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x0034F208 File Offset: 0x0034D408
		private List<IntVec3> FindLargestContiguousRockFormation(Map map)
		{
			HashSet<IntVec3> checkedCells = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			HashSet<IntVec3> current = new HashSet<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (TileMutatorWorker_Caves.IsRock(intVec, elevation, map) && !checkedCells.Contains(intVec))
				{
					current.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => TileMutatorWorker_Caves.IsRock(x, elevation, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							current.Add(x);
							checkedCells.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					if (current.Count > hashSet.Count)
					{
						hashSet.Clear();
						hashSet.AddRange(current);
					}
				}
			}
			return hashSet.ToList<IntVec3>();
		}

		// Token: 0x04007E44 RID: 32324
		private const float OpeningNoiseFreq = 0.015f;

		// Token: 0x04007E45 RID: 32325
		private const float OpeningNoiseStrength = 40f;

		// Token: 0x04007E46 RID: 32326
		private const float OpeningRadius = 0.4f;

		// Token: 0x04007E47 RID: 32327
		private const float OpeningThreshold = 0.5f;

		// Token: 0x04007E48 RID: 32328
		private static readonly FloatRange OpeningSquashRange = new FloatRange(1f, 1.3f);

		// Token: 0x04007E49 RID: 32329
		private const float CoastFalloff = 0.1f;

		// Token: 0x04007E4A RID: 32330
		private const float HolesFrequency = 0.02f;

		// Token: 0x04007E4B RID: 32331
		private const float HolesThreshold = 0.7f;

		// Token: 0x04007E4C RID: 32332
		private const float BaseWidth = 7f;

		// Token: 0x04007E4D RID: 32333
		private const int AllowBranchingAfterThisManyCells = 10;

		// Token: 0x04007E4E RID: 32334
		private static readonly IntRange NumTunnelsRange = new IntRange(4, 6);

		// Token: 0x04007E4F RID: 32335
		private ModuleBase openingNoise;

		// Token: 0x04007E50 RID: 32336
		private ModuleBase holesNoise;
	}
}
