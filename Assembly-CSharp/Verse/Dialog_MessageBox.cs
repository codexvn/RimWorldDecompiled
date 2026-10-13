using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public class Dialog_MessageBox : Window
	{
		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x00199A88 File Offset: 0x00197C88
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x00199A99 File Offset: 0x00197C99
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x00199AAE File Offset: 0x00197CAE
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x00199AC0 File Offset: 0x00197CC0
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false, string title = null, WindowLayer layer = WindowLayer.Dialog)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive, confirmedAct, delegate()
			{
			}, layer);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x00199B18 File Offset: 0x00197D18
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, Action cancelAct, bool destructive = false, string title = null, WindowLayer layer = WindowLayer.Dialog)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), cancelAct, title, destructive, confirmedAct, cancelAct, layer);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x00199B54 File Offset: 0x00197D54
		public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null, WindowLayer layer = WindowLayer.Dialog) : base(null)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			this.layer = layer;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x00199C48 File Offset: 0x00197E48
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			if (this.image != null)
			{
				float num2 = (float)this.image.width / (float)this.image.height;
				float num3 = 270f * num2;
				GUI.DrawTexture(new Rect(inRect.x + (inRect.width - num3) / 2f, num, num3, 270f), this.image);
				num += 280f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num4 = this.buttonCText.NullOrEmpty() ? 2 : 3;
			float num5 = inRect.width / (float)num4;
			float width2 = num5 - 10f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = this.InteractionDelayExpired ? this.buttonAText : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
			if (Widgets.ButtonText(new Rect(num5 * (float)(num4 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true, null) && this.InteractionDelayExpired)
			{
				if (this.buttonAAction != null)
				{
					this.buttonAAction();
				}
				this.Close(true);
			}
			GUI.color = Color.white;
			if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true, null))
			{
				if (this.buttonBAction != null)
				{
					this.buttonBAction();
				}
				this.Close(true);
			}
			if (this.buttonCText != null && Widgets.ButtonText(new Rect(num5, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true, null))
			{
				if (this.buttonCAction != null)
				{
					this.buttonCAction();
				}
				if (this.buttonCClose)
				{
					this.Close(true);
				}
			}
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x00199F4F File Offset: 0x0019814F
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
				Event.current.Use();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x00199F7C File Offset: 0x0019817C
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
				Event.current.Use();
				return;
			}
			base.OnAcceptKeyPressed();
		}

		// Token: 0x04003933 RID: 14643
		public TaggedString text;

		// Token: 0x04003934 RID: 14644
		public string title;

		// Token: 0x04003935 RID: 14645
		public string buttonAText;

		// Token: 0x04003936 RID: 14646
		public Action buttonAAction;

		// Token: 0x04003937 RID: 14647
		public bool buttonADestructive;

		// Token: 0x04003938 RID: 14648
		public string buttonBText;

		// Token: 0x04003939 RID: 14649
		public Action buttonBAction;

		// Token: 0x0400393A RID: 14650
		public string buttonCText;

		// Token: 0x0400393B RID: 14651
		public Action buttonCAction;

		// Token: 0x0400393C RID: 14652
		public bool buttonCClose = true;

		// Token: 0x0400393D RID: 14653
		public float interactionDelay;

		// Token: 0x0400393E RID: 14654
		public Action acceptAction;

		// Token: 0x0400393F RID: 14655
		public Action cancelAction;

		// Token: 0x04003940 RID: 14656
		public Texture2D image;

		// Token: 0x04003941 RID: 14657
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003942 RID: 14658
		private float creationRealTime = -1f;

		// Token: 0x04003943 RID: 14659
		private const float TitleHeight = 42f;

		// Token: 0x04003944 RID: 14660
		protected const float ButtonHeight = 35f;
	}
}
