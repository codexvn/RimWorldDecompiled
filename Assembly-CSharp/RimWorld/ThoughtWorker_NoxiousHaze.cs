using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CE RID: 6350
	public class ThoughtWorker_NoxiousHaze : ThoughtWorker
	{
		// Token: 0x060090C0 RID: 37056 RVA: 0x002AA18F File Offset: 0x002A838F
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (NoxiousHazeUtility.IsExposedToNoxiousHaze(p) && this.AppliesTo(p))
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x060090C1 RID: 37057 RVA: 0x002AA1B0 File Offset: 0x002A83B0
		private bool AppliesTo(Pawn pawn)
		{
			return ThoughtUtility.NullifyingGene(this.def, pawn) != null || (pawn.GetStatValue(StatDefOf.ToxicResistance, true, -1) < 1f && pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < 1f && !pawn.kindDef.immuneToGameConditionEffects);
		}
	}
}
