using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012E6 RID: 4838
	public class JobDriver_RecolorApparel : JobDriver
	{
		// Token: 0x0600769E RID: 30366 RVA: 0x00241FE4 File Offset: 0x002401E4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			Thing thing = this.job.GetTarget(TargetIndex.C).Thing;
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			int num = this.job.GetTargetQueue(TargetIndex.B).Count;
			foreach (LocalTargetInfo target in this.job.GetTargetQueue(TargetIndex.A))
			{
				int num2 = Mathf.Min(num, target.Thing.stackCount);
				if (!this.pawn.Reserve(target, this.job, 1, num2, null, errorOnFailed, false))
				{
					return false;
				}
				num -= num2;
				if (num2 <= 0)
				{
					break;
				}
			}
			return true;
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x002420F4 File Offset: 0x002402F4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Apparel recoloring"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.C);
			foreach (Toil toil in JobDriver_DoBill.CollectIngredientsToils(TargetIndex.A, TargetIndex.C, TargetIndex.B, false, false, false))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell, false);
			Toil work = Toils_General.WaitWith(TargetIndex.C, 1000, true, false, false, TargetIndex.C, PathEndMode.Touch);
			work.PlaySustainerOrSound(SoundDefOf.Interact_RecolorApparel, 1f);
			yield return work;
			Toil toil2 = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return toil2;
			yield return Toils_General.Do(delegate
			{
				ThingCountClass thingCountClass = this.job.placedThings[0];
				if (thingCountClass.thing.stackCount == 1)
				{
					thingCountClass.thing.Destroy(DestroyMode.Vanish);
					this.job.placedThings.RemoveAt(0);
				}
				else if (!thingCountClass.thing.Destroyed)
				{
					thingCountClass.thing.SplitOff(1).Destroy(DestroyMode.Vanish);
				}
				this.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompColorable>().Recolor();
				Job job = this.job;
				List<LocalTargetInfo> targetQueue = this.job.GetTargetQueue(TargetIndex.B);
				job.count = ((targetQueue != null) ? targetQueue.Count : 0);
			});
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, work);
			work = null;
			yield break;
			yield break;
		}

		// Token: 0x04005103 RID: 20739
		public const TargetIndex DyeInd = TargetIndex.A;

		// Token: 0x04005104 RID: 20740
		public const TargetIndex ApparelInd = TargetIndex.B;

		// Token: 0x04005105 RID: 20741
		public const TargetIndex StylingStationInd = TargetIndex.C;

		// Token: 0x04005106 RID: 20742
		public const int WorkTimeTicks = 1000;
	}
}
