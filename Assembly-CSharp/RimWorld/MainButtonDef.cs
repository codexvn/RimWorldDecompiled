using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A85 RID: 6789
	public class MainButtonDef : Def
	{
		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x06009841 RID: 38977 RVA: 0x002C6A8A File Offset: 0x002C4C8A
		public MainButtonWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (MainButtonWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x06009842 RID: 38978 RVA: 0x002C6ABC File Offset: 0x002C4CBC
		public MainTabWindow TabWindow
		{
			get
			{
				if (this.tabWindowInt == null && this.tabWindowClass != null)
				{
					this.tabWindowInt = (MainTabWindow)Activator.CreateInstance(this.tabWindowClass);
					this.tabWindowInt.def = this;
				}
				return this.tabWindowInt;
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x06009843 RID: 38979 RVA: 0x002C6AFC File Offset: 0x002C4CFC
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = this.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x06009844 RID: 38980 RVA: 0x002C6B30 File Offset: 0x002C4D30
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(this.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x06009845 RID: 38981 RVA: 0x002C6B70 File Offset: 0x002C4D70
		public float ShortenedLabelCapWidth
		{
			get
			{
				if (this.cachedShortenedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedShortenedLabelCapWidth = Text.CalcSize(this.ShortenedLabelCap).x;
					Text.Font = font;
				}
				return this.cachedShortenedLabelCapWidth;
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x06009846 RID: 38982 RVA: 0x002C6BAB File Offset: 0x002C4DAB
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.iconPath != null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x06009847 RID: 38983 RVA: 0x002C6BDB File Offset: 0x002C4DDB
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x06009848 RID: 38984 RVA: 0x002C6BFE File Offset: 0x002C4DFE
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		// Token: 0x06009849 RID: 38985 RVA: 0x002C6C21 File Offset: 0x002C4E21
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		// Token: 0x04006640 RID: 26176
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x04006641 RID: 26177
		public Type tabWindowClass;

		// Token: 0x04006642 RID: 26178
		public bool buttonVisible = true;

		// Token: 0x04006643 RID: 26179
		public int order;

		// Token: 0x04006644 RID: 26180
		public KeyCode defaultHotKey;

		// Token: 0x04006645 RID: 26181
		public bool canBeTutorDenied = true;

		// Token: 0x04006646 RID: 26182
		public bool validWithoutMap;

		// Token: 0x04006647 RID: 26183
		public bool minimized;

		// Token: 0x04006648 RID: 26184
		public string iconPath;

		// Token: 0x04006649 RID: 26185
		public bool closesWorldView;

		// Token: 0x0400664A RID: 26186
		public bool validWithClassicIdeo = true;

		// Token: 0x0400664B RID: 26187
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		// Token: 0x0400664C RID: 26188
		[Unsaved(false)]
		public string cachedTutorTag;

		// Token: 0x0400664D RID: 26189
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		// Token: 0x0400664E RID: 26190
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		// Token: 0x0400664F RID: 26191
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		// Token: 0x04006650 RID: 26192
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		// Token: 0x04006651 RID: 26193
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x04006652 RID: 26194
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x04006653 RID: 26195
		[Unsaved(false)]
		private Texture2D icon;

		// Token: 0x04006654 RID: 26196
		public const int ButtonHeight = 35;
	}
}
