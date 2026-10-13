using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7D RID: 8061
	internal struct QueuedCellEffecter
	{
		// Token: 0x0600B228 RID: 45608 RVA: 0x0032EC09 File Offset: 0x0032CE09
		public QueuedCellEffecter(EffecterDef edef, IntVec3 cell, int tick)
		{
			this.effecterDef = edef;
			this.tick = tick;
			this.cell = cell;
		}

		// Token: 0x040079D1 RID: 31185
		public EffecterDef effecterDef;

		// Token: 0x040079D2 RID: 31186
		public int tick;

		// Token: 0x040079D3 RID: 31187
		public IntVec3 cell;
	}
}
