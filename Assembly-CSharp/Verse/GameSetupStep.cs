using System;

namespace Verse
{
	// Token: 0x0200027F RID: 639
	public abstract class GameSetupStep
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600101D RID: 4125
		public abstract int SeedPart { get; }

		// Token: 0x0600101E RID: 4126
		public abstract void GenerateFresh();

		// Token: 0x0600101F RID: 4127 RVA: 0x0004BF1B File Offset: 0x0004A11B
		public virtual void GenerateWithoutWorldData()
		{
			this.GenerateFresh();
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GenerateFromScribe()
		{
		}

		// Token: 0x04000CB5 RID: 3253
		public GameSetupStepDef def;
	}
}
