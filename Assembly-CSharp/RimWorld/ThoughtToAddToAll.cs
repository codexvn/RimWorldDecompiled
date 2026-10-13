using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002335 RID: 9013
	public struct ThoughtToAddToAll
	{
		// Token: 0x0600C396 RID: 50070 RVA: 0x0038BB0A File Offset: 0x00389D0A
		public ThoughtToAddToAll(ThoughtDef thoughtDef, Pawn otherPawn = null)
		{
			if (thoughtDef == null)
			{
				throw new NullReferenceException("Thought def cant be null!");
			}
			this.thoughtDef = thoughtDef;
			this.otherPawn = otherPawn;
		}

		// Token: 0x0600C397 RID: 50071 RVA: 0x0038BB28 File Offset: 0x00389D28
		public void Add(Pawn to)
		{
			if (to.needs == null || to.needs.mood == null)
			{
				return;
			}
			if (this.otherPawn == null)
			{
				to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, null, null);
				return;
			}
			to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, this.otherPawn, null);
		}

		// Token: 0x04008543 RID: 34115
		public ThoughtDef thoughtDef;

		// Token: 0x04008544 RID: 34116
		public Pawn otherPawn;
	}
}
