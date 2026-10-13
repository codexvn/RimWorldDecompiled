using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D44 RID: 3396
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06005666 RID: 22118 RVA: 0x001BE235 File Offset: 0x001BC435
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x001BE247 File Offset: 0x001BC447
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04003EE5 RID: 16101
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003EE6 RID: 16102
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
