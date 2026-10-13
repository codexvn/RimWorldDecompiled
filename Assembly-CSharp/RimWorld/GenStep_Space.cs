using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200A RID: 8202
	public class GenStep_Space : GenStep
	{
		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x0600B4A8 RID: 46248 RVA: 0x0033D4D8 File Offset: 0x0033B6D8
		public override int SeedPart
		{
			get
			{
				return 196743;
			}
		}

		// Token: 0x0600B4A9 RID: 46249 RVA: 0x0033D4E0 File Offset: 0x0033B6E0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = false;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in map.AllCells)
			{
				terrainGrid.SetTerrain(c, TerrainDefOf.Space);
			}
		}
	}
}
