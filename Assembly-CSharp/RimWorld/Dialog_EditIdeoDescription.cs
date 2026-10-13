using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003158 RID: 12632
	public class Dialog_EditIdeoDescription : Window
	{
		// Token: 0x17002E31 RID: 11825
		// (get) Token: 0x06011C51 RID: 72785 RVA: 0x00522A59 File Offset: 0x00520C59
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 400f);
			}
		}

		// Token: 0x06011C52 RID: 72786 RVA: 0x00522A6A File Offset: 0x00520C6A
		public Dialog_EditIdeoDescription(Ideo ideo) : base(null)
		{
			this.ideo = ideo;
			this.newDescription = ideo.description;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011C53 RID: 72787 RVA: 0x00522A8D File Offset: 0x00520C8D
		public override void OnAcceptKeyPressed()
		{
			Event.current.Use();
		}

		// Token: 0x06011C54 RID: 72788 RVA: 0x00522A9C File Offset: 0x00520C9C
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditNarrative".Translate());
			Text.Font = GameFont.Small;
			float num = rect.y + 35f + 10f;
			string a = Widgets.TextArea(new Rect(rect.x, num, rect.width, rect.height - Dialog_EditIdeoDescription.ButSize.y - 17f - num), this.newDescription, false);
			if (a != this.newDescription)
			{
				this.newDescription = a;
				this.newDescriptionTemplate = null;
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Cancel".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_EditIdeoDescription.ButSize.x / 2f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Randomize".Translate(), true, true, true, null))
			{
				IdeoDescriptionResult ideoDescriptionResult = this.ideo.GetNewDescription(true);
				this.newDescription = ideoDescriptionResult.text;
				this.newDescriptionTemplate = ideoDescriptionResult.template;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoDescription.ButSize.x, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x06011C55 RID: 72789 RVA: 0x00522CA8 File Offset: 0x00520EA8
		private void ApplyChanges()
		{
			if (this.ideo.description != this.newDescription)
			{
				this.ideo.description = this.newDescription;
				this.ideo.descriptionTemplate = this.newDescriptionTemplate;
				this.ideo.descriptionLocked = true;
			}
			this.Close(true);
		}

		// Token: 0x0400B765 RID: 46949
		private Ideo ideo;

		// Token: 0x0400B766 RID: 46950
		private string newDescription;

		// Token: 0x0400B767 RID: 46951
		private string newDescriptionTemplate;

		// Token: 0x0400B768 RID: 46952
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x0400B769 RID: 46953
		private const float HeaderHeight = 35f;
	}
}
