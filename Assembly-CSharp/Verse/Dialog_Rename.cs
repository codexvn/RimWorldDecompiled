using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C32 RID: 3122
	public abstract class Dialog_Rename<T> : Window where T : class, IRenameable
	{
		// Token: 0x06005004 RID: 20484 RVA: 0x0019C744 File Offset: 0x0019A944
		protected Dialog_Rename(T renaming) : base(null)
		{
			this.renaming = renaming;
			T t = renaming;
			this.curName = ((t != null) ? t.RenamableLabel : null);
			this.doCloseX = true;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x0019C79A File Offset: 0x0019A99A
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x0019C7AC File Offset: 0x0019A9AC
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x0019C7B0 File Offset: 0x0019A9B0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x0019C7C1 File Offset: 0x0019A9C1
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x0019C7D0 File Offset: 0x0019A9D0
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			return name.Length != 0;
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x0019C7E0 File Offset: 0x0019A9E0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect = new Rect(inRect);
			Text.Font = GameFont.Medium;
			rect.height = Text.LineHeight + 10f;
			Widgets.Label(rect, "Rename".Translate());
			Text.Font = GameFont.Small;
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, rect.height, inRect.width, 35f), this.curName);
			if (this.AcceptsInput && text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			else if (!this.AcceptsInput)
			{
				((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 10f, inRect.width - 15f - 15f, 35f), "OK", true, true, true, null) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					return;
				}
				else
				{
					if (this.renaming != null)
					{
						this.renaming.RenamableLabel = this.curName;
					}
					this.OnRenamed(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void OnRenamed(string name)
		{
		}

		// Token: 0x040039B3 RID: 14771
		protected string curName;

		// Token: 0x040039B4 RID: 14772
		private bool focusedRenameField;

		// Token: 0x040039B5 RID: 14773
		private int startAcceptingInputAtFrame;

		// Token: 0x040039B6 RID: 14774
		protected readonly T renaming;
	}
}
