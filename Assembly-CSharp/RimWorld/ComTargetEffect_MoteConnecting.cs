using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F39 RID: 12089
	public class ComTargetEffect_MoteConnecting : CompTargetEffect
	{
		// Token: 0x17002C13 RID: 11283
		// (get) Token: 0x060110E1 RID: 69857 RVA: 0x004E6AD8 File Offset: 0x004E4CD8
		private CompProperties_TargetEffect_MoteConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteConnecting)this.props;
			}
		}

		// Token: 0x060110E2 RID: 69858 RVA: 0x004E6AE5 File Offset: 0x004E4CE5
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeConnectingLine(user.DrawPos, target.DrawPos, this.Props.moteDef, user.Map, 1f);
			}
		}
	}
}
