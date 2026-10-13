using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200143F RID: 5183
	public class JobGiver_GatherOfferingsForPsychicRitual : ThinkNode_JobGiver
	{
		// Token: 0x06007DAC RID: 32172 RVA: 0x0025DA18 File Offset: 0x0025BC18
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			LordToil_PsychicRitual lordToil_PsychicRitual = lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return null;
			}
			PsychicRitualDef def = lordToil_PsychicRitual.RitualData.psychicRitual.def;
			PsychicRitualDef_InvocationCircle ritualDef = def as PsychicRitualDef_InvocationCircle;
			if (ritualDef == null)
			{
				return null;
			}
			if (ritualDef.RequiredOffering == null)
			{
				return null;
			}
			PsychicRitual psychicRitual = lordToil_PsychicRitual.RitualData.psychicRitual;
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return null;
			}
			float num = PsychicRitualToil_GatherOfferings.PawnsOfferingCount(psychicRitual.assignments.AssignedPawns(psychicRitualRoleDef), ritualDef.RequiredOffering);
			int needed = Mathf.CeilToInt(ritualDef.RequiredOffering.GetBaseCount() - num);
			if (needed == 0)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThingReachable(pawn.PositionHeld, pawn.MapHeld, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing thing)
			{
				if (!ritualDef.RequiredOffering.filter.Allows(thing))
				{
					return false;
				}
				if (thing.IsForbidden(pawn))
				{
					return false;
				}
				int stackCount = Mathf.Min(needed, thing.stackCount);
				return pawn.CanReserve(thing, 10, stackCount, null, false);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing2);
			job.count = Mathf.Min(needed, thing2.stackCount);
			return job;
		}
	}
}
