using System;

namespace Verse.AI
{
	// Token: 0x02000FAB RID: 4011
	public class MentalStateWorker_RitualBlinding : MentalStateWorker
	{
		// Token: 0x060062A0 RID: 25248 RVA: 0x001F15FD File Offset: 0x001EF7FD
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && RitualBlindingMentalStateUtility.FindPawnToBlind(pawn) != null;
		}
	}
}
