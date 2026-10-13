using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LudeonTK
{
	// Token: 0x02000082 RID: 130
	public class Dialog_Debug : Window_Dev
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002BC RID: 700 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000940B File Offset: 0x0000760B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000ED02 File Offset: 0x0000CF02
		public string Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000ED0A File Offset: 0x0000CF0A
		private float FilterX
		{
			get
			{
				DebugActionNode debugActionNode = this.currentNode;
				if (((debugActionNode != null) ? debugActionNode.parent : null) == null || !this.currentNode.parent.IsRoot)
				{
					return 130f;
				}
				return 0f;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000ED3D File Offset: 0x0000CF3D
		private int HighlightedIndex
		{
			get
			{
				return this.currentTabMenu.HighlightedIndex(this.currentHighlightIndex, this.prioritizedHighlightedIndex);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000ED58 File Offset: 0x0000CF58
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.scrollPosition;
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000EDD7 File Offset: 0x0000CFD7
		public DebugActionNode CurrentNode
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		public Dialog_Debug()
		{
			this.Setup();
			this.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EE2C File Offset: 0x0000D02C
		public Dialog_Debug(DebugTabMenuDef def)
		{
			this.Setup();
			this.SwitchTab(def);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000EE69 File Offset: 0x0000D069
		public void NewColumn(float columnWidth)
		{
			this.curY = 0f;
			this.curX += columnWidth + 17f;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EE8A File Offset: 0x0000D08A
		protected void NewColumnIfNeeded(float columnWidth, float neededHeight)
		{
			if (this.curY + neededHeight > this.heightPerColumn)
			{
				this.NewColumn(columnWidth);
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000EEA3 File Offset: 0x0000D0A3
		public Rect GetRect(float height, float columnWidth, float widthPct = 1f)
		{
			this.NewColumnIfNeeded(columnWidth, height);
			Rect result = new Rect(this.curX, this.curY, columnWidth * widthPct, height);
			this.curY += height;
			return result;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
			this.focusFilter = true;
			this.menuDefsSorted.AddRange(DefDatabase<DebugTabMenuDef>.AllDefs.ToList<DebugTabMenuDef>());
			this.menuDefsSorted.SortBy((DebugTabMenuDef x) => x.displayOrder, (DebugTabMenuDef y) => y.label);
			DebugTabMenu debugTabMenu = this.currentTabMenu;
			if (debugTabMenu == null)
			{
				return;
			}
			debugTabMenu.Recache();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EF70 File Offset: 0x0000D170
		public void SwitchTab(DebugTabMenuDef def)
		{
			Dialog_Debug.TrySetupNodeGraph();
			this.scrollPosition = Vector2.zero;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			this.currentTabMenu = (this.menus.ContainsKey(def) ? this.menus[def] : DebugTabMenu.CreateMenu(def, this, Dialog_Debug.rootNode));
			this.currentTabMenu.Enter(Dialog_Debug.roots[def]);
			SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public static void TrySetupNodeGraph()
		{
			if (Dialog_Debug.rootNode != null)
			{
				return;
			}
			Dialog_Debug.rootNode = new DebugActionNode("Root", DebugActionType.Action, null, null);
			foreach (DebugTabMenuDef debugTabMenuDef in DefDatabase<DebugTabMenuDef>.AllDefs)
			{
				Dialog_Debug.roots.Add(debugTabMenuDef, DebugTabMenu.CreateMenu(debugTabMenuDef, null, Dialog_Debug.rootNode).InitActions(Dialog_Debug.rootNode));
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F06C File Offset: 0x0000D26C
		private void DrawTabs(Rect rect)
		{
			float num = Mathf.Min(rect.width / (float)this.menuDefsSorted.Count, 200f);
			for (int i = 0; i < this.menuDefsSorted.Count; i++)
			{
				DebugTabMenuDef debugTabMenuDef = this.menuDefsSorted[i];
				Rect rect2 = new Rect(rect.x + (float)i * num, rect.y, num, rect.height).ContractedBy(1f);
				if (debugTabMenuDef == this.currentTabMenu.def)
				{
					GUI.DrawTexture(rect2, DevGUI.ButtonBackgroundClick);
					Text.Anchor = TextAnchor.MiddleCenter;
					DevGUI.Label(rect2, debugTabMenuDef.LabelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (DevGUI.ButtonText(rect2, debugTabMenuDef.LabelCap, null))
				{
					this.SwitchTab(debugTabMenuDef);
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000F148 File Offset: 0x0000D348
		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.KeyDownEvent)
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					for (int i = 0; i < this.currentTabMenu.Count; i++)
					{
						int index = (highlightedIndex + i + 1) % this.currentTabMenu.Count;
						if (this.FilterAllows(this.currentTabMenu.LabelAtIndex(index)))
						{
							this.prioritizedHighlightedIndex = index;
							break;
						}
					}
				}
			}
			GUI.SetNextControlName("DebugFilter");
			Text.Font = GameFont.Small;
			Rect rect = new Rect(this.FilterX, 0f, Dialog_Debug.FilterInputSize.x, Dialog_Debug.FilterInputSize.y);
			this.filter = DevGUI.TextField(rect, this.filter);
			Rect rect2 = new Rect(rect.xMax + 10f, rect.y, inRect.width - rect.width - 10f, 32f);
			this.DrawTabs(rect2);
			if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.Repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = string.Empty;
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 42f;
			int num = (int)(this.InitialSize.x / 200f);
			this.heightPerColumn = Mathf.Max(outRect.height, (this.totalOptionsHeight + 50f * (float)(num - 1)) / (float)num);
			this.curX = 0f;
			this.curY = 0f;
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, this.heightPerColumn);
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, rect3, true);
			DevGUI.BeginGroup(rect3);
			float columnWidth = (rect3.width - 17f * (float)(num - 1)) / (float)num;
			this.currentTabMenu.ListOptions(this.HighlightedIndex, columnWidth);
			DevGUI.EndGroup();
			DevGUI.EndScrollView();
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				if (DevGUI.ButtonText(new Rect(0f, 0f, 120f, 32f), "Back", null))
				{
					this.currentNode.parent.Enter(this);
				}
				if (!this.currentNode.IsRoot)
				{
					Text.Anchor = TextAnchor.UpperRight;
					Text.Font = GameFont.Tiny;
					DevGUI.Label(new Rect(0f, 0f, outRect.width - 24f - 10f, 32f), this.currentNode.Path.Colorize(ColoredText.SubtleGrayColor));
					Text.Anchor = TextAnchor.UpperLeft;
				}
				Text.Font = font;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000F444 File Offset: 0x0000D644
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				this.currentTabMenu.OnAcceptKeyPressed(highlightedIndex);
				Event.current.Use();
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F480 File Offset: 0x0000D680
		public override void OnCancelKeyPressed()
		{
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				this.currentNode.parent.Enter(this);
				Event.current.Use();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		public static DebugActionNode GetNode(string path)
		{
			Dialog_Debug.TrySetupNodeGraph();
			DebugActionNode debugActionNode = Dialog_Debug.rootNode;
			string[] s = path.Split('\\', StringSplitOptions.None);
			int i;
			Predicate<DebugActionNode> <>9__0;
			int j;
			for (i = 0; i < s.Length; i = j + 1)
			{
				List<DebugActionNode> children = debugActionNode.children;
				Predicate<DebugActionNode> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((DebugActionNode x) => x.label == s[i]));
				}
				DebugActionNode debugActionNode2 = children.FirstOrDefault(predicate);
				if (debugActionNode2 == null)
				{
					return null;
				}
				debugActionNode = debugActionNode2;
				debugActionNode.TrySetupChildren();
				j = i;
			}
			return debugActionNode;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F564 File Offset: 0x0000D764
		public void SetCurrentNode(DebugActionNode node)
		{
			this.currentNode = node;
			foreach (DebugActionNode debugActionNode in this.currentNode.children)
			{
				debugActionNode.DirtyLabelCache();
			}
			this.scrollPosition = Vector2.zero;
			this.filter = string.Empty;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			DebugTabMenu debugTabMenu = this.currentTabMenu;
			if (debugTabMenu == null)
			{
				return;
			}
			debugTabMenu.Recache();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		public void DrawNode(DebugActionNode node, float columnWidth, bool highlight)
		{
			if (node.settingsField != null)
			{
				this.DoCheckbox(node, columnWidth, highlight);
				return;
			}
			this.DoButton(node, columnWidth, highlight);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F618 File Offset: 0x0000D818
		public DebugActionButtonResult ButtonDebugPinnable(string label, float columnWidth, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = DevGUI.ButtonDebugPinnable(rect, label, highlight, pinned);
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		public DebugActionButtonResult CheckboxPinnable(string label, float columnWidth, ref bool checkOn, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = DevGUI.CheckboxPinnable(rect, label, ref checkOn, highlight, pinned);
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F734 File Offset: 0x0000D934
		private void DoButton(DebugActionNode node, float columnWidth, bool highlight)
		{
			string labelNow = node.LabelNow;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.ButtonDebugPinnable(labelNow, columnWidth, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 22f + this.verticalSpacing;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F7C4 File Offset: 0x0000D9C4
		private void DoCheckbox(DebugActionNode node, float columnWidth, bool highlight)
		{
			string labelNow = node.LabelNow;
			FieldInfo settingsField = node.settingsField;
			bool flag = (bool)settingsField.GetValue(null);
			bool flag2 = flag;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.CheckboxPinnable(labelNow, columnWidth, ref flag, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += Text.LineHeight;
			}
			if (flag != flag2)
			{
				settingsField.SetValue(null, flag);
				MethodInfo method = settingsField.DeclaringType.GetMethod(settingsField.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public void DoLabel(string label, float columnWidth)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(label, columnWidth);
			DevGUI.Label(new Rect(this.curX, this.curY, columnWidth, num), label);
			this.curY += num;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += num;
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F909 File Offset: 0x0000DB09
		public void DoGap(float gapSize = 7f)
		{
			this.curY += gapSize;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += gapSize;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F934 File Offset: 0x0000DB34
		public bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F962 File Offset: 0x0000DB62
		public static void ResetStaticData()
		{
			Dialog_Debug.rootNode = null;
			Dialog_Debug.roots.Clear();
		}

		// Token: 0x04000187 RID: 391
		public static DebugActionNode rootNode;

		// Token: 0x04000188 RID: 392
		private DebugActionNode currentNode;

		// Token: 0x04000189 RID: 393
		private Dictionary<DebugTabMenuDef, DebugTabMenu> menus = new Dictionary<DebugTabMenuDef, DebugTabMenu>();

		// Token: 0x0400018A RID: 394
		private static Dictionary<DebugTabMenuDef, DebugActionNode> roots = new Dictionary<DebugTabMenuDef, DebugActionNode>();

		// Token: 0x0400018B RID: 395
		private List<DebugTabMenuDef> menuDefsSorted = new List<DebugTabMenuDef>();

		// Token: 0x0400018C RID: 396
		private DebugTabMenu currentTabMenu;

		// Token: 0x0400018D RID: 397
		private float totalOptionsHeight;

		// Token: 0x0400018E RID: 398
		private string filter;

		// Token: 0x0400018F RID: 399
		private bool focusFilter;

		// Token: 0x04000190 RID: 400
		private int currentHighlightIndex;

		// Token: 0x04000191 RID: 401
		private int prioritizedHighlightedIndex;

		// Token: 0x04000192 RID: 402
		private Vector2 scrollPosition;

		// Token: 0x04000193 RID: 403
		protected float curY;

		// Token: 0x04000194 RID: 404
		protected float curX;

		// Token: 0x04000195 RID: 405
		private int boundingRectCachedForFrame = -1;

		// Token: 0x04000196 RID: 406
		private Rect? boundingRectCached;

		// Token: 0x04000197 RID: 407
		private Rect? boundingRect;

		// Token: 0x04000198 RID: 408
		public float verticalSpacing = 2f;

		// Token: 0x04000199 RID: 409
		private float heightPerColumn;

		// Token: 0x0400019A RID: 410
		private const string FilterControlName = "DebugFilter";

		// Token: 0x0400019B RID: 411
		private const float DebugOptionsGap = 7f;

		// Token: 0x0400019C RID: 412
		private static readonly Color DisallowedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x0400019D RID: 413
		private static readonly Vector2 FilterInputSize = new Vector2(200f, 30f);

		// Token: 0x0400019E RID: 414
		private const float AssumedBiggestElementHeight = 50f;

		// Token: 0x0400019F RID: 415
		private const float BackButtonWidth = 120f;

		// Token: 0x040001A0 RID: 416
		private const float PinnableActionHeight = 22f;

		// Token: 0x040001A1 RID: 417
		private const float TabHeight = 32f;

		// Token: 0x040001A2 RID: 418
		private const float MaxTabWidth = 200f;
	}
}
