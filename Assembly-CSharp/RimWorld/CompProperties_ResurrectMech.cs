using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200226F RID: 8815
	public class CompProperties_ResurrectMech : CompProperties_AbilityEffect
	{
		// Token: 0x0600C016 RID: 49174 RVA: 0x0037A7AC File Offset: 0x003789AC
		public CompProperties_ResurrectMech()
		{
			this.compClass = typeof(CompAbilityEffect_ResurrectMech);
		}

		// Token: 0x0600C017 RID: 49175 RVA: 0x0037A7DA File Offset: 0x003789DA
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.costs.NullOrEmpty<MechChargeCosts>())
			{
				yield return "costs list is null";
			}
			else
			{
				foreach (MechChargeCosts mechChargeCosts in this.costs)
				{
					if (mechChargeCosts.weightClass == null)
					{
						yield return string.Format("costs list contains null weight class with cost {0}", mechChargeCosts.cost);
					}
				}
				List<MechChargeCosts>.Enumerator enumerator2 = default(List<MechChargeCosts>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040083A3 RID: 33699
		public int maxCorpseAgeTicks = int.MaxValue;

		// Token: 0x040083A4 RID: 33700
		public List<MechChargeCosts> costs = new List<MechChargeCosts>();

		// Token: 0x040083A5 RID: 33701
		public EffecterDef appliedEffecterDef;

		// Token: 0x040083A6 RID: 33702
		public EffecterDef centerEffecterDef;
	}
}
