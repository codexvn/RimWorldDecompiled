using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B6A RID: 2922
	public class FloatMenuGridOption
	{
		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x0017CFCF File Offset: 0x0017B1CF
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x0017CFDA File Offset: 0x0017B1DA
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return MenuOptionPriority.Default;
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0017CFE8 File Offset: 0x0017B1E8
		public FloatMenuGridOption(Texture2D texture, Action action, Color? color = null, TipSignal? tooltip = null)
		{
			this.texture = texture;
			this.action = action;
			this.color = (color ?? Color.white);
			this.tooltip = tooltip;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0017D05C File Offset: 0x0017B25C
		public bool OnGUI(Rect rect)
		{
			bool flag = !this.Disabled && Mouse.IsOver(rect);
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			GUI.color = new Color(this.color.r, this.color.g, this.color.b, this.color.a * color.a);
			Rect rect2 = rect.ContractedBy(2f);
			if (!flag)
			{
				rect2 = rect2.ContractedBy(2f);
			}
			Material mat = this.Disabled ? TexUI.GrayscaleGUI : null;
			Widgets.DrawTextureFitted(rect2, this.texture, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, mat, 1f);
			GUI.color = color;
			Action<Rect> action = this.postDrawAction;
			if (action != null)
			{
				action(rect2);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Chosen();
				return true;
			}
			return false;
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0017D1E5 File Offset: 0x0017B3E5
		public void Chosen()
		{
			if (this.Disabled)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
				return;
			}
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04003531 RID: 13617
		public Texture2D texture;

		// Token: 0x04003532 RID: 13618
		public Color color = Color.white;

		// Token: 0x04003533 RID: 13619
		public Action action;

		// Token: 0x04003534 RID: 13620
		public TipSignal? tooltip;

		// Token: 0x04003535 RID: 13621
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04003536 RID: 13622
		public Action<Rect> postDrawAction;
	}
}
