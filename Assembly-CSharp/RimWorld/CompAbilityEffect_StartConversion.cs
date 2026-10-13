using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002286 RID: 8838
	public class CompAbilityEffect_StartConversion : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x17001D6B RID: 7531
		// (get) Token: 0x0600C079 RID: 49273 RVA: 0x0037BFBB File Offset: 0x0037A1BB
		public new CompProperties_AbilityStartConversion Props
		{
			get
			{
				return (CompProperties_AbilityStartConversion)this.props;
			}
		}

		// Token: 0x0600C07A RID: 49274 RVA: 0x0037BFC8 File Offset: 0x0037A1C8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return RitualBehaviorWorker_Conversion.ValidateConvertee(target.Pawn, this.parent.pawn, throwMessages) && base.Valid(target, throwMessages);
		}
	}
}
