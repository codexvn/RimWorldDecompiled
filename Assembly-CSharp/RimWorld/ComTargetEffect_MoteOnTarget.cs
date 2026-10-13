using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F3B RID: 12091
	public class ComTargetEffect_MoteOnTarget : CompTargetEffect
	{
		// Token: 0x17002C14 RID: 11284
		// (get) Token: 0x060110E5 RID: 69861 RVA: 0x004E6B34 File Offset: 0x004E4D34
		private CompProperties_TargetEffect_MoteOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteOnTarget)this.props;
			}
		}

		// Token: 0x060110E6 RID: 69862 RVA: 0x004E6B41 File Offset: 0x004E4D41
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeAttachedOverlay(target, this.Props.moteDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
