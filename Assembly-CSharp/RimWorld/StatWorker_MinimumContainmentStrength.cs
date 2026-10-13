using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003751 RID: 14161
	public class StatWorker_MinimumContainmentStrength : StatWorker
	{
		// Token: 0x06013A1D RID: 80413 RVA: 0x005D4B8F File Offset: 0x005D2D8F
		public override bool ShouldShowFor(StatRequest req)
		{
			return ContainmentUtility.ShowContainmentStats(req.Thing);
		}
	}
}
