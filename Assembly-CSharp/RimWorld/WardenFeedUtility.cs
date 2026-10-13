using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158F RID: 5519
	public static class WardenFeedUtility
	{
		// Token: 0x060082BC RID: 33468 RVA: 0x00274520 File Offset: 0x00272720
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && (p.Downed || HealthAIUtility.ShouldSeekMedicalRest(p));
		}
	}
}
