using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BE4 RID: 3044
	public static class DragAndDropWidget
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x0018BFA1 File Offset: 0x0018A1A1
		public static bool Dragging
		{
			get
			{
				return DragAndDropWidget.dragBegun;
			}
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x0018BFA8 File Offset: 0x0018A1A8
		public static void DragAndDropWidgetOnGUI_BeforeWindowStack()
		{
			if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0 && DragAndDropWidget.draggingDraggable < DragAndDropWidget.draggables.Count)
			{
				int groupID = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].groupID;
				if (groupID >= 0 && groupID < DragAndDropWidget.groups.Count && DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI(DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context, DragAndDropWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x0018C03C File Offset: 0x0018A23C
		public static void DragAndDropWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (DragAndDropWidget.clicked)
				{
					DragAndDropWidget.StopDragging();
					for (int i = 0; i < DragAndDropWidget.draggables.Count; i++)
					{
						if (DragAndDropWidget.draggables[i].rect == DragAndDropWidget.clickedInRect)
						{
							DragAndDropWidget.draggingDraggable = i;
							Action onStartDragging = DragAndDropWidget.draggables[i].onStartDragging;
							if (onStartDragging != null)
							{
								onStartDragging();
							}
							DragAndDropWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					DragAndDropWidget.mouseIsDown = true;
					DragAndDropWidget.clicked = false;
				}
				if (DragAndDropWidget.draggingDraggable >= DragAndDropWidget.draggables.Count)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.draggables.Count != DragAndDropWidget.lastFrameDraggableCount)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.released)
				{
					DragAndDropWidget.released = false;
					if (!DragAndDropWidget.dragBegun && DragAndDropWidget.mouseIsDown)
					{
						foreach (DragAndDropWidget.DraggableInstance draggableInstance in DragAndDropWidget.draggables)
						{
							Rect absRect = draggableInstance.absRect;
							if (absRect.Contains(Event.current.mousePosition) && draggableInstance.clickHandler != null)
							{
								draggableInstance.clickHandler();
							}
						}
					}
					DragAndDropWidget.mouseIsDown = false;
					if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0)
					{
						DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable];
						DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
						for (int j = DragAndDropWidget.dropAreas.Count - 1; j >= 0; j--)
						{
							DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[j];
							if (draggableInstance2.groupID == dropAreaInstance2.groupID && dropAreaInstance2.absRect.Contains(Event.current.mousePosition))
							{
								dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
							}
						}
						if (dropAreaInstance != null)
						{
							Action<object> onDrop = dropAreaInstance.Value.onDrop;
							if (onDrop != null)
							{
								onDrop(draggableInstance2.context);
							}
						}
						else
						{
							SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
						}
					}
					DragAndDropWidget.StopDragging();
				}
				DragAndDropWidget.lastFrameDraggableCount = DragAndDropWidget.draggables.Count;
				DragAndDropWidget.groups.Clear();
				DragAndDropWidget.draggables.Clear();
				DragAndDropWidget.dropAreas.Clear();
			}
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0018C290 File Offset: 0x0018A490
		public static int NewGroup(Action<object, Vector2> extraDraggedItemOnGUI = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			DragAndDropWidget.DraggableGroup item = default(DragAndDropWidget.DraggableGroup);
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			DragAndDropWidget.groups.Add(item);
			return DragAndDropWidget.groups.Count - 1;
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x0018C2D4 File Offset: 0x0018A4D4
		public static bool Draggable(int groupID, Rect rect, object context, Action clickHandler = null, Action onStartDragging = null)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DraggableInstance item = default(DragAndDropWidget.DraggableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.context = context;
				item.clickHandler = clickHandler;
				item.onStartDragging = onStartDragging;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				DragAndDropWidget.draggables.Add(item);
				int num = DragAndDropWidget.draggables.Count - 1;
				if (DragAndDropWidget.draggingDraggable != -1 && (DragAndDropWidget.dragBegun || Vector2.Distance(DragAndDropWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!DragAndDropWidget.dragBegun)
					{
						SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						DragAndDropWidget.dragBegun = true;
					}
					if (DragAndDropWidget.draggingDraggable == num)
					{
						GUI.color = DragAndDropWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
				}
				return DragAndDropWidget.draggingDraggable == num && DragAndDropWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragAndDropWidget.clicked = true;
				DragAndDropWidget.clickedAt = Event.current.mousePosition;
				DragAndDropWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x0018C41C File Offset: 0x0018A61C
		public static void DropArea(int groupID, Rect rect, Action<object> onDrop, object context)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DropAreaInstance item = default(DragAndDropWidget.DropAreaInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.onDrop = onDrop;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				item.context = context;
				DragAndDropWidget.dropAreas.Add(item);
			}
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0018C488 File Offset: 0x0018A688
		public static object CurrentlyDraggedDraggable()
		{
			if (!DragAndDropWidget.dragBegun || DragAndDropWidget.draggingDraggable < 0)
			{
				return null;
			}
			return DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context;
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0018C4B0 File Offset: 0x0018A6B0
		public static object HoveringDropArea(int groupID)
		{
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(Event.current.mousePosition))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return dropAreaInstance.Value.context;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0018C528 File Offset: 0x0018A728
		public static Rect? HoveringDropAreaRect(int groupID, Vector3? mousePos = null)
		{
			Vector3 point = mousePos ?? Event.current.mousePosition;
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(point))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return new Rect?(dropAreaInstance.GetValueOrDefault().rect);
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0018C5CC File Offset: 0x0018A7CC
		public static object DraggableAt(int groupID, Vector3 mousePos)
		{
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID && draggableInstance2.rect.Contains(mousePos))
				{
					draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0018C63C File Offset: 0x0018A83C
		private static object GetDraggable(int groupID, Vector3 mousePosAbs, int direction)
		{
			float num = float.PositiveInfinity;
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID)
				{
					Rect absRect = draggableInstance2.absRect;
					if (mousePosAbs.y >= absRect.yMin && mousePosAbs.y <= absRect.yMax)
					{
						float num2 = (mousePosAbs.x - absRect.xMax) * (float)direction;
						if (num2 >= 0f && num2 < num)
						{
							num = num2;
							draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
						}
					}
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0018C6EC File Offset: 0x0018A8EC
		public static object GetDraggableBefore(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, 1);
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x0018C6F6 File Offset: 0x0018A8F6
		public static object GetDraggableAfter(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, -1);
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0018C700 File Offset: 0x0018A900
		private static void StopDragging()
		{
			DragAndDropWidget.draggingDraggable = -1;
			DragAndDropWidget.dragStartPos = default(Vector2);
			DragAndDropWidget.dragBegun = false;
		}

		// Token: 0x04003748 RID: 14152
		private static List<DragAndDropWidget.DraggableGroup> groups = new List<DragAndDropWidget.DraggableGroup>();

		// Token: 0x04003749 RID: 14153
		private static List<DragAndDropWidget.DropAreaInstance> dropAreas = new List<DragAndDropWidget.DropAreaInstance>();

		// Token: 0x0400374A RID: 14154
		private static List<DragAndDropWidget.DraggableInstance> draggables = new List<DragAndDropWidget.DraggableInstance>();

		// Token: 0x0400374B RID: 14155
		private static int draggingDraggable = -1;

		// Token: 0x0400374C RID: 14156
		private static Vector2 dragStartPos;

		// Token: 0x0400374D RID: 14157
		private static bool mouseIsDown;

		// Token: 0x0400374E RID: 14158
		private static bool clicked;

		// Token: 0x0400374F RID: 14159
		private static bool released;

		// Token: 0x04003750 RID: 14160
		private static bool dragBegun;

		// Token: 0x04003751 RID: 14161
		private static Vector2 clickedAt;

		// Token: 0x04003752 RID: 14162
		private static Rect clickedInRect;

		// Token: 0x04003753 RID: 14163
		private static int lastFrameDraggableCount = -1;

		// Token: 0x04003754 RID: 14164
		private const float MinMouseMoveToHighlightDraggable = 5f;

		// Token: 0x04003755 RID: 14165
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x04003756 RID: 14166
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003757 RID: 14167
		private const float LineWidth = 2f;

		// Token: 0x02000BE5 RID: 3045
		private struct DraggableGroup
		{
			// Token: 0x04003758 RID: 14168
			public Action<object, Vector2> extraDraggedItemOnGUI;
		}

		// Token: 0x02000BE6 RID: 3046
		private struct DropAreaInstance
		{
			// Token: 0x04003759 RID: 14169
			public int groupID;

			// Token: 0x0400375A RID: 14170
			public Rect rect;

			// Token: 0x0400375B RID: 14171
			public Rect absRect;

			// Token: 0x0400375C RID: 14172
			public Action<object> onDrop;

			// Token: 0x0400375D RID: 14173
			public object context;
		}

		// Token: 0x02000BE7 RID: 3047
		private struct DraggableInstance
		{
			// Token: 0x0400375E RID: 14174
			public int groupID;

			// Token: 0x0400375F RID: 14175
			public Rect rect;

			// Token: 0x04003760 RID: 14176
			public Rect absRect;

			// Token: 0x04003761 RID: 14177
			public object context;

			// Token: 0x04003762 RID: 14178
			public Action clickHandler;

			// Token: 0x04003763 RID: 14179
			public Action onStartDragging;
		}
	}
}
