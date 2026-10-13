using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200373F RID: 14143
	public class StatWorker_VacuumResistance : StatWorker
	{
		// Token: 0x060139C9 RID: 80329 RVA: 0x005D3334 File Offset: 0x005D1534
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.RaceProps.IsFlesh;
		}

		// Token: 0x060139CA RID: 80330 RVA: 0x005D336C File Offset: 0x005D156C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn.IsMutant && !pawn.mutant.Def.breathesAir)
			{
				return 1f;
			}
			return base.GetValueUnfinalized(req, applyPostProcess);
		}
	}
}
