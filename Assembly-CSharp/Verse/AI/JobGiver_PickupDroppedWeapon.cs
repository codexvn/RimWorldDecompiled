using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E5F RID: 3679
	public class JobGiver_PickupDroppedWeapon : ThinkNode_JobGiver
	{
		// Token: 0x06005CA7 RID: 23719 RVA: 0x001D8D08 File Offset: 0x001D6F08
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			Thing thing = (mindState != null) ? mindState.droppedWeapon : null;
			if (thing == null || thing.Destroyed || !thing.Spawned || thing.Map != pawn.Map)
			{
				return null;
			}
			return JobGiver_PickupDroppedWeapon.PickupWeaponJob(pawn, thing, this.ignoreForbidden);
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x001D8D58 File Offset: 0x001D6F58
		public static Job PickupWeaponJob(Pawn pawn, Thing weapon, bool ignoreForbidden)
		{
			if (!pawn.CanReserveAndReach(weapon, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			if (weapon.IsBurning())
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Equip, weapon);
			job.ignoreForbidden = ignoreForbidden;
			return job;
		}

		// Token: 0x04004301 RID: 17153
		public bool ignoreForbidden;
	}
}
