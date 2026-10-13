using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001164 RID: 4452
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06006CE9 RID: 27881 RVA: 0x00222E94 File Offset: 0x00221094
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06006CEA RID: 27882
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x06006CEB RID: 27883
		protected abstract void DoEffect();

		// Token: 0x06006CEC RID: 27884
		protected abstract bool DoWorkFailOn();

		// Token: 0x06006CED RID: 27885 RVA: 0x00222EB5 File Offset: 0x002210B5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x06006CEE RID: 27886 RVA: 0x00222ED3 File Offset: 0x002210D3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed, false);
		}

		// Token: 0x06006CEF RID: 27887 RVA: 0x00222EFA File Offset: 0x002210FA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils");
			doWork.initAction = delegate()
			{
				this.workLeft = 65f;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				float num = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true, -1) * 1.7f * (float)delta;
				this.workLeft -= num;
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
			doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A, null);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / 65f, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x04004C67 RID: 19559
		private float workLeft;

		// Token: 0x04004C68 RID: 19560
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x04004C69 RID: 19561
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x04004C6A RID: 19562
		private const float BaseWorkAmount = 65f;
	}
}
