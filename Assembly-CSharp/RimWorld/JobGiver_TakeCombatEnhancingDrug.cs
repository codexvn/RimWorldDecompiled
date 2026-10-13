using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200146D RID: 5229
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		// Token: 0x06007E29 RID: 32297 RVA: 0x0025FD6B File Offset: 0x0025DF6B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x0025FD88 File Offset: 0x0025DF88
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (pawn.IsTeetotaler())
			{
				return null;
			}
			if (Find.TickManager.TicksGame - pawn.mindState.lastTakeCombatEnhancingDrugTick < 20000)
			{
				return null;
			}
			Thing thing = pawn.inventory.FindCombatEnhancingDrug();
			if (thing == null)
			{
				return null;
			}
			if (this.onlyIfInDanger)
			{
				Lord lord = pawn.GetLord();
				if (lord == null)
				{
					if (!this.HarmedRecently(pawn))
					{
						return null;
					}
				}
				else
				{
					int num = 0;
					int num2 = Mathf.Clamp(lord.ownedPawns.Count / 2, 1, 4);
					for (int i = 0; i < lord.ownedPawns.Count; i++)
					{
						if (this.HarmedRecently(lord.ownedPawns[i]))
						{
							num++;
							if (num >= num2)
							{
								break;
							}
						}
					}
					if (num < num2)
					{
						return null;
					}
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = 1;
			return job;
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x0025FE66 File Offset: 0x0025E066
		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x0025FE85 File Offset: 0x0025E085
		[Obsolete("Will be removed in a future update, use pawn.inventory.FindCombatEnhancingDrug()")]
		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			return pawn.inventory.FindCombatEnhancingDrug();
		}

		// Token: 0x040054AF RID: 21679
		private bool onlyIfInDanger;

		// Token: 0x040054B0 RID: 21680
		private const int TakeEveryTicks = 20000;
	}
}
