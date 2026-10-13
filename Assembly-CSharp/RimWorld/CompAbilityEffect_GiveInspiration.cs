using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200223D RID: 8765
	public class CompAbilityEffect_GiveInspiration : CompAbilityEffect
	{
		// Token: 0x0600BF70 RID: 49008 RVA: 0x00378168 File Offset: 0x00376368
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					base.Apply(target, dest);
					pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterPsychicInspiration".Translate(pawn.Named("PAWN"), this.parent.pawn.Named("CASTER")), true);
				}
			}
		}

		// Token: 0x0600BF71 RID: 49009 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600BF72 RID: 49010 RVA: 0x003781E0 File Offset: 0x003763E0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateNoInspiration(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!AbilityUtility.ValidateCanGetInspiration(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return base.Valid(target, throwMessages);
		}
	}
}
