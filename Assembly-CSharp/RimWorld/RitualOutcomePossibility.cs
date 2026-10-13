using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200272E RID: 10030
	public class RitualOutcomePossibility : ILordJobOutcomePossibility
	{
		// Token: 0x170021B6 RID: 8630
		// (get) Token: 0x0600DC1D RID: 56349 RVA: 0x0040D867 File Offset: 0x0040BA67
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170021B7 RID: 8631
		// (get) Token: 0x0600DC1E RID: 56350 RVA: 0x0040D874 File Offset: 0x0040BA74
		public TaggedString ToolTip
		{
			get
			{
				return this.potentialExtraOutcomeDesc;
			}
		}

		// Token: 0x170021B8 RID: 8632
		// (get) Token: 0x0600DC1F RID: 56351 RVA: 0x0040D881 File Offset: 0x0040BA81
		public bool Positive
		{
			get
			{
				return this.positivityIndex >= 0;
			}
		}

		// Token: 0x0600DC20 RID: 56352 RVA: 0x0040D890 File Offset: 0x0040BA90
		public bool BestPositiveOutcome(LordJob_Ritual ritual)
		{
			using (List<RitualOutcomePossibility>.Enumerator enumerator = ritual.Ritual.outcomeEffect.def.outcomeChances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.positivityIndex > this.positivityIndex)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600DC21 RID: 56353 RVA: 0x0040D900 File Offset: 0x0040BB00
		public float Weight(FloatRange qualityRange)
		{
			if (!this.Positive)
			{
				return this.chance;
			}
			return this.chance * qualityRange.min;
		}

		// Token: 0x040094A6 RID: 38054
		[MustTranslate]
		public string label;

		// Token: 0x040094A7 RID: 38055
		[MustTranslate]
		public string description;

		// Token: 0x040094A8 RID: 38056
		[MustTranslate]
		public string potentialExtraOutcomeDesc;

		// Token: 0x040094A9 RID: 38057
		public float chance;

		// Token: 0x040094AA RID: 38058
		public ThoughtDef memory;

		// Token: 0x040094AB RID: 38059
		public int positivityIndex;

		// Token: 0x040094AC RID: 38060
		[NoTranslate]
		public List<string> roleIdsNotGainingMemory;

		// Token: 0x040094AD RID: 38061
		public float ideoCertaintyOffset;
	}
}
