using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C3 RID: 10179
	public class RitualSpectatorFilter_WillWitnessExecution : RitualSpectatorFilter
	{
		// Token: 0x0600DEEF RID: 57071 RVA: 0x00418B8A File Offset: 0x00416D8A
		public override bool Allowed(Pawn p)
		{
			return p.IsSlave || p.Ideo == null || p.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, p.Named(HistoryEventArgsNames.Doer)));
		}
	}
}
