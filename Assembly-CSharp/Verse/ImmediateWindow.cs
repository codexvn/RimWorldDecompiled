using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C42 RID: 3138
	public class ImmediateWindow : Window
	{
		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x0019D3D4 File Offset: 0x0019B5D4
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06005036 RID: 20534 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x0019D3E4 File Offset: 0x0019B5E4
		public ImmediateWindow() : base(null)
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0019D437 File Offset: 0x0019B637
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x0019D444 File Offset: 0x0019B644
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			Action action = this.doClickOutsideFunc;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040039CF RID: 14799
		public Action doWindowFunc;

		// Token: 0x040039D0 RID: 14800
		public Action doClickOutsideFunc;
	}
}
