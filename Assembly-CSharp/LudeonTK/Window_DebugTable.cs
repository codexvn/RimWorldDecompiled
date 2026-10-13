using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LudeonTK
{
	// Token: 0x02000096 RID: 150
	public class Window_DebugTable : Window_Dev
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000940B File Offset: 0x0000760B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00011D48 File Offset: 0x0000FF48
		public Window_DebugTable(string[,] tables)
		{
			this.tableRaw = tables;
			this.colVisible = new bool[this.tableRaw.GetLength(0)];
			this.rowsVisible = new bool[this.tableRaw.GetLength(1)];
			for (int i = 0; i < this.colVisible.Length; i++)
			{
				this.colVisible[i] = true;
			}
			for (int j = 0; j < this.rowsVisible.Length; j++)
			{
				this.rowsVisible[j] = true;
			}
			this.doCloseButton = true;
			this.doCloseX = true;
			this.focusFilter = true;
			Text.Font = GameFont.Tiny;
			this.BuildTableSorted();
			this.visibleWidth = this.colWidths.Sum();
			this.visibleHeight = this.rowHeights.Sum() - this.rowHeights[0];
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00011E4C File Offset: 0x0001004C
		private void BuildTableSorted()
		{
			if (this.sortMode == Window_DebugTable.SortMode.Off)
			{
				this.tableSorted = this.tableRaw;
			}
			else
			{
				List<List<string>> list = new List<List<string>>();
				for (int i = 1; i < this.tableRaw.GetLength(1); i++)
				{
					list.Add(new List<string>());
					for (int j = 0; j < this.tableRaw.GetLength(0); j++)
					{
						list[i - 1].Add(this.tableRaw[j, i]);
					}
				}
				NumericStringComparer comparer = new NumericStringComparer();
				switch (this.sortMode)
				{
				case Window_DebugTable.SortMode.Off:
					throw new Exception();
				case Window_DebugTable.SortMode.Ascending:
					list = list.OrderBy((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				case Window_DebugTable.SortMode.Descending:
					list = list.OrderByDescending((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				}
				this.tableSorted = new string[this.tableRaw.GetLength(0), this.tableRaw.GetLength(1)];
				for (int k = 0; k < this.tableRaw.GetLength(1); k++)
				{
					for (int l = 0; l < this.tableRaw.GetLength(0); l++)
					{
						if (k == 0)
						{
							this.tableSorted[l, k] = this.tableRaw[l, k];
						}
						else
						{
							this.tableSorted[l, k] = list[k - 1][l];
						}
					}
				}
			}
			this.colWidths.Clear();
			for (int m = 0; m < this.tableRaw.GetLength(0); m++)
			{
				float item;
				if (this.colVisible[m])
				{
					float num = 0f;
					for (int n = 0; n < this.tableRaw.GetLength(1); n++)
					{
						float x2 = Text.CalcSize(this.tableRaw[m, n]).x;
						if (x2 > num)
						{
							num = x2;
						}
					}
					item = num + 4f;
				}
				else
				{
					item = 10f;
				}
				this.colWidths.Add(item);
			}
			this.rowHeights.Clear();
			for (int num2 = 0; num2 < this.tableSorted.GetLength(1); num2++)
			{
				float num3 = 0f;
				for (int num4 = 0; num4 < this.tableSorted.GetLength(0); num4++)
				{
					float y = Text.CalcSize(this.tableSorted[num4, num2]).y;
					if (y > num3)
					{
						num3 = y;
					}
				}
				this.rowHeights.Add(num3 + 2f);
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000120E4 File Offset: 0x000102E4
		private void RefreshSearchFilter()
		{
			this.visibleHeight = 0f;
			for (int i = 1; i < this.tableSorted.GetLength(1); i++)
			{
				bool flag = false;
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (this.quickSearchWidget.filter.Matches(this.tableSorted[j, i]))
					{
						flag = true;
						break;
					}
				}
				if (this.rowsVisible[i] != flag)
				{
					this.scrollPosition = Vector2.zero;
				}
				if (flag)
				{
					this.visibleHeight += this.rowHeights[i];
				}
				this.rowsVisible[i] = flag;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00012190 File Offset: 0x00010390
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			Rect outRect = inRect;
			float num = this.rowHeights[0] + 2f;
			outRect.y = num;
			outRect.yMax -= 40f + num;
			Rect viewRect = new Rect(0f, 0f, this.colWidths.Sum(), this.visibleHeight);
			float num2 = 0f;
			for (int i = 0; i < this.tableSorted.GetLength(0); i++)
			{
				Rect rect = new Rect(num2 - this.scrollPosition.x, 0f, this.colWidths[i], this.rowHeights[0]);
				num2 += this.colWidths[i];
				MouseoverSounds.DoRegion(rect);
				if (!Mouse.IsOver(rect))
				{
					DevGUI.Label(rect, this.tableSorted[i, 0]);
				}
				else
				{
					DevGUI.DrawHighlight(rect);
					if (Event.current.type == EventType.MouseDown)
					{
						if (Event.current.button == 0)
						{
							if (i != this.sortColumn)
							{
								this.sortMode = Window_DebugTable.SortMode.Off;
							}
							switch (this.sortMode)
							{
							case Window_DebugTable.SortMode.Off:
								this.sortMode = Window_DebugTable.SortMode.Descending;
								this.sortColumn = i;
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								break;
							case Window_DebugTable.SortMode.Ascending:
								this.sortMode = Window_DebugTable.SortMode.Off;
								this.sortColumn = -1;
								SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
								break;
							case Window_DebugTable.SortMode.Descending:
								this.sortMode = Window_DebugTable.SortMode.Ascending;
								this.sortColumn = i;
								SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
								break;
							}
							this.BuildTableSorted();
						}
						else if (Event.current.button == 1)
						{
							this.colVisible[i] = !this.colVisible[i];
							SoundDefOf.Crunch.PlayOneShotOnCamera(null);
							this.BuildTableSorted();
						}
						Event.current.Use();
					}
					DevGUI.Label(rect, this.tableSorted[i, 0]);
				}
			}
			Rect rect2 = new Rect
			{
				x = 0f,
				width = 300f,
				y = inRect.yMax - 24f - 4f,
				height = 24f
			};
			this.quickSearchWidget.OnGUI(rect2, new Action(this.RefreshSearchFilter), null);
			if (this.focusFilter)
			{
				this.quickSearchWidget.Focus();
				this.focusFilter = false;
			}
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num3 = 0f;
			for (int j = 1; j < this.tableSorted.GetLength(1); j++)
			{
				if (this.rowsVisible[j])
				{
					if (num3 + this.rowHeights[j] < viewRect.yMin + this.scrollPosition.y || num3 > viewRect.yMax + this.scrollPosition.y)
					{
						num3 += this.rowHeights[j];
					}
					else
					{
						float num4 = 0f;
						bool flag = Mouse.IsOver(new Rect(num4, num3, this.visibleWidth, this.rowHeights[j]));
						for (int k = 0; k < this.tableSorted.GetLength(0); k++)
						{
							Rect rect3 = new Rect(num4, num3, this.colWidths[k], this.rowHeights[j]);
							if (flag)
							{
								DevGUI.DrawHighlight(rect3);
							}
							else if (k % 2 == 0)
							{
								DevGUI.DrawLightHighlight(rect3);
							}
							if (this.colVisible[k])
							{
								DevGUI.Label(rect3, this.tableSorted[k, j]);
							}
							num4 += this.colWidths[k];
						}
						num3 += this.rowHeights[j];
					}
				}
			}
			DevGUI.EndScrollView();
			Text.Font = GameFont.Small;
			if (DevGUI.ButtonText(new Rect(inRect.xMax - 120f, inRect.yMax - 30f, 120f, 30f), "Copy CSV", null))
			{
				this.CopyCSVToClipboard();
				Messages.Message("Copied table data to clipboard in CSV format.", MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000125D0 File Offset: 0x000107D0
		private void MouseOverHeaderRowCell(Rect cell, int col)
		{
			MouseoverSounds.DoRegion(cell);
			if (Mouse.IsOver(cell) && Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (col != this.sortColumn)
					{
						this.sortMode = Window_DebugTable.SortMode.Off;
					}
					switch (this.sortMode)
					{
					case Window_DebugTable.SortMode.Off:
						this.sortMode = Window_DebugTable.SortMode.Descending;
						this.sortColumn = col;
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Ascending:
						this.sortMode = Window_DebugTable.SortMode.Off;
						this.sortColumn = -1;
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Descending:
						this.sortMode = Window_DebugTable.SortMode.Ascending;
						this.sortColumn = col;
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						break;
					}
					this.BuildTableSorted();
				}
				else if (Event.current.button == 1)
				{
					this.colVisible[col] = !this.colVisible[col];
					SoundDefOf.Crunch.PlayOneShotOnCamera(null);
					this.BuildTableSorted();
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000126CC File Offset: 0x000108CC
		private static void MouseCenterButtonDrag(ref Vector2 scroll)
		{
			if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
			{
				Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
				Event.current.Use();
				if (currentEventDelta != Vector2.zero)
				{
					scroll += -1f * currentEventDelta;
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001272C File Offset: 0x0001092C
		private static bool IsMouseOverRow(Rect cell)
		{
			cell.xMin -= 9999999f;
			cell.xMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001275A File Offset: 0x0001095A
		private static bool IsMouseOverCol(Rect cell)
		{
			cell.yMin -= 9999999f;
			cell.yMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00012788 File Offset: 0x00010988
		private static void DrawOpaqueLabel(Rect r, string label, bool highlighted = true, bool selectedRow = false, bool selectedCol = false, bool offsetLabel = false)
		{
			if (offsetLabel)
			{
				r.height += 2f;
			}
			Color color = GUI.color;
			GUI.color = DevGUI.WindowBGFillColor;
			GUI.DrawTexture(r, BaseContent.WhiteTex);
			GUI.color = color;
			if (selectedRow)
			{
				DevGUI.DrawHighlightSelected(r);
			}
			if (selectedCol)
			{
				DevGUI.DrawHighlightSelected(r);
			}
			if (highlighted)
			{
				DevGUI.DrawHighlight(r);
			}
			else
			{
				DevGUI.DrawLightHighlight(r);
			}
			if (offsetLabel)
			{
				r.y += 2f;
			}
			DevGUI.Label(r, label);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00012810 File Offset: 0x00010A10
		private void CopyCSVToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.tableSorted.GetLength(1); i++)
			{
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(",");
					}
					string text = this.tableSorted[j, i] ?? "";
					stringBuilder.Append("\"" + text.Replace("\n", " ") + "\"");
				}
				stringBuilder.Append("\n");
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x040001F5 RID: 501
		private string[,] tableRaw;

		// Token: 0x040001F6 RID: 502
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040001F7 RID: 503
		private string[,] tableSorted;

		// Token: 0x040001F8 RID: 504
		private List<float> colWidths = new List<float>();

		// Token: 0x040001F9 RID: 505
		private List<float> rowHeights = new List<float>();

		// Token: 0x040001FA RID: 506
		private int sortColumn = -1;

		// Token: 0x040001FB RID: 507
		private Window_DebugTable.SortMode sortMode;

		// Token: 0x040001FC RID: 508
		private bool[] colVisible;

		// Token: 0x040001FD RID: 509
		private bool[] rowsVisible;

		// Token: 0x040001FE RID: 510
		private float visibleWidth;

		// Token: 0x040001FF RID: 511
		private float visibleHeight;

		// Token: 0x04000200 RID: 512
		private bool focusFilter;

		// Token: 0x04000201 RID: 513
		private readonly QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04000202 RID: 514
		private const float ColExtraWidth = 4f;

		// Token: 0x04000203 RID: 515
		private const float RowExtraHeight = 2f;

		// Token: 0x04000204 RID: 516
		private const float HiddenColumnWidth = 10f;

		// Token: 0x04000205 RID: 517
		private const float MouseoverOffset = 2f;

		// Token: 0x02000097 RID: 151
		private enum SortMode
		{
			// Token: 0x04000207 RID: 519
			Off,
			// Token: 0x04000208 RID: 520
			Ascending,
			// Token: 0x04000209 RID: 521
			Descending
		}
	}
}
