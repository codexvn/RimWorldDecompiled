using System;

namespace Verse
{
	// Token: 0x02000C17 RID: 3095
	internal static class DraggableResultUtility
	{
		// Token: 0x06004F81 RID: 20353 RVA: 0x00198906 File Offset: 0x00196B06
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
