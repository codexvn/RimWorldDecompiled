using System;

namespace Verse.AI.Group
{
	// Token: 0x020010B1 RID: 4273
	public class TransitionAction_Letter : TransitionAction
	{
		// Token: 0x060068DB RID: 26843 RVA: 0x00211CAC File Offset: 0x0020FEAC
		public TransitionAction_Letter(string title, string message, LetterDef type)
		{
			this.title = title;
			this.message = message;
			this.type = type;
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x00211CCC File Offset: 0x0020FECC
		public override void DoAction(Transition trans)
		{
			Find.LetterStack.ReceiveLetter(this.title, this.message, this.type, trans.target.lord.ownedPawns, null, null, null, null, 0, true);
		}

		// Token: 0x04004A52 RID: 19026
		public string title;

		// Token: 0x04004A53 RID: 19027
		public string message;

		// Token: 0x04004A54 RID: 19028
		public LetterDef type;
	}
}
