using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001456 RID: 5206
	public class JobGiver_EnterTransporter : ThinkNode_JobGiver
	{
		// Token: 0x06007DE7 RID: 32231 RVA: 0x0025E9D8 File Offset: 0x0025CBD8
		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			if (transportersGroup != -1)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i] != pawn && allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == transportersGroup)
						{
							return null;
						}
					}
				}
				TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_EnterTransporter.tmpTransporters);
				CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(JobGiver_EnterTransporter.tmpTransporters, pawn);
				JobGiver_EnterTransporter.tmpTransporters.Clear();
				if (compTransporter == null || !pawn.CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.EnterTransporter, compTransporter.parent);
			}
			else
			{
				Thing thing = pawn.mindState.duty.focus.Thing;
				if (thing == null || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return null;
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, pawn.mindState.duty.focus.Thing);
				job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, LocomotionUrgency.Walk);
				return job;
			}
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x0025EB28 File Offset: 0x0025CD28
		public static CompTransporter FindMyTransporter(List<CompTransporter> transporters, Pawn me)
		{
			if (transporters == null)
			{
				return null;
			}
			for (int i = 0; i < transporters.Count; i++)
			{
				List<TransferableOneWay> leftToLoad = transporters[i].leftToLoad;
				if (leftToLoad != null)
				{
					for (int j = 0; j < leftToLoad.Count; j++)
					{
						if (leftToLoad[j].AnyThing is Pawn)
						{
							List<Thing> things = leftToLoad[j].things;
							for (int k = 0; k < things.Count; k++)
							{
								if (things[k] == me)
								{
									return transporters[i];
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0400548B RID: 21643
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
