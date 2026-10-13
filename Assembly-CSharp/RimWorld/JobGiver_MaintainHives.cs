using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013CB RID: 5067
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		// Token: 0x06007C15 RID: 31765 RVA: 0x00255E3B File Offset: 0x0025403B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_MaintainHives jobGiver_MaintainHives = (JobGiver_MaintainHives)base.DeepCopy(resolve);
			jobGiver_MaintainHives.onlyIfDamagingState = this.onlyIfDamagingState;
			return jobGiver_MaintainHives;
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x00255E58 File Offset: 0x00254058
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			int num = 0;
			while ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map) == room)
				{
					Hive hive = (Hive)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.Hive);
					if (hive != null && pawn.CanReserve(hive, 1, -1, null, false))
					{
						CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
						if (compMaintainable.CurStage != MaintainableStage.Healthy && (!this.onlyIfDamagingState || compMaintainable.CurStage == MaintainableStage.Damaging))
						{
							return JobMaker.MakeJob(JobDefOf.Maintain, hive);
						}
					}
				}
				num++;
			}
			return null;
		}

		// Token: 0x040053C6 RID: 21446
		private bool onlyIfDamagingState;

		// Token: 0x040053C7 RID: 21447
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
