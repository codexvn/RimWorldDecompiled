using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002542 RID: 9538
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x1700201E RID: 8222
		// (set) Token: 0x0600D0B8 RID: 53432 RVA: 0x003D6006 File Offset: 0x003D4206
		public virtual Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x0600D0B9 RID: 53433 RVA: 0x003D6014 File Offset: 0x003D4214
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x0600D0BA RID: 53434 RVA: 0x003D6030 File Offset: 0x003D4230
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			Thought_MemoryObservation thought_MemoryObservation = null;
			List<Thought_Memory> memories = thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemoryObservation thought_MemoryObservation2 = memories[i] as Thought_MemoryObservation;
				if (thought_MemoryObservation2 != null && thought_MemoryObservation2.def == this.def && thought_MemoryObservation2.targetThingID == this.targetThingID && (thought_MemoryObservation == null || thought_MemoryObservation2.age > thought_MemoryObservation.age))
				{
					thought_MemoryObservation = thought_MemoryObservation2;
				}
			}
			if (thought_MemoryObservation != null)
			{
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				return true;
			}
			showBubble = true;
			return false;
		}

		// Token: 0x04008DE3 RID: 36323
		protected int targetThingID;
	}
}
