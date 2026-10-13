using System;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200073C RID: 1852
	public class Hediff_CubeWithdrawal : Hediff
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000E92BC File Offset: 0x000E74BC
		public Hediff_CubeInterest InterestHediff
		{
			get
			{
				Hediff_CubeInterest result;
				if ((result = this.cachedHediff) == null)
				{
					result = (this.cachedHediff = this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>());
				}
				return result;
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000E92F4 File Offset: 0x000E74F4
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.InterestHediff.InCaravanWithCube)
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			if (this.pawn.CurJobDef == JobDefOf.GoldenCubePlay || !this.pawn.Awake())
			{
				return;
			}
			Hediff_CubeInterest interestHediff = this.InterestHediff;
			float x = (interestHediff != null) ? interestHediff.Severity : 0f;
			this.Severity += Hediff_CubeWithdrawal.SeverityPerHour.Evaluate(x) / 2500f;
			if (this.Severity >= 1f && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				((HediffWithComps)this.pawn.health.AddHediff(HediffDefOf.CubeComa, null, null, null)).GetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(Hediff_CubeWithdrawal.ComaSeverityDaysCurve.Evaluate(x) * 60000f);
				this.pawn.health.RemoveHediff(this.InterestHediff);
				this.pawn.health.RemoveHediff(this);
				TaggedString label = "LetterLabelGoldenCubeComa".Translate(this.pawn.Named("PAWN"));
				TaggedString text = "LetterGoldenCubeComa".Translate(this.pawn.Named("PAWN"));
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000E9460 File Offset: 0x000E7660
		protected override void OnStageIndexChanged(int stageIndex)
		{
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			string text;
			if (stageIndex == 1)
			{
				text = "MessageGoldenCubeWithdrawal".Translate(this.pawn.Named("PAWN"));
			}
			else
			{
				text = "MessageGoldenCubeWithdrawalIncreased".Translate(this.pawn.Named("PAWN"));
				Thing t;
				if (!this.pawn.InMentalState && !this.pawn.Drafted && this.pawn.GetLord() == null && this.pawn.CurJobDef != JobDefOf.GoldenCubePlay && JobGiver_PlayWithGoldenCube.TryGetNearestCube(this.pawn, out t) && !this.pawn.Downed)
				{
					Job job = JobMaker.MakeJob(JobDefOf.GoldenCubePlay, t);
					job.count = 1;
					this.pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				}
			}
			Messages.Message(text, this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x040023E2 RID: 9186
		private Hediff_CubeInterest cachedHediff;

		// Token: 0x040023E3 RID: 9187
		private static readonly SimpleCurve SeverityPerHour = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, -0.25f),
			new CurvePoint(0.1f, 0.0138f),
			new CurvePoint(0.5f, 0.0208f),
			new CurvePoint(1f, 0.0416f)
		});

		// Token: 0x040023E4 RID: 9188
		private static readonly SimpleCurve ComaSeverityDaysCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0.1f, 3f),
			new CurvePoint(0.5f, 10f),
			new CurvePoint(1f, 24f)
		});
	}
}
