using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F97 RID: 3991
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x06006262 RID: 25186
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06006263 RID: 25187 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x001F0C8A File Offset: 0x001EEE8A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x001F0CA4 File Offset: 0x001EEEA4
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x001F0CB4 File Offset: 0x001EEEB4
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500, delta) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x001F0D48 File Offset: 0x001EEF48
		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any<Thing>())
			{
				this.target = null;
				this.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_TantrumRandom.candidates.Any((Thing x) => x != this.target))
				{
					thing = (from x in MentalState_TantrumRandom.candidates
					where x != this.target
					select x).RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				else
				{
					thing = MentalState_TantrumRandom.candidates.RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				if (thing != this.target)
				{
					this.target = thing;
					this.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x001F0E3C File Offset: 0x001EF03C
		private float GetCandidateWeight(Thing candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return (1f - num) * (1f - num) + 0.01f;
		}

		// Token: 0x04004675 RID: 18037
		private int targetFoundTicks;

		// Token: 0x04004676 RID: 18038
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04004677 RID: 18039
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04004678 RID: 18040
		private static List<Thing> candidates = new List<Thing>();
	}
}
