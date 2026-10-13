using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C26 RID: 11302
	public class CompBiosculpterPod_PleasureCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x0600FD64 RID: 64868 RVA: 0x00498364 File Offset: 0x00496564
		public override void CycleCompleted(Pawn pawn)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BiosculpterPleasure, null, null);
			Messages.Message("BiosculpterPleasureCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
