using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC1 RID: 8129
	public class GenStep_MutatorPostTerrain : GenStep
	{
		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x0600B33D RID: 45885 RVA: 0x003359C4 File Offset: 0x00333BC4
		public override int SeedPart
		{
			get
			{
				return 562343345;
			}
		}

		// Token: 0x0600B33E RID: 45886 RVA: 0x003359CC File Offset: 0x00333BCC
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostTerrain(map);
				}
			}
		}
	}
}
