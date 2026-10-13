using System;

namespace RimWorld
{
	// Token: 0x02002907 RID: 10503
	public class MusicTransition
	{
		// Token: 0x170022EC RID: 8940
		// (get) Token: 0x0600E5D6 RID: 58838 RVA: 0x00431130 File Offset: 0x0042F330
		// (set) Token: 0x0600E5D7 RID: 58839 RVA: 0x00431138 File Offset: 0x0042F338
		public MusicTransitionDef def { get; private set; }

		// Token: 0x170022ED RID: 8941
		// (get) Token: 0x0600E5D8 RID: 58840 RVA: 0x00431141 File Offset: 0x0042F341
		// (set) Token: 0x0600E5D9 RID: 58841 RVA: 0x00431149 File Offset: 0x0042F349
		public MusicManagerPlay musicManager { get; private set; }

		// Token: 0x0600E5DA RID: 58842 RVA: 0x00431152 File Offset: 0x0042F352
		public void InitializeWorker(MusicTransitionDef def, MusicManagerPlay musicManager)
		{
			this.def = def;
			this.musicManager = musicManager;
		}

		// Token: 0x0600E5DB RID: 58843 RVA: 0x00431162 File Offset: 0x0042F362
		public virtual bool IsTransitionSatisfied()
		{
			return (this.def.dangerRequirement != MusicDangerRequirement.RequiresDanger || this.musicManager.DangerMusicMode) && (this.def.dangerRequirement != MusicDangerRequirement.RequiresNoDanger || !this.musicManager.DangerMusicMode);
		}
	}
}
