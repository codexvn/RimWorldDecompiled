using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000632 RID: 1586
	public class PawnRenderNodeProperties_NociosphereSegment : PawnRenderNodeProperties
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x000D5770 File Offset: 0x000D3970
		public PawnRenderNodeProperties_NociosphereSegment()
		{
			this.nodeClass = typeof(PawnRenderNode_NociosphereSegment);
			this.workerClass = typeof(PawnRenderNodeWorker_NociosphereSegment);
			this.drawSize = new Vector2(1.35f, 1.35f);
		}

		// Token: 0x04002176 RID: 8566
		[NoTranslate]
		public string maskPath;

		// Token: 0x04002177 RID: 8567
		public Vector2 offset;
	}
}
