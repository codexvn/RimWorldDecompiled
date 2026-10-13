using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AF RID: 431
	public abstract class InspectTabBase
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C1C RID: 3100
		protected abstract float PaneTopY { get; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C1D RID: 3101
		protected abstract bool StillValid { get; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool Hidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00039AE1 File Offset: 0x00037CE1
		public virtual bool VisibleInBlueprintMode { get; } = 1;

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00039AE9 File Offset: 0x00037CE9
		public string TutorHighlightTagClosed
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorHighlightTagClosed == null)
				{
					this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
				}
				return this.cachedTutorHighlightTagClosed;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00039B20 File Offset: 0x00037D20
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00039B70 File Offset: 0x00037D70
		public void DoTabGUI()
		{
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
			{
				if (!this.StillValid || !this.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(rect.AtZero()))
				{
					this.CloseTab();
				}
				try
				{
					this.FillTab();
				}
				catch (Exception ex)
				{
					string str = "Exception filling tab ";
					Type type = this.GetType();
					string str2 = (type != null) ? type.ToString() : null;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), 49827);
				}
			}, true, false, 1f, new Action(this.Notify_ClickOutsideWindow), false);
			this.ExtraOnGUI();
		}

		// Token: 0x06000C24 RID: 3108
		protected abstract void CloseTab();

		// Token: 0x06000C25 RID: 3109
		protected abstract void FillTab();

		// Token: 0x06000C26 RID: 3110 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnOpen()
		{
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TabTick()
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TabUpdate()
		{
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ClickOutsideWindow()
		{
		}

		// Token: 0x040006FC RID: 1788
		public string labelKey;

		// Token: 0x040006FD RID: 1789
		protected Vector2 size;

		// Token: 0x040006FE RID: 1790
		public string tutorTag;

		// Token: 0x040006FF RID: 1791
		private string cachedTutorHighlightTagClosed;
	}
}
