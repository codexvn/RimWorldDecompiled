using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003272 RID: 12914
	public abstract class Page : Window
	{
		// Token: 0x17002EF7 RID: 12023
		// (get) Token: 0x060122AE RID: 74414 RVA: 0x00553D3D File Offset: 0x00551F3D
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x17002EF8 RID: 12024
		// (get) Token: 0x060122AF RID: 74415 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060122B0 RID: 74416 RVA: 0x00553D44 File Offset: 0x00551F44
		public Page() : base(null)
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x060122B1 RID: 74417 RVA: 0x00553D70 File Offset: 0x00551F70
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x060122B2 RID: 74418 RVA: 0x00553DA4 File Offset: 0x00551FA4
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x060122B3 RID: 74419 RVA: 0x00553DEC File Offset: 0x00551FEC
		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true, bool doNextOnKeypress = true)
		{
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			if ((Widgets.ButtonText(new Rect(rect.x, y, Page.BottomButSize.x, Page.BottomButSize.y), label, true, true, true, null) || KeyBindingDefOf.Cancel.KeyDownEvent) && this.CanDoBack())
			{
				this.DoBack();
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				Rect rect2 = new Rect(rect.x + rect.width - Page.BottomButSize.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if ((Widgets.ButtonText(rect2, nextLabel, true, true, true, null) || (doNextOnKeypress && KeyBindingDefOf.Accept.KeyDownEvent)) && this.CanDoNext())
				{
					this.DoNext();
				}
				UIHighlighter.HighlightOpportunity(rect2, "NextPage");
			}
			if (midAct != null && Widgets.ButtonText(new Rect(rect.x + rect.width / 2f - Page.BottomButSize.x / 2f, y, Page.BottomButSize.x, Page.BottomButSize.y), midLabel, true, true, true, null))
			{
				midAct();
			}
		}

		// Token: 0x060122B4 RID: 74420 RVA: 0x00553F64 File Offset: 0x00552164
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x060122B5 RID: 74421 RVA: 0x00553F7E File Offset: 0x0055217E
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x060122B6 RID: 74422 RVA: 0x00553F98 File Offset: 0x00552198
		protected virtual void DoNext()
		{
			if (this.next != null)
			{
				Find.WindowStack.Add(this.next);
			}
			if (this.nextAct != null)
			{
				this.nextAct();
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToNextPage");
			this.Close(true);
		}

		// Token: 0x060122B7 RID: 74423 RVA: 0x00553FF5 File Offset: 0x005521F5
		protected virtual void DoBack()
		{
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToPrevPage");
			if (this.prev != null)
			{
				Find.WindowStack.Add(this.prev);
			}
			this.Close(true);
		}

		// Token: 0x060122B8 RID: 74424 RVA: 0x00554034 File Offset: 0x00552234
		public override void OnCancelKeyPressed()
		{
			if (!this.closeOnCancel)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoBack())
			{
				this.DoBack();
			}
			else
			{
				this.Close(true);
			}
			Event.current.Use();
			base.OnCancelKeyPressed();
		}

		// Token: 0x060122B9 RID: 74425 RVA: 0x00554085 File Offset: 0x00552285
		public override void OnAcceptKeyPressed()
		{
			if (!this.closeOnAccept)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoNext())
			{
				this.DoNext();
			}
			Event.current.Use();
		}

		// Token: 0x0400BCEA RID: 48362
		public Page prev;

		// Token: 0x0400BCEB RID: 48363
		public Page next;

		// Token: 0x0400BCEC RID: 48364
		public Action nextAct;

		// Token: 0x0400BCED RID: 48365
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x0400BCEE RID: 48366
		public const float TitleAreaHeight = 45f;

		// Token: 0x0400BCEF RID: 48367
		public const float BottomButHeight = 38f;

		// Token: 0x0400BCF0 RID: 48368
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
