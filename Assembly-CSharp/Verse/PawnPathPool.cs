using System.Collections.Generic;
using Verse.AI;

namespace Verse;

public class PawnPathPool
{
	private readonly Map map;

	private int pathsCreated;

	private readonly Stack<PawnPath> paths = new Stack<PawnPath>(64);

	public PawnPathPool(Map map)
	{
		this.map = map;
	}

	public PawnPath GetPath()
	{
		if (!paths.TryPop(out var result))
		{
			result = new PawnPath();
			result.pool = this;
			int num = map.mapPawns.AllPawnsSpawnedCount * 2 + 1 + 4;
			if (++pathsCreated > num)
			{
				Log.ErrorOnce($"Leak suspected in object pool for {"PawnPath"}s, created: {pathsCreated}, expected less than {num}. Dispose them so they can be reused by the {"PawnPathPool"}.", 664788);
			}
		}
		return result;
	}

	public void ReturnPath(PawnPath path)
	{
		paths.Push(path);
	}
}
