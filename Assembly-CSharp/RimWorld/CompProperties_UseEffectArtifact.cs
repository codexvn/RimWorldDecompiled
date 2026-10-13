using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F55 RID: 12117
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		// Token: 0x06011152 RID: 69970 RVA: 0x004E7D99 File Offset: 0x004E5F99
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		// Token: 0x0400B15B RID: 45403
		public SoundDef sound;
	}
}
