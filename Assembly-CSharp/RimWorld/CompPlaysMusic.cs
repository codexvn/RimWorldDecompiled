using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F4B RID: 12107
	public abstract class CompPlaysMusic : ThingComp
	{
		// Token: 0x17002C27 RID: 11303
		// (get) Token: 0x0601112E RID: 69934
		public abstract bool Playing { get; }

		// Token: 0x17002C28 RID: 11304
		// (get) Token: 0x0601112F RID: 69935
		public abstract FloatRange SoundRange { get; }
	}
}
