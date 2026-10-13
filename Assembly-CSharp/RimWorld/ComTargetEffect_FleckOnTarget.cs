using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F31 RID: 12081
	public class ComTargetEffect_FleckOnTarget : CompTargetEffect
	{
		// Token: 0x17002C10 RID: 11280
		// (get) Token: 0x060110D1 RID: 69841 RVA: 0x004E68BC File Offset: 0x004E4ABC
		private CompProperties_TargetEffect_FleckOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckOnTarget)this.props;
			}
		}

		// Token: 0x060110D2 RID: 69842 RVA: 0x004E68C9 File Offset: 0x004E4AC9
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.AttachedOverlay(target, this.Props.fleckDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
