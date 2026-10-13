using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003784 RID: 14212
	public class Verb_MeleeApplyHediff : Verb_MeleeAttack
	{
		// Token: 0x06013AFF RID: 80639 RVA: 0x005D9224 File Offset: 0x005D7424
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (this.tool == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without a tool", 38381735);
				return damageResult;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				Log.ErrorOnce("Attempted to apply melee hediff without pawn target", 78330053);
				return damageResult;
			}
			foreach (BodyPartRecord part in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, this.verbProps.bodypartTagTarget, null))
			{
				damageResult.AddHediff(pawn.health.AddHediff(this.tool.hediff, part, null, null));
				damageResult.AddPart(pawn, part);
				damageResult.wounded = true;
			}
			return damageResult;
		}

		// Token: 0x06013B00 RID: 80640 RVA: 0x00014CA0 File Offset: 0x00012EA0
		public override bool IsUsableOn(Thing target)
		{
			return target is Pawn;
		}
	}
}
