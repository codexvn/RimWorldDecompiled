using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200128A RID: 4746
	public class JobDriver_Hack : JobDriver
	{
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06007429 RID: 29737 RVA: 0x00229653 File Offset: 0x00227853
		private Thing HackTarget
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x0600742A RID: 29738 RVA: 0x00239BF3 File Offset: 0x00237DF3
		private CompHackable CompHacking
		{
			get
			{
				return this.HackTarget.TryGetComp<CompHackable>();
			}
		}

		// Token: 0x0600742B RID: 29739 RVA: 0x00239C00 File Offset: 0x00237E00
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.HackTarget, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x00239C23 File Offset: 0x00237E23
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.CompHacking.Props.intellectualSkillPrerequisite > 0 && this.pawn.skills.GetSkill(SkillDefOf.Intellectual).Level < this.CompHacking.Props.intellectualSkillPrerequisite);
			PathEndMode pathEndMode = base.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.ClosestTouch;
			yield return Toils_Goto.GotoThing(TargetIndex.A, pathEndMode, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.handlingFacing = true;
			toil.tickAction = delegate()
			{
				float statValue = this.pawn.GetStatValue(StatDefOf.HackingSpeed, true, -1);
				this.CompHacking.Hack(statValue, this.pawn, false);
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f, false, false);
				this.pawn.rotationTracker.FaceTarget(this.HackTarget);
			};
			toil.WithEffect(EffecterDefOf.Hacking, TargetIndex.A, null);
			if (this.CompHacking.Props.effectHacking != null)
			{
				toil.WithEffect(() => this.CompHacking.Props.effectHacking, () => this.HackTarget.OccupiedRect().ClosestCellTo(this.pawn.Position), null);
			}
			toil.WithProgressBar(TargetIndex.A, () => this.CompHacking.ProgressPercent, false, -0.5f, true);
			toil.PlaySoundAtStart(SoundDefOf.Hacking_Started);
			toil.PlaySustainerOrSound(SoundDefOf.Hacking_InProgress, 1f);
			toil.AddFinishAction(delegate
			{
				if (!this.CompHacking.IsHacked)
				{
					SoundDefOf.Hacking_Suspended.PlayOneShot(this.HackTarget);
					return;
				}
				SoundDefOf.Hacking_Completed.PlayOneShot(this.HackTarget);
				SoundDef hackingCompletedSound = this.CompHacking.Props.hackingCompletedSound;
				if (hackingCompletedSound == null)
				{
					return;
				}
				hackingCompletedSound.PlayOneShot(this.HackTarget);
			});
			toil.FailOnCannotTouch(TargetIndex.A, pathEndMode);
			toil.FailOn(() => this.CompHacking.IsHacked || this.CompHacking.LockedOut);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			yield return toil;
			yield break;
		}
	}
}
