using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FB7 RID: 4023
	public class Pathing : IDisposable
	{
		// Token: 0x060062D1 RID: 25297 RVA: 0x001F267C File Offset: 0x001F087C
		public Pathing(Map map)
		{
			this.map = map;
			foreach (PathGridDef pathGridDef in DefDatabase<PathGridDef>.AllDefsListForReading)
			{
				if (!(pathGridDef.workerType == null))
				{
					PathGrid pathGrid = (PathGrid)Activator.CreateInstance(pathGridDef.workerType, new object[]
					{
						map,
						pathGridDef
					});
					PathingContext pathingContext = this.contexts[pathGridDef] = new PathingContext(map, pathGrid);
					if (pathGridDef == PathGridDefOf.Normal)
					{
						this.normal = pathingContext;
					}
					else if (pathGridDef == PathGridDefOf.FenceBlocked)
					{
						this.fenceBlocked = pathingContext;
					}
					else if (pathGridDef == PathGridDefOf.Flying)
					{
						this.flying = pathingContext;
					}
				}
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060062D2 RID: 25298 RVA: 0x001F275C File Offset: 0x001F095C
		public PathingContext Normal
		{
			get
			{
				return this.normal;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060062D3 RID: 25299 RVA: 0x001F2764 File Offset: 0x001F0964
		public PathingContext FenceBlocked
		{
			get
			{
				return this.fenceBlocked;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060062D4 RID: 25300 RVA: 0x001F276C File Offset: 0x001F096C
		public PathingContext Flying
		{
			get
			{
				return this.flying;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060062D5 RID: 25301 RVA: 0x001F2774 File Offset: 0x001F0974
		public bool IncrementalDirtyingDisabled
		{
			get
			{
				return this.incrementalDirtyingDisabled;
			}
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x001F277C File Offset: 0x001F097C
		public PathingContext Get(PathGridDef def)
		{
			return this.contexts[def];
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x001F278A File Offset: 0x001F098A
		public PathingContext For(TraverseParms parms)
		{
			if (parms.fenceBlocked && !parms.canBashFences)
			{
				return this.fenceBlocked;
			}
			return this.normal;
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x001F27A9 File Offset: 0x001F09A9
		public PathingContext For(Pawn pawn)
		{
			if (pawn == null)
			{
				return this.normal;
			}
			return pawn.GetPathContext(this) ?? this.normal;
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x001F27C8 File Offset: 0x001F09C8
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x001F282C File Offset: 0x001F0A2C
		public void RecalculatePerceivedPathCostUnderThing(Thing thing)
		{
			this.map.pathFinder.MapData.Notify_CellDelta(thing.OccupiedRect());
			ThingDef def = thing.def;
			if (def.pathCost == 0 && def.passability != Traversability.Impassable)
			{
				return;
			}
			if (thing.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(thing.Position);
				return;
			}
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x001F28D8 File Offset: 0x001F0AD8
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.RecalculatePerceivedPathCostAt(c, ref flag);
			}
			this.map.events.Notify_PathCostRecalculated(c);
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x001F2954 File Offset: 0x001F0B54
		public Pathing.DisableDirtyingScope DisableIncrementalScope()
		{
			return new Pathing.DisableDirtyingScope(this);
		}

		// Token: 0x060062DD RID: 25309 RVA: 0x001F295C File Offset: 0x001F0B5C
		public void DisableIncrementalDirtying()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.DisableIncrementalDirtying();
			}
			this.incrementalDirtyingDisabled = true;
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x001F29C8 File Offset: 0x001F0BC8
		public void ReEnableIncrementalDirtying()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.ReEnableIncrementalDirtying();
			}
			this.incrementalDirtyingDisabled = false;
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x001F2A34 File Offset: 0x001F0C34
		public void Dispose()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.Dispose();
			}
		}

		// Token: 0x0400469C RID: 18076
		private readonly Map map;

		// Token: 0x0400469D RID: 18077
		private readonly Dictionary<PathGridDef, PathingContext> contexts = new Dictionary<PathGridDef, PathingContext>();

		// Token: 0x0400469E RID: 18078
		private bool incrementalDirtyingDisabled;

		// Token: 0x0400469F RID: 18079
		private readonly PathingContext normal;

		// Token: 0x040046A0 RID: 18080
		private readonly PathingContext fenceBlocked;

		// Token: 0x040046A1 RID: 18081
		private readonly PathingContext flying;

		// Token: 0x02000FB8 RID: 4024
		public class DisableDirtyingScope : IDisposable
		{
			// Token: 0x060062E0 RID: 25312 RVA: 0x001F2A94 File Offset: 0x001F0C94
			public DisableDirtyingScope(Pathing pathing)
			{
				this.pathing = pathing;
				pathing.DisableIncrementalDirtying();
			}

			// Token: 0x060062E1 RID: 25313 RVA: 0x001F2AA9 File Offset: 0x001F0CA9
			public void Dispose()
			{
				this.pathing.ReEnableIncrementalDirtying();
			}

			// Token: 0x040046A2 RID: 18082
			private readonly Pathing pathing;
		}
	}
}
