using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200381B RID: 14363
	[StaticConstructorOnStartup]
	public class PlaceWorker_RitualFocusNoArrow : PlaceWorker_RitualFocus
	{
		// Token: 0x170032A4 RID: 12964
		// (get) Token: 0x06013DDA RID: 81370 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool UseArrow
		{
			get
			{
				return false;
			}
		}
	}
}
