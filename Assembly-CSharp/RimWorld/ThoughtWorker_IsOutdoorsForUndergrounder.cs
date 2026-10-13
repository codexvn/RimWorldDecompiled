using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001891 RID: 6289
	public class ThoughtWorker_IsOutdoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06009013 RID: 36883 RVA: 0x002A8020 File Offset: 0x002A6220
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (p.Position.UsesOutdoorTemperature(p.Map) || !p.Position.Roofed(p.Map));
		}
	}
}
