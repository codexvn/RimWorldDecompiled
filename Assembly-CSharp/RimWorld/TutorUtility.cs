using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034F2 RID: 13554
	public static class TutorUtility
	{
		// Token: 0x060130AD RID: 77997 RVA: 0x005A4B7C File Offset: 0x005A2D7C
		public static bool BuildingOrBlueprintOrFrameCenterExists(IntVec3 c, Map map, ThingDef buildingDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (!(thing.Position != c))
				{
					if (thing.def == buildingDef)
					{
						return true;
					}
					if (thing.def.entityDefToBuild == buildingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060130AE RID: 77998 RVA: 0x005A4BD4 File Offset: 0x005A2DD4
		public static CellRect FindUsableRect(int width, int height, Map map, float minFertility = 0f, bool noItems = false)
		{
			IntVec3 center = map.Center;
			float num = 1f;
			CellRect cellRect;
			for (;;)
			{
				cellRect = CellRect.CenteredOn(center + new IntVec3((int)Rand.Range(-num, num), 0, (int)Rand.Range(-num, num)), width / 2);
				cellRect.Width = width;
				cellRect.Height = height;
				cellRect = cellRect.ExpandedBy(1);
				bool flag = true;
				foreach (IntVec3 intVec in cellRect)
				{
					if (intVec.Fogged(map) || !intVec.Walkable(map) || !intVec.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy) || intVec.GetFertility(map) < minFertility || intVec.GetZone(map) != null || TutorUtility.ContainsBlockingThing(intVec, map, noItems) || intVec.InNoBuildEdgeArea(map) || intVec.InNoZoneEdgeArea(map))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num += 0.25f;
			}
			return cellRect.ContractedBy(1);
		}

		// Token: 0x060130AF RID: 77999 RVA: 0x005A4CE8 File Offset: 0x005A2EE8
		private static bool ContainsBlockingThing(IntVec3 cell, Map map, bool noItems)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Building)
				{
					return true;
				}
				if (thingList[i] is Blueprint)
				{
					return true;
				}
				if (noItems && thingList[i].def.category == ThingCategory.Item)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060130B0 RID: 78000 RVA: 0x005A4D50 File Offset: 0x005A2F50
		public static void DrawLabelOnThingOnGUI(Thing t, string label)
		{
			Vector2 vector = (t.DrawPos + new Vector3(0f, 0f, 0.5f)).MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060130B1 RID: 78001 RVA: 0x005A4DE4 File Offset: 0x005A2FE4
		public static void DrawLabelOnGUI(Vector3 mapPos, string label)
		{
			Vector2 vector = mapPos.MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060130B2 RID: 78002 RVA: 0x005A4E59 File Offset: 0x005A3059
		public static void DrawCellRectOnGUI(CellRect cellRect, string label = null)
		{
			if (label != null)
			{
				TutorUtility.DrawLabelOnGUI(cellRect.CenterVector3, label);
			}
		}

		// Token: 0x060130B3 RID: 78003 RVA: 0x005A4E6C File Offset: 0x005A306C
		public static void DrawCellRectUpdate(CellRect cellRect)
		{
			foreach (IntVec3 c in cellRect)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x060130B4 RID: 78004 RVA: 0x005A4EC0 File Offset: 0x005A30C0
		public static void DoModalDialogIfNotKnown(ConceptDef conc, params string[] input)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, string.Format(conc.HelpTextAdjusted, input));
			}
		}

		// Token: 0x060130B5 RID: 78005 RVA: 0x005A4EE9 File Offset: 0x005A30E9
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, conc.HelpTextAdjusted.Formatted(args));
			}
		}

		// Token: 0x060130B6 RID: 78006 RVA: 0x005A4F0C File Offset: 0x005A310C
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, string buttonAText, Action buttonAAction, string buttonBText = null, Action buttonBAction = null, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				Find.WindowStack.Add(new Dialog_MessageBox(conc.HelpTextAdjusted.Formatted(args), buttonAText, buttonAAction, buttonBText, buttonBAction, null, false, null, null, WindowLayer.Dialog));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
			}
		}

		// Token: 0x060130B7 RID: 78007 RVA: 0x005A4F50 File Offset: 0x005A3150
		private static void DoModalDialogIfNotKnownInner(ConceptDef conc, string msg)
		{
			Find.WindowStack.Add(new Dialog_MessageBox(msg, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
		}

		// Token: 0x060130B8 RID: 78008 RVA: 0x005A4F84 File Offset: 0x005A3184
		public static bool EventCellsMatchExactly(EventPack ep, List<IntVec3> targetCells)
		{
			if (ep.Cell.IsValid)
			{
				return targetCells.Count == 1 && ep.Cell == targetCells[0];
			}
			if (ep.Cells == null)
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 item in ep.Cells)
			{
				if (!targetCells.Contains(item))
				{
					return false;
				}
				num++;
			}
			return num == targetCells.Count;
		}

		// Token: 0x060130B9 RID: 78009 RVA: 0x005A5028 File Offset: 0x005A3228
		public static bool EventCellsAreWithin(EventPack ep, List<IntVec3> targetCells)
		{
			if (ep.Cell.IsValid)
			{
				return targetCells.Contains(ep.Cell);
			}
			return ep.Cells != null && !ep.Cells.Any((IntVec3 c) => !targetCells.Contains(c));
		}
	}
}
