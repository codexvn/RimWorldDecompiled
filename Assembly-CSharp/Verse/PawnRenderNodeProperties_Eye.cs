using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200062D RID: 1581
	public class PawnRenderNodeProperties_Eye : PawnRenderNodeProperties
	{
		// Token: 0x06002841 RID: 10305 RVA: 0x000D55B4 File Offset: 0x000D37B4
		public PawnRenderNodeProperties_Eye()
		{
			this.visibleFacing = new List<Rot4>
			{
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			this.workerClass = typeof(PawnRenderNodeWorker_Eye);
			this.nodeClass = typeof(PawnRenderNode_AttachmentHead);
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000D5613 File Offset: 0x000D3813
		public override void ResolveReferences()
		{
			this.skipFlag = RenderSkipFlagDefOf.Eyes;
		}
	}
}
