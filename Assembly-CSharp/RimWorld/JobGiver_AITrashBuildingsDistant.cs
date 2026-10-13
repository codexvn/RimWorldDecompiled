using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001416 RID: 5142
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		// Token: 0x06007D29 RID: 32041 RVA: 0x0025AF33 File Offset: 0x00259133
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x0025AF50 File Offset: 0x00259150
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				return null;
			}
			JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Clear();
			foreach (Building item in allBuildingsColonist)
			{
				JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Add(item);
			}
			if (JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < 75; i++)
			{
				Building building = JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.RandomElement<Building>();
				if (TrashUtility.ShouldTrashBuilding(pawn, building, this.attackAllInert))
				{
					Job job = TrashUtility.TrashJob(pawn, building, this.attackAllInert, false);
					if (job != null)
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x04005438 RID: 21560
		public bool attackAllInert;

		// Token: 0x04005439 RID: 21561
		private static readonly List<Building> tmpTrashableBuildingCandidates = new List<Building>();
	}
}
