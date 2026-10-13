using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C23 RID: 3107
	public class Dialog_NodeTree : Window
	{
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x0019A5D0 File Offset: 0x001987D0
		public override Vector2 InitialSize
		{
			get
			{
				int num = 480;
				if (this.curNode.options.Count > 5)
				{
					Text.Font = GameFont.Small;
					num += (this.curNode.options.Count - 5) * (int)(Text.LineHeight + 7f);
				}
				return new Vector2(620f, (float)Mathf.Min(num, UI.screenHeight));
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x0019A634 File Offset: 0x00198834
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x0019A648 File Offset: 0x00198848
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = RealTime.LastRealTime + 1f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x0019A6CA File Offset: 0x001988CA
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x0019A6DD File Offset: 0x001988DD
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x0019A6F8 File Offset: 0x001988F8
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x0019A758 File Offset: 0x00198958
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x0019A7DC File Offset: 0x001989DC
		protected void DrawNode(Rect rect)
		{
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Mathf.Min(this.optTotalHeight, rect.height - 100f - this.Margin * 2f);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(num, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text.Resolve());
			Widgets.EndScrollView();
			Widgets.BeginScrollView(new Rect(0f, rect.height - num, rect.width, num), ref this.optsScrollPosition, new Rect(0f, 0f, rect.width - 16f, this.optTotalHeight), true);
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num2, rect.width - 30f, 999f);
				float num4 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num2 += num4 + 7f;
				num3 += num4 + 7f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.optTotalHeight = num3;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x0019A990 File Offset: 0x00198B90
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x04003949 RID: 14665
		private Vector2 scrollPosition;

		// Token: 0x0400394A RID: 14666
		private Vector2 optsScrollPosition;

		// Token: 0x0400394B RID: 14667
		protected string title;

		// Token: 0x0400394C RID: 14668
		protected DiaNode curNode;

		// Token: 0x0400394D RID: 14669
		public Action closeAction;

		// Token: 0x0400394E RID: 14670
		private float makeInteractiveAtTime;

		// Token: 0x0400394F RID: 14671
		public Color screenFillColor = Color.clear;

		// Token: 0x04003950 RID: 14672
		protected float minOptionsAreaHeight;

		// Token: 0x04003951 RID: 14673
		private const float InteractivityDelay = 1f;

		// Token: 0x04003952 RID: 14674
		private const float TitleHeight = 36f;

		// Token: 0x04003953 RID: 14675
		protected const float OptHorMargin = 15f;

		// Token: 0x04003954 RID: 14676
		protected const float OptVerticalSpace = 7f;

		// Token: 0x04003955 RID: 14677
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x04003956 RID: 14678
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x04003957 RID: 14679
		private float optTotalHeight;
	}
}
