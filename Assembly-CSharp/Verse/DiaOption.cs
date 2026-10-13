using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C28 RID: 3112
	public class DiaOption
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004FDC RID: 20444 RVA: 0x0019ACD9 File Offset: 0x00198ED9
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x0019ACF6 File Offset: 0x00198EF6
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x0019AD04 File Offset: 0x00198F04
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x0019AD51 File Offset: 0x00198F51
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x0019AD88 File Offset: 0x00198F88
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x0019ADE8 File Offset: 0x00198FE8
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x0019AE42 File Offset: 0x00199042
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x0019AE52 File Offset: 0x00199052
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x0019AE5C File Offset: 0x0019905C
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (this.hyperlink.def != null)
			{
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f, null, false, null);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled, null))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x0019AF24 File Offset: 0x00199124
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
				return;
			}
			if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x0400396A RID: 14698
		public Window dialog;

		// Token: 0x0400396B RID: 14699
		protected string text;

		// Token: 0x0400396C RID: 14700
		public DiaNode link;

		// Token: 0x0400396D RID: 14701
		public Func<DiaNode> linkLateBind;

		// Token: 0x0400396E RID: 14702
		public bool resolveTree;

		// Token: 0x0400396F RID: 14703
		public Action action;

		// Token: 0x04003970 RID: 14704
		public bool disabled;

		// Token: 0x04003971 RID: 14705
		public string disabledReason;

		// Token: 0x04003972 RID: 14706
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04003973 RID: 14707
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x04003974 RID: 14708
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
