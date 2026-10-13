using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200337E RID: 13182
	public interface IInspectPane
	{
		// Token: 0x17003069 RID: 12393
		// (get) Token: 0x06012887 RID: 75911
		// (set) Token: 0x06012888 RID: 75912
		float RecentHeight { get; set; }

		// Token: 0x1700306A RID: 12394
		// (get) Token: 0x06012889 RID: 75913
		// (set) Token: 0x0601288A RID: 75914
		Type OpenTabType { get; set; }

		// Token: 0x1700306B RID: 12395
		// (get) Token: 0x0601288B RID: 75915
		bool AnythingSelected { get; }

		// Token: 0x1700306C RID: 12396
		// (get) Token: 0x0601288C RID: 75916
		IEnumerable<InspectTabBase> CurTabs { get; }

		// Token: 0x1700306D RID: 12397
		// (get) Token: 0x0601288D RID: 75917
		bool ShouldShowSelectNextInCellButton { get; }

		// Token: 0x1700306E RID: 12398
		// (get) Token: 0x0601288E RID: 75918
		bool ShouldShowPaneContents { get; }

		// Token: 0x1700306F RID: 12399
		// (get) Token: 0x0601288F RID: 75919
		float PaneTopY { get; }

		// Token: 0x06012890 RID: 75920
		string GetLabel(Rect rect);

		// Token: 0x06012891 RID: 75921
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		// Token: 0x06012892 RID: 75922
		void SelectNextInCell();

		// Token: 0x06012893 RID: 75923
		void DoPaneContents(Rect rect);

		// Token: 0x06012894 RID: 75924
		void CloseOpenTab();

		// Token: 0x06012895 RID: 75925
		void Reset();
	}
}
