using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CBC RID: 3260
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x0600536D RID: 21357 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x001ADD0F File Offset: 0x001ABF0F
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x001ADD31 File Offset: 0x001ABF31
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04003C3B RID: 15419
		private Mote interactMote;
	}
}
