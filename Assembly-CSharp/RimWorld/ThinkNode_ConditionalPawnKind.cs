using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017BD RID: 6077
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x06008DCA RID: 36298 RVA: 0x002A1360 File Offset: 0x0029F560
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x06008DCB RID: 36299 RVA: 0x002A137A File Offset: 0x0029F57A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x04005BD5 RID: 23509
		public PawnKindDef pawnKind;
	}
}
