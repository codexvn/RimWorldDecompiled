using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038E6 RID: 14566
	public class GlobalSettings
	{
		// Token: 0x060141A1 RID: 82337 RVA: 0x005FB44C File Offset: 0x005F964C
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.requiredWorshippedTerminalRooms = 0;
			this.requiredGravcoreRooms = 0;
			this.minEmptyNodes = 0;
			this.minLandingPads = 0;
			this.minThroneRooms = 0;
			this.maxFarms = -1;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_landingPadsResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_throneRoomsResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_farmsCount = 0;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
			this.basePart_worshippedTerminalsResolved = 0;
			this.basePart_gravcoresResolved = 0;
		}

		// Token: 0x060141A2 RID: 82338 RVA: 0x005FB507 File Offset: 0x005F9707
		public void ClearResult()
		{
			this.landingPadsGenerated = 0;
		}

		// Token: 0x0400DDF4 RID: 56820
		public Map map;

		// Token: 0x0400DDF5 RID: 56821
		public int minBuildings;

		// Token: 0x0400DDF6 RID: 56822
		public int minEmptyNodes;

		// Token: 0x0400DDF7 RID: 56823
		public int minLandingPads;

		// Token: 0x0400DDF8 RID: 56824
		public int minBarracks;

		// Token: 0x0400DDF9 RID: 56825
		public int requiredWorshippedTerminalRooms;

		// Token: 0x0400DDFA RID: 56826
		public int requiredGravcoreRooms;

		// Token: 0x0400DDFB RID: 56827
		public int minThroneRooms;

		// Token: 0x0400DDFC RID: 56828
		public int maxFarms = -1;

		// Token: 0x0400DDFD RID: 56829
		public CellRect mainRect;

		// Token: 0x0400DDFE RID: 56830
		public int basePart_buildingsResolved;

		// Token: 0x0400DDFF RID: 56831
		public int basePart_emptyNodesResolved;

		// Token: 0x0400DE00 RID: 56832
		public int basePart_landingPadsResolved;

		// Token: 0x0400DE01 RID: 56833
		public int basePart_barracksResolved;

		// Token: 0x0400DE02 RID: 56834
		public int basePart_throneRoomsResolved;

		// Token: 0x0400DE03 RID: 56835
		public int basePart_worshippedTerminalsResolved;

		// Token: 0x0400DE04 RID: 56836
		public int basePart_gravcoresResolved;

		// Token: 0x0400DE05 RID: 56837
		public float basePart_batteriesCoverage;

		// Token: 0x0400DE06 RID: 56838
		public float basePart_farmsCoverage;

		// Token: 0x0400DE07 RID: 56839
		public int basePart_farmsCount;

		// Token: 0x0400DE08 RID: 56840
		public float basePart_powerPlantsCoverage;

		// Token: 0x0400DE09 RID: 56841
		public float basePart_breweriesCoverage;

		// Token: 0x0400DE0A RID: 56842
		public int landingPadsGenerated;
	}
}
