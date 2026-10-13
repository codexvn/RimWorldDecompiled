using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200220E RID: 8718
	public class CompAbilityEffect_DeactivateMechanoid : CompAbilityEffect
	{
		// Token: 0x0600BEC8 RID: 48840 RVA: 0x0037567C File Offset: 0x0037387C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			CompMechanoid comp = pawn.GetComp<CompMechanoid>();
			if (comp != null)
			{
				comp.Deactivate();
			}
			Effecter effecter = EffecterDefOf.MechDisabled.Spawn();
			effecter.Trigger(new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), -1);
			effecter.Cleanup();
		}

		// Token: 0x0600BEC9 RID: 48841 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600BECA RID: 48842 RVA: 0x003756F8 File Offset: 0x003738F8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && pawn.RaceProps.IsMechanoid && pawn.HostileTo(Faction.OfPlayer) && base.Valid(target, throwMessages);
		}
	}
}
