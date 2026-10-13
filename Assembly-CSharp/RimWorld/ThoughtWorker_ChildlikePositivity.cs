using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D3 RID: 6355
	public class ThoughtWorker_ChildlikePositivity : ThoughtWorker
	{
		// Token: 0x060090CC RID: 37068 RVA: 0x002AA46C File Offset: 0x002A866C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsFreeColonist || p.Suspended || p.DevelopmentalStage.Adult())
			{
				return ThoughtState.Inactive;
			}
			float ageBiologicalYearsFloat = p.ageTracker.AgeBiologicalYearsFloat;
			if (ageBiologicalYearsFloat < 3f)
			{
				return ThoughtState.Inactive;
			}
			for (int i = 0; i < ThoughtWorker_ChildlikePositivity.AgeThresholds.Length; i++)
			{
				if (ageBiologicalYearsFloat <= (float)ThoughtWorker_ChildlikePositivity.AgeThresholds[i])
				{
					return ThoughtState.ActiveAtStage(i);
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04005C60 RID: 23648
		private const int MinAge = 3;

		// Token: 0x04005C61 RID: 23649
		private static readonly int[] AgeThresholds = new int[]
		{
			6,
			8,
			10,
			13
		};
	}
}
