using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DAD RID: 11693
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		// Token: 0x06010711 RID: 67345 RVA: 0x004BE57B File Offset: 0x004BC77B
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		// Token: 0x0400AB7F RID: 43903
		public SoundDef sound;

		// Token: 0x0400AB80 RID: 43904
		public bool onlyWhenKilled;

		// Token: 0x0400AB81 RID: 43905
		public bool ignoreOnVanish = true;
	}
}
