using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000705 RID: 1797
	public class HediffCompProperties_SeverityFromGasDensityDirect : HediffCompProperties
	{
		// Token: 0x06002BF6 RID: 11254 RVA: 0x000E4C49 File Offset: 0x000E2E49
		public HediffCompProperties_SeverityFromGasDensityDirect()
		{
			this.compClass = typeof(HediffComp_SeverityFromGasDensityDirect);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000E4C74 File Offset: 0x000E2E74
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.densityStages.NullOrEmpty<float>())
			{
				yield return "densityStages is empty";
			}
			else if (parentDef.stages.NullOrEmpty<HediffStage>())
			{
				yield return "has no stages";
			}
			else if (this.densityStages.Count != parentDef.stages.Count)
			{
				yield return "densityStages count doesn't match stages count";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400235B RID: 9051
		public GasType gasType;

		// Token: 0x0400235C RID: 9052
		public int intervalTicks = 60;

		// Token: 0x0400235D RID: 9053
		public List<float> densityStages = new List<float>();
	}
}
