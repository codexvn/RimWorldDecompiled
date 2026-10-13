using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C44 RID: 3140
	public interface IWindowDrawing
	{
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600503A RID: 20538
		GUIStyle EmptyStyle { get; }

		// Token: 0x0600503B RID: 20539
		void DoWindowBackground(Rect rect);

		// Token: 0x0600503C RID: 20540
		bool DoCloseButton(Rect rect, string text);

		// Token: 0x0600503D RID: 20541
		bool DoClostButtonSmall(Rect rect);

		// Token: 0x0600503E RID: 20542
		void BeginGroup(Rect rect);

		// Token: 0x0600503F RID: 20543
		void EndGroup();

		// Token: 0x06005040 RID: 20544
		void DoGrayOut(Rect rect);
	}
}
