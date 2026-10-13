using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001541 RID: 5441
	public class JobGiver_Work : ThinkNode
	{
		// Token: 0x06008126 RID: 33062 RVA: 0x0026DEB4 File Offset: 0x0026C0B4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x0026DED0 File Offset: 0x0026C0D0
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				return 5.5f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Work)
			{
				return 9f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				return 3f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				return 2f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
			{
				return 2f;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x0026DF60 File Offset: 0x0026C160
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobGiver_Work.<>c__DisplayClass3_0 CS$<>8__locals1 = new JobGiver_Work.<>c__DisplayClass3_0();
			CS$<>8__locals1.pawn = pawn;
			if (CS$<>8__locals1.pawn.RaceProps.Humanlike && CS$<>8__locals1.pawn.health.hediffSet.InLabor(true))
			{
				return ThinkResult.NoJob;
			}
			if (this.emergency && CS$<>8__locals1.pawn.mindState.priorityWork.IsPrioritized)
			{
				List<WorkGiverDef> workGiversByPriority = CS$<>8__locals1.pawn.mindState.priorityWork.WorkGiver.workType.workGiversByPriority;
				for (int i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					if (this.WorkGiversRelated(CS$<>8__locals1.pawn.mindState.priorityWork.WorkGiver, worker.def))
					{
						Job job = this.GiverTryGiveJobPrioritized(CS$<>8__locals1.pawn, worker, CS$<>8__locals1.pawn.mindState.priorityWork.Cell);
						if (job != null)
						{
							job.playerForced = true;
							if (CS$<>8__locals1.pawn.jobs.debugLog)
							{
								CS$<>8__locals1.pawn.jobs.DebugLogEvent(string.Format("JobGiver_Work produced emergency Job {0} from {1}", job.ToStringSafe<Job>(), worker));
							}
							return new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive), false);
						}
					}
				}
				CS$<>8__locals1.pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = (!this.emergency) ? CS$<>8__locals1.pawn.workSettings.WorkGiversInOrderNormal : CS$<>8__locals1.pawn.workSettings.WorkGiversInOrderEmergency;
			int num = -999;
			CS$<>8__locals1.bestTargetOfLastPriority = TargetInfo.Invalid;
			CS$<>8__locals1.scannerWhoProvidedTarget = null;
			for (int j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && CS$<>8__locals1.bestTargetOfLastPriority.IsValid)
				{
					break;
				}
				if (this.PawnCanUseWorkGiver(CS$<>8__locals1.pawn, workGiver))
				{
					try
					{
						JobGiver_Work.<>c__DisplayClass3_1 CS$<>8__locals2 = new JobGiver_Work.<>c__DisplayClass3_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						Job job2 = workGiver.NonScanJob(CS$<>8__locals2.CS$<>8__locals1.pawn);
						if (job2 != null)
						{
							if (CS$<>8__locals2.CS$<>8__locals1.pawn.jobs.debugLog)
							{
								CS$<>8__locals2.CS$<>8__locals1.pawn.jobs.DebugLogEvent(string.Format("JobGiver_Work produced non-scan Job {0} from {1}", job2.ToStringSafe<Job>(), workGiver));
							}
							return new ThinkResult(job2, this, new JobTag?(list[j].def.tagToGive), false);
						}
						CS$<>8__locals2.scanner = (workGiver as WorkGiver_Scanner);
						if (CS$<>8__locals2.scanner != null)
						{
							if (CS$<>8__locals2.scanner.def.scanThings)
							{
								IEnumerable<Thing> enumerable = CS$<>8__locals2.scanner.PotentialWorkThingsGlobal(CS$<>8__locals2.CS$<>8__locals1.pawn);
								Pawn_CarryTracker carryTracker = CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker;
								bool flag = ((carryTracker != null) ? carryTracker.CarriedThing : null) != null && CS$<>8__locals2.scanner.PotentialWorkThingRequest.Accepts(CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing) && CS$<>8__locals2.<TryIssueJobPackage>g__Validator|0(CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing);
								Thing thing;
								if (CS$<>8__locals2.scanner.Prioritized)
								{
									IEnumerable<Thing> searchSet = enumerable ?? CS$<>8__locals2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(CS$<>8__locals2.scanner.PotentialWorkThingRequest);
									if (CS$<>8__locals2.scanner.AllowUnreachable)
									{
										thing = GenClosest.ClosestThing_Global(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, searchSet, 99999f, new Predicate<Thing>(CS$<>8__locals2.<TryIssueJobPackage>g__Validator|0), (Thing x) => CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, x), false);
									}
									else
									{
										thing = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, CS$<>8__locals2.CS$<>8__locals1.pawn.Map, searchSet, CS$<>8__locals2.scanner.PathEndMode, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals2.<TryIssueJobPackage>g__Validator|0), (Thing x) => CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, x), false);
									}
									if (flag)
									{
										if (thing != null)
										{
											float priority = CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing);
											float priority2 = CS$<>8__locals2.scanner.GetPriority(CS$<>8__locals2.CS$<>8__locals1.pawn, thing);
											if (priority >= priority2)
											{
												thing = CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing;
											}
										}
										else
										{
											thing = CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing;
										}
									}
								}
								else if (flag)
								{
									thing = CS$<>8__locals2.CS$<>8__locals1.pawn.carryTracker.CarriedThing;
								}
								else if (CS$<>8__locals2.scanner.AllowUnreachable)
								{
									IEnumerable<Thing> searchSet2 = enumerable ?? CS$<>8__locals2.CS$<>8__locals1.pawn.Map.listerThings.ThingsMatching(CS$<>8__locals2.scanner.PotentialWorkThingRequest);
									thing = GenClosest.ClosestThing_Global(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, searchSet2, 99999f, new Predicate<Thing>(CS$<>8__locals2.<TryIssueJobPackage>g__Validator|0), null, false);
								}
								else
								{
									thing = GenClosest.ClosestThingReachable(CS$<>8__locals2.CS$<>8__locals1.pawn.Position, CS$<>8__locals2.CS$<>8__locals1.pawn.Map, CS$<>8__locals2.scanner.PotentialWorkThingRequest, CS$<>8__locals2.scanner.PathEndMode, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn), TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals2.<TryIssueJobPackage>g__Validator|0), enumerable, 0, CS$<>8__locals2.scanner.MaxRegionsToScanBeforeGlobalSearch, enumerable != null, RegionType.Set_Passable, false, false);
								}
								if (thing != null)
								{
									CS$<>8__locals2.CS$<>8__locals1.bestTargetOfLastPriority = thing;
									CS$<>8__locals2.CS$<>8__locals1.scannerWhoProvidedTarget = CS$<>8__locals2.scanner;
								}
							}
							if (CS$<>8__locals2.scanner.def.scanCells)
							{
								JobGiver_Work.<>c__DisplayClass3_2 CS$<>8__locals3;
								CS$<>8__locals3.pawnPosition = CS$<>8__locals2.CS$<>8__locals1.pawn.Position;
								CS$<>8__locals3.closestDistSquared = 99999f;
								CS$<>8__locals3.bestPriority = float.MinValue;
								CS$<>8__locals3.prioritized = CS$<>8__locals2.scanner.Prioritized;
								CS$<>8__locals3.allowUnreachable = CS$<>8__locals2.scanner.AllowUnreachable;
								CS$<>8__locals3.maxPathDanger = CS$<>8__locals2.scanner.MaxPathDanger(CS$<>8__locals2.CS$<>8__locals1.pawn);
								IEnumerable<IntVec3> enumerable2 = CS$<>8__locals2.scanner.PotentialWorkCellsGlobal(CS$<>8__locals2.CS$<>8__locals1.pawn);
								IList<IntVec3> list2 = enumerable2 as IList<IntVec3>;
								if (list2 != null)
								{
									for (int k = 0; k < list2.Count; k++)
									{
										CS$<>8__locals2.<TryIssueJobPackage>g__ProcessCell|3(list2[k], ref CS$<>8__locals3);
									}
								}
								else
								{
									foreach (IntVec3 c in enumerable2)
									{
										CS$<>8__locals2.<TryIssueJobPackage>g__ProcessCell|3(c, ref CS$<>8__locals3);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						string[] array = new string[5];
						int num2 = 0;
						Pawn pawn2 = CS$<>8__locals1.pawn;
						array[num2] = ((pawn2 != null) ? pawn2.ToString() : null);
						array[1] = " threw exception in WorkGiver ";
						array[2] = workGiver.def.defName;
						array[3] = ": ";
						array[4] = ex.ToString();
						Log.Error(string.Concat(array));
					}
					finally
					{
					}
					if (CS$<>8__locals1.bestTargetOfLastPriority.IsValid)
					{
						Job job3;
						if (CS$<>8__locals1.bestTargetOfLastPriority.HasThing)
						{
							job3 = CS$<>8__locals1.scannerWhoProvidedTarget.JobOnThing(CS$<>8__locals1.pawn, CS$<>8__locals1.bestTargetOfLastPriority.Thing, false);
						}
						else
						{
							job3 = CS$<>8__locals1.scannerWhoProvidedTarget.JobOnCell(CS$<>8__locals1.pawn, CS$<>8__locals1.bestTargetOfLastPriority.Cell, false);
						}
						if (job3 != null)
						{
							job3.workGiverDef = CS$<>8__locals1.scannerWhoProvidedTarget.def;
							if (CS$<>8__locals1.pawn.jobs.debugLog)
							{
								CS$<>8__locals1.pawn.jobs.DebugLogEvent(string.Format("JobGiver_Work produced scan Job {0} from {1}", job3.ToStringSafe<Job>(), CS$<>8__locals1.scannerWhoProvidedTarget));
							}
							return new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive), false);
						}
						string[] array2 = new string[6];
						int num3 = 0;
						WorkGiver_Scanner scannerWhoProvidedTarget = CS$<>8__locals1.scannerWhoProvidedTarget;
						array2[num3] = ((scannerWhoProvidedTarget != null) ? scannerWhoProvidedTarget.ToString() : null);
						array2[1] = " provided target ";
						int num4 = 2;
						TargetInfo bestTargetOfLastPriority = CS$<>8__locals1.bestTargetOfLastPriority;
						array2[num4] = bestTargetOfLastPriority.ToString();
						array2[3] = " but yielded no actual job for pawn ";
						int num5 = 4;
						Pawn pawn3 = CS$<>8__locals1.pawn;
						array2[num5] = ((pawn3 != null) ? pawn3.ToString() : null);
						array2[5] = ". The CanGiveJob and JobOnX methods may not be synchronized.";
						Log.ErrorOnce(string.Concat(array2), 6112651);
					}
					num = workGiver.def.priorityInType;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x0026E8C8 File Offset: 0x0026CAC8
		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			return (giver.def.nonColonistsCanDo || pawn.IsColonist || pawn.IsColonyMech || pawn.IsColonySubhuman) && !pawn.WorkTagIsDisabled(giver.def.workTags) && (giver.def.workType == null || !pawn.WorkTypeIsDisabled(giver.def.workType)) && !giver.ShouldSkip(pawn, false) && giver.MissingRequiredCapacity(pawn) == null && (!pawn.RaceProps.IsMechanoid || giver.def.canBeDoneByMechs);
		}

		// Token: 0x0600812A RID: 33066 RVA: 0x0026E967 File Offset: 0x0026CB67
		private bool WorkGiversRelated(WorkGiverDef current, WorkGiverDef next)
		{
			return (next != WorkGiverDefOf.Repair || current == WorkGiverDefOf.Repair) && next.doesSmoothing == current.doesSmoothing;
		}

		// Token: 0x0600812B RID: 33067 RVA: 0x0026E98C File Offset: 0x0026CB8C
		private Job GiverTryGiveJobPrioritized(Pawn pawn, WorkGiver giver, IntVec3 cell)
		{
			if (!this.PawnCanUseWorkGiver(pawn, giver))
			{
				return null;
			}
			try
			{
				Job job = giver.NonScanJob(pawn);
				if (job != null)
				{
					return job;
				}
				WorkGiver_Scanner scanner = giver as WorkGiver_Scanner;
				if (scanner != null)
				{
					if (giver.def.scanThings)
					{
						Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
						List<Thing> thingList = cell.GetThingList(pawn.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (scanner.PotentialWorkThingRequest.Accepts(thing) && predicate(thing))
							{
								Job job2 = scanner.JobOnThing(pawn, thing, false);
								if (job2 != null)
								{
									job2.workGiverDef = giver.def;
								}
								return job2;
							}
						}
					}
					if (giver.def.scanCells && !cell.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, cell, false))
					{
						Job job3 = scanner.JobOnCell(pawn, cell, false);
						if (job3 != null)
						{
							job3.workGiverDef = giver.def;
						}
						return job3;
					}
				}
			}
			catch (Exception ex)
			{
				string[] array = new string[5];
				int num = 0;
				Pawn pawn2 = pawn;
				array[num] = ((pawn2 != null) ? pawn2.ToString() : null);
				array[1] = " threw exception in GiverTryGiveJobTargeted on WorkGiver ";
				array[2] = giver.def.defName;
				array[3] = ": ";
				array[4] = ex.ToString();
				Log.Error(string.Concat(array));
			}
			return null;
		}

		// Token: 0x04005629 RID: 22057
		public bool emergency;
	}
}
