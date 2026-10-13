using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200210A RID: 8458
	public class TileMutatorWorker_ObsidianDeposits : TileMutatorWorker
	{
		// Token: 0x0600B893 RID: 47251 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_ObsidianDeposits(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B894 RID: 47252 RVA: 0x00353D04 File Offset: 0x00351F04
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float num = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map) * 0.1f;
			new GenStep_ScatterLumpsMineable
			{
				maxValue = float.MaxValue,
				countPer10kCellsRange = new FloatRange(num, num),
				forcedDefToScatter = ThingDefOf.MineableObsidian
			}.Generate(map, default(GenStepParams));
		}

		// Token: 0x04007F3C RID: 32572
		private const float BlotchFactor = 0.1f;
	}
}
