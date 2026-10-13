using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002258 RID: 8792
	public class CompAbilityEffect_PreachHealth : CompAbilityEffect
	{
		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x0600BFCC RID: 49100 RVA: 0x00379803 File Offset: 0x00377A03
		public new CompProperties_PreachHealth Props
		{
			get
			{
				return (CompProperties_PreachHealth)this.props;
			}
		}

		// Token: 0x17001D4B RID: 7499
		// (get) Token: 0x0600BFCD RID: 49101 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BFCE RID: 49102 RVA: 0x00379810 File Offset: 0x00377A10
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages, this.parent) && AbilityUtility.ValidateSickOrInjured(pawn, throwMessages, this.parent);
		}
	}
}
