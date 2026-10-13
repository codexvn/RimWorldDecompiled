using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200070F RID: 1807
	public class HediffCompProperties_ThoughtSetter : HediffCompProperties
	{
		// Token: 0x06002C26 RID: 11302 RVA: 0x000E5945 File Offset: 0x000E3B45
		public HediffCompProperties_ThoughtSetter()
		{
			this.compClass = typeof(HediffComp_ThoughtSetter);
		}

		// Token: 0x0400237A RID: 9082
		public ThoughtDef thought;

		// Token: 0x0400237B RID: 9083
		public int moodOffset;

		// Token: 0x0400237C RID: 9084
		public FloatRange moodOffsetRange = FloatRange.Zero;
	}
}
