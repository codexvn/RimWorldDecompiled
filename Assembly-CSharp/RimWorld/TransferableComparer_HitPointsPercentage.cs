using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034BC RID: 13500
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x06012F9C RID: 77724 RVA: 0x005A178C File Offset: 0x0059F98C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06012F9D RID: 77725 RVA: 0x005A17B0 File Offset: 0x0059F9B0
		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null)
			{
				return pawn.health.summaryHealth.SummaryHealthPercent;
			}
			if (!anyThing.def.useHitPoints || !anyThing.def.healthAffectsPrice)
			{
				return 1f;
			}
			return (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
		}
	}
}
