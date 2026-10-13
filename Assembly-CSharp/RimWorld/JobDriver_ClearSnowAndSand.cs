using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001242 RID: 4674
	public class JobDriver_ClearSnowAndSand : JobDriver
	{
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06007259 RID: 29273 RVA: 0x00234584 File Offset: 0x00232784
		private float TotalNeededWork
		{
			get
			{
				float num = 50f;
				float depth = base.Map.snowGrid.GetDepth(base.TargetLocA);
				SandGrid sandGrid = base.Map.sandGrid;
				return num * (depth + ((sandGrid != null) ? new float?(sandGrid.GetDepth(base.TargetLocA)) : null)).GetValueOrDefault();
			}
		}

		// Token: 0x0600725A RID: 29274 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600725B RID: 29275 RVA: 0x00234603 File Offset: 0x00232803
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = ToilMaker.MakeToil("MakeNewToils");
			clearToil.tickIntervalAction = delegate(int delta)
			{
				float statValue = clearToil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed, true, -1);
				this.workDone += statValue;
				if (this.workDone >= this.TotalNeededWork)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					SandGrid sandGrid = this.Map.sandGrid;
					if (sandGrid != null)
					{
						sandGrid.SetDepth(this.TargetLocA, 0f);
					}
					this.ReadyForNextToil();
					return;
				}
			};
			EffecterDef effectDef = this.IsSand(base.TargetLocA) ? EffecterDefOf.ClearSand : EffecterDefOf.ClearSnow;
			clearToil.defaultCompleteMode = ToilCompleteMode.Never;
			clearToil.WithEffect(effectDef, TargetIndex.A, null);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth, 1f);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f, false);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		// Token: 0x0600725C RID: 29276 RVA: 0x00234614 File Offset: 0x00232814
		private bool IsSand(IntVec3 cell)
		{
			Map map = base.Map;
			float? num;
			if (map == null)
			{
				num = null;
			}
			else
			{
				SandGrid sandGrid = map.sandGrid;
				num = ((sandGrid != null) ? new float?(sandGrid.GetDepth(cell)) : null);
			}
			float? num2 = num;
			if (num2 != null)
			{
				float valueOrDefault = num2.GetValueOrDefault();
				float num3 = valueOrDefault;
				Map map2 = base.Map;
				num2 = ((map2 != null) ? new float?(map2.snowGrid.GetDepth(cell)) : null);
				return num3 > num2.GetValueOrDefault() & num2 != null;
			}
			return false;
		}

		// Token: 0x0600725D RID: 29277 RVA: 0x002346A4 File Offset: 0x002328A4
		public override string GetReport()
		{
			string reportStringOverride = base.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			LocalTargetInfo localTargetInfo = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			if (localTargetInfo.Cell.IsValid && this.IsSand(localTargetInfo.Cell))
			{
				return "JobClearingSand".Translate();
			}
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x04004F13 RID: 20243
		private float workDone;

		// Token: 0x04004F14 RID: 20244
		private const float ClearWorkPerDepositedDepth = 50f;
	}
}
