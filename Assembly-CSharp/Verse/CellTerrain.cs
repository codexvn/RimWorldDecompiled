using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000487 RID: 1159
	public struct CellTerrain
	{
		// Token: 0x06001E65 RID: 7781 RVA: 0x0009EAE7 File Offset: 0x0009CCE7
		public CellTerrain(TerrainDef def, bool polluted, float snowCoverage, float sandCoverage, ColorDef color)
		{
			this.def = def;
			this.polluted = polluted;
			this.snowCoverage = snowCoverage;
			this.sandCoverage = sandCoverage;
			this.color = color;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0009EB10 File Offset: 0x0009CD10
		public override bool Equals(object obj)
		{
			if (obj is CellTerrain)
			{
				CellTerrain terrain = (CellTerrain)obj;
				return this.Equals(terrain);
			}
			return false;
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0009EB38 File Offset: 0x0009CD38
		public bool Equals(CellTerrain terrain)
		{
			return terrain.def == this.def && terrain.color == this.color && terrain.polluted == this.polluted && Mathf.Abs(terrain.snowCoverage - this.snowCoverage) < float.Epsilon && Mathf.Abs(terrain.sandCoverage - this.sandCoverage) < float.Epsilon;
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0009EBA3 File Offset: 0x0009CDA3
		public override int GetHashCode()
		{
			return Gen.HashCombine<ColorDef>(Gen.HashCombine<float>(Gen.HashCombine<float>(Gen.HashCombine<bool>(Gen.HashCombine<TerrainDef>(0, this.def), this.polluted), this.snowCoverage), this.sandCoverage), this.color);
		}

		// Token: 0x04001A31 RID: 6705
		public TerrainDef def;

		// Token: 0x04001A32 RID: 6706
		public bool polluted;

		// Token: 0x04001A33 RID: 6707
		public float snowCoverage;

		// Token: 0x04001A34 RID: 6708
		public float sandCoverage;

		// Token: 0x04001A35 RID: 6709
		public ColorDef color;
	}
}
