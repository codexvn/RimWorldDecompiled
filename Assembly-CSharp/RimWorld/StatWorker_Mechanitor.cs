using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003743 RID: 14147
	public class StatWorker_Mechanitor : StatWorker
	{
		// Token: 0x060139D4 RID: 80340 RVA: 0x005D3934 File Offset: 0x005D1B34
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			if (req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return MechanitorUtility.IsMechanitor(pawn);
				}
			}
			return false;
		}
	}
}
