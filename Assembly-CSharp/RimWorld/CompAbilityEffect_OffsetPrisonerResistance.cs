using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002253 RID: 8787
	public class CompAbilityEffect_OffsetPrisonerResistance : CompAbilityEffect
	{
		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x0600BFBE RID: 49086 RVA: 0x00379658 File Offset: 0x00377858
		public new CompProperties_AbilityOffsetPrisonerResistance Props
		{
			get
			{
				return (CompProperties_AbilityOffsetPrisonerResistance)this.props;
			}
		}

		// Token: 0x0600BFBF RID: 49087 RVA: 0x00379668 File Offset: 0x00377868
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.guest.resistance = Mathf.Max(pawn.guest.resistance + this.Props.offset, 0f);
			}
		}

		// Token: 0x0600BFC0 RID: 49088 RVA: 0x003796B4 File Offset: 0x003778B4
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!pawn.IsPrisonerOfColony)
				{
					return false;
				}
				if (pawn != null && pawn.guest.resistance < 1E-45f)
				{
					return false;
				}
				if (pawn.Downed)
				{
					return false;
				}
			}
			return this.Valid(target, false);
		}

		// Token: 0x0600BFC1 RID: 49089 RVA: 0x00379700 File Offset: 0x00377900
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateHasResistance(pawn, throwMessages, this.parent);
		}
	}
}
