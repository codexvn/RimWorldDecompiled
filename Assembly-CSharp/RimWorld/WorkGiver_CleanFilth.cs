using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A5 RID: 5541
	public class WorkGiver_CleanFilth : WorkGiver_Scanner
	{
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06008304 RID: 33540 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x06008305 RID: 33541 RVA: 0x0027532B File Offset: 0x0027352B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Filth);
			}
		}

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x06008306 RID: 33542 RVA: 0x000E9647 File Offset: 0x000E7847
		public override int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06008307 RID: 33543 RVA: 0x00275334 File Offset: 0x00273534
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x06008308 RID: 33544 RVA: 0x00275346 File Offset: 0x00273546
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x06008309 RID: 33545 RVA: 0x00275360 File Offset: 0x00273560
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && !filth.Fogged() && pawn.CanReserve(t, 1, -1, null, forced) && filth.TicksSinceThickened >= 600;
		}

		// Token: 0x0600830A RID: 33546 RVA: 0x002753C4 File Offset: 0x002735C4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			WorkGiver_CleanFilth.<>c__DisplayClass10_0 CS$<>8__locals1 = new WorkGiver_CleanFilth.<>c__DisplayClass10_0();
			CS$<>8__locals1.pawn = pawn;
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(TargetIndex.A, t);
			int num = 15;
			CS$<>8__locals1.map = t.Map;
			CS$<>8__locals1.room = t.GetRoom(RegionType.Set_All);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (CS$<>8__locals1.<JobOnThing>g__ShouldClean|0(c))
				{
					List<Thing> thingList = c.GetThingList(CS$<>8__locals1.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (this.HasJobOnThing(CS$<>8__locals1.pawn, thing, forced) && thing != t)
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(CS$<>8__locals1.pawn.Position));
			}
			return job;
		}

		// Token: 0x040056AF RID: 22191
		private const int MinTicksSinceThickened = 600;
	}
}
