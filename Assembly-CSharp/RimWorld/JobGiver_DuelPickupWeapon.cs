using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001439 RID: 5177
	public class JobGiver_DuelPickupWeapon : ThinkNode_JobGiver
	{
		// Token: 0x06007D9D RID: 32157 RVA: 0x0025D574 File Offset: 0x0025B774
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DuelPickupWeapon jobGiver_DuelPickupWeapon = (JobGiver_DuelPickupWeapon)base.DeepCopy(resolve);
			jobGiver_DuelPickupWeapon.scanRadius = this.scanRadius;
			return jobGiver_DuelPickupWeapon;
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x0025D590 File Offset: 0x0025B790
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment.AllEquipmentListForReading.Any((ThingWithComps e) => e.def.IsMeleeWeapon))
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)pawn.GetLord().LordJob;
			Pawn pawn2 = lordJob_Ritual_Duel.Opponent(pawn);
			TargetInfo selectedTarget = lordJob_Ritual_Duel.selectedTarget;
			Thing thing = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < GenRadial.NumCellsInRadius((float)this.scanRadius); i++)
			{
				IntVec3 c = selectedTarget.Cell + GenRadial.RadialPattern[i];
				if (c.InBounds(selectedTarget.Map))
				{
					foreach (Thing thing2 in c.GetThingList(selectedTarget.Map))
					{
						float num2 = pawn.Position.DistanceTo(thing2.Position);
						if (thing2.def.IsMeleeWeapon && pawn.CanReserveAndReach(thing2, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false) && EquipmentUtility.CanEquip(thing2, pawn) && num2 < pawn.Position.DistanceTo(pawn2.Position) && num2 < pawn2.Position.DistanceTo(thing2.Position) && num > num2)
						{
							thing = thing2;
							num = num2;
						}
					}
				}
			}
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Equip, thing);
				job.ignoreForbidden = true;
				return job;
			}
			return null;
		}

		// Token: 0x04005463 RID: 21603
		public int scanRadius = 3;
	}
}
