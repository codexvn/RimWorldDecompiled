using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200062F RID: 1583
	public class PawnRenderNodeProperties_Tattoo : PawnRenderNodeProperties
	{
		// Token: 0x06002844 RID: 10308 RVA: 0x000D5671 File Offset: 0x000D3871
		public override void ResolveReferences()
		{
			this.skipFlag = RenderSkipFlagDefOf.Tattoos;
		}
	}
}
