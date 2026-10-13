using System;

namespace Verse.Sound
{
	// Token: 0x02000E39 RID: 3641
	public class SoundRoot
	{
		// Token: 0x06005BE6 RID: 23526 RVA: 0x001D60FF File Offset: 0x001D42FF
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x001D6128 File Offset: 0x001D4328
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04004296 RID: 17046
		public AudioSourcePool sourcePool;

		// Token: 0x04004297 RID: 17047
		public SampleOneShotManager oneShotManager;

		// Token: 0x04004298 RID: 17048
		public SustainerManager sustainerManager;
	}
}
