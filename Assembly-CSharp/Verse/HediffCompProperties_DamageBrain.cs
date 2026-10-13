using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200069B RID: 1691
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x06002A8F RID: 10895 RVA: 0x000DFD23 File Offset: 0x000DDF23
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000DFD46 File Offset: 0x000DDF46
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.damageAmount == IntRange.Zero)
			{
				yield return "damageAmount is not defined";
			}
			if (this.mtbDaysPerStage == null)
			{
				yield return "mtbDaysPerStage is not defined";
			}
			else if (this.mtbDaysPerStage.Count != parentDef.stages.Count)
			{
				yield return "mtbDaysPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002279 RID: 8825
		public IntRange damageAmount = IntRange.Zero;

		// Token: 0x0400227A RID: 8826
		public List<float> mtbDaysPerStage;
	}
}
