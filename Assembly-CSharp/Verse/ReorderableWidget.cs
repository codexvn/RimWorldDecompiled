using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C02 RID: 3074
	public static class ReorderableWidget
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06004E7B RID: 20091 RVA: 0x0018F328 File Offset: 0x0018D528
		public static bool Dragging
		{
			get
			{
				return ReorderableWidget.dragBegun;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06004E7C RID: 20092 RVA: 0x0018F32F File Offset: 0x0018D52F
		public static int GetDraggedIndex
		{
			get
			{
				return ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06004E7D RID: 20093 RVA: 0x0018F33B File Offset: 0x0018D53B
		public static int GetDraggedFromGroupID
		{
			get
			{
				return ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			}
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0018F354 File Offset: 0x0018D554
		public static void ReorderableWidgetOnGUI_BeforeWindowStack()
		{
			if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.draggingReorderable < ReorderableWidget.reorderables.Count)
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					ReorderableWidget.groups[groupID].extraDraggedItemOnGUI(ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable), ReorderableWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0018F3E0 File Offset: 0x0018D5E0
		public static void ReorderableWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.StopDragging();
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.groupClicked && ReorderableWidget.reorderables[i].rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.StopDragging();
				}
				if (ReorderableWidget.reorderables.Count != ReorderableWidget.lastFrameReorderableCount)
				{
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastInsertNear = ReorderableWidget.CurrentInsertNear(out ReorderableWidget.lastInsertNearLeft);
				ReorderableWidget.hoveredGroup = -1;
				for (int j = 0; j < ReorderableWidget.groups.Count; j++)
				{
					if (ReorderableWidget.groups[j].absRect.Contains(Event.current.mousePosition))
					{
						ReorderableWidget.hoveredGroup = j;
						if (ReorderableWidget.lastInsertNear >= 0 && ReorderableWidget.AreInMultiGroup(j, ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID) && ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID != j)
						{
							ReorderableWidget.lastInsertNear = ReorderableWidget.FindLastReorderableIndexWithinGroup(j);
							ReorderableWidget.lastInsertNearLeft = (ReorderableWidget.lastInsertNear < 0);
						}
					}
				}
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0)
					{
						int indexWithinGroup = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
						int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
						int num;
						if (ReorderableWidget.lastInsertNear == ReorderableWidget.draggingReorderable)
						{
							num = indexWithinGroup;
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear);
						}
						else
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear) + 1;
						}
						int num2 = -1;
						if (ReorderableWidget.lastInsertNear >= 0)
						{
							num2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, ReorderableWidget.hoveredGroup) && ReorderableWidget.hoveredGroup >= 0 && ReorderableWidget.hoveredGroup != num2)
						{
							num2 = ReorderableWidget.hoveredGroup;
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.FindLastReorderableIndexWithinGroup(num2)) + 1;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, num2))
						{
							ReorderableWidget.GetMultiGroupByGroupID(groupID).Value.reorderedAction(indexWithinGroup, groupID, num, num2);
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
						}
						else if (num >= 0 && num != indexWithinGroup && num != indexWithinGroup + 1)
						{
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
							try
							{
								ReorderableWidget.groups[ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID].reorderedAction(indexWithinGroup, num);
							}
							catch (Exception ex)
							{
								string[] array = new string[6];
								array[0] = "Could not reorder elements (from ";
								array[1] = indexWithinGroup.ToString();
								array[2] = " to ";
								array[3] = num.ToString();
								array[4] = "): ";
								int num3 = 5;
								Exception ex2 = ex;
								array[num3] = ((ex2 != null) ? ex2.ToString() : null);
								Log.Error(string.Concat(array));
							}
						}
					}
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastFrameReorderableCount = ReorderableWidget.reorderables.Count;
				ReorderableWidget.multiGroups.Clear();
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0018F74C File Offset: 0x0018D94C
		public static int NewGroup(Action<int, int> reorderedAction, ReorderableDirection direction, Rect rect, float drawLineExactlyBetween_space = -1f, Action<int, Vector2> extraDraggedItemOnGUI = null, bool playSoundOnStartReorder = true)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			int count = ReorderableWidget.groups.Count;
			ReorderableWidget.ReorderableGroup item = default(ReorderableWidget.ReorderableGroup);
			item.reorderedAction = reorderedAction;
			item.direction = direction;
			item.absRect = new Rect(UI.GUIToScreenPoint(Vector2.zero), rect.size);
			item.drawLineExactlyBetween_space = drawLineExactlyBetween_space;
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			item.playSoundOnStartReorder = playSoundOnStartReorder;
			ReorderableWidget.groups.Add(item);
			if (ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.hoveredGroup == count && ReorderableWidget.lastInsertNear == -1)
			{
				ReorderableWidget.DrawLine(count, new Rect(0f, 0f, rect.width, rect.height));
			}
			return count;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0018F80C File Offset: 0x0018DA0C
		public static int NewMultiGroup(List<int> includedGroups, Action<int, int, int, int> reorderedAction)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			ReorderableWidget.ReorderableMultiGroup item = default(ReorderableWidget.ReorderableMultiGroup);
			item.includedGroups = includedGroups;
			item.reorderedAction = reorderedAction;
			ReorderableWidget.multiGroups.Add(item);
			return ReorderableWidget.multiGroups.Count - 1;
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0018F858 File Offset: 0x0018DA58
		public static bool Reorderable(int groupID, Rect rect, bool useRightButton = false, bool highlightDragged = true)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableWidget.ReorderableInstance item = default(ReorderableWidget.ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && (ReorderableWidget.dragBegun || (Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5f && ReorderableWidget.groupClicked == groupID)))
				{
					if (!ReorderableWidget.dragBegun)
					{
						if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].playSoundOnStartReorder)
						{
							SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						}
						ReorderableWidget.dragBegun = true;
					}
					if (highlightDragged && ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertNear == num && groupID >= 0 && groupID < ReorderableWidget.groups.Count)
					{
						Rect rect2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].rect;
						ReorderableWidget.DrawLine(groupID, rect2);
					}
				}
				return ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && ((useRightButton && Event.current.button == 1) || (!useRightButton && Event.current.button == 0)) && Mouse.IsOver(rect))
			{
				ReorderableWidget.clicked = true;
				ReorderableWidget.clickedAt = Event.current.mousePosition;
				ReorderableWidget.groupClicked = groupID;
				ReorderableWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0018FA0C File Offset: 0x0018DC0C
		private static int CurrentInsertNear(out bool toTheLeft)
		{
			toTheLeft = false;
			if (ReorderableWidget.draggingReorderable < 0)
			{
				return -1;
			}
			int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				Log.ErrorOnce("Reorderable used invalid group.", 1968375560);
				return -1;
			}
			int num = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
				if ((reorderableInstance.groupID == groupID || ReorderableWidget.AreInMultiGroup(reorderableInstance.groupID, groupID)) && (num == -1 || Event.current.mousePosition.DistanceToRect(reorderableInstance.absRect) < Event.current.mousePosition.DistanceToRect(ReorderableWidget.reorderables[num].absRect)))
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[num];
				if (ReorderableWidget.groups[reorderableInstance2.groupID].direction == ReorderableDirection.Horizontal)
				{
					toTheLeft = (Event.current.mousePosition.x < reorderableInstance2.absRect.center.x);
				}
				else
				{
					toTheLeft = (Event.current.mousePosition.y < reorderableInstance2.absRect.center.y);
				}
			}
			return num;
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x0018FB48 File Offset: 0x0018DD48
		private static void DrawLine(int groupID, Rect r)
		{
			ReorderableWidget.ReorderableGroup reorderableGroup = ReorderableWidget.groups[groupID];
			if (reorderableGroup.DrawLineExactlyBetween)
			{
				if (reorderableGroup.direction == ReorderableDirection.Horizontal)
				{
					r.xMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.xMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
				else
				{
					r.yMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.yMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
			}
			GUI.color = ReorderableWidget.LineColor;
			if (reorderableGroup.direction == ReorderableDirection.Horizontal)
			{
				if (ReorderableWidget.lastInsertNearLeft)
				{
					Widgets.DrawLine(r.position, new Vector2(r.x, r.yMax), ReorderableWidget.LineColor, 2f);
				}
				else
				{
					Widgets.DrawLine(new Vector2(r.xMax, r.y), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
				}
			}
			else if (ReorderableWidget.lastInsertNearLeft)
			{
				Widgets.DrawLine(r.position, new Vector2(r.xMax, r.y), ReorderableWidget.LineColor, 2f);
			}
			else
			{
				Widgets.DrawLine(new Vector2(r.x, r.yMax), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0018FCCC File Offset: 0x0018DECC
		private static int GetIndexWithinGroup(int index)
		{
			if (index < 0 || index >= ReorderableWidget.reorderables.Count)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i <= index; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.reorderables[index].groupID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0018FD24 File Offset: 0x0018DF24
		private static int FindLastReorderableIndexWithinGroup(int groupID)
		{
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				return -1;
			}
			int result = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == groupID)
				{
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0018FD74 File Offset: 0x0018DF74
		private static ReorderableWidget.ReorderableMultiGroup? GetMultiGroupByGroupID(int groupID)
		{
			foreach (ReorderableWidget.ReorderableMultiGroup reorderableMultiGroup in ReorderableWidget.multiGroups)
			{
				if (reorderableMultiGroup.includedGroups.Contains(groupID))
				{
					return new ReorderableWidget.ReorderableMultiGroup?(reorderableMultiGroup);
				}
			}
			return null;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0018FDE4 File Offset: 0x0018DFE4
		private static bool AreInMultiGroup(int groupA, int groupB)
		{
			ReorderableWidget.ReorderableMultiGroup? multiGroupByGroupID = ReorderableWidget.GetMultiGroupByGroupID(groupA);
			return multiGroupByGroupID != null && groupA != groupB && multiGroupByGroupID.Value.includedGroups.Contains(groupB);
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0018FE19 File Offset: 0x0018E019
		private static void StopDragging()
		{
			ReorderableWidget.draggingReorderable = -1;
			ReorderableWidget.dragStartPos = default(Vector2);
			ReorderableWidget.lastInsertNear = -1;
			ReorderableWidget.dragBegun = false;
		}

		// Token: 0x040037C3 RID: 14275
		private static List<ReorderableWidget.ReorderableGroup> groups = new List<ReorderableWidget.ReorderableGroup>();

		// Token: 0x040037C4 RID: 14276
		private static List<ReorderableWidget.ReorderableMultiGroup> multiGroups = new List<ReorderableWidget.ReorderableMultiGroup>();

		// Token: 0x040037C5 RID: 14277
		private static List<ReorderableWidget.ReorderableInstance> reorderables = new List<ReorderableWidget.ReorderableInstance>();

		// Token: 0x040037C6 RID: 14278
		private static int draggingReorderable = -1;

		// Token: 0x040037C7 RID: 14279
		private static Vector2 dragStartPos;

		// Token: 0x040037C8 RID: 14280
		private static bool clicked;

		// Token: 0x040037C9 RID: 14281
		private static bool released;

		// Token: 0x040037CA RID: 14282
		private static bool dragBegun;

		// Token: 0x040037CB RID: 14283
		private static Vector2 clickedAt;

		// Token: 0x040037CC RID: 14284
		private static int groupClicked;

		// Token: 0x040037CD RID: 14285
		private static Rect clickedInRect;

		// Token: 0x040037CE RID: 14286
		private static int lastInsertNear = -1;

		// Token: 0x040037CF RID: 14287
		private static int hoveredGroup = -1;

		// Token: 0x040037D0 RID: 14288
		private static bool lastInsertNearLeft;

		// Token: 0x040037D1 RID: 14289
		private static int lastFrameReorderableCount = -1;

		// Token: 0x040037D2 RID: 14290
		private const float MinMouseMoveToHighlightReorderable = 5f;

		// Token: 0x040037D3 RID: 14291
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x040037D4 RID: 14292
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040037D5 RID: 14293
		private const float LineWidth = 2f;

		// Token: 0x02000C03 RID: 3075
		private struct ReorderableGroup
		{
			// Token: 0x17000C0B RID: 3083
			// (get) Token: 0x06004E8B RID: 20107 RVA: 0x0018FEB7 File Offset: 0x0018E0B7
			public bool DrawLineExactlyBetween
			{
				get
				{
					return this.drawLineExactlyBetween_space > 0f;
				}
			}

			// Token: 0x040037D6 RID: 14294
			public Action<int, int> reorderedAction;

			// Token: 0x040037D7 RID: 14295
			public ReorderableDirection direction;

			// Token: 0x040037D8 RID: 14296
			public float drawLineExactlyBetween_space;

			// Token: 0x040037D9 RID: 14297
			public Action<int, Vector2> extraDraggedItemOnGUI;

			// Token: 0x040037DA RID: 14298
			public Rect absRect;

			// Token: 0x040037DB RID: 14299
			public bool playSoundOnStartReorder;
		}

		// Token: 0x02000C04 RID: 3076
		private struct ReorderableMultiGroup
		{
			// Token: 0x040037DC RID: 14300
			public Action<int, int, int, int> reorderedAction;

			// Token: 0x040037DD RID: 14301
			public List<int> includedGroups;
		}

		// Token: 0x02000C05 RID: 3077
		private struct ReorderableInstance
		{
			// Token: 0x040037DE RID: 14302
			public int groupID;

			// Token: 0x040037DF RID: 14303
			public Rect rect;

			// Token: 0x040037E0 RID: 14304
			public Rect absRect;
		}
	}
}
