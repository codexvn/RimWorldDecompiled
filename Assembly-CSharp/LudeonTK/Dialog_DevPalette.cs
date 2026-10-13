using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000093 RID: 147
	[StaticConstructorOnStartup]
	public class Dialog_DevPalette : Window_Dev
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000112BC File Offset: 0x0000F4BC
		private List<DebugActionNode> Nodes
		{
			get
			{
				if (Dialog_DevPalette.cachedNodes == null)
				{
					Dialog_DevPalette.cachedNodes = new List<DebugActionNode>();
					for (int i = 0; i < Prefs.DebugActionsPalette.Count; i++)
					{
						DebugActionNode node = Dialog_Debug.GetNode(Prefs.DebugActionsPalette[i]);
						if (node != null)
						{
							Dialog_DevPalette.cachedNodes.Add(node);
						}
					}
				}
				return Dialog_DevPalette.cachedNodes;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00011314 File Offset: 0x0000F514
		public Dialog_DevPalette()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
			this.EnsureAllNodesValid();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001138C File Offset: 0x0000F58C
		private void EnsureAllNodesValid()
		{
			Dialog_DevPalette.cachedNodes = null;
			for (int i = Prefs.DebugActionsPalette.Count - 1; i >= 0; i--)
			{
				string text = Prefs.DebugActionsPalette[i];
				if (Dialog_Debug.GetNode(text) == null)
				{
					Log.Warning("Could not find node from path '" + text + "'. Removing.");
					Prefs.DebugActionsPalette.RemoveAt(i);
					Prefs.Save();
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000113F0 File Offset: 0x0000F5F0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			DevGUI.Label(new Rect(inRect.x, inRect.y, inRect.width, 24f), "Dev palette");
			inRect.yMin += 26f;
			if (Prefs.DebugActionsPalette.Count == 0)
			{
				GUI.color = ColoredText.SubtleGrayColor;
				DevGUI.Label(inRect, "<i>To add commands here, open the debug actions menu and click the pin icons.</i>");
				GUI.color = Color.white;
			}
			else
			{
				if (Event.current.type == EventType.Repaint)
				{
					this.reorderableGroupID = ReorderableWidget.NewGroup(delegate(int from, int to)
					{
						string item = Prefs.DebugActionsPalette[from];
						Prefs.DebugActionsPalette.Insert(to, item);
						Prefs.DebugActionsPalette.RemoveAt((from < to) ? from : (from + 1));
						Dialog_DevPalette.cachedNodes = null;
						Prefs.Save();
					}, ReorderableDirection.Vertical, inRect, 2f, null, false);
				}
				GUI.BeginGroup(inRect);
				float num = 0f;
				Text.Font = GameFont.Tiny;
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					DebugActionNode debugActionNode = this.Nodes[i];
					float num2 = 0f;
					num2 += 22f;
					Rect rect = new Rect(num2, num, inRect.width - 44f, 22f);
					if (debugActionNode.ActiveNow)
					{
						if (debugActionNode.settingsField != null)
						{
							Rect rect2 = rect;
							rect2.xMax -= rect2.height + 4f;
							DevGUI.Label(rect2, "  " + this.PrettifyNodeName(debugActionNode));
							GUI.DrawTexture(new Rect(rect2.xMax, rect2.y, rect2.height, rect2.height), debugActionNode.On ? DevGUI.CheckOn : DevGUI.CheckOff);
							DevGUI.DrawHighlightIfMouseover(rect);
							if (DevGUI.ButtonInvisible(rect))
							{
								debugActionNode.Enter(null);
							}
						}
						else if (DevGUI.ButtonText(rect, "  " + this.PrettifyNodeName(debugActionNode), new TextAnchor?(TextAnchor.MiddleLeft)))
						{
							debugActionNode.Enter(Find.WindowStack.WindowOfType<Dialog_Debug>());
						}
					}
					else
					{
						DevGUI.Label(rect, "  " + this.PrettifyNodeName(debugActionNode));
					}
					num2 += rect.width;
					Rect rect3 = new Rect(0f, num, 22f, 22f);
					Rect rect4 = new Rect(rect3.x, rect3.y, inRect.width - 22f, 22f);
					if ((Event.current.type != EventType.MouseDown || Mouse.IsOver(rect3)) && ReorderableWidget.Reorderable(this.reorderableGroupID, rect4, false, true))
					{
						DevGUI.DrawRect(rect4, DevGUI.WindowBGFillColor * new Color(1f, 1f, 1f, 0.5f));
					}
					DevGUI.ButtonImage(rect3.ContractedBy(1f), TexButton.DragHash);
					Rect butRect = new Rect(num2, num, 22f, 22f);
					if (Widgets.ButtonImage(butRect, TexButton.Delete, true, null))
					{
						Prefs.DebugActionsPalette.RemoveAt(i);
						Prefs.Save();
						Dialog_DevPalette.cachedNodes = null;
						this.SetInitialSizeAndPosition();
					}
					num2 += butRect.width;
					num += 24f;
				}
				Text.Font = GameFont.Small;
				GUI.EndGroup();
			}
			if (!Mathf.Approximately(this.windowRect.x, this.windowPosition.x) || !Mathf.Approximately(this.windowRect.y, this.windowPosition.y))
			{
				this.windowPosition = new Vector2(this.windowRect.x, this.windowRect.y);
				Prefs.DevPalettePosition = this.windowPosition;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00011774 File Offset: 0x0000F974
		public static void ToggleAction(string actionLabel)
		{
			if (Prefs.DebugActionsPalette.Contains(actionLabel))
			{
				Prefs.DebugActionsPalette.Remove(actionLabel);
			}
			else
			{
				Prefs.DebugActionsPalette.Add(actionLabel);
			}
			Prefs.Save();
			Dialog_DevPalette.cachedNodes = null;
			Dialog_DevPalette dialog_DevPalette = Find.WindowStack.WindowOfType<Dialog_DevPalette>();
			if (dialog_DevPalette == null)
			{
				return;
			}
			dialog_DevPalette.SetInitialSizeAndPosition();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000117C8 File Offset: 0x0000F9C8
		protected override void SetInitialSizeAndPosition()
		{
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Vector2 vector = new Vector2(Text.CalcSize("Dev palette").x + 48f + 10f, 28f);
			if (!this.Nodes.Any<DebugActionNode>())
			{
				vector.x = Mathf.Max(vector.x, 200f);
				vector.y += Text.CalcHeight("<i>To add commands here, open the debug actions menu and click the pin icons.</i>", vector.x) + this.Margin * 2f;
			}
			else
			{
				Text.Font = GameFont.Tiny;
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					vector.x = Mathf.Max(vector.x, Text.CalcSize("  " + this.PrettifyNodeName(this.Nodes[i]) + "  ").x + 48f);
				}
				vector.y += (float)this.Nodes.Count * 22f + (float)((this.Nodes.Count + 1) * 2) + this.Margin;
			}
			this.windowPosition.x = Mathf.Clamp(this.windowPosition.x, 0f, (float)UI.screenWidth - vector.x);
			this.windowPosition.y = Mathf.Clamp(this.windowPosition.y, 0f, (float)UI.screenHeight - vector.y);
			this.windowRect = new Rect(this.windowPosition.x, this.windowPosition.y, vector.x, vector.y);
			this.windowRect = this.windowRect.Rounded();
			Text.Font = font;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001198C File Offset: 0x0000FB8C
		private string PrettifyNodeName(DebugActionNode node)
		{
			string path = node.Path;
			string text;
			if (this.nameCache.TryGetValue(path, out text))
			{
				return text;
			}
			DebugActionNode debugActionNode = node;
			text = debugActionNode.LabelNow.Replace("...", "");
			while (debugActionNode.parent != null && !debugActionNode.parent.IsRoot && (debugActionNode.parent.parent == null || !debugActionNode.parent.parent.IsRoot))
			{
				text = debugActionNode.parent.LabelNow.Replace("...", "") + "\\" + text;
				debugActionNode = debugActionNode.parent;
			}
			this.nameCache[path] = text;
			return text;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00011A3B File Offset: 0x0000FC3B
		public override void PostClose()
		{
			base.PostOpen();
			DebugSettings.devPalette = false;
		}

		// Token: 0x040001DF RID: 479
		private Vector2 windowPosition;

		// Token: 0x040001E0 RID: 480
		private static List<DebugActionNode> cachedNodes;

		// Token: 0x040001E1 RID: 481
		private int reorderableGroupID = -1;

		// Token: 0x040001E2 RID: 482
		private Dictionary<string, string> nameCache = new Dictionary<string, string>();

		// Token: 0x040001E3 RID: 483
		private const string Title = "Dev palette";

		// Token: 0x040001E4 RID: 484
		private const float ButtonSize = 24f;

		// Token: 0x040001E5 RID: 485
		private const float ButtonSize_Small = 22f;

		// Token: 0x040001E6 RID: 486
		private const string NoActionDesc = "<i>To add commands here, open the debug actions menu and click the pin icons.</i>";
	}
}
