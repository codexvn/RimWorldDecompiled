using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBB RID: 7867
	public class ComplexLayoutSketch : LayoutSketch
	{
		// Token: 0x0600AE7F RID: 44671 RVA: 0x0031C818 File Offset: 0x0031AA18
		protected override ThingDef GetWallStuff(int roomId)
		{
			if (roomId % 2 != 0)
			{
				return ThingDefOf.Steel;
			}
			return this.wallStuff;
		}
	}
}
