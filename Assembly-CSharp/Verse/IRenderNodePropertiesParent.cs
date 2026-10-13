using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005F6 RID: 1526
	public interface IRenderNodePropertiesParent
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002720 RID: 10016
		bool HasDefinedGraphicProperties { get; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002721 RID: 10017
		List<PawnRenderNodeProperties> RenderNodeProperties { get; }
	}
}
