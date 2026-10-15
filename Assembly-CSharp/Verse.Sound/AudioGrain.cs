using System.Collections.Generic;

namespace Verse.Sound;

public abstract class AudioGrain
{
	public bool forceToMono = true;

	public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
}
