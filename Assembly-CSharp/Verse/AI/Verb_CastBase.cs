using System;
using RimWorld;
using RimWorld.Utility;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E6E RID: 3694
	public abstract class Verb_CastBase : Verb
	{
		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005D19 RID: 23833 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x001C67BA File Offset: 0x001C49BA
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x001DA8C0 File Offset: 0x001D8AC0
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06005D1C RID: 23836 RVA: 0x001DA8FC File Offset: 0x001D8AFC
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.verbProps.targetParams.CanTarget(target.ToTargetInfo(this.caster.Map), null))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x001DA94C File Offset: 0x001D8B4C
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.CanHitTarget(target))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				base.DrawHighlightFieldRadiusAroundTarget(target);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => this.CanHitTarget(c));
		}
	}
}
