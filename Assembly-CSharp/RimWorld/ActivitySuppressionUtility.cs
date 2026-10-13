using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001594 RID: 5524
	public static class ActivitySuppressionUtility
	{
		// Token: 0x060082CA RID: 33482 RVA: 0x00274958 File Offset: 0x00272B58
		public static bool CanBeSuppressed(Thing thing, bool considerMinActivity = true, bool playerForced = false)
		{
			CompActivity compActivity;
			if (!thing.TryGetComp(out compActivity))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (pawn.Downed)
				{
					return false;
				}
				if (compActivity.Props.requiresHoldingPlatform && !pawn.IsOnHoldingPlatform)
				{
					return false;
				}
			}
			return !compActivity.IsActive && compActivity.CanBeSuppressed && (playerForced || compActivity.suppressionEnabled) && (!considerMinActivity || compActivity.ActivityLevel >= 0.05f);
		}

		// Token: 0x040056A5 RID: 22181
		private const float MinActivityToSuppress = 0.05f;
	}
}
