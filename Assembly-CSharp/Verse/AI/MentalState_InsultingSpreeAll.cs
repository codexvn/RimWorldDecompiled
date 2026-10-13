using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F6E RID: 3950
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x060061A2 RID: 24994 RVA: 0x001EE7D6 File Offset: 0x001EC9D6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x001EE7F0 File Offset: 0x001EC9F0
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x001EE800 File Offset: 0x001ECA00
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(250, delta) && (this.target == null || this.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x060061A5 RID: 24997 RVA: 0x001EE860 File Offset: 0x001ECA60
		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any<Pawn>())
			{
				this.target = null;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
				return;
			}
			Pawn pawn;
			if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != this.target))
			{
				pawn = (from x in MentalState_InsultingSpreeAll.candidates
				where x != this.target
				select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			else
			{
				pawn = MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			if (pawn != this.target)
			{
				this.target = pawn;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060061A6 RID: 24998 RVA: 0x001EE940 File Offset: 0x001ECB40
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		// Token: 0x04004635 RID: 17973
		private int targetFoundTicks;

		// Token: 0x04004636 RID: 17974
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04004637 RID: 17975
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04004638 RID: 17976
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
