using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200121F RID: 4639
	public class JobDriver_AnalyzeItem : JobDriver_StudyItem
	{
		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06007150 RID: 29008 RVA: 0x00230C47 File Offset: 0x0022EE47
		public CompAnalyzable AnalyzableComp
		{
			get
			{
				return base.ThingToStudy.TryGetComp<CompAnalyzable>();
			}
		}

		// Token: 0x06007151 RID: 29009 RVA: 0x00230C54 File Offset: 0x0022EE54
		protected override IEnumerable<Toil> GetStudyToils()
		{
			int num = Mathf.CeilToInt((float)Mathf.CeilToInt(this.AnalyzableComp.Props.analysisDurationHours * 2500f) / this.pawn.GetStatValue(StatDefOf.ResearchSpeed, true, -1));
			Toil toil = Toils_General.Wait(num, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A, num, false, -0.5f);
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			toil.handlingFacing = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.job.GetTarget(TargetIndex.A));
			};
			yield return toil;
			yield return Toils_General.DoAtomic(delegate
			{
				this.AnalyzableComp.OnAnalyzed(this.pawn);
			});
			yield break;
		}
	}
}
