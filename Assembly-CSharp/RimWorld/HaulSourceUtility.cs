using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020029AA RID: 10666
	public static class HaulSourceUtility
	{
		// Token: 0x0600EB11 RID: 60177 RVA: 0x00445A5F File Offset: 0x00443C5F
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(IHaulSource source, Pawn pawn)
		{
			Action <>9__0;
			foreach (Thing thing in ((IEnumerable<Thing>)source.GetDirectlyHeldThings()))
			{
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing, true);
				IntVec3 intVec;
				IHaulDestination haulDestination;
				if (thing.MapHeld.listerHaulables.ThingsPotentiallyNeedingHauling().Contains(thing) && StoreUtility.TryFindBestBetterStorageFor(thing, pawn, pawn.MapHeld, currentPriority, pawn.Faction, out intVec, out haulDestination, true))
				{
					string label = "HaulFromSource".Translate();
					Action action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate()
						{
							bool flag = false;
							foreach (Thing t in ((IEnumerable<Thing>)source.GetDirectlyHeldThings()))
							{
								Job job = HaulAIUtility.HaulToStorageJob(pawn, t, true);
								if (job != null)
								{
									job.playerForced = true;
									if (!flag)
									{
										flag = true;
										pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
									}
									else
									{
										pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), true);
									}
								}
							}
						});
					}
					yield return new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					break;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}
	}
}
