using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C63 RID: 11363
	public class CompProperties_DisruptorFlarePack : CompProperties_AIUSablePack
	{
		// Token: 0x0600FED1 RID: 65233 RVA: 0x0049DACF File Offset: 0x0049BCCF
		public CompProperties_DisruptorFlarePack()
		{
			this.compClass = typeof(CompDisruptorFlarePack);
		}

		// Token: 0x0400A6EA RID: 42730
		public float fieldRadius;

		// Token: 0x0400A6EB RID: 42731
		public float durationSeconds;
	}
}
