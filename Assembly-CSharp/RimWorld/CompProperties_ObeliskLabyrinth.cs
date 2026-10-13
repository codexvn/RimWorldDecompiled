using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D8A RID: 11658
	public class CompProperties_ObeliskLabyrinth : CompProperties_Interactable
	{
		// Token: 0x06010645 RID: 67141 RVA: 0x004BADAA File Offset: 0x004B8FAA
		public CompProperties_ObeliskLabyrinth()
		{
			this.compClass = typeof(CompObelisk_Labyrinth);
		}

		// Token: 0x0400AB0D RID: 43789
		[MustTranslate]
		public string messageActivating;
	}
}
