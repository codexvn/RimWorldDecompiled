using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200178B RID: 6027
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x06008D5F RID: 36191 RVA: 0x002A0A54 File Offset: 0x0029EC54
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x04005BCB RID: 23499
		public float min;

		// Token: 0x04005BCC RID: 23500
		public float max = 99999f;
	}
}
