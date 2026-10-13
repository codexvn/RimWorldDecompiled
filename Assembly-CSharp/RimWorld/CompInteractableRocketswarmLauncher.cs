using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002D00 RID: 11520
	public class CompInteractableRocketswarmLauncher : CompInteractable
	{
		// Token: 0x1700290F RID: 10511
		// (get) Token: 0x06010303 RID: 66307 RVA: 0x004AE907 File Offset: 0x004ACB07
		private Building_TurretGun ParentGun
		{
			get
			{
				return (Building_TurretGun)this.parent;
			}
		}

		// Token: 0x06010304 RID: 66308 RVA: 0x004AEA1B File Offset: 0x004ACC1B
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.TargetLocation(target.Pawn);
			}
		}

		// Token: 0x06010305 RID: 66309 RVA: 0x004AEA34 File Offset: 0x004ACC34
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (this.CanInteract(selPawn, true))
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(base.Props.jobString.CapitalizeFirst(), delegate()
				{
					this.TargetLocation(selPawn);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy", null);
			}
			yield break;
		}

		// Token: 0x06010306 RID: 66310 RVA: 0x004AEA4C File Offset: 0x004ACC4C
		private void TargetLocation(Pawn caster)
		{
			Find.Targeter.BeginTargeting(this.ParentGun.AttackVerb, null, true, null, delegate()
			{
				if (this.ParentGun.ForcedTarget.IsValid)
				{
					Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
					caster.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			}, true);
		}

		// Token: 0x06010307 RID: 66311 RVA: 0x004AEA94 File Offset: 0x004ACC94
		protected override bool TryInteractTick()
		{
			this.ParentGun.TryActivateBurst();
			if (this.ParentGun.CurrentTarget.IsValid)
			{
				this.ParentGun.Top.ForceFaceTarget(this.ParentGun.CurrentTarget);
				return true;
			}
			return true;
		}

		// Token: 0x06010308 RID: 66312 RVA: 0x004AEAE0 File Offset: 0x004ACCE0
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null && activateBy.WorkTagIsDisabled(WorkTags.Violent))
			{
				return "IsIncapableOfViolence".Translate(activateBy.LabelShort, activateBy);
			}
			return true;
		}
	}
}
