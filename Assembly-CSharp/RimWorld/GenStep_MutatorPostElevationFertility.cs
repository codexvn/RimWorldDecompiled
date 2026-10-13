using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC0 RID: 8128
	public class GenStep_MutatorPostElevationFertility : GenStep
	{
		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x0600B33A RID: 45882 RVA: 0x00335961 File Offset: 0x00333B61
		public override int SeedPart
		{
			get
			{
				return 1239847543;
			}
		}

		// Token: 0x0600B33B RID: 45883 RVA: 0x00335968 File Offset: 0x00333B68
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostElevationFertility(map);
				}
			}
		}
	}
}
