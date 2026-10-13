using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C2 RID: 10178
	public class RitualSpectatorFilter_NatureFocus : RitualSpectatorFilter
	{
		// Token: 0x0600DEED RID: 57069 RVA: 0x00418B75 File Offset: 0x00416D75
		public override bool Allowed(Pawn p)
		{
			return MeditationFocusDefOf.Natural.CanPawnUse(p);
		}
	}
}
