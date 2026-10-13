using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B7 RID: 439
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0003A34C File Offset: 0x0003854C
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.linearScale.x = 0.68f;
				this.mote.linearScale.z = 0.12f;
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0003A3A8 File Offset: 0x000385A8
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04000707 RID: 1799
		public MoteProgressBar mote;

		// Token: 0x04000708 RID: 1800
		private const float Width = 0.68f;

		// Token: 0x04000709 RID: 1801
		private const float Height = 0.12f;
	}
}
