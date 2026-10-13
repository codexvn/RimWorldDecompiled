using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC3 RID: 8131
	public class GenStep_MutatorNonCriticalStructures : GenStep
	{
		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600B343 RID: 45891 RVA: 0x00335A8C File Offset: 0x00333C8C
		public override int SeedPart
		{
			get
			{
				return 125123443;
			}
		}

		// Token: 0x0600B344 RID: 45892 RVA: 0x00335A94 File Offset: 0x00333C94
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GenerateNonCriticalStructures(map);
				}
			}
		}
	}
}
