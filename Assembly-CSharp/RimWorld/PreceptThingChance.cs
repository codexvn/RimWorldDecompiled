using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB8 RID: 6840
	public struct PreceptThingChance
	{
		// Token: 0x06009930 RID: 39216 RVA: 0x002CA170 File Offset: 0x002C8370
		public static implicit operator PreceptThingChance(PreceptThingChanceClass c)
		{
			return new PreceptThingChance
			{
				chance = c.chance,
				def = c.def
			};
		}

		// Token: 0x0400682C RID: 26668
		public ThingDef def;

		// Token: 0x0400682D RID: 26669
		public float chance;
	}
}
