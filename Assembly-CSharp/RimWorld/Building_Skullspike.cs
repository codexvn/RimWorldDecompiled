using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A48 RID: 10824
	public class Building_Skullspike : Building, IObservedThoughtGiver
	{
		// Token: 0x0600F05B RID: 61531 RVA: 0x0045B4E4 File Offset: 0x004596E4
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if ((observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery()) || !observer.IsSlave)
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedSkullspike);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x0600F05C RID: 61532 RVA: 0x00002C42 File Offset: 0x00000E42
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}
	}
}
