using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187F RID: 6271
	public class ThoughtWorker_WrongApparelGender : ThoughtWorker
	{
		// Token: 0x06008FEA RID: 36842 RVA: 0x002A7854 File Offset: 0x002A5A54
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.Named("PAWN"));
		}

		// Token: 0x06008FEB RID: 36843 RVA: 0x002A7888 File Offset: 0x002A5A88
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.gender.GetLabel(false), p.Named("PAWN"));
		}

		// Token: 0x06008FEC RID: 36844 RVA: 0x002A78D8 File Offset: 0x002A5AD8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!wornApparel[i].def.apparel.CorrectGenderForWearing(p.gender))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return false;
		}
	}
}
