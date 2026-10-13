using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C3D RID: 11325
	public class CompProperties_CauseHediff_AoE : CompProperties
	{
		// Token: 0x0600FDF0 RID: 65008 RVA: 0x0049A6A3 File Offset: 0x004988A3
		public CompProperties_CauseHediff_AoE()
		{
			this.compClass = typeof(CompCauseHediff_AoE);
		}

		// Token: 0x0400A673 RID: 42611
		public HediffDef hediff;

		// Token: 0x0400A674 RID: 42612
		public BodyPartDef part;

		// Token: 0x0400A675 RID: 42613
		public float range;

		// Token: 0x0400A676 RID: 42614
		public bool onlyTargetMechs;

		// Token: 0x0400A677 RID: 42615
		public bool ignoreMechs;

		// Token: 0x0400A678 RID: 42616
		public int checkInterval = 10;

		// Token: 0x0400A679 RID: 42617
		public SoundDef activeSound;

		// Token: 0x0400A67A RID: 42618
		public bool canTargetSelf = true;

		// Token: 0x0400A67B RID: 42619
		public bool drawLines = true;
	}
}
