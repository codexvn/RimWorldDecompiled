using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CAE RID: 15534
	public class SurfaceTile : Tile
	{
		// Token: 0x170034A4 RID: 13476
		// (get) Token: 0x060151ED RID: 86509 RVA: 0x00652611 File Offset: 0x00650811
		public override bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x170034A5 RID: 13477
		// (get) Token: 0x060151EE RID: 86510 RVA: 0x00652623 File Offset: 0x00650823
		public List<SurfaceTile.RoadLink> Roads
		{
			get
			{
				if (!base.PrimaryBiome.allowRoads)
				{
					return null;
				}
				return this.potentialRoads;
			}
		}

		// Token: 0x170034A6 RID: 13478
		// (get) Token: 0x060151EF RID: 86511 RVA: 0x0065263A File Offset: 0x0065083A
		public List<SurfaceTile.RiverLink> Rivers
		{
			get
			{
				if (!base.PrimaryBiome.allowRivers)
				{
					return null;
				}
				return this.potentialRivers;
			}
		}

		// Token: 0x060151F0 RID: 86512 RVA: 0x00652651 File Offset: 0x00650851
		public SurfaceTile()
		{
		}

		// Token: 0x060151F1 RID: 86513 RVA: 0x00652659 File Offset: 0x00650859
		public SurfaceTile(PlanetTile tile) : base(tile)
		{
		}

		// Token: 0x060151F2 RID: 86514 RVA: 0x00652664 File Offset: 0x00650864
		public override string ToString()
		{
			string str = string.Format("({0} elev={1}m hill={2} temp={3}°C rain={4}mm", new object[]
			{
				base.PrimaryBiome,
				this.elevation,
				this.hilliness,
				this.temperature,
				this.rainfall
			});
			string format = " swampiness={0} potentialRoads={1} (allowed={2})";
			object arg = this.swampiness.ToStringPercent();
			List<SurfaceTile.RoadLink> list = this.potentialRoads;
			string str2 = string.Format(format, arg, (list != null) ? list.Count : 0, base.PrimaryBiome.allowRoads);
			string format2 = " potentialRivers={0} (allowed={1}))";
			List<SurfaceTile.RiverLink> list2 = this.potentialRivers;
			return str + str2 + string.Format(format2, (list2 != null) ? list2.Count : 0, base.PrimaryBiome.allowRivers);
		}

		// Token: 0x060151F3 RID: 86515 RVA: 0x00652734 File Offset: 0x00650934
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.riverDist, "riverDist", 0, false);
		}

		// Token: 0x0400EB9B RID: 60315
		public List<SurfaceTile.RoadLink> potentialRoads;

		// Token: 0x0400EB9C RID: 60316
		public List<SurfaceTile.RiverLink> potentialRivers;

		// Token: 0x0400EB9D RID: 60317
		public int riverDist;

		// Token: 0x02003CAF RID: 15535
		public struct RoadLink
		{
			// Token: 0x0400EB9E RID: 60318
			public PlanetTile neighbor;

			// Token: 0x0400EB9F RID: 60319
			public RoadDef road;
		}

		// Token: 0x02003CB0 RID: 15536
		public struct RiverLink
		{
			// Token: 0x0400EBA0 RID: 60320
			public PlanetTile neighbor;

			// Token: 0x0400EBA1 RID: 60321
			public RiverDef river;
		}
	}
}
