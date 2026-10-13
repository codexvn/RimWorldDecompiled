using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC4 RID: 8132
	public class GenStep_MutatorFinal : GenStep
	{
		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x0600B346 RID: 45894 RVA: 0x00335AF0 File Offset: 0x00333CF0
		public override int SeedPart
		{
			get
			{
				return 756452345;
			}
		}

		// Token: 0x0600B347 RID: 45895 RVA: 0x00335AF8 File Offset: 0x00333CF8
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostFog(map);
				}
			}
		}
	}
}
