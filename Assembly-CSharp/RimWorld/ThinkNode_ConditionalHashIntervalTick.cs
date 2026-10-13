using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D2 RID: 6098
	public class ThinkNode_ConditionalHashIntervalTick : ThinkNode_Conditional
	{
		// Token: 0x06008DF8 RID: 36344 RVA: 0x002A19C9 File Offset: 0x0029FBC9
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsHashIntervalTick(this.interval);
		}

		// Token: 0x06008DF9 RID: 36345 RVA: 0x002A19D7 File Offset: 0x0029FBD7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHashIntervalTick thinkNode_ConditionalHashIntervalTick = (ThinkNode_ConditionalHashIntervalTick)base.DeepCopy(resolve);
			thinkNode_ConditionalHashIntervalTick.interval = this.interval;
			return thinkNode_ConditionalHashIntervalTick;
		}

		// Token: 0x04005BE2 RID: 23522
		public int interval;
	}
}
