using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C5 RID: 6341
	public class ThoughtWorker_RotStink : ThoughtWorker
	{
		// Token: 0x060090AE RID: 37038 RVA: 0x002A9E14 File Offset: 0x002A8014
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			byte b = p.Position.GasDensity(p.Map, GasType.RotStink);
			if (b == 0)
			{
				return ThoughtState.Inactive;
			}
			if (b <= 127)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}

		// Token: 0x04005C4B RID: 23627
		private const byte MinorDensity = 127;
	}
}
