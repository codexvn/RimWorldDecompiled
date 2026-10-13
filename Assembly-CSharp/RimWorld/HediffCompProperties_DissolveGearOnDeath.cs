using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003852 RID: 14418
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		// Token: 0x06013F0A RID: 81674 RVA: 0x005EA8B4 File Offset: 0x005E8AB4
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		// Token: 0x0400DC31 RID: 56369
		public FleckDef fleck;

		// Token: 0x0400DC32 RID: 56370
		public ThingDef mote;

		// Token: 0x0400DC33 RID: 56371
		public int moteCount = 3;

		// Token: 0x0400DC34 RID: 56372
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x0400DC35 RID: 56373
		public ThingDef filth;

		// Token: 0x0400DC36 RID: 56374
		public int filthCount = 4;

		// Token: 0x0400DC37 RID: 56375
		public HediffDef injuryCreatedOnDeath;

		// Token: 0x0400DC38 RID: 56376
		public IntRange injuryCount;

		// Token: 0x0400DC39 RID: 56377
		public SoundDef sound;
	}
}
