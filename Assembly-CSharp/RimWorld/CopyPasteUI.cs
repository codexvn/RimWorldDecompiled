using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200307A RID: 12410
	public static class CopyPasteUI
	{
		// Token: 0x060116C2 RID: 71362 RVA: 0x0050197C File Offset: 0x004FFB7C
		public static void DoCopyPasteButtons(Rect rect, Action copyAction, Action pasteAction)
		{
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, TexButton.Copy, true, null))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect2, "Copy");
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, TexButton.Paste, true, null))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "Paste");
			}
		}

		// Token: 0x0400B482 RID: 46210
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x0400B483 RID: 46211
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x0400B484 RID: 46212
		public const float CopyPasteColumnWidth = 36f;
	}
}
