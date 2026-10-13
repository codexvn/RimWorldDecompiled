using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200134C RID: 4940
	public static class Toils_Misc
	{
		// Token: 0x06007956 RID: 31062 RVA: 0x0024A760 File Offset: 0x00248960
		public static Toil Learn(SkillDef skill, float xp)
		{
			Toil toil = ToilMaker.MakeToil("Learn");
			toil.initAction = delegate()
			{
				toil.actor.skills.Learn(skill, xp, false, false);
			};
			return toil;
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x0024A7B0 File Offset: 0x002489B0
		public static Toil SetForbidden(TargetIndex ind, bool forbidden)
		{
			Toil toil = ToilMaker.MakeToil("SetForbidden");
			toil.initAction = delegate()
			{
				toil.actor.CurJob.GetTarget(ind).Thing.SetForbidden(forbidden, true);
			};
			return toil;
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x0024A800 File Offset: 0x00248A00
		public static Toil TakeItemFromInventoryToCarrier(Pawn pawn, TargetIndex itemInd)
		{
			Toil toil = ToilMaker.MakeToil("TakeItemFromInventoryToCarrier");
			toil.initAction = delegate()
			{
				Job curJob = pawn.CurJob;
				Thing thing = (Thing)curJob.GetTarget(itemInd);
				int count = Mathf.Min(thing.stackCount, curJob.count);
				pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, count, true);
				curJob.SetTarget(itemInd, pawn.carryTracker.CarriedThing);
			};
			return toil;
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x0024A840 File Offset: 0x00248A40
		public static Toil ThrowColonistAttackingMote(TargetIndex target)
		{
			Toil toil = ToilMaker.MakeToil("ThrowColonistAttackingMote");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted && !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(target).Thing) && (!actor.IsMutant || !actor.mutant.Def.canBeDrafted))
				{
					MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
				}
			};
			return toil;
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x0024A888 File Offset: 0x00248A88
		public static Toil FindRandomAdjacentReachableCell(TargetIndex adjacentToInd, TargetIndex cellInd)
		{
			Toil findCell = ToilMaker.MakeToil("FindRandomAdjacentReachableCell");
			findCell.initAction = delegate()
			{
				Pawn actor = findCell.actor;
				Job curJob = actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(adjacentToInd);
				LocalTargetInfo localTargetInfo;
				if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
				{
					Pawn pawn = actor;
					string str = (pawn != null) ? pawn.ToString() : null;
					string str2 = " could not find standable cell adjacent to ";
					localTargetInfo = target;
					Log.Error(str + str2 + localTargetInfo.ToString() + " because this thing is either unspawned or spawned somewhere else.");
					actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
					return;
				}
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					num++;
					if (num > 100)
					{
						break;
					}
					if (target.HasThing)
					{
						c = target.Thing.RandomAdjacentCell8Way();
					}
					else
					{
						c = target.Cell.RandomAdjacentCell8Way();
					}
					if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) && actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						goto Block_9;
					}
				}
				Pawn pawn2 = actor;
				string str3 = (pawn2 != null) ? pawn2.ToString() : null;
				string str4 = " could not find standable cell adjacent to ";
				localTargetInfo = target;
				Log.Error(str3 + str4 + localTargetInfo.ToString());
				actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
				return;
				Block_9:
				curJob.SetTarget(cellInd, c);
			};
			return findCell;
		}
	}
}
