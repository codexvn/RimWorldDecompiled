using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC2 RID: 8130
	public class GenStep_MutatorCriticalStructures : GenStep
	{
		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600B340 RID: 45888 RVA: 0x00335A28 File Offset: 0x00333C28
		public override int SeedPart
		{
			get
			{
				return 651234136;
			}
		}

		// Token: 0x0600B341 RID: 45889 RVA: 0x00335A30 File Offset: 0x00333C30
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GenerateCriticalStructures(map);
				}
			}
		}
	}
}
