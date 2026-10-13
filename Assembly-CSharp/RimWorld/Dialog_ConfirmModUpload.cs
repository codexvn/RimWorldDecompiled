using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003299 RID: 12953
	public class Dialog_ConfirmModUpload : Dialog_MessageBox
	{
		// Token: 0x0601239B RID: 74651 RVA: 0x0055B8B4 File Offset: 0x00559AB4
		public Dialog_ConfirmModUpload(ModMetaData mod, Action acceptAction) : base("ConfirmSteamWorkshopUpload".Translate(), "Confirm".Translate(), acceptAction, "GoBack".Translate(), null, null, true, acceptAction, null, WindowLayer.Dialog)
		{
			this.mod = mod;
		}

		// Token: 0x0601239C RID: 74652 RVA: 0x0055B900 File Offset: 0x00559B00
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Vector2 vector = new Vector2(inRect.x + 10f, inRect.height - 35f - 24f - 10f);
			Widgets.Checkbox(vector, ref this.mod.translationMod, 24f, false, false, null, null);
			Widgets.Label(new Rect(vector.x + 24f + 10f, vector.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f), "TagAsTranslation".Translate());
		}

		// Token: 0x0400BDAF RID: 48559
		private ModMetaData mod;
	}
}
