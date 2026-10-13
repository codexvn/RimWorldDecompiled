using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003156 RID: 12630
	public class Dialog_EditDeity : Window
	{
		// Token: 0x17002E30 RID: 11824
		// (get) Token: 0x06011C49 RID: 72777 RVA: 0x005225E3 File Offset: 0x005207E3
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 250f);
			}
		}

		// Token: 0x06011C4A RID: 72778 RVA: 0x005225F4 File Offset: 0x005207F4
		public Dialog_EditDeity(IdeoFoundation_Deity.Deity deity, Ideo ideo) : base(null)
		{
			this.deity = deity;
			this.ideo = ideo;
			this.absorbInputAroundWindow = true;
			this.newDeityName = deity.name;
			this.newDeityTitle = deity.type;
			this.newDeityGender = deity.gender;
		}

		// Token: 0x06011C4B RID: 72779 RVA: 0x00522641 File Offset: 0x00520841
		public override void OnAcceptKeyPressed()
		{
			this.ApplyChanges();
			Event.current.Use();
		}

		// Token: 0x06011C4C RID: 72780 RVA: 0x00522654 File Offset: 0x00520854
		public override void DoWindowContents(Rect rect)
		{
			float num = rect.x + rect.width / 3f;
			float width = rect.xMax - num;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditDeity".Translate());
			Text.Font = GameFont.Small;
			float num2 = rect.y + 35f + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityName".Translate());
			this.newDeityName = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityName);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityTitle".Translate());
			this.newDeityTitle = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityTitle);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityGender".Translate());
			Rect rect2 = new Rect(num, num2, Dialog_EditDeity.EditFieldHeight + 8f + Text.CalcSize(this.newDeityGender.GetLabel(false).CapitalizeFirst()).x, Dialog_EditDeity.EditFieldHeight);
			Rect rect3 = new Rect(rect2.x, num2, Dialog_EditDeity.EditFieldHeight, Dialog_EditDeity.EditFieldHeight);
			GUI.DrawTexture(rect3.ContractedBy(2f), this.newDeityGender.GetIcon());
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect3.xMax + 4f, num2, rect2.width - rect3.width, Dialog_EditDeity.EditFieldHeight), this.newDeityGender.GetLabel(false).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawHighlightIfMouseover(rect2);
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				Gender[] array = (Gender[])Enum.GetValues(typeof(Gender));
				for (int i = 0; i < array.Length; i++)
				{
					Gender g = array[i];
					list.Add(new FloatMenuOption(g.GetLabel(false).CapitalizeFirst(), delegate()
					{
						this.newDeityGender = g;
					}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "Back".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditDeity.ButSize.x, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x06011C4D RID: 72781 RVA: 0x005229C8 File Offset: 0x00520BC8
		private void ApplyChanges()
		{
			this.deity.name = this.newDeityName;
			this.deity.type = this.newDeityTitle;
			this.deity.gender = this.newDeityGender;
			this.ideo.RegenerateAllPreceptNames();
			this.ideo.RegenerateDescription(false);
			this.Close(true);
		}

		// Token: 0x0400B75C RID: 46940
		private IdeoFoundation_Deity.Deity deity;

		// Token: 0x0400B75D RID: 46941
		private Ideo ideo;

		// Token: 0x0400B75E RID: 46942
		private string newDeityName;

		// Token: 0x0400B75F RID: 46943
		private string newDeityTitle;

		// Token: 0x0400B760 RID: 46944
		private Gender newDeityGender;

		// Token: 0x0400B761 RID: 46945
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x0400B762 RID: 46946
		private static readonly float EditFieldHeight = 30f;
	}
}
