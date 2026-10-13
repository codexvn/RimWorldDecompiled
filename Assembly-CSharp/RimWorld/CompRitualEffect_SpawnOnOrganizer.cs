using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002805 RID: 10245
	public class CompRitualEffect_SpawnOnOrganizer : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x0600E00C RID: 57356 RVA: 0x0041C94C File Offset: 0x0041AB4C
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.Organizer;
		}
	}
}
