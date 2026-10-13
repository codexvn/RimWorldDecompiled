using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003758 RID: 14168
	public class StatWorker_Terror : StatWorker
	{
		// Token: 0x06013A34 RID: 80436 RVA: 0x005D54F8 File Offset: 0x005D36F8
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x06013A35 RID: 80437 RVA: 0x005D5528 File Offset: 0x005D3728
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return ((Pawn)req.Thing).GetTerrorLevel();
		}
	}
}
