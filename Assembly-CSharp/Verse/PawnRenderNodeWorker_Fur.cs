using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200063A RID: 1594
	public class PawnRenderNodeWorker_Fur : PawnRenderNodeWorker_Body
	{
		// Token: 0x06002859 RID: 10329 RVA: 0x000D5BA3 File Offset: 0x000D3DA3
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			Pawn_StoryTracker story = parms.pawn.story;
			return ((story != null) ? story.furDef : null) != null;
		}
	}
}
