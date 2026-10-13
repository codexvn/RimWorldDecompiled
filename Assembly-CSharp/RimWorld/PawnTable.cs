using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003486 RID: 13446
	public class PawnTable
	{
		// Token: 0x17003157 RID: 12631
		// (get) Token: 0x06012E8C RID: 77452 RVA: 0x005996AC File Offset: 0x005978AC
		public List<PawnColumnDef> Columns
		{
			get
			{
				this.columns.Clear();
				foreach (PawnColumnDef pawnColumnDef in this.def.columns)
				{
					if (pawnColumnDef.Worker.VisibleCurrently)
					{
						this.columns.Add(pawnColumnDef);
					}
				}
				return this.columns;
			}
		}

		// Token: 0x17003158 RID: 12632
		// (get) Token: 0x06012E8D RID: 77453 RVA: 0x00599728 File Offset: 0x00597928
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x17003159 RID: 12633
		// (get) Token: 0x06012E8E RID: 77454 RVA: 0x00599730 File Offset: 0x00597930
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x1700315A RID: 12634
		// (get) Token: 0x06012E8F RID: 77455 RVA: 0x00599742 File Offset: 0x00597942
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x1700315B RID: 12635
		// (get) Token: 0x06012E90 RID: 77456 RVA: 0x00599750 File Offset: 0x00597950
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x1700315C RID: 12636
		// (get) Token: 0x06012E91 RID: 77457 RVA: 0x0059975E File Offset: 0x0059795E
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x1700315D RID: 12637
		// (get) Token: 0x06012E92 RID: 77458 RVA: 0x0059976C File Offset: 0x0059796C
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x06012E93 RID: 77459 RVA: 0x0059977C File Offset: 0x0059797C
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x06012E94 RID: 77460 RVA: 0x00599800 File Offset: 0x00597A00
		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.RecacheIfDirty();
			float num = this.cachedSize.x - 16f;
			List<PawnColumnDef> list = this.Columns;
			int num2 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				int num3;
				if (i == list.Count - 1)
				{
					num3 = (int)(num - (float)num2);
				}
				else
				{
					num3 = (int)this.cachedColumnWidths[i];
				}
				Rect rect = new Rect((float)((int)position.x + num2), (float)((int)position.y), (float)num3, (float)((int)this.cachedHeaderHeight));
				list[i].Worker.DoHeader(rect, this);
				num2 += num3;
			}
			Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			num2 = 0;
			int num4;
			for (int j = 0; j < list.Count; j++)
			{
				num4 = 0;
				PawnColumnDef pawnColumnDef = list[j];
				int num5;
				if (j == list.Count - 1)
				{
					num5 = (int)(num - (float)num2);
				}
				else
				{
					num5 = (int)this.cachedColumnWidths[j];
				}
				for (int k = 0; k < this.cachedPawns.Count; k++)
				{
					GUI.color = PawnTable.BorderColor;
					Widgets.DrawLineHorizontal((float)num2, (float)num4, (float)num5);
					GUI.color = Color.white;
					Rect rect2 = new Rect((float)num2, (float)num4, (float)num5, (float)((int)this.cachedRowHeights[k]));
					Pawn pawn = this.cachedPawns[k];
					bool flag = false;
					if (pawnColumnDef.groupable)
					{
						int num6 = k;
						int num7 = k + 1;
						while (num7 < this.cachedPawns.Count && list[j].Worker.CanGroupWith(this.cachedPawns[k], this.cachedPawns[num7]))
						{
							rect2.yMax += (float)((int)this.cachedRowHeights[num7]);
							num6 = num7;
							flag = true;
							num7++;
						}
						k = num6;
					}
					if ((float)num4 - this.scrollPosition.y + (float)((int)this.cachedRowHeights[k]) >= 0f && (float)num4 - this.scrollPosition.y <= outRect.height)
					{
						list[j].Worker.DoCell(rect2, pawn, this);
						if (pawnColumnDef.groupable && flag)
						{
							GUI.color = PawnTable.BorderColor;
							Widgets.DrawLineVertical(rect2.xMin, rect2.yMin, rect2.height);
							Widgets.DrawLineVertical(rect2.xMax, rect2.yMin, rect2.height);
							GUI.color = Color.white;
						}
					}
					GUI.color = Color.white;
					num4 += (int)rect2.height;
				}
				num2 += num5;
			}
			num4 = 0;
			for (int l = 0; l < this.cachedPawns.Count; l++)
			{
				Rect rect3 = new Rect(0f, (float)num4, viewRect.width, (float)((int)this.cachedRowHeights[l]));
				if (Find.Selector.IsSelected(this.cachedPawns[l]))
				{
					Widgets.DrawHighlight(rect3, 0.6f);
				}
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					this.cachedLookTargets[l].Highlight(true, this.cachedPawns[l].IsColonist, false);
				}
				if (this.cachedPawns[l].Downed)
				{
					GUI.color = new Color(1f, 0f, 0f, 0.5f);
					Widgets.DrawLineHorizontal(0f, rect3.center.y, viewRect.width);
					GUI.color = Color.white;
				}
				num4 += (int)this.cachedRowHeights[l];
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06012E95 RID: 77461 RVA: 0x00599C53 File Offset: 0x00597E53
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06012E96 RID: 77462 RVA: 0x00599C5C File Offset: 0x00597E5C
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x06012E97 RID: 77463 RVA: 0x00599C88 File Offset: 0x00597E88
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x06012E98 RID: 77464 RVA: 0x00599C9E File Offset: 0x00597E9E
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x06012E99 RID: 77465 RVA: 0x00599CB4 File Offset: 0x00597EB4
		private void RecacheIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.RecacheColumns();
			this.RecachePawns();
			this.RecacheRowHeights();
			this.cachedHeaderHeight = this.CalculateHeaderHeight();
			this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
			this.RecacheSize();
			this.RecacheColumnWidths();
			this.RecacheLookTargets();
		}

		// Token: 0x06012E9A RID: 77466 RVA: 0x00599D10 File Offset: 0x00597F10
		private void RecacheColumns()
		{
			foreach (PawnColumnDef pawnColumnDef in this.def.columns)
			{
				pawnColumnDef.Worker.Recache();
			}
		}

		// Token: 0x06012E9B RID: 77467 RVA: 0x00599D6C File Offset: 0x00597F6C
		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			this.cachedPawns = this.LabelSortFunction(this.cachedPawns).ToList<Pawn>();
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.SortStable(new Func<Pawn, Pawn, int>(this.sortByColumn.Worker.Compare));
				}
				else
				{
					this.cachedPawns.SortStable((Pawn a, Pawn b) => this.sortByColumn.Worker.Compare(b, a));
				}
			}
			this.cachedPawns = this.PrimarySortFunction(this.cachedPawns).ToList<Pawn>();
		}

		// Token: 0x06012E9C RID: 77468 RVA: 0x00599E13 File Offset: 0x00598013
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x06012E9D RID: 77469 RVA: 0x00009C2B File Offset: 0x00007E2B
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x06012E9E RID: 77470 RVA: 0x00599E3C File Offset: 0x0059803C
		private void RecacheColumnWidths()
		{
			float num = this.cachedSize.x - 16f;
			float num2 = 0f;
			this.RecacheColumnWidths_StartWithMinWidths(out num2);
			if (num2 == num)
			{
				return;
			}
			if (num2 > num)
			{
				this.SubtractProportionally(num2 - num, num2);
				return;
			}
			bool flag;
			this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
			if (flag)
			{
				return;
			}
			this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
		}

		// Token: 0x06012E9F RID: 77471 RVA: 0x00599E98 File Offset: 0x00598098
		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				float minWidth = this.GetMinWidth(list[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		// Token: 0x06012EA0 RID: 77472 RVA: 0x00599EF0 File Offset: 0x005980F0
		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(list[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = float.MinValue;
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.columnAtOptimalWidth[j])
					{
						num2 = Mathf.Max(num2, (float)list[j].widthPriority);
					}
				}
				float num3 = 0f;
				for (int k = 0; k < this.cachedColumnWidths.Count; k++)
				{
					if (!this.columnAtOptimalWidth[k] && (float)list[k].widthPriority == num2)
					{
						num3 += this.GetOptimalWidth(list[k]);
					}
				}
				float num4 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				bool flag2 = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtOptimalWidth[l])
					{
						if ((float)list[l].widthPriority != num2)
						{
							flag = true;
						}
						else
						{
							float num5 = num4 * this.GetOptimalWidth(list[l]) / num3;
							float num6 = this.GetOptimalWidth(list[l]) - this.cachedColumnWidths[l];
							if (num5 >= num6)
							{
								num5 = num6;
								this.columnAtOptimalWidth[l] = true;
								flag2 = true;
							}
							else
							{
								flag = true;
							}
							if (num5 > 0f)
							{
								List<float> list2 = this.cachedColumnWidths;
								int index = l;
								list2[index] += num5;
								usedWidth += num5;
							}
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_13;
				}
				if (!flag || !flag2)
				{
					goto IL_1F3;
				}
			}
			Log.Error("Too many iterations.");
			goto IL_1F3;
			Block_13:
			noMoreFreeSpace = true;
			IL_1F3:
			noMoreFreeSpace = false;
		}

		// Token: 0x06012EA1 RID: 77473 RVA: 0x0059A0F4 File Offset: 0x005982F4
		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(list[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = 0f;
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.columnAtMaxWidth[j])
					{
						num2 += Mathf.Max(this.GetOptimalWidth(list[j]), 1f);
					}
				}
				float num3 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (!this.columnAtMaxWidth[k])
					{
						float num4 = num3 * Mathf.Max(this.GetOptimalWidth(list[k]), 1f) / num2;
						float num5 = this.GetMaxWidth(list[k]) - this.cachedColumnWidths[k];
						if (num4 >= num5)
						{
							num4 = num5;
							this.columnAtMaxWidth[k] = true;
						}
						else
						{
							flag = true;
						}
						if (num4 > 0f)
						{
							List<float> list2 = this.cachedColumnWidths;
							int index = k;
							list2[index] += num4;
							usedWidth += num4;
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					return;
				}
				if (!flag)
				{
					goto Block_10;
				}
			}
			Log.Error("Too many iterations.");
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x06012EA2 RID: 77474 RVA: 0x0059A288 File Offset: 0x00598488
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x06012EA3 RID: 77475 RVA: 0x0059A2D4 File Offset: 0x005984D4
		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
				return;
			}
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].ignoreWhenCalculatingOptimalTableSize)
				{
					num += this.GetOptimalWidth(list[i]);
				}
			}
			float num2 = Mathf.Clamp(num + 16f, (float)this.minTableWidth, (float)this.maxTableWidth);
			float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
			num2 = Mathf.Min(num2, (float)UI.screenWidth);
			num3 = Mathf.Min(num3, (float)UI.screenHeight);
			this.cachedSize = new Vector2(num2, num3);
		}

		// Token: 0x06012EA4 RID: 77476 RVA: 0x0059A394 File Offset: 0x00598594
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		// Token: 0x06012EA5 RID: 77477 RVA: 0x0059A3E4 File Offset: 0x005985E4
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x06012EA6 RID: 77478 RVA: 0x0059A430 File Offset: 0x00598630
		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(list[i]), 1f);
			}
			for (int j = 0; j < list.Count; j++)
			{
				List<float> list2 = this.cachedColumnWidths;
				int index = j;
				list2[index] += toDistribute * Mathf.Max(this.GetOptimalWidth(list[j]), 1f) / num;
			}
		}

		// Token: 0x06012EA7 RID: 77479 RVA: 0x0059A4BF File Offset: 0x005986BF
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x06012EA8 RID: 77480 RVA: 0x0059A4D8 File Offset: 0x005986D8
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x06012EA9 RID: 77481 RVA: 0x0059A4F1 File Offset: 0x005986F1
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x06012EAA RID: 77482 RVA: 0x0059A50C File Offset: 0x0059870C
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num = Mathf.Max(num, (float)list[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x06012EAB RID: 77483 RVA: 0x0059A554 File Offset: 0x00598754
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num = Mathf.Max(num, (float)list[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x06012EAC RID: 77484 RVA: 0x0059A59C File Offset: 0x0059879C
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x0400C3DC RID: 50140
		private static readonly Color BorderColor = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x0400C3DD RID: 50141
		private PawnTableDef def;

		// Token: 0x0400C3DE RID: 50142
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x0400C3DF RID: 50143
		private int minTableWidth;

		// Token: 0x0400C3E0 RID: 50144
		private int maxTableWidth;

		// Token: 0x0400C3E1 RID: 50145
		private int minTableHeight;

		// Token: 0x0400C3E2 RID: 50146
		private int maxTableHeight;

		// Token: 0x0400C3E3 RID: 50147
		private Vector2 fixedSize;

		// Token: 0x0400C3E4 RID: 50148
		private bool hasFixedSize;

		// Token: 0x0400C3E5 RID: 50149
		private bool dirty;

		// Token: 0x0400C3E6 RID: 50150
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x0400C3E7 RID: 50151
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x0400C3E8 RID: 50152
		private Vector2 scrollPosition;

		// Token: 0x0400C3E9 RID: 50153
		private PawnColumnDef sortByColumn;

		// Token: 0x0400C3EA RID: 50154
		private bool sortDescending;

		// Token: 0x0400C3EB RID: 50155
		private Vector2 cachedSize;

		// Token: 0x0400C3EC RID: 50156
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x0400C3ED RID: 50157
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x0400C3EE RID: 50158
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x0400C3EF RID: 50159
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		// Token: 0x0400C3F0 RID: 50160
		private List<PawnColumnDef> columns = new List<PawnColumnDef>();

		// Token: 0x0400C3F1 RID: 50161
		private float cachedHeaderHeight;

		// Token: 0x0400C3F2 RID: 50162
		private float cachedHeightNoScrollbar;
	}
}
