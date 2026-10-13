using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000F94 RID: 3988
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06006256 RID: 25174 RVA: 0x001F0BA3 File Offset: 0x001EEDA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x001F0BBD File Offset: 0x001EEDBD
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x001F0BDF File Offset: 0x001EEDDF
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x001F0BED File Offset: 0x001EEDED
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hits++;
				if (this.hits >= 7)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x04004671 RID: 18033
		private int hits;

		// Token: 0x04004672 RID: 18034
		public const int MaxHits = 7;
	}
}
