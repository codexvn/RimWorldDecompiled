using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001959 RID: 6489
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06009372 RID: 37746 RVA: 0x002B5BFA File Offset: 0x002B3DFA
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06009373 RID: 37747 RVA: 0x002B5C09 File Offset: 0x002B3E09
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x06009374 RID: 37748 RVA: 0x002B5C1D File Offset: 0x002B3E1D
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x06009375 RID: 37749 RVA: 0x002B5C56 File Offset: 0x002B3E56
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005EDF RID: 24287
		public ChemicalDef chemical;

		// Token: 0x04005EE0 RID: 24288
		public float addictiveness;

		// Token: 0x04005EE1 RID: 24289
		public float minToleranceToAddict;

		// Token: 0x04005EE2 RID: 24290
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x04005EE3 RID: 24291
		public float needLevelOffset = 1f;

		// Token: 0x04005EE4 RID: 24292
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x04005EE5 RID: 24293
		public float largeOverdoseChance;

		// Token: 0x04005EE6 RID: 24294
		public bool isCombatEnhancingDrug;

		// Token: 0x04005EE7 RID: 24295
		public bool teetotalerCanConsume;

		// Token: 0x04005EE8 RID: 24296
		public float listOrder;
	}
}
