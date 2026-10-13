using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200139C RID: 5020
	public class JobDriver_Meditate : JobDriver
	{
		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06007B14 RID: 31508 RVA: 0x00250C17 File Offset: 0x0024EE17
		public LocalTargetInfo Focus
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C);
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06007B15 RID: 31509 RVA: 0x00250C28 File Offset: 0x0024EE28
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).IsValid;
			}
		}

		// Token: 0x06007B16 RID: 31510 RVA: 0x00250C4C File Offset: 0x0024EE4C
		protected string PsyfocusPerDayReport()
		{
			if (!this.pawn.HasPsylink)
			{
				return "";
			}
			Thing thing = this.Focus.Thing;
			float f = MeditationUtility.PsyfocusGainPerTick(this.pawn, thing) * 60000f;
			return "\n" + "PsyfocusPerDayOfMeditation".Translate(f.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x06007B17 RID: 31511 RVA: 0x00250CBC File Offset: 0x0024EEBC
		public override string GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.GetReport();
			}
			Thing thing = this.Focus.Thing;
			if (thing != null && !thing.Destroyed)
			{
				return "MeditatingAt".Translate() + ": " + thing.LabelShort.CapitalizeFirst() + "." + this.PsyfocusPerDayReport();
			}
			return base.GetReport() + this.PsyfocusPerDayReport();
		}

		// Token: 0x06007B18 RID: 31512 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007B19 RID: 31513 RVA: 0x00250D41 File Offset: 0x0024EF41
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil meditate = ToilMaker.MakeToil("MakeNewToils");
			meditate.socialMode = RandomSocialMode.Off;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.B);
				meditate = Toils_LayDown.LayDown(TargetIndex.B, this.job.GetTarget(TargetIndex.B).Thing is Building_Bed, false, false, true, PawnPosture.LayingOnGroundNormal, false);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				meditate.initAction = delegate()
				{
					LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
					if (target.IsValid)
					{
						this.faceDir = target.Cell - this.pawn.Position;
						return;
					}
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random).FacingCell;
				};
				if (this.Focus != null)
				{
					meditate.FailOnDespawnedOrNull(TargetIndex.C);
					meditate.FailOnForbidden(TargetIndex.A);
					if (this.pawn.HasPsylink && this.Focus.Thing != null)
					{
						meditate.FailOn(() => this.Focus.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, this.pawn, true) < float.Epsilon);
					}
				}
				meditate.handlingFacing = true;
			}
			meditate.defaultCompleteMode = ToilCompleteMode.Delay;
			meditate.defaultDuration = this.job.def.joyDuration;
			meditate.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			meditate.AddPreTickAction(delegate
			{
				bool flag = this.pawn.GetTimeAssignment() == TimeAssignmentDefOf.Meditate;
				if (this.job.ignoreJoyTimeAssignment)
				{
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					bool flag2 = !flag && this.job.wasOnMeditationTimeAssignment;
					if (this.pawn.IsHashIntervalTick(4000) && psychicEntropy != null && !flag && psychicEntropy.CurrentPsyfocus >= Mathf.Max(psychicEntropy.TargetPsyfocus + 0.05f, 0.99f))
					{
						this.pawn.jobs.CheckForJobOverride(0f, true);
						return;
					}
					if (flag2 && psychicEntropy.TargetPsyfocus < psychicEntropy.CurrentPsyfocus)
					{
						base.EndJobWith(JobCondition.InterruptForced);
						return;
					}
					this.job.psyfocusTargetLast = psychicEntropy.TargetPsyfocus;
					this.job.wasOnMeditationTimeAssignment = flag;
				}
				else if (this.pawn.needs.joy.CurLevelPercentage >= 1f)
				{
					base.EndJobWith(JobCondition.InterruptForced);
					return;
				}
				if (this.faceDir.IsValid && !this.FromBed)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir);
				}
				this.MeditationTick();
				if (ModsConfig.RoyaltyActive && MeditationFocusDefOf.Natural.CanPawnUse(this.pawn))
				{
					int num = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = this.pawn.Position + GenRadial.RadialPattern[i];
						if (c.InBounds(this.pawn.Map))
						{
							Plant plant = c.GetPlant(this.pawn.Map);
							if (plant != null && plant.def == ThingDefOf.Plant_TreeAnima)
							{
								CompSpawnSubplant compSpawnSubplant = plant.TryGetComp<CompSpawnSubplant>();
								if (compSpawnSubplant != null)
								{
									compSpawnSubplant.AddProgress(JobDriver_Meditate.AnimaTreeSubplantProgressPerTick, false);
								}
							}
						}
					}
				}
			});
			yield return meditate;
			yield break;
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x00250D51 File Offset: 0x0024EF51
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (ModsConfig.RoyaltyActive && this.pawn.psychicEntropy != null)
			{
				this.job.psyfocusTargetLast = this.pawn.psychicEntropy.TargetPsyfocus;
			}
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x00250D88 File Offset: 0x0024EF88
		protected void MeditationTick()
		{
			this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.018000001f, false, false);
			this.pawn.GainComfortFromCellIfPossible(1, false);
			if (this.pawn.needs.joy != null)
			{
				JoyUtility.JoyTickCheckEnd(this.pawn, 1, JoyTickFullJoyAction.None, 1f, null);
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Meditating, 0.42f);
			}
			if (ModsConfig.RoyaltyActive && this.pawn.psychicEntropy != null)
			{
				this.pawn.psychicEntropy.Notify_Meditated();
				if (this.pawn.HasPsylink && this.pawn.psychicEntropy.PsychicSensitivity > 1E-45f)
				{
					float yOffset = (float)(this.pawn.Position.x % 2 + this.pawn.Position.z % 2) / 10f;
					if (this.psyfocusMote == null || this.psyfocusMote.Destroyed)
					{
						this.psyfocusMote = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero, 1f, -1f);
						this.psyfocusMote.yOffset = yOffset;
					}
					this.psyfocusMote.Maintain();
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = SoundDefOf.MeditationGainPsyfocus.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
					}
					this.sustainer.Maintain();
					this.pawn.psychicEntropy.GainPsyfocus_NewTemp(1, this.Focus.Thing);
				}
			}
		}

		// Token: 0x06007B1C RID: 31516 RVA: 0x00250F4C File Offset: 0x0024F14C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.faceDir, "faceDir", default(IntVec3), false);
		}

		// Token: 0x04005336 RID: 21302
		protected IntVec3 faceDir;

		// Token: 0x04005337 RID: 21303
		private Mote psyfocusMote;

		// Token: 0x04005338 RID: 21304
		protected Sustainer sustainer;

		// Token: 0x04005339 RID: 21305
		protected const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x0400533A RID: 21306
		protected const TargetIndex BedInd = TargetIndex.B;

		// Token: 0x0400533B RID: 21307
		protected const TargetIndex FocusInd = TargetIndex.C;

		// Token: 0x0400533C RID: 21308
		public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

		// Token: 0x0400533D RID: 21309
		private const int TicksBetweenMotesBase = 100;

		// Token: 0x0400533E RID: 21310
		private const int JobCheckIntervalTicks = 4000;
	}
}
