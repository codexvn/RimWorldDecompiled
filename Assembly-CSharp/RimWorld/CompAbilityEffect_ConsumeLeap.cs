using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002204 RID: 8708
	public class CompAbilityEffect_ConsumeLeap : CompAbilityEffect, ICompAbilityEffectOnJumpCompleted
	{
		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x0600BEA0 RID: 48800 RVA: 0x00374438 File Offset: 0x00372638
		private new CompProperties_ConsumeLeap Props
		{
			get
			{
				return (CompProperties_ConsumeLeap)this.props;
			}
		}

		// Token: 0x0600BEA1 RID: 48801 RVA: 0x00374448 File Offset: 0x00372648
		public void OnJumpCompleted(IntVec3 origin, LocalTargetInfo target)
		{
			CompDevourer compDevourer;
			if (this.parent.pawn.TryGetComp(out compDevourer))
			{
				compDevourer.StartDigesting(origin, target);
			}
		}

		// Token: 0x0600BEA2 RID: 48802 RVA: 0x00374474 File Offset: 0x00372674
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && pawn.BodySize <= this.Props.maxBodySize;
		}

		// Token: 0x0600BEA3 RID: 48803 RVA: 0x003744A4 File Offset: 0x003726A4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return target.Pawn.BodySize <= this.Props.maxBodySize && base.Valid(target, throwMessages);
		}
	}
}
