using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public class MentalBreakDef : Def
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0004F0E2 File Offset: 0x0004D2E2
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0004F122 File Offset: 0x0004D322
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000E0F RID: 3599
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x04000E10 RID: 3600
		public MentalStateDef mentalState;

		// Token: 0x04000E11 RID: 3601
		public float baseCommonality;

		// Token: 0x04000E12 RID: 3602
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x04000E13 RID: 3603
		public MentalBreakIntensity intensity;

		// Token: 0x04000E14 RID: 3604
		public TraitDef requiredTrait;

		// Token: 0x04000E15 RID: 3605
		public GeneDef requiredGene;

		// Token: 0x04000E16 RID: 3606
		public bool anomalousBreak;

		// Token: 0x04000E17 RID: 3607
		public bool questLodgersCanDo = true;

		// Token: 0x04000E18 RID: 3608
		public PreceptDef requiredPrecept;

		// Token: 0x04000E19 RID: 3609
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04000E1A RID: 3610
		private MentalBreakWorker workerInt;
	}
}
