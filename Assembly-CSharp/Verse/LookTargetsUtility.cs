using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C7F RID: 3199
	public static class LookTargetsUtility
	{
		// Token: 0x06005200 RID: 20992 RVA: 0x001A8A3B File Offset: 0x001A6C3B
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x001A8A48 File Offset: 0x001A6C48
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001A8A59 File Offset: 0x001A6C59
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets == null)
			{
				return;
			}
			lookTargets.Highlight(arrow, colonistBar, circleOverlay);
		}
	}
}
