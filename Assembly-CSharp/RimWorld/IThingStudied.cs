using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D16 RID: 7446
	public interface IThingStudied
	{
		// Token: 0x0600A68A RID: 42634
		void OnStudied(Pawn studier, float amount, KnowledgeCategoryDef category = null);
	}
}
