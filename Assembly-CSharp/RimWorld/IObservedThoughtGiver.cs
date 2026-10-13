using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002496 RID: 9366
	public interface IObservedThoughtGiver
	{
		// Token: 0x0600CB18 RID: 51992
		Thought_Memory GiveObservedThought(Pawn observer);

		// Token: 0x0600CB19 RID: 51993
		HistoryEventDef GiveObservedHistoryEvent(Pawn observer);
	}
}
