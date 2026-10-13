using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002C5D RID: 11357
	public class CompDisableUnnaturalCorpse : CompInteractable
	{
		// Token: 0x1700281E RID: 10270
		// (get) Token: 0x0600FEAB RID: 65195 RVA: 0x0049D37E File Offset: 0x0049B57E
		public UnnaturalCorpse Corpse
		{
			get
			{
				return (UnnaturalCorpse)this.parent;
			}
		}

		// Token: 0x1700281F RID: 10271
		// (get) Token: 0x0600FEAC RID: 65196 RVA: 0x0049D38B File Offset: 0x0049B58B
		public new CompProperties_DisableUnnaturalCorpse Props
		{
			get
			{
				return (CompProperties_DisableUnnaturalCorpse)this.props;
			}
		}

		// Token: 0x0600FEAD RID: 65197 RVA: 0x0049D398 File Offset: 0x0049B598
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.OrderActivation(target.Pawn);
		}

		// Token: 0x0600FEAE RID: 65198 RVA: 0x0049D3A7 File Offset: 0x0049B5A7
		protected override void OnInteracted(Pawn caster)
		{
			this.Corpse.DoStudiedDeactivation(caster);
		}

		// Token: 0x0600FEAF RID: 65199 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600FEB0 RID: 65200 RVA: 0x0049D3B5 File Offset: 0x0049B5B5
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.Corpse.Tracker.CanDestroyViaResearch)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanInteract(selPawn, true);
			FloatMenuOption floatMenuOption = new FloatMenuOption(this.Props.jobString.CapitalizeFirst(), delegate()
			{
				this.OrderActivation(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				FloatMenuOption floatMenuOption2 = floatMenuOption;
				floatMenuOption2.Label = floatMenuOption2.Label + " (" + acceptanceReport.Reason + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0600FEB1 RID: 65201 RVA: 0x0049D3CC File Offset: 0x0049B5CC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Corpse.Tracker.CanDestroyViaResearch)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600FEB2 RID: 65202 RVA: 0x0049D3DC File Offset: 0x0049B5DC
		private void OrderActivation(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
			job.count = 1;
			job.playerForced = true;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
