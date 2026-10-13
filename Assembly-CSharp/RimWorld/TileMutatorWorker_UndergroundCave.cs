using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002122 RID: 8482
	public class TileMutatorWorker_UndergroundCave : TileMutatorWorker_Caves
	{
		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x0600B901 RID: 47361 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected override float BranchChance
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x0600B902 RID: 47362 RVA: 0x00356938 File Offset: 0x00354B38
		protected override float WidthOffsetPerCell
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x0600B903 RID: 47363 RVA: 0x0034EE21 File Offset: 0x0034D021
		protected override float MinTunnelWidth
		{
			get
			{
				return 1.6f;
			}
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x0034EBAC File Offset: 0x0034CDAC
		public TileMutatorWorker_UndergroundCave(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x00356940 File Offset: 0x00354B40
		public override void GeneratePostElevationFertility(Map map)
		{
			TileMutatorWorker_UndergroundCave.<>c__DisplayClass11_0 CS$<>8__locals1 = new TileMutatorWorker_UndergroundCave.<>c__DisplayClass11_0();
			CS$<>8__locals1.map = map;
			base.GeneratePostElevationFertility(CS$<>8__locals1.map);
			this.directionNoise = new Perlin(0.002050000010058284, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			List<IntVec3> list = CS$<>8__locals1.map.AllCells.ToList<IntVec3>();
			int num = 0;
			while (hashSet.Count < 2000)
			{
				num++;
				hashSet.Clear();
				MapGenerator.Caves.Clear();
				IntVec3 start = list.RandomElement<IntVec3>();
				float width = Rand.Range(3.2f, 4f);
				MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
				@default.widthOffsetPerCell = this.WidthOffsetPerCell;
				@default.minTunnelWidth = this.MinTunnelWidth;
				@default.branchChance = this.BranchChance;
				@default.allowBranchingAfterThisManyCells = 10;
				MapGenCavesUtility.Dig(start, Rand.Range(0f, 360f), width, list, CS$<>8__locals1.map, true, this.directionNoise, @default, new Predicate<IntVec3>(CS$<>8__locals1.<GeneratePostElevationFertility>g__Rock|0), hashSet);
				if (num > 100)
				{
					Log.Error(string.Format("Underground cave generation exceeded {0} tries", 100));
					return;
				}
			}
		}

		// Token: 0x04007F9E RID: 32670
		private const int MinCaveSize = 2000;

		// Token: 0x04007F9F RID: 32671
		private const int MaxTries = 100;

		// Token: 0x04007FA0 RID: 32672
		private const float BaseWidth = 4f;

		// Token: 0x04007FA1 RID: 32673
		private const int AllowBranchingAfterThisManyCells = 10;
	}
}
