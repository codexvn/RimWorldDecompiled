using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002202 RID: 8706
	public class CompAbilityEffect_Coagulate : CompAbilityEffect
	{
		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x0600BE9B RID: 48795 RVA: 0x003742EB File Offset: 0x003724EB
		private new CompProperties_AbilityCoagulate Props
		{
			get
			{
				return (CompProperties_AbilityCoagulate)this.props;
			}
		}

		// Token: 0x0600BE9C RID: 48796 RVA: 0x003742F8 File Offset: 0x003724F8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				if ((hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart) && hediffs[i].TendableNow(false))
				{
					hediffs[i].Tended(this.Props.tendQualityRange.RandomInRange, this.Props.tendQualityRange.TrueMax, 1);
					num++;
				}
			}
			if (num > 0)
			{
				MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(num), 3.65f);
			}
			FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, Vector3.zero, 1.5f, -1f);
		}

		// Token: 0x0600BE9D RID: 48797 RVA: 0x003743E4 File Offset: 0x003725E4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return (pawn == null || AbilityUtility.ValidateHasTendableWound(pawn, throwMessages, this.parent)) && base.Valid(target, throwMessages);
		}
	}
}
