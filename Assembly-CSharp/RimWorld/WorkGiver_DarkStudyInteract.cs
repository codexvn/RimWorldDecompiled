using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001615 RID: 5653
	public class WorkGiver_DarkStudyInteract : WorkGiver_StudyBase
	{
		// Token: 0x0600855F RID: 34143 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x06008560 RID: 34144 RVA: 0x0027E120 File Offset: 0x0027C320
		public override string PostProcessedGerund(Job job)
		{
			if (job.targetC == null)
			{
				return base.PostProcessedGerund(job);
			}
			return "DoWorkAtThing".Translate(this.def.gerund.Named("GERUND"), job.targetC.Label.Named("TARGETLABEL"));
		}

		// Token: 0x06008561 RID: 34145 RVA: 0x0027E184 File Offset: 0x0027C384
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Thing thing = t;
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
					if (thing == null || !pawn.CanReserve(thing, 1, -1, null, forced))
					{
						return false;
					}
				}
			}
			if (thing == null)
			{
				return false;
			}
			if (thing == pawn)
			{
				return false;
			}
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable.KnowledgeCategory == null)
			{
				return false;
			}
			if (!compStudiable.EverStudiable())
			{
				JobFailReason.IsSilent();
				return false;
			}
			if (!compStudiable.CurrentlyStudiable())
			{
				if (compStudiable.Props.frequencyTicks > 0 && compStudiable.TicksTilNextStudy > 0)
				{
					JobFailReason.Is("CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst(), null);
				}
				else
				{
					JobFailReason.IsSilent();
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008562 RID: 34146 RVA: 0x0027E25C File Offset: 0x0027C45C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			JobDef studyInteract = JobDefOf.StudyInteract;
			LocalTargetInfo targetA = t;
			LocalTargetInfo targetB = null;
			Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
			return JobMaker.MakeJob(studyInteract, targetA, targetB, (building_HoldingPlatform != null) ? building_HoldingPlatform.HeldPawn : null);
		}
	}
}
