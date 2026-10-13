using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002E77 RID: 11895
	public class CompTechprint : ThingComp
	{
		// Token: 0x17002AFE RID: 11006
		// (get) Token: 0x06010BFB RID: 68603 RVA: 0x004D3A53 File Offset: 0x004D1C53
		public CompProperties_Techprint Props
		{
			get
			{
				return (CompProperties_Techprint)this.props;
			}
		}

		// Token: 0x06010BFC RID: 68604 RVA: 0x004D3A60 File Offset: 0x004D1C60
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!ModLister.CheckRoyalty("Techprint"))
			{
				yield break;
			}
			JobFailReason.Clear();
			if (selPawn.WorkTypeIsDisabled(WorkTypeDefOf.Research) || selPawn.WorkTagIsDisabled(WorkTags.Intellectual))
			{
				JobFailReason.Is("WillNever".Translate("Research".TranslateSimple().UncapitalizeFirst()), null);
			}
			else if (!selPawn.CanReach(this.parent, PathEndMode.ClosestTouch, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				JobFailReason.Is("CannotReach".Translate(), null);
			}
			HaulAIUtility.PawnCanAutomaticallyHaul(selPawn, this.parent, true);
			Thing thing2 = GenClosest.ClosestThingReachable(selPawn.Position, selPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(selPawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing thing) => thing is Building_ResearchBench && !thing.IsForbidden(selPawn) && selPawn.CanReserve(thing, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
			Job job = null;
			if (thing2 != null)
			{
				job = JobMaker.MakeJob(JobDefOf.ApplyTechprint);
				job.targetA = thing2;
				job.targetB = this.parent;
				job.targetC = thing2.Position;
			}
			if (JobFailReason.HaveReason)
			{
				yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("ApplyTechprint".Translate(this.parent.Label)) + ": " + JobFailReason.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				JobFailReason.Clear();
			}
			else
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ApplyTechprint".Translate(this.parent.Label).CapitalizeFirst(), delegate()
				{
					if (job == null)
					{
						Messages.Message("MessageNoResearchBenchForTechprint".Translate(), MessageTypeDefOf.RejectInput, true);
						return;
					}
					selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy", null);
			}
			yield break;
		}
	}
}
