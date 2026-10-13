using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BE9 RID: 3049
	public static class DragSliderManager
	{
		// Token: 0x06004DF3 RID: 19955 RVA: 0x0018C78F File Offset: 0x0018A98F
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x0018C797 File Offset: 0x0018A997
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				return true;
			}
			return false;
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0018C7D6 File Offset: 0x0018A9D6
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0018C7F9 File Offset: 0x0018A9F9
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0018C80C File Offset: 0x0018AA0C
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if (DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0018C85A File Offset: 0x0018AA5A
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x04003764 RID: 14180
		private static bool dragging = false;

		// Token: 0x04003765 RID: 14181
		private static float rootX;

		// Token: 0x04003766 RID: 14182
		private static float lastRateFactor = 1f;

		// Token: 0x04003767 RID: 14183
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04003768 RID: 14184
		private static DragSliderCallback completedMethod;
	}
}
