using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001070 RID: 4208
	public class PsychicRitualToil_GatherOfferings : PsychicRitualToil
	{
		// Token: 0x060066DB RID: 26331 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_GatherOfferings()
		{
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x002085EF File Offset: 0x002067EF
		public PsychicRitualToil_GatherOfferings(PsychicRitualRoleDef offeringGatherer, IngredientCount requiredOffering)
		{
			this.gathererRole = offeringGatherer;
			this.requiredOffering = requiredOffering;
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x00208608 File Offset: 0x00206808
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			DutyDef def;
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				def = DutyDefOf.Idle;
			}
			else
			{
				def = DutyDefOf.GatherOfferingsForPsychicRitual;
			}
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.gathererRole))
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, def, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x002086B4 File Offset: 0x002068B4
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return this.offeringsGathered;
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x002086BC File Offset: 0x002068BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.gathererRole, "gathererRole");
			Scribe_Values.Look<bool>(ref this.offeringsGathered, "offeringsGathered", false, false);
			Scribe_Deep.Look<IngredientCount>(ref this.requiredOffering, "requiredOffering", Array.Empty<object>());
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x002086FC File Offset: 0x002068FC
		public static float PawnsOfferingCount(IEnumerable<Pawn> pawns, IngredientCount offering)
		{
			float num = 0f;
			foreach (Pawn pawn in pawns)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)pawn.inventory.GetDirectlyHeldThings()))
				{
					if (offering.filter.Allows(thing))
					{
						num += (float)thing.stackCount;
						if (num >= offering.GetBaseCount())
						{
							return offering.GetBaseCount();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x002087AC File Offset: 0x002069AC
		private bool PawnsHaveOfferings(PsychicRitual psychicRitual)
		{
			float baseCount = this.requiredOffering.GetBaseCount();
			return PsychicRitualToil_GatherOfferings.PawnsOfferingCount(psychicRitual.assignments.AssignedPawns(this.gathererRole), this.requiredOffering) >= baseCount;
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x002087E8 File Offset: 0x002069E8
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			base.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
			if (psychicRitual.assignments.RoleForPawn(pawn, true) != this.gathererRole)
			{
				return;
			}
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x00208868 File Offset: 0x00206A68
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			result = base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
			if (result.Job != null)
			{
				return result;
			}
			if (psychicRitual.assignments.RoleForPawn(pawn, true) != this.gathererRole)
			{
				return result;
			}
			if (this.offeringsGathered || this.PawnsHaveOfferings(psychicRitual))
			{
				this.offeringsGathered = true;
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
				return new ThinkResult(JobMaker.MakeJob(JobDefOf.Wait, 1, false), null, null, false);
			}
			TaggedString reason = "PsychicRitualToil_GatherOfferings_OfferingUnavailable".Translate(pawn.Named("PAWN"), this.requiredOffering.filter.Summary);
			psychicRitual.LeaveOrCancelPsychicRitual(this.gathererRole, pawn, reason);
			return result;
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x00208952 File Offset: 0x00206B52
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			if (psychicRitual.assignments.RoleForPawn(pawn, true) == this.gathererRole)
			{
				return "PsychicRitualToil_GatherOfferings_JobReport".Translate();
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x04004962 RID: 18786
		protected PsychicRitualRoleDef gathererRole;

		// Token: 0x04004963 RID: 18787
		protected bool offeringsGathered;

		// Token: 0x04004964 RID: 18788
		protected IngredientCount requiredOffering;
	}
}
