using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002246 RID: 8774
	public class CompAbilityEffect_HemogenCost : CompAbilityEffect
	{
		// Token: 0x17001D3F RID: 7487
		// (get) Token: 0x0600BF8F RID: 49039 RVA: 0x00378883 File Offset: 0x00376A83
		public new CompProperties_AbilityHemogenCost Props
		{
			get
			{
				return (CompProperties_AbilityHemogenCost)this.props;
			}
		}

		// Token: 0x17001D40 RID: 7488
		// (get) Token: 0x0600BF90 RID: 49040 RVA: 0x00378890 File Offset: 0x00376A90
		private bool HasEnoughHemogen
		{
			get
			{
				Pawn_GeneTracker genes = this.parent.pawn.genes;
				Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
				return gene_Hemogen != null && gene_Hemogen.Value >= this.Props.hemogenCost;
			}
		}

		// Token: 0x0600BF91 RID: 49041 RVA: 0x003788D3 File Offset: 0x00376AD3
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GeneUtility.OffsetHemogen(this.parent.pawn, -this.Props.hemogenCost, true);
		}

		// Token: 0x0600BF92 RID: 49042 RVA: 0x003788FC File Offset: 0x00376AFC
		public override bool GizmoDisabled(out string reason)
		{
			Pawn_GeneTracker genes = this.parent.pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				reason = "AbilityDisabledNoHemogenGene".Translate(this.parent.pawn);
				return true;
			}
			if (gene_Hemogen.Value < this.Props.hemogenCost)
			{
				reason = "AbilityDisabledNoHemogen".Translate(this.parent.pawn);
				return true;
			}
			float num = this.TotalHemogenCostOfQueuedAbilities();
			float num2 = this.Props.hemogenCost + num;
			if (this.Props.hemogenCost > 1E-45f && num2 > gene_Hemogen.Value)
			{
				reason = "AbilityDisabledNoHemogen".Translate(this.parent.pawn);
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x0600BF93 RID: 49043 RVA: 0x003789D9 File Offset: 0x00376BD9
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.HasEnoughHemogen;
		}

		// Token: 0x0600BF94 RID: 49044 RVA: 0x003789E4 File Offset: 0x00376BE4
		private float TotalHemogenCostOfQueuedAbilities()
		{
			Pawn_JobTracker jobs = this.parent.pawn.jobs;
			object obj;
			if (jobs == null)
			{
				obj = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				obj = ((curJob != null) ? curJob.verbToUse : null);
			}
			Verb_CastAbility verb_CastAbility = obj as Verb_CastAbility;
			float num;
			if (verb_CastAbility == null)
			{
				num = 0f;
			}
			else
			{
				Ability ability = verb_CastAbility.ability;
				num = ((ability != null) ? ability.HemogenCost() : 0f);
			}
			float num2 = num;
			if (this.parent.pawn.jobs != null)
			{
				for (int i = 0; i < this.parent.pawn.jobs.jobQueue.Count; i++)
				{
					Verb_CastAbility verb_CastAbility2 = this.parent.pawn.jobs.jobQueue[i].job.verbToUse as Verb_CastAbility;
					if (verb_CastAbility2 != null)
					{
						float num3 = num2;
						Ability ability2 = verb_CastAbility2.ability;
						num2 = num3 + ((ability2 != null) ? ability2.HemogenCost() : 0f);
					}
				}
			}
			return num2;
		}
	}
}
