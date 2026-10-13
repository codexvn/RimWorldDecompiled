using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EE6 RID: 12006
	public class CompProperties_EntityHolderPlatform : CompProperties_EntityHolder
	{
		// Token: 0x06010F2F RID: 69423 RVA: 0x004E08A8 File Offset: 0x004DEAA8
		public CompProperties_EntityHolderPlatform()
		{
			this.compClass = typeof(CompEntityHolderPlatform);
		}

		// Token: 0x0400B03C RID: 45116
		[NoTranslate]
		public string untetheredGraphicTexPath;

		// Token: 0x0400B03D RID: 45117
		[NoTranslate]
		public string tilingChainTexPath;

		// Token: 0x0400B03E RID: 45118
		[NoTranslate]
		public string baseChainFastenerTexPath;

		// Token: 0x0400B03F RID: 45119
		[NoTranslate]
		public string targetChainFastenerTexPath;

		// Token: 0x0400B040 RID: 45120
		public SoundDef entityLungeSoundHi;

		// Token: 0x0400B041 RID: 45121
		public SoundDef entityLungeSoundLow;
	}
}
