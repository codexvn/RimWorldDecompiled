using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001172 RID: 4466
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06006D41 RID: 27969 RVA: 0x00223D0C File Offset: 0x00221F0C
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06006D42 RID: 27970 RVA: 0x00223D34 File Offset: 0x00221F34
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x00223D58 File Offset: 0x00221F58
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x00223DAB File Offset: 0x00221FAB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(1000, TargetIndex.None);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(this.Building.def.repairEffect, TargetIndex.A, null);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.activeSkill = (() => SkillDefOf.Construction);
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				this.Components.Destroy(DestroyMode.Vanish);
				if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true, -1))
				{
					MoteMaker.ThrowText((this.pawn.DrawPos + this.Building.DrawPos) / 2f, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
					return;
				}
				this.Building.GetComp<CompBreakdownable>().Notify_Repaired();
			};
			yield return toil2;
			yield break;
		}

		// Token: 0x04004C8B RID: 19595
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x04004C8C RID: 19596
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x04004C8D RID: 19597
		private const int TicksDuration = 1000;
	}
}
