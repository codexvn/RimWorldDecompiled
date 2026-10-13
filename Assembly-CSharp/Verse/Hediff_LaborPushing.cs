using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200074D RID: 1869
	public class Hediff_LaborPushing : HediffWithParents
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x000EB3BC File Offset: 0x000E95BC
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)hediffComp_Disappears.ticksToDisappear / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborDuration);
			}
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000EB3F2 File Offset: 0x000E95F2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return PregnancyUtility.BirthQualityGizmo(this.pawn);
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force stillborn",
					action = delegate()
					{
						this.ForceBirth(-1, false);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force infant illness",
					action = delegate()
					{
						this.ForceBirth(0, false);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force healthy",
					action = delegate()
					{
						this.ForceBirth(1, false);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force end",
					action = delegate()
					{
						this.pawn.health.RemoveHediff(this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000EB404 File Offset: 0x000E9604
		public void ForceBirth(int positivityIndex, bool dontSendLetter = false)
		{
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			this.debugForceBirthOutcome = precept_Ritual.outcomeEffect.def.outcomeChances.FirstOrDefault((RitualOutcomePossibility o) => o.positivityIndex == positivityIndex);
			this.preventLetter = dontSendLetter;
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000EB478 File Offset: 0x000E9678
		public override void PreRemoved()
		{
			base.PreRemoved();
			Lord lord = this.pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			if (((lordJob_Ritual != null) ? lordJob_Ritual.Ritual : null) == null || lordJob_Ritual.Ritual.def != PreceptDefOf.ChildBirth || lordJob_Ritual.assignments.FirstAssignedPawn("mother") != this.pawn)
			{
				float birthQualityFor = PregnancyUtility.GetBirthQualityFor(this.pawn);
				RitualOutcomePossibility outcome = this.debugForceBirthOutcome ?? ((RitualOutcomeEffectWorker_ChildBirth)precept_Ritual.outcomeEffect).GetOutcome(birthQualityFor, null);
				RitualRoleAssignments assignments = PregnancyUtility.RitualAssignmentsForBirth(precept_Ritual, this.pawn);
				float quality = birthQualityFor;
				Precept_Ritual ritual = precept_Ritual;
				GeneSet geneSet = this.geneSet;
				PregnancyUtility.ApplyBirthOutcome(outcome, quality, ritual, (geneSet != null) ? geneSet.GenesListForReading : null, base.Mother ?? this.pawn, this.pawn, base.Father, null, null, assignments, this.preventLetter);
				return;
			}
			if (lordJob_Ritual != null)
			{
				lordJob_Ritual.ApplyOutcome(1f, true, true, false);
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000EB584 File Offset: 0x000E9784
		public override void TickInterval(int delta)
		{
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.pawn, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = this.Progress;
					return;
				}
			}
			else
			{
				Effecter effecter = this.progressBar;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.progressBar = null;
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000EB615 File Offset: 0x000E9815
		public override void PostRemoved()
		{
			base.PostRemoved();
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000EB635 File Offset: 0x000E9835
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000EB657 File Offset: 0x000E9857
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborDuration, "laborDuration", 0, false);
		}

		// Token: 0x04002414 RID: 9236
		public int laborDuration;

		// Token: 0x04002415 RID: 9237
		private Effecter progressBar;

		// Token: 0x04002416 RID: 9238
		private RitualOutcomePossibility debugForceBirthOutcome;

		// Token: 0x04002417 RID: 9239
		private bool preventLetter;
	}
}
