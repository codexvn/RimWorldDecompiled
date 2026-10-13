using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C2 RID: 6338
	public class ThoughtWorker_InSunlight : ThoughtWorker
	{
		// Token: 0x060090A5 RID: 37029 RVA: 0x002A9BC9 File Offset: 0x002A7DC9
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Spawned && p.Position.InSunlight(p.Map))
			{
				return true;
			}
			return ThoughtState.Inactive;
		}
	}
}
