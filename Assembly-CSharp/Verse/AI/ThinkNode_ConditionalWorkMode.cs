using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FEA RID: 4074
	public class ThinkNode_ConditionalWorkMode : ThinkNode_Conditional
	{
		// Token: 0x0600645F RID: 25695 RVA: 0x001FD467 File Offset: 0x001FB667
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalWorkMode thinkNode_ConditionalWorkMode = (ThinkNode_ConditionalWorkMode)base.DeepCopy(resolve);
			thinkNode_ConditionalWorkMode.workMode = this.workMode;
			return thinkNode_ConditionalWorkMode;
		}

		// Token: 0x06006460 RID: 25696 RVA: 0x001FD484 File Offset: 0x001FB684
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.RaceProps.IsMechanoid || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && overseer.mechanitor.GetControlGroup(pawn).WorkMode == this.workMode;
		}

		// Token: 0x040047D3 RID: 18387
		public MechWorkModeDef workMode;
	}
}
