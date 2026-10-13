using System;

namespace Verse.AI.Group
{
	// Token: 0x020010AF RID: 4271
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x060068D3 RID: 26835 RVA: 0x00211B0E File Offset: 0x0020FD0E
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x00211B1D File Offset: 0x0020FD1D
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x00211B2C File Offset: 0x0020FD2C
		public override void DoAction(Transition trans)
		{
			if (this.actionWithArg != null)
			{
				this.actionWithArg(trans);
			}
			if (this.action != null)
			{
				this.action();
			}
		}

		// Token: 0x04004A49 RID: 19017
		public Action action;

		// Token: 0x04004A4A RID: 19018
		public Action<Transition> actionWithArg;
	}
}
