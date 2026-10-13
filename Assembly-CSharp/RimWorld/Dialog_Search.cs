using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031B4 RID: 12724
	public abstract class Dialog_Search<T> : Window where T : class
	{
		// Token: 0x17002E7D RID: 11901
		// (get) Token: 0x06011E60 RID: 73312 RVA: 0x0053448F File Offset: 0x0053268F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(350f, 100f);
			}
		}

		// Token: 0x17002E7E RID: 11902
		// (get) Token: 0x06011E61 RID: 73313 RVA: 0x005344A0 File Offset: 0x005326A0
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return this.quickSearchWidget;
			}
		}

		// Token: 0x17002E7F RID: 11903
		// (get) Token: 0x06011E62 RID: 73314 RVA: 0x005344A8 File Offset: 0x005326A8
		protected bool Searching
		{
			get
			{
				return !this.quickSearchWidget.filter.Text.NullOrEmpty() && this.allElements.Any<T>() && this.searchIndex < this.allElements.Count;
			}
		}

		// Token: 0x17002E80 RID: 11904
		// (get) Token: 0x06011E63 RID: 73315
		protected abstract List<T> SearchSet { get; }

		// Token: 0x17002E81 RID: 11905
		// (get) Token: 0x06011E64 RID: 73316
		protected abstract bool ShouldClose { get; }

		// Token: 0x17002E82 RID: 11906
		// (get) Token: 0x06011E65 RID: 73317
		protected abstract TaggedString SearchLabel { get; }

		// Token: 0x17002E83 RID: 11907
		// (get) Token: 0x06011E66 RID: 73318 RVA: 0x005344E3 File Offset: 0x005326E3
		public T Highlighted
		{
			get
			{
				return this.highlightedElement;
			}
		}

		// Token: 0x06011E67 RID: 73319
		protected abstract void TryAddElement(T element);

		// Token: 0x06011E68 RID: 73320
		protected abstract void TryRemoveElement(T element);

		// Token: 0x06011E69 RID: 73321
		protected abstract void DoIcon(T element, Rect iconRect);

		// Token: 0x06011E6A RID: 73322
		protected abstract void DoLabel(T element, Rect labelRect);

		// Token: 0x06011E6B RID: 73323 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void DoExtraIcon(T element, Rect iconRect)
		{
		}

		// Token: 0x06011E6C RID: 73324
		protected abstract void ClikedOnElement(T element);

		// Token: 0x06011E6D RID: 73325
		protected abstract bool ShouldSkipElement(T element);

		// Token: 0x06011E6E RID: 73326
		protected abstract void OnHighlightUpdate(T element);

		// Token: 0x06011E6F RID: 73327 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void CheckAnyElementRemoved()
		{
		}

		// Token: 0x06011E70 RID: 73328 RVA: 0x005344EB File Offset: 0x005326EB
		public bool IsListed(T element)
		{
			return this.searchResultsSet.Contains(element);
		}

		// Token: 0x06011E71 RID: 73329 RVA: 0x005344F9 File Offset: 0x005326F9
		protected override Rect QuickSearchWidgetRect(Rect winRect, Rect inRect)
		{
			return new Rect(inRect.x, inRect.yMax - 24f, inRect.width, 24f);
		}

		// Token: 0x06011E72 RID: 73330 RVA: 0x00534520 File Offset: 0x00532720
		public Dialog_Search() : base(null)
		{
			this.doCloseX = true;
			this.closeOnAccept = false;
			this.preventCameraMotion = false;
			this.quickSearchWidget = new QuickSearchWidget();
			this.searchResults = new SortedList<string, T>(new DuplicateKeyComparer<string>());
			this.searchResultsSet = new HashSet<T>();
			this.allElements = new List<T>();
		}

		// Token: 0x06011E73 RID: 73331 RVA: 0x0053457C File Offset: 0x0053277C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			this.highlightedElement = default(T);
			float num = Text.CalcHeight(this.SearchLabel, inRect.width);
			Rect rect = new Rect(0f, inRect.yMax - 24f - num, inRect.width, num);
			using (new TextBlock(ColoredText.SubtleGrayColor, TextAnchor.MiddleLeft, true))
			{
				string label;
				if (this.Searching)
				{
					label = QuickSearchUtility.CurrentSearchText;
				}
				else if (this.quickSearchWidget.filter.Text.Length > 0)
				{
					label = ((this.searchResults.Count == 1) ? "MapSearchResultSingular".Translate() : "MapSearchResults".Translate(this.searchResults.Count));
				}
				else
				{
					label = this.SearchLabel;
				}
				Widgets.Label(rect, label);
			}
			if (this.searchResults.Count > 0)
			{
				Rect outRect = new Rect(0f, 0f, inRect.width, inRect.height);
				outRect.yMax = rect.yMin;
				Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, this.scrollHeight);
				Rect rect2 = new Rect(0f, this.scrollPos.y, outRect.width, outRect.height);
				bool flag = this.scrollHeight >= outRect.height;
				Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
				using (new ProfilerBlock("DrawSearchResults"))
				{
					for (int i = 0; i < this.searchResults.Count; i++)
					{
						Rect rect3 = new Rect(0f, 26f * (float)i, inRect.width, 26f);
						if (rect2.Overlaps(rect3))
						{
							if (i % 2 == 1)
							{
								Widgets.DrawLightHighlight(rect3);
							}
							T t = this.searchResults.Values[i];
							if (t != null && !this.ShouldSkipElement(t))
							{
								Rect iconRect = rect3;
								iconRect.xMax = 26f;
								this.DoIcon(t, iconRect);
								Rect iconRect2 = rect3;
								if (flag)
								{
									iconRect2.xMin = rect3.xMax - 26f - 16f;
									iconRect2.width = 26f;
								}
								else
								{
									iconRect2.xMin = rect3.xMax - 26f;
								}
								this.DoExtraIcon(t, iconRect2);
								Rect labelRect = rect3;
								labelRect.xMin = iconRect.xMax + 4f;
								labelRect.xMax = iconRect2.xMin - 4f;
								this.DoLabel(t, labelRect);
								if (Mouse.IsOver(rect3))
								{
									Widgets.DrawHighlight(rect3);
									this.highlightedElement = t;
								}
								if (Widgets.ButtonInvisible(rect3, true))
								{
									this.ClikedOnElement(t);
								}
							}
						}
					}
				}
				Widgets.EndScrollView();
			}
			if (!this.triedToFocus && this.openFrames == 2)
			{
				this.quickSearchWidget.Focus();
				this.triedToFocus = true;
			}
		}

		// Token: 0x06011E74 RID: 73332 RVA: 0x005348E0 File Offset: 0x00532AE0
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			this.openFrames++;
			if (this.ShouldClose)
			{
				this.Close(true);
				return;
			}
			if (this.highlightedElement != null)
			{
				this.OnHighlightUpdate(this.highlightedElement);
			}
			if (this.Searching)
			{
				using (new ProfilerBlock("Searching"))
				{
					for (int i = 0; i < 500; i++)
					{
						this.searchIndex++;
						if (this.searchIndex >= this.allElements.Count)
						{
							this.allElements.Clear();
							break;
						}
						this.TryAddElement(this.allElements[this.searchIndex]);
					}
					return;
				}
			}
			if (Time.frameCount % 20 == 0)
			{
				this.CheckAnyElementRemoved();
			}
		}

		// Token: 0x06011E75 RID: 73333 RVA: 0x005349C4 File Offset: 0x00532BC4
		protected override void SetInitialSizeAndPosition()
		{
			this.scrollHeight = (float)this.searchResults.Count * 26f;
			Vector2 initialSize = this.InitialSize;
			initialSize.y = Mathf.Clamp(initialSize.y + this.scrollHeight, this.InitialSize.y, (float)UI.screenHeight / 2f);
			this.windowRect = new Rect((float)UI.screenWidth - initialSize.x, (float)UI.screenHeight - initialSize.y - 35f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x06011E76 RID: 73334 RVA: 0x00534A5C File Offset: 0x00532C5C
		public override void Notify_CommonSearchChanged()
		{
			this.scrollPos = Vector2.zero;
			this.searchIndex = 0;
			this.searchResults.Clear();
			this.searchResultsSet.Clear();
			this.allElements.Clear();
			this.allElements.AddRange(this.SearchSet);
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06011E77 RID: 73335 RVA: 0x00534AB3 File Offset: 0x00532CB3
		protected bool TextMatch(string text)
		{
			return !text.NullOrEmpty() && text.IndexOf(this.quickSearchWidget.filter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}

		// Token: 0x0400B92D RID: 47405
		protected QuickSearchWidget quickSearchWidget;

		// Token: 0x0400B92E RID: 47406
		protected SortedList<string, T> searchResults;

		// Token: 0x0400B92F RID: 47407
		protected HashSet<T> searchResultsSet;

		// Token: 0x0400B930 RID: 47408
		protected T highlightedElement;

		// Token: 0x0400B931 RID: 47409
		protected Vector2 scrollPos;

		// Token: 0x0400B932 RID: 47410
		protected float scrollHeight;

		// Token: 0x0400B933 RID: 47411
		protected List<T> allElements;

		// Token: 0x0400B934 RID: 47412
		protected int searchIndex;

		// Token: 0x0400B935 RID: 47413
		protected bool triedToFocus;

		// Token: 0x0400B936 RID: 47414
		private int openFrames;

		// Token: 0x0400B937 RID: 47415
		protected const float ElementHeight = 26f;

		// Token: 0x0400B938 RID: 47416
		private const int MaxSearchesPerFrame = 500;
	}
}
