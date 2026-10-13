using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003197 RID: 12695
	public class Dialog_ModSettings : Window
	{
		// Token: 0x17002E6E RID: 11886
		// (get) Token: 0x06011DD7 RID: 73175 RVA: 0x00530118 File Offset: 0x0052E318
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06011DD8 RID: 73176 RVA: 0x00530129 File Offset: 0x0052E329
		public Dialog_ModSettings(Mod mod) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.mod = mod;
		}

		// Token: 0x06011DD9 RID: 73177 RVA: 0x0053015C File Offset: 0x0052E35C
		public override void PreClose()
		{
			base.PreClose();
			this.mod.WriteSettings();
		}

		// Token: 0x06011DDA RID: 73178 RVA: 0x00530170 File Offset: 0x0052E370
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, inRect.width - 150f - 17f, 35f), this.mod.SettingsCategory());
			Text.Font = GameFont.Small;
			Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y);
			this.mod.DoSettingsWindowContents(inRect2);
		}

		// Token: 0x0400B8C8 RID: 47304
		private Mod mod;

		// Token: 0x0400B8C9 RID: 47305
		private const float TopAreaHeight = 40f;

		// Token: 0x0400B8CA RID: 47306
		private const float TopButtonHeight = 35f;

		// Token: 0x0400B8CB RID: 47307
		private const float TopButtonWidth = 150f;
	}
}
