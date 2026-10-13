using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B5 RID: 6069
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x06008DB8 RID: 36280 RVA: 0x002A11B1 File Offset: 0x0029F3B1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x002A11CB File Offset: 0x0029F3CB
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x04005BD2 RID: 23506
		public float chance = 0.5f;
	}
}
