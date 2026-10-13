using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B5 RID: 5813
	public class PsychicRitualRoleDef_DeathRefusalTarget : PsychicRitualRoleDef
	{
		// Token: 0x0600890B RID: 35083 RVA: 0x0028DE64 File Offset: 0x0028C064
		protected override bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
		{
			if (!base.PawnCanDo(context, pawn, target, out reason))
			{
				return false;
			}
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null && firstHediff.UsesLeft >= firstHediff.MaxUses)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason>(PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason.MaxDeathRefusal);
				return false;
			}
			return true;
		}

		// Token: 0x0600890C RID: 35084 RVA: 0x0028DEB4 File Offset: 0x0028C0B4
		public override TaggedString PawnCannotDoReason(AnyEnum reason, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target)
		{
			PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason? deathRefusalTargetReason = reason.As<PsychicRitualRoleDef_DeathRefusalTarget.DeathRefusalTargetReason>();
			if (deathRefusalTargetReason != null)
			{
				deathRefusalTargetReason.GetValueOrDefault();
				return this.psychicRitualLeaveReason_MaxDeathRefusal.Formatted(pawn.Named("PAWN"));
			}
			return base.PawnCannotDoReason(reason, context, pawn, target);
		}

		// Token: 0x04005991 RID: 22929
		[MustTranslate]
		public string psychicRitualLeaveReason_MaxDeathRefusal;

		// Token: 0x020016B6 RID: 5814
		public enum DeathRefusalTargetReason
		{
			// Token: 0x04005993 RID: 22931
			None,
			// Token: 0x04005994 RID: 22932
			MaxDeathRefusal
		}
	}
}
