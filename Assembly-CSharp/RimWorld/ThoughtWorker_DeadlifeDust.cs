using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C7 RID: 6343
	public class ThoughtWorker_DeadlifeDust : ThoughtWorker
	{
		// Token: 0x060090B2 RID: 37042 RVA: 0x002A9ED0 File Offset: 0x002A80D0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			byte b = p.Position.GasDensity(p.Map, GasType.DeadlifeDust);
			if (b == 0)
			{
				return ThoughtState.Inactive;
			}
			if (b <= 51)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (b <= 127)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(2);
		}

		// Token: 0x04005C4D RID: 23629
		private const byte MinorDensity = 51;

		// Token: 0x04005C4E RID: 23630
		private const byte MediumDensity = 127;
	}
}
