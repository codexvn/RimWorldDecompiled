using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000095 RID: 149
	public abstract class Dialog_OptionLister : Window_Dev
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000940B File Offset: 0x0000760B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00011A9C File Offset: 0x0000FC9C
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

		// Token: 0x06000331 RID: 817 RVA: 0x00011B1C File Offset: 0x0000FD1C
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00011B68 File Offset: 0x0000FD68
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.focusOnFilterOnOpen)
			{
				this.focusFilter = true;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00011B80 File Offset: 0x0000FD80
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
			if (Event.current.type == EventType.KeyDown && (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent || KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent))
			{
				return;
			}
			this.filter = DevGUI.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.Repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = "";
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			this.curX = 0f;
			this.curY = 0f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, num2);
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			DevGUI.BeginGroup(inRect);
			float columnWidth = (viewRect.width - 17f * (float)(num - 1)) / (float)num;
			this.DoListingItems(inRect.AtZero(), columnWidth);
			DevGUI.EndGroup();
			DevGUI.EndScrollView();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00011D0A File Offset: 0x0000FF0A
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x06000335 RID: 821
		protected abstract void DoListingItems(Rect inRect, float columnWidth);

		// Token: 0x06000336 RID: 822 RVA: 0x00011D17 File Offset: 0x0000FF17
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040001E9 RID: 489
		protected Vector2 scrollPosition;

		// Token: 0x040001EA RID: 490
		protected string filter = "";

		// Token: 0x040001EB RID: 491
		protected float totalOptionsHeight;

		// Token: 0x040001EC RID: 492
		protected bool focusOnFilterOnOpen = true;

		// Token: 0x040001ED RID: 493
		private bool focusFilter;

		// Token: 0x040001EE RID: 494
		protected float curY;

		// Token: 0x040001EF RID: 495
		protected float curX;

		// Token: 0x040001F0 RID: 496
		private int boundingRectCachedForFrame = -1;

		// Token: 0x040001F1 RID: 497
		private Rect? boundingRectCached;

		// Token: 0x040001F2 RID: 498
		private Rect? boundingRect;

		// Token: 0x040001F3 RID: 499
		public float verticalSpacing = 2f;

		// Token: 0x040001F4 RID: 500
		protected const string FilterControlName = "DebugFilter";
	}
}
