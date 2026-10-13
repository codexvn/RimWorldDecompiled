using System;
using System.Collections.Generic;
using System.Text;
using Gilzoide.ManagedJobs;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005B3 RID: 1459
	public class PathFinder : IDisposable
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000C5917 File Offset: 0x000C3B17
		public PathFinderMapData MapData
		{
			get
			{
				return this.mapData;
			}
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000C5920 File Offset: 0x000C3B20
		public PathFinder(Map map)
		{
			this.map = map;
			this.mapData = new PathFinderMapData(map);
			int numGridCells = map.cellIndices.NumGridCells;
			int maxJobWorkerCount = UnityData.MaxJobWorkerCount;
			for (int i = 0; i < maxJobWorkerCount; i++)
			{
				this.batches.Add(PathFinder.JobBatchState.AllocateNew(numGridCells));
			}
			this.immediateBatch = PathFinder.JobBatchState.AllocateNew(numGridCells);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000C5A08 File Offset: 0x000C3C08
		public void PathFinderTick()
		{
			if (this.disposed)
			{
				return;
			}
			this.ForceCompleteScheduledJobs();
			this.FinalizeRecyclePathJobData();
			this.ComputeWorkThisTick();
			if (this.tmpCurrentWork.Count == 0)
			{
				return;
			}
			if (this.mapData.GatherData(this.tmpCurrentWork))
			{
				this.RecycleGridJobData();
			}
			JobHandle lastGridHandle = this.ScheduleGridJobs();
			this.ScheduleBatchedPathJobs(lastGridHandle);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000C5A68 File Offset: 0x000C3C68
		private void ComputeWorkThisTick()
		{
			if (this.tmpCurrentWork.Count != 0)
			{
				Log.Error(string.Format("Pathfinder current work queue is not empty, count: {0}", this.tmpCurrentWork.Count));
			}
			this.tmpCurrentWork.Clear();
			int ticksGame = GenTicks.TicksGame;
			for (int i = this.workQueue.Count - 1; i >= 0; i--)
			{
				PathRequest pathRequest = this.workQueue[i];
				if (pathRequest.Cancelled)
				{
					this.workQueue.RemoveAt(i);
				}
				else if (!pathRequest.Validate())
				{
					this.workQueue.RemoveAt(i);
				}
				else if (pathRequest.TickStart <= ticksGame)
				{
					this.tmpCurrentWork.Add(pathRequest);
					this.workQueue.RemoveAt(i);
				}
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000C5B24 File Offset: 0x000C3D24
		private void FinalizeRecyclePathJobData()
		{
			foreach (PathFinder.ScheduledPathJob scheduledPathJob in this.scheduledPathJobs)
			{
				if (!scheduledPathJob.request.Cancelled && scheduledPathJob.output.path.Length == 0)
				{
					Log.WarningOnce(string.Format("Resolved path returned no nodes, request: {0}", scheduledPathJob.request), HashCode.Combine<PathRequest, int>(scheduledPathJob.request, 163511));
					scheduledPathJob.request.Resolve(null);
				}
				else if (!scheduledPathJob.request.Cancelled && (scheduledPathJob.request.pawn == null || scheduledPathJob.request.pawn.Spawned))
				{
					PawnPath p = this.EmitPath(scheduledPathJob.output);
					scheduledPathJob.request.Resolve(p);
				}
				this.pathUniqueStates.Enqueue(scheduledPathJob.state);
				this.pathJobOutputs.Enqueue(scheduledPathJob.output);
			}
			this.scheduledPathJobs.Clear();
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000C5C3C File Offset: 0x000C3E3C
		private void RecycleGridJobData()
		{
			foreach (PathFinder.ScheduledGridJob scheduledGridJob in this.scheduledGridJobs)
			{
				this.gridJobOutputs.Enqueue(scheduledGridJob.output);
			}
			this.scheduledGridJobs.Clear();
			this.gridJobLookup.Clear();
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000C5CB0 File Offset: 0x000C3EB0
		private void ScheduleBatchedPathJobs(JobHandle lastGridHandle = default(JobHandle))
		{
			int count = this.batches.Count;
			int count2 = this.tmpCurrentWork.Count;
			int num2;
			int num = Math.DivRem(count2, count, out num2);
			if (num2 > 0)
			{
				num++;
			}
			int i = 0;
			int num3 = 0;
			while (i < count)
			{
				JobHandle jobHandle = lastGridHandle;
				PathFinder.JobBatchState jobBatchState = this.batches[i];
				int num4 = 0;
				while (num4 < num && num3 < count2)
				{
					PathRequest request = this.tmpCurrentWork.Pop<PathRequest>();
					PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
					PathFinder.ScheduledGridJob scheduledGridJob = this.gridJobLookup[key];
					PathFinderJob jobData = default(PathFinderJob);
					PathFinder.PathUniqueState state;
					this.GetOrCreatePathJobUniqueState(out state);
					PathFinder.PathJobOutput output;
					this.GetOrCreatePathJobOutput(out output);
					jobHandle = this.GetDoorBlockedJob(request, ref state).Schedule(jobHandle);
					this.ParameterizePathJob(ref jobData, ref key, ref output, ref state, ref jobBatchState, ref scheduledGridJob, request);
					this.mapData.ParameterizePathJob(ref jobData);
					jobHandle = jobData.Schedule(jobHandle);
					this.scheduledPathJobs.Add(new PathFinder.ScheduledPathJob
					{
						request = request,
						handle = jobHandle,
						state = state,
						output = output
					});
					num4++;
					num3++;
				}
				i++;
			}
			JobHandle.ScheduleBatchedJobs();
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000C5DF4 File Offset: 0x000C3FF4
		private ManagedJob GetDoorBlockedJob(PathRequest request, ref PathFinder.PathUniqueState jobState)
		{
			PathGridDoorsBlockedJob pathGridDoorsBlockedJob = new PathGridDoorsBlockedJob();
			this.ParameterizeDoorBlockedJob(pathGridDoorsBlockedJob, request, ref jobState);
			return new ManagedJob(pathGridDoorsBlockedJob);
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000C5E18 File Offset: 0x000C4018
		private JobHandle ScheduleGridJobs()
		{
			JobHandle result;
			using (ProfilerBlock.Scope("PathFinder.ScheduleGridJobs()"))
			{
				JobHandle jobHandle = default(JobHandle);
				foreach (PathRequest request in this.tmpCurrentWork)
				{
					PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
					if (!this.gridJobLookup.ContainsKey(key))
					{
						jobHandle = this.ScheduleGridJob(request, jobHandle).handle;
					}
				}
				result = jobHandle;
			}
			return result;
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000C5EC0 File Offset: 0x000C40C0
		private PathFinder.ScheduledGridJob ScheduleGridJob(PathRequest request, JobHandle handle = default(JobHandle))
		{
			PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(request);
			PathFinder.ScheduledGridJob scheduledGridJob;
			scheduledGridJob.lastAccessed = (scheduledGridJob.computed = GenTicks.TicksGame);
			this.GetOrCreateGridJobOutput(out scheduledGridJob.output);
			PathGridJob jobData = default(PathGridJob);
			this.mapData.ParameterizeGridJob(request, ref key, ref jobData, ref scheduledGridJob.output);
			int numGridCells = this.map.cellIndices.NumGridCells;
			int idealBatchCount = UnityData.GetIdealBatchCount(numGridCells);
			scheduledGridJob.handle = (handle.Equals(default(JobHandle)) ? jobData.Schedule(numGridCells, idealBatchCount, default(JobHandle)) : jobData.Schedule(numGridCells, idealBatchCount, handle));
			this.scheduledGridJobs.Add(scheduledGridJob);
			return this.gridJobLookup[key] = scheduledGridJob;
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000C5F88 File Offset: 0x000C4188
		private void ForceCompleteScheduledJobs()
		{
			int num = this.scheduledPathJobs.Count + this.scheduledGridJobs.Count;
			if (num <= 0)
			{
				return;
			}
			NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.scheduledPathJobs.Count; i++)
			{
				jobs[i] = this.scheduledPathJobs[i].handle;
			}
			for (int j = 0; j < this.scheduledGridJobs.Count; j++)
			{
				jobs[this.scheduledPathJobs.Count + j] = this.scheduledGridJobs[j].handle;
			}
			JobHandle.CompleteAll(jobs);
			jobs.Dispose();
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000C6034 File Offset: 0x000C4234
		private PawnPath EmitPath(in PathFinder.PathJobOutput pathJobOutput)
		{
			NativeList<IntVec3> path = pathJobOutput.path;
			NativeReference<PathFinderJob.ResultData> data = pathJobOutput.data;
			PathFinderJob.ResultData value = data.Value;
			PawnPath path2 = this.map.pawnPathPool.GetPath();
			path2.Initialize(path, value.pathCost);
			return path2;
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000C6074 File Offset: 0x000C4274
		private void GetOrCreateGridJobOutput(out PathFinder.GridJobOutput output)
		{
			if (!this.gridJobOutputs.TryDequeue(out output))
			{
				output.Initalize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000C609A File Offset: 0x000C429A
		private void GetOrCreatePathJobUniqueState(out PathFinder.PathUniqueState state)
		{
			if (!this.pathUniqueStates.TryDequeue(out state))
			{
				state.Initialize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000C60C0 File Offset: 0x000C42C0
		private void GetOrCreatePathJobOutput(out PathFinder.PathJobOutput output)
		{
			if (!this.pathJobOutputs.TryDequeue(out output))
			{
				output.Initialize(this.map.cellIndices.NumGridCells);
			}
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000C60E8 File Offset: 0x000C42E8
		public PathRequest CreateRequest(IntVec3 start, LocalTargetInfo target, IntVec3? dest, Pawn pawn, PathFinderCostTuning? tuning, PathEndMode peMode = PathEndMode.OnCell, PathRequest.IPathGridCustomizer customizer = null)
		{
			bool canBashDoors;
			bool flag;
			this.ResolveCanBash(pawn, out canBashDoors, out flag);
			Danger maxDanger = Danger.Deadly;
			TraverseMode mode = TraverseMode.ByPawn;
			bool canBashFences = flag;
			return this.CreateRequest(start, target, dest, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true), tuning, peMode, pawn, customizer);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x000C6124 File Offset: 0x000C4324
		public PathRequest CreateRequest(IntVec3 start, LocalTargetInfo target, IntVec3? dest, TraverseParms traverseParms, PathFinderCostTuning? mtuning, PathEndMode peMode = PathEndMode.OnCell, Pawn pawn = null, PathRequest.IPathGridCustomizer customizer = null)
		{
			int ticksGame = GenTicks.TicksGame;
			PathFinderCostTuning tuning = mtuning ?? PathFinderCostTuning.DefaultTuning;
			return new PathRequest(this.map, start, target, dest, traverseParms, tuning, peMode, pawn, ticksGame, ticksGame, ticksGame, customizer);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000C616C File Offset: 0x000C436C
		public void PushRequest(PathRequest request)
		{
			if (this.workQueue.Contains(request))
			{
				Log.Error("Tried to add the same PathRequest twice to PathFinder.");
				return;
			}
			if (request.ResultIsReady)
			{
				Log.Error("Tried to add PathRequest to PathFinder but it already has its result calculated.");
				return;
			}
			this.workQueue.Add(request);
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000C61A8 File Offset: 0x000C43A8
		private static float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			if (pawn == null)
			{
				return 1f;
			}
			if (pawn.RaceProps.Animal)
			{
				return 1.75f;
			}
			if (!pawn.IsColonist)
			{
				float magnitude = (start - dest.Cell).Magnitude;
				return (float)Mathf.RoundToInt(PathFinder.HeuristicScaleAI.Evaluate(magnitude));
			}
			if (GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(pawn.Map) && GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn))
			{
				return 1.5f;
			}
			return 1f;
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000C6224 File Offset: 0x000C4424
		public PawnPath FindPathNow(IntVec3 start, LocalTargetInfo target, Pawn pawn, PathFinderCostTuning? tuning = null, PathEndMode peMode = PathEndMode.OnCell)
		{
			bool canBashDoors;
			bool flag;
			this.ResolveCanBash(pawn, out canBashDoors, out flag);
			Danger maxDanger = Danger.Deadly;
			TraverseMode mode = TraverseMode.ByPawn;
			bool canBashFences = flag;
			return this.FindPathNow(start, target, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true), tuning, peMode, null);
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000C6258 File Offset: 0x000C4458
		public PawnPath FindPathNow(IntVec3 start, LocalTargetInfo target, TraverseParms traverseParms, PathFinderCostTuning? tuning = null, PathEndMode peMode = PathEndMode.OnCell, PathRequest.IPathGridCustomizer customizer = null)
		{
			if (this.disposed)
			{
				return PawnPath.NotFound;
			}
			PathRequest pathRequest = this.CreateRequest(start, target, null, traverseParms, tuning, peMode, traverseParms.pawn, customizer);
			if (!pathRequest.Validate())
			{
				return PawnPath.NotFound;
			}
			this.ForceCompleteScheduledJobs();
			if (this.mapData.GatherData(new PathRequest[]
			{
				pathRequest
			}))
			{
				this.RecycleGridJobData();
			}
			PathFinder.MapGridRequest key = PathFinder.MapGridRequest.For(pathRequest);
			PathFinder.ScheduledGridJob scheduledGridJob;
			if (!this.gridJobLookup.TryGetValue(key, out scheduledGridJob))
			{
				scheduledGridJob = this.ScheduleGridJob(pathRequest, default(JobHandle));
			}
			PathFinderJob jobData = default(PathFinderJob);
			PathFinder.PathUniqueState item;
			this.GetOrCreatePathJobUniqueState(out item);
			PathFinder.PathJobOutput item2;
			this.GetOrCreatePathJobOutput(out item2);
			scheduledGridJob.handle.Complete();
			this.GetDoorBlockedJob(pathRequest, ref item).Run();
			this.ParameterizePathJob(ref jobData, ref key, ref item2, ref item, ref this.immediateBatch, ref scheduledGridJob, pathRequest);
			this.mapData.ParameterizePathJob(ref jobData);
			jobData.Run<PathFinderJob>();
			if (item2.path.Length == 0)
			{
				this.pathUniqueStates.Enqueue(item);
				this.pathJobOutputs.Enqueue(item2);
				pathRequest.Resolve(null);
				return PawnPath.NotFound;
			}
			PawnPath result = this.EmitPath(item2);
			this.pathUniqueStates.Enqueue(item);
			this.pathJobOutputs.Enqueue(item2);
			return result;
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000C63A4 File Offset: 0x000C45A4
		private void ParameterizeDoorBlockedJob(PathGridDoorsBlockedJob job, PathRequest request, ref PathFinder.PathUniqueState state)
		{
			job.start = request.Start;
			job.dest = request.Target.Cell;
			job.map = this.map;
			job.pawn = request.pawn;
			job.traverseParams = request.TraverseParms;
			job.tuning = request.Tuning;
			this.EnsureDoorsPawnsCached();
			job.doors = this.cachedDoors;
			job.providers = this.cachedProviders;
			job.pawns = this.cachedPawns;
			job.providerCost = state.providers;
			job.blocked = state.blocked;
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000C6444 File Offset: 0x000C4644
		private void EnsureDoorsPawnsCached()
		{
			if (this.cachedTick == GenTicks.TicksGame)
			{
				return;
			}
			this.cachedTick = GenTicks.TicksGame;
			this.cachedProviders.Clear();
			this.cachedDoors.Clear();
			this.cachedPawns.Clear();
			foreach (Thing thing in this.map.listerThings.ThingsInGroup(ThingRequestGroup.CostProvider))
			{
				this.cachedProviders.Add((IPathFindCostProvider)thing);
			}
			this.cachedDoors.AddRange(this.map.listerThings.ThingsInGroup(ThingRequestGroup.Door));
			this.cachedPawns.AddRange(this.map.mapPawns.AllPawnsSpawned);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000C6520 File Offset: 0x000C4720
		private void ParameterizePathJob(ref PathFinderJob job, ref PathFinder.MapGridRequest gridRequest, ref PathFinder.PathJobOutput output, ref PathFinder.PathUniqueState uniqueState, ref PathFinder.JobBatchState batchState, ref PathFinder.ScheduledGridJob scheduledGridJob, PathRequest request)
		{
			using (ProfilerBlock.Scope("PathFinder.Parameterize"))
			{
				job.indices = this.map.cellIndices;
				CellRect destRect;
				PathFinder.MakeDestination(request.Target, request.EndMode, this.map.pathing.For(request.TraverseParms), out destRect, ref uniqueState.excludedDestIndices);
				job.grid = scheduledGridJob.output.grid.AsReadOnly();
				scheduledGridJob.lastAccessed = GenTicks.TicksGame;
				job.start = request.Start;
				job.destCell = request.Target.Cell;
				job.destRect = destRect;
				job.heuristicStrength = PathFinder.DetermineHeuristicStrength(request.TraverseParms.pawn, request.Start, request.Target);
				job.traverseParams = gridRequest.traverseParams;
				if (request.TraverseParms.pawn != null)
				{
					job.moveTicksCardinal = Mathf.RoundToInt(request.TraverseParms.pawn.TicksPerMoveCardinal);
					job.moveTicksDiagonal = Mathf.RoundToInt(request.TraverseParms.pawn.TicksPerMoveDiagonal);
				}
				else
				{
					job.moveTicksCardinal = 18;
					job.moveTicksDiagonal = 18;
				}
				job.calcGrid = batchState.calcGrid;
				job.frontier = batchState.frontier;
				job.path = output.path;
				job.result = output.data;
				job.providerCost = uniqueState.providers.AsReadOnly();
				job.blocked = uniqueState.blocked.AsReadOnly();
				job.excludedRectIndices = uniqueState.excludedDestIndices;
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000C66E0 File Offset: 0x000C48E0
		private static void MakeDestination(LocalTargetInfo target, PathEndMode peMode, PathingContext context, out CellRect rect, ref NativeList<int> excluded)
		{
			excluded.Clear();
			Map map = context.map;
			CellIndices cellIndices = map.cellIndices;
			rect = PathFinder.CalculateDestinationRect(target, peMode, context);
			if (peMode == PathEndMode.Touch && rect.Area != 1)
			{
				IntVec3 b = new IntVec3(1, 0, 0);
				IntVec3 b2 = new IntVec3(0, 0, 1);
				IntVec3 intVec;
				IntVec3 intVec2;
				IntVec3 intVec3;
				IntVec3 intVec4;
				rect.GetInternalCorners(out intVec, out intVec2, out intVec3, out intVec4);
				if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec + b2, intVec + b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec);
					excluded.Add(num);
				}
				if (intVec3.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec3 - b2, intVec3 - b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec3);
					excluded.Add(num);
				}
				if (intVec2.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec2 - b2, intVec2 + b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec2);
					excluded.Add(num);
				}
				if (intVec4.InBounds(map) && !TouchPathEndModeUtility.IsCornerTouchAllowed_NewTemp(intVec4 + b2, intVec4 - b, context, target.Thing))
				{
					int num = cellIndices.CellToIndex(intVec4);
					excluded.Add(num);
				}
			}
			rect = rect.ClipInsideMap(map);
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000C6848 File Offset: 0x000C4A48
		private static CellRect CalculateDestinationRect(LocalTargetInfo dest, PathEndMode peMode, PathingContext context)
		{
			CellRect result;
			if (!dest.HasThing || peMode == PathEndMode.OnCell)
			{
				result = CellRect.SingleCell(dest.Cell);
			}
			else
			{
				result = dest.Thing.OccupiedRect();
			}
			if (peMode == PathEndMode.Touch)
			{
				result = result.ExpandedBy(1);
			}
			return result;
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000C688C File Offset: 0x000C4A8C
		private void ResolveCanBash(Pawn pawn, out bool canBashDoors, out bool canBashFences)
		{
			canBashDoors = (pawn != null && pawn.CurJob != null && pawn.CurJob.canBashDoors);
			canBashFences = (pawn != null && pawn.CurJob != null && pawn.CurJob.canBashFences);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000C68C4 File Offset: 0x000C4AC4
		public void LogPathfinderState()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pathfinder State");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Avaliable resources:");
			stringBuilder.AppendLine(string.Format("Grid outputs: {0}", this.gridJobOutputs.Count));
			stringBuilder.AppendLine(string.Format("Path states: {0}", this.pathUniqueStates.Count));
			stringBuilder.AppendLine(string.Format("Path outputs: {0}", this.pathJobOutputs.Count));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("Current grid results: {0}", this.gridJobLookup.Count));
			foreach (KeyValuePair<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> keyValuePair in this.gridJobLookup)
			{
				PathFinder.MapGridRequest mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob;
				keyValuePair.Deconstruct(out mapGridRequest, out scheduledGridJob);
				PathFinder.MapGridRequest mapGridRequest2 = mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob2 = scheduledGridJob;
				stringBuilder.AppendLine(string.Format("   - {0} ({1:F2}s old, last accessed: {2:F2}s ago)", mapGridRequest2, (GenTicks.TicksGame - scheduledGridJob2.computed).TicksToSeconds(), (GenTicks.TicksGame - scheduledGridJob2.lastAccessed).TicksToSeconds()));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000C6A20 File Offset: 0x000C4C20
		public void LogGridCellResult(IntVec3 cell)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (KeyValuePair<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> keyValuePair in this.gridJobLookup)
			{
				PathFinder.MapGridRequest mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob;
				keyValuePair.Deconstruct(out mapGridRequest, out scheduledGridJob);
				PathFinder.MapGridRequest mapGridRequest2 = mapGridRequest;
				PathFinder.ScheduledGridJob scheduledGridJob2 = scheduledGridJob;
				PathFinder.ScheduledGridJob local = scheduledGridJob2;
				list.Add(new FloatMenuOption(string.Format("{0} ({1:F2}s old)", mapGridRequest2, (GenTicks.TicksGame - scheduledGridJob2.computed).TicksToSeconds()), delegate()
				{
					Log.Message(string.Format("{0}: {1}", cell, local.output.grid[this.map.cellIndices[cell]]));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnDraw()
		{
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnGUI()
		{
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000C6B10 File Offset: 0x000C4D10
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			foreach (PathFinder.ScheduledGridJob scheduledGridJob in this.scheduledGridJobs)
			{
				JobHandle handle = scheduledGridJob.handle;
				handle.Complete();
			}
			foreach (PathFinder.ScheduledPathJob scheduledPathJob in this.scheduledPathJobs)
			{
				JobHandle handle = scheduledPathJob.handle;
				handle.Complete();
			}
			foreach (PathFinder.ScheduledGridJob scheduledGridJob2 in this.scheduledGridJobs)
			{
				PathFinder.GridJobOutput output = scheduledGridJob2.output;
				output.Dispose();
			}
			foreach (PathFinder.ScheduledPathJob scheduledPathJob2 in this.scheduledPathJobs)
			{
				PathFinder.PathUniqueState state = scheduledPathJob2.state;
				state.Dispose();
				PathFinder.PathJobOutput output2 = scheduledPathJob2.output;
				output2.Dispose();
			}
			this.mapData.Dispose();
			foreach (PathFinder.JobBatchState jobBatchState in this.batches)
			{
				jobBatchState.Dispose();
			}
			this.immediateBatch.Dispose();
			foreach (PathFinder.GridJobOutput gridJobOutput in this.gridJobOutputs)
			{
				gridJobOutput.Dispose();
			}
			foreach (PathFinder.PathUniqueState pathUniqueState in this.pathUniqueStates)
			{
				pathUniqueState.Dispose();
			}
			foreach (PathFinder.PathJobOutput pathJobOutput in this.pathJobOutputs)
			{
				pathJobOutput.Dispose();
			}
		}

		// Token: 0x04001EEE RID: 7918
		private readonly Map map;

		// Token: 0x04001EEF RID: 7919
		private readonly PathFinderMapData mapData;

		// Token: 0x04001EF0 RID: 7920
		private readonly List<PathFinder.JobBatchState> batches = new List<PathFinder.JobBatchState>();

		// Token: 0x04001EF1 RID: 7921
		private PathFinder.JobBatchState immediateBatch;

		// Token: 0x04001EF2 RID: 7922
		private readonly List<PathRequest> workQueue = new List<PathRequest>();

		// Token: 0x04001EF3 RID: 7923
		private readonly List<PathFinder.ScheduledPathJob> scheduledPathJobs = new List<PathFinder.ScheduledPathJob>();

		// Token: 0x04001EF4 RID: 7924
		private readonly List<PathFinder.ScheduledGridJob> scheduledGridJobs = new List<PathFinder.ScheduledGridJob>();

		// Token: 0x04001EF5 RID: 7925
		private readonly Queue<PathFinder.PathJobOutput> pathJobOutputs = new Queue<PathFinder.PathJobOutput>();

		// Token: 0x04001EF6 RID: 7926
		private readonly Queue<PathFinder.PathUniqueState> pathUniqueStates = new Queue<PathFinder.PathUniqueState>();

		// Token: 0x04001EF7 RID: 7927
		private readonly Queue<PathFinder.GridJobOutput> gridJobOutputs = new Queue<PathFinder.GridJobOutput>();

		// Token: 0x04001EF8 RID: 7928
		private readonly Dictionary<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob> gridJobLookup = new Dictionary<PathFinder.MapGridRequest, PathFinder.ScheduledGridJob>();

		// Token: 0x04001EF9 RID: 7929
		private readonly List<PathRequest> tmpCurrentWork = new List<PathRequest>();

		// Token: 0x04001EFA RID: 7930
		private bool disposed;

		// Token: 0x04001EFB RID: 7931
		public const int DefaultMoveTicksCardinal = 13;

		// Token: 0x04001EFC RID: 7932
		public const int DefaultMoveTicksDiagonal = 18;

		// Token: 0x04001EFD RID: 7933
		public const float DiagonalMoveFactor = 1.41421f;

		// Token: 0x04001EFE RID: 7934
		private const float HeuristicScaleAnimal = 1.75f;

		// Token: 0x04001EFF RID: 7935
		private static readonly SimpleCurve HeuristicScaleAI = new SimpleCurve
		{
			{
				new CurvePoint(40f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x04001F00 RID: 7936
		private readonly List<IPathFindCostProvider> cachedProviders = new List<IPathFindCostProvider>();

		// Token: 0x04001F01 RID: 7937
		private readonly List<Thing> cachedDoors = new List<Thing>();

		// Token: 0x04001F02 RID: 7938
		private readonly List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x04001F03 RID: 7939
		private int cachedTick;

		// Token: 0x020005B4 RID: 1460
		private struct ScheduledPathJob
		{
			// Token: 0x04001F04 RID: 7940
			public PathRequest request;

			// Token: 0x04001F05 RID: 7941
			public JobHandle handle;

			// Token: 0x04001F06 RID: 7942
			public PathFinder.PathUniqueState state;

			// Token: 0x04001F07 RID: 7943
			public PathFinder.PathJobOutput output;
		}

		// Token: 0x020005B5 RID: 1461
		private struct ScheduledGridJob
		{
			// Token: 0x04001F08 RID: 7944
			public int computed;

			// Token: 0x04001F09 RID: 7945
			public int lastAccessed;

			// Token: 0x04001F0A RID: 7946
			public JobHandle handle;

			// Token: 0x04001F0B RID: 7947
			public PathFinder.GridJobOutput output;
		}

		// Token: 0x020005B6 RID: 1462
		public readonly struct UnmanagedGridTraverseParams : IEquatable<PathFinder.UnmanagedGridTraverseParams>
		{
			// Token: 0x060025CB RID: 9675 RVA: 0x000C6DCC File Offset: 0x000C4FCC
			public UnmanagedGridTraverseParams(TraverseMode mode, bool canBashFences, bool fenceBlocked, bool avoidPersistentDanger, bool avoidDarknessDanger, bool avoidFog, CellRect targetBuildable)
			{
				this.mode = mode;
				this.canBashFences = canBashFences;
				this.fenceBlocked = fenceBlocked;
				this.avoidPersistentDanger = avoidPersistentDanger;
				this.avoidDarknessDanger = avoidDarknessDanger;
				this.avoidFog = avoidFog;
				this.targetBuildable = targetBuildable;
			}

			// Token: 0x060025CC RID: 9676 RVA: 0x000C6E03 File Offset: 0x000C5003
			public static PathFinder.UnmanagedGridTraverseParams For(TraverseParms parms)
			{
				return new PathFinder.UnmanagedGridTraverseParams(parms.mode, parms.canBashFences, parms.fenceBlocked, parms.avoidPersistentDanger, parms.avoidDarknessDanger, parms.avoidFog, parms.targetBuildable);
			}

			// Token: 0x060025CD RID: 9677 RVA: 0x000C6E34 File Offset: 0x000C5034
			public bool Equals(PathFinder.UnmanagedGridTraverseParams other)
			{
				return this.mode == other.mode && this.canBashFences == other.canBashFences && this.fenceBlocked == other.fenceBlocked && this.avoidPersistentDanger == other.avoidPersistentDanger && this.avoidDarknessDanger == other.avoidDarknessDanger && this.avoidFog == other.avoidFog && this.targetBuildable == other.targetBuildable;
			}

			// Token: 0x060025CE RID: 9678 RVA: 0x000C6EA8 File Offset: 0x000C50A8
			public override bool Equals(object obj)
			{
				if (obj is PathFinder.UnmanagedGridTraverseParams)
				{
					PathFinder.UnmanagedGridTraverseParams other = (PathFinder.UnmanagedGridTraverseParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060025CF RID: 9679 RVA: 0x000C6ECD File Offset: 0x000C50CD
			public override int GetHashCode()
			{
				return HashCode.Combine<int, bool, bool, bool, bool, bool, CellRect>((int)this.mode, this.canBashFences, this.fenceBlocked, this.avoidPersistentDanger, this.avoidDarknessDanger, this.avoidFog, this.targetBuildable);
			}

			// Token: 0x04001F0C RID: 7948
			public readonly TraverseMode mode;

			// Token: 0x04001F0D RID: 7949
			public readonly bool canBashFences;

			// Token: 0x04001F0E RID: 7950
			public readonly bool fenceBlocked;

			// Token: 0x04001F0F RID: 7951
			public readonly bool avoidPersistentDanger;

			// Token: 0x04001F10 RID: 7952
			public readonly bool avoidDarknessDanger;

			// Token: 0x04001F11 RID: 7953
			public readonly bool avoidFog;

			// Token: 0x04001F12 RID: 7954
			public readonly CellRect targetBuildable;
		}

		// Token: 0x020005B7 RID: 1463
		public struct MapGridRequest : IEquatable<PathFinder.MapGridRequest>
		{
			// Token: 0x060025D0 RID: 9680 RVA: 0x000C6F00 File Offset: 0x000C5100
			public static PathFinder.MapGridRequest For(PathRequest request)
			{
				AvoidGrid avoidGrid = null;
				if (request.pawn != null)
				{
					request.pawn.TryGetAvoidGrid(out avoidGrid, true);
				}
				return new PathFinder.MapGridRequest
				{
					traverseParams = PathFinder.UnmanagedGridTraverseParams.For(request.TraverseParms),
					tuning = request.Tuning,
					customizer = request.customizer,
					allowedArea = request.area,
					avoidGrid = avoidGrid
				};
			}

			// Token: 0x060025D1 RID: 9681 RVA: 0x000C6F70 File Offset: 0x000C5170
			public bool Equals(PathFinder.MapGridRequest other)
			{
				return this.traverseParams.Equals(other.traverseParams) && this.tuning.Equals(other.tuning) && object.Equals(this.customizer, other.customizer) && object.Equals(this.allowedArea, other.allowedArea) && object.Equals(this.avoidGrid, other.avoidGrid);
			}

			// Token: 0x060025D2 RID: 9682 RVA: 0x000C6FDC File Offset: 0x000C51DC
			public override bool Equals(object obj)
			{
				if (obj is PathFinder.MapGridRequest)
				{
					PathFinder.MapGridRequest other = (PathFinder.MapGridRequest)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060025D3 RID: 9683 RVA: 0x000C7001 File Offset: 0x000C5201
			public override int GetHashCode()
			{
				return HashCode.Combine<PathFinder.UnmanagedGridTraverseParams, PathFinderCostTuning, PathRequest.IPathGridCustomizer, Area, AvoidGrid>(this.traverseParams, this.tuning, this.customizer, this.allowedArea, this.avoidGrid);
			}

			// Token: 0x060025D4 RID: 9684 RVA: 0x000C7028 File Offset: 0x000C5228
			public override string ToString()
			{
				return string.Format("{0}{1}{2}{3}{4}", new object[]
				{
					this.traverseParams.mode,
					this.traverseParams.fenceBlocked ? ", fence blocked" : "",
					this.traverseParams.avoidFog ? ", avoid fog" : "",
					(this.allowedArea != null) ? (", area: " + this.allowedArea.RenamableLabel) : "",
					(this.avoidGrid != null) ? ", avoid grid: true" : ""
				});
			}

			// Token: 0x04001F13 RID: 7955
			public PathFinder.UnmanagedGridTraverseParams traverseParams;

			// Token: 0x04001F14 RID: 7956
			public PathFinderCostTuning tuning;

			// Token: 0x04001F15 RID: 7957
			public PathRequest.IPathGridCustomizer customizer;

			// Token: 0x04001F16 RID: 7958
			public Area allowedArea;

			// Token: 0x04001F17 RID: 7959
			public AvoidGrid avoidGrid;
		}

		// Token: 0x020005B8 RID: 1464
		private struct JobBatchState : IDisposable
		{
			// Token: 0x060025D5 RID: 9685 RVA: 0x000C70D0 File Offset: 0x000C52D0
			public static PathFinder.JobBatchState AllocateNew(int cells)
			{
				PathFinder.JobBatchState result;
				result.calcGrid = new NativeArray<PathFinderJob.CalcNode>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				result.frontier = new NativePriorityQueue<int, float, FloatMinComparer>(cells, default(FloatMinComparer), Allocator.Persistent);
				return result;
			}

			// Token: 0x060025D6 RID: 9686 RVA: 0x000C7104 File Offset: 0x000C5304
			public void Dispose()
			{
				this.calcGrid.Dispose();
				this.frontier.Dispose();
			}

			// Token: 0x04001F18 RID: 7960
			public NativeArray<PathFinderJob.CalcNode> calcGrid;

			// Token: 0x04001F19 RID: 7961
			public NativePriorityQueue<int, float, FloatMinComparer> frontier;
		}

		// Token: 0x020005B9 RID: 1465
		public struct GridJobOutput : IDisposable
		{
			// Token: 0x060025D7 RID: 9687 RVA: 0x000C711C File Offset: 0x000C531C
			public void Initalize(int cells)
			{
				this.grid = new NativeArray<int>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}

			// Token: 0x060025D8 RID: 9688 RVA: 0x000C712C File Offset: 0x000C532C
			public void Dispose()
			{
				this.grid.Dispose();
			}

			// Token: 0x04001F1A RID: 7962
			public NativeArray<int> grid;
		}

		// Token: 0x020005BA RID: 1466
		private struct PathUniqueState : IDisposable
		{
			// Token: 0x060025D9 RID: 9689 RVA: 0x000C7139 File Offset: 0x000C5339
			public void Initialize(int cells)
			{
				this.excludedDestIndices = new NativeList<int>(4, Allocator.Persistent);
				this.providers = new NativeArray<ushort>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.blocked = new NativeArray<bool>(cells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}

			// Token: 0x060025DA RID: 9690 RVA: 0x000C7169 File Offset: 0x000C5369
			public void Dispose()
			{
				this.excludedDestIndices.Dispose();
				this.providers.Dispose();
				this.blocked.Dispose();
			}

			// Token: 0x04001F1B RID: 7963
			public NativeList<int> excludedDestIndices;

			// Token: 0x04001F1C RID: 7964
			public NativeArray<ushort> providers;

			// Token: 0x04001F1D RID: 7965
			public NativeArray<bool> blocked;
		}

		// Token: 0x020005BB RID: 1467
		private struct PathJobOutput : IDisposable
		{
			// Token: 0x060025DB RID: 9691 RVA: 0x000C718C File Offset: 0x000C538C
			public void Initialize(int cells)
			{
				this.path = new NativeList<IntVec3>(cells, Allocator.Persistent);
				this.data = new NativeReference<PathFinderJob.ResultData>(default(PathFinderJob.ResultData), Allocator.Persistent);
			}

			// Token: 0x060025DC RID: 9692 RVA: 0x000C71C5 File Offset: 0x000C53C5
			public void Dispose()
			{
				this.path.Dispose();
				this.data.Dispose();
			}

			// Token: 0x04001F1E RID: 7966
			public NativeList<IntVec3> path;

			// Token: 0x04001F1F RID: 7967
			public NativeReference<PathFinderJob.ResultData> data;
		}
	}
}
