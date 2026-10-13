using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001486 RID: 5254
	public class JobGiver_FleeDanger : ThinkNode_JobGiver
	{
		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06007E72 RID: 32370 RVA: 0x00260DB1 File Offset: 0x0025EFB1
		public override string CrawlingReportStringOverride
		{
			get
			{
				return base.CrawlingReportStringOverride ?? "ReportStringCrawlingToSafety".Translate();
			}
		}

		// Token: 0x06007E73 RID: 32371 RVA: 0x00260DCC File Offset: 0x0025EFCC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed && !pawn.health.CanCrawl)
			{
				return null;
			}
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, 4, 10);
			if (job != null)
			{
				return job;
			}
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				Thing thing = potentialTargetsFor[i].Thing;
				if (pawn.Position.InHorDistOf(thing.Position, 4f) && FleeUtility.ShouldFleeFrom(thing, pawn, false, true))
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 == null || !pawn2.AnimalOrWildMan() || pawn2.Faction != null)
					{
						Job job2 = FleeUtility.FleeJob(pawn, thing, 10);
						if (job2 != null)
						{
							return job2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040054C8 RID: 21704
		private const int FleeDistance = 10;

		// Token: 0x040054C9 RID: 21705
		private const int DistToFireToFlee = 4;

		// Token: 0x040054CA RID: 21706
		private const int DistToDangerToFlee = 4;
	}
}
