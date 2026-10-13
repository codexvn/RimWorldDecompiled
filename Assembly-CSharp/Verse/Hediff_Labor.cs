using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200074A RID: 1866
	public class Hediff_Labor : HediffWithParents
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002D8F RID: 11663 RVA: 0x000EAD15 File Offset: 0x000E8F15
		public override string LabelBase
		{
			get
			{
				return "Labor".Translate();
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x000EAD28 File Offset: 0x000E8F28
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				if (!labelInBrackets.NullOrEmpty())
				{
					return "LaborDilation".Translate() + ", " + labelInBrackets;
				}
				return "LaborDilation".Translate();
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002D91 RID: 11665 RVA: 0x000EAD74 File Offset: 0x000E8F74
		public float Progress
		{
			get
			{
				HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
				return 1f - (float)(hediffComp_Disappears.ticksToDisappear + this.laborPushingDuration) / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + this.laborPushingDuration);
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000EADB1 File Offset: 0x000E8FB1
		private static int GetRandomLaborPushingDuration()
		{
			return HediffDefOf.PregnancyLaborPushing.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000EADC7 File Offset: 0x000E8FC7
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.Severity = this.def.stages.RandomElement<HediffStage>().minSeverity;
			this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000EADF6 File Offset: 0x000E8FF6
		private static TargetInfo BestBed(Pawn mother)
		{
			return mother.CurrentBed() ?? RestUtility.FindPatientBedFor(mother);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000EAE0D File Offset: 0x000E900D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Lord lord = this.pawn.GetLord();
			if (lord == null || lord.LordJob == null)
			{
				Precept_Ritual precept_Ritual = (Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
				TargetInfo targetInfo = Hediff_Labor.BestBed(this.pawn);
				Command_Ritual command_Ritual = new Command_Ritual(precept_Ritual, targetInfo, null, new Dictionary<string, Pawn>
				{
					{
						"mother",
						this.pawn
					}
				});
				if (!targetInfo.IsValid)
				{
					command_Ritual.Disabled = true;
					command_Ritual.disabledReason = "NoAppropriateBedChildBirth".Translate();
				}
				else if (!this.FoundBirthDoctor(precept_Ritual))
				{
					command_Ritual.Disabled = true;
					command_Ritual.disabledReason = "NoDoctorChildBirth".Translate();
				}
				yield return command_Ritual;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return PregnancyUtility.BirthQualityGizmo(this.pawn);
				yield return new Command_Action
				{
					defaultLabel = "DEV: Force progress to labor pushing",
					action = delegate()
					{
						this.pawn.health.RemoveHediff(this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000EAE20 File Offset: 0x000E9020
		public override void PreRemoved()
		{
			base.PreRemoved();
			Hediff_LaborPushing hediff_LaborPushing = (Hediff_LaborPushing)this.pawn.health.AddHediff(HediffDefOf.PregnancyLaborPushing, null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageColonistInFinalStagesOfLabor".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			hediff_LaborPushing.SetParents(base.Mother, base.Father, this.geneSet);
			HediffComp_Disappears hediffComp_Disappears = hediff_LaborPushing.TryGetComp<HediffComp_Disappears>();
			hediffComp_Disappears.disappearsAfterTicks = this.laborPushingDuration;
			hediffComp_Disappears.ticksToDisappear = this.laborPushingDuration;
			hediff_LaborPushing.laborDuration = this.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000EAEE0 File Offset: 0x000E90E0
		private bool FoundBirthDoctor(Precept_Ritual birthRitual)
		{
			if (this.testForDoctor == null)
			{
				this.testForDoctor = birthRitual.behavior.def.roles.First((RitualRole r) => r.id == "doctor");
			}
			foreach (Pawn pawn in this.pawn.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				bool flag;
				if (pawn != this.pawn && RitualRoleAssignments.PawnNotAssignableReason(pawn, this.testForDoctor, birthRitual, null, null, out flag) == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000EAFA8 File Offset: 0x000E91A8
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

		// Token: 0x06002D99 RID: 11673 RVA: 0x000EB039 File Offset: 0x000E9239
		public override void PostRemoved()
		{
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
			base.PostRemoved();
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000EB059 File Offset: 0x000E9259
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

		// Token: 0x06002D9B RID: 11675 RVA: 0x000EB07B File Offset: 0x000E927B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.laborPushingDuration, "laborPushingDuration", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.laborPushingDuration == -1)
			{
				this.laborPushingDuration = Hediff_Labor.GetRandomLaborPushingDuration();
			}
		}

		// Token: 0x0400240A RID: 9226
		private RitualRole testForDoctor;

		// Token: 0x0400240B RID: 9227
		private int laborPushingDuration;

		// Token: 0x0400240C RID: 9228
		private Effecter progressBar;
	}
}
