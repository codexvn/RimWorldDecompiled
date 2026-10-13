using System;

namespace Verse
{
	// Token: 0x0200027E RID: 638
	public class GameSetupStepDef : Def
	{
		// Token: 0x0600101B RID: 4123 RVA: 0x0004BF07 File Offset: 0x0004A107
		public override void PostLoad()
		{
			base.PostLoad();
			this.setupStep.def = this;
		}

		// Token: 0x04000CB2 RID: 3250
		public float order;

		// Token: 0x04000CB3 RID: 3251
		public bool executeAfterWorldSetup;

		// Token: 0x04000CB4 RID: 3252
		public GameSetupStep setupStep;
	}
}
