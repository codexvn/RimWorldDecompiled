using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200349A RID: 13466
	public interface IPawnRoleSelectionWidget
	{
		// Token: 0x06012EFB RID: 77563
		void DrawPawnList(Rect listRectPawns);

		// Token: 0x06012EFC RID: 77564
		void WindowUpdate();
	}
}
