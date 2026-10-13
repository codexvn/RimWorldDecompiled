using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020C0 RID: 8384
	public class TileMutatorWorker_AncientRuins : TileMutatorWorker
	{
		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x0600B771 RID: 46961 RVA: 0x0034D364 File Offset: 0x0034B564
		protected static GenStep_AncientRuins GenStep
		{
			get
			{
				return (GenStep_AncientRuins)GenStepDefOf.AncientRuins_Special.genStep;
			}
		}

		// Token: 0x0600B772 RID: 46962 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_AncientRuins(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B773 RID: 46963 RVA: 0x0034D378 File Offset: 0x0034B578
		public override void GenerateNonCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			try
			{
				TileMutatorWorker_AncientRuins.GenStep.GenerateRuins(map, default(GenStepParams), TileMutatorWorker_AncientRuins.MapFillPercentRange);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating ancient ruins: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04007DFC RID: 32252
		private static readonly FloatRange MapFillPercentRange = new FloatRange(0.3f, 0.5f);
	}
}
