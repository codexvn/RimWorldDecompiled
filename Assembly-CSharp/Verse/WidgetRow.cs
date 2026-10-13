using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C0F RID: 3087
	public class WidgetRow
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06004EB6 RID: 20150 RVA: 0x00191493 File Offset: 0x0018F693
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x0019149B File Offset: 0x0018F69B
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004EB8 RID: 20152 RVA: 0x001914A3 File Offset: 0x0018F6A3
		// (set) Token: 0x06004EB9 RID: 20153 RVA: 0x001914AB File Offset: 0x0018F6AB
		public float CellGap
		{
			get
			{
				return this.gap;
			}
			set
			{
				this.gap = value;
			}
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x001914B4 File Offset: 0x0018F6B4
		public WidgetRow()
		{
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x001914CE File Offset: 0x0018F6CE
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x001914F5 File Offset: 0x0018F6F5
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x00191523 File Offset: 0x0018F723
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x00191548 File Offset: 0x0018F748
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001915A8 File Offset: 0x0018F7A8
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00191606 File Offset: 0x0018F806
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x0019162F File Offset: 0x0018F82F
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x00191648 File Offset: 0x0018F848
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, Color? backgroundColor = null, Color? mouseoverBackgroundColor = null, bool doMouseoverSound = true, float overrideSize = -1f)
		{
			Rect rect = this.ButtonIconRect(overrideSize);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (mouseoverBackgroundColor != null && Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, mouseoverBackgroundColor.Value, null);
			}
			else if (backgroundColor != null && !Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, backgroundColor.Value, null);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true, null);
			this.IncrementPosition((overrideSize > 0f) ? overrideSize : 24f);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001916FC File Offset: 0x0018F8FC
		public Rect ButtonIconRect(float overrideSize = -1f)
		{
			float num = (overrideSize > 0f) ? overrideSize : 24f;
			float num2 = (24f - num) / 2f;
			this.IncrementYIfWillExceedMaxWidth(num);
			return new Rect(this.LeftX(num) + num2, this.curY + num2, num, num);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x00191748 File Offset: 0x0018F948
		public bool ButtonIconWithBG(Texture2D texture, float width = -1f, string tooltip = null, bool doMouseoverSound = true)
		{
			if (width < 0f)
			{
				width = 24f;
			}
			width += 16f;
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 26f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImageWithBG(rect, texture, new Vector2?(Vector2.one * 24f));
			this.IncrementPosition(width + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001917D8 File Offset: 0x0018F9D8
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex, true, null);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = toggleable ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001918D0 File Offset: 0x0018FAD0
		public Rect Icon(Texture tex, string tooltip = null, float contractedBy = 0f)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			rect = rect.ContractedBy(contractedBy);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x00191940 File Offset: 0x0018FB40
		public Rect DefIcon(ThingDef def, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			Widgets.DefIcon(rect, def, null, 1f, null, false, null, null, null, 1f);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x001919C8 File Offset: 0x0018FBC8
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, float? fixedWidth = null)
		{
			Rect rect = this.ButtonRect(label, fixedWidth);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, active, null);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x00191A08 File Offset: 0x0018FC08
		public Rect ButtonRect(string label, float? fixedWidth = null)
		{
			Vector2 vector = (fixedWidth != null) ? new Vector2(fixedWidth.Value, 24f) : Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect result = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			this.IncrementPosition(result.width + this.gap);
			return result;
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x00191AA0 File Offset: 0x0018FCA0
		public Rect Label(string text, float width = -1f, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.Label(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x00191B38 File Offset: 0x0018FD38
		public Rect LabelEllipses(string text, float width, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.LabelEllipses(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x00191BB8 File Offset: 0x0018FDB8
		public Rect TextFieldNumeric<T>(ref int val, ref string buffer, float width = -1f) where T : struct
		{
			if (width < 0f)
			{
				width = Text.CalcSize(val.ToString()).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			Widgets.TextFieldNumeric<int>(rect, ref val, ref buffer, 0f, 1E+09f);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x00191C3C File Offset: 0x0018FE3C
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (!Text.TinyFontSupported)
				{
					rect2.y -= 2f;
				}
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				using (new TextBlock(new GameFont?(GameFont.Tiny), null, new bool?(false)))
				{
					Widgets.Label(rect2, label);
				}
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x0400387C RID: 14460
		private float startX;

		// Token: 0x0400387D RID: 14461
		private float curX;

		// Token: 0x0400387E RID: 14462
		private float curY;

		// Token: 0x0400387F RID: 14463
		private float maxWidth = 99999f;

		// Token: 0x04003880 RID: 14464
		private float gap;

		// Token: 0x04003881 RID: 14465
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04003882 RID: 14466
		public const float IconSize = 24f;

		// Token: 0x04003883 RID: 14467
		public const float DefaultGap = 4f;

		// Token: 0x04003884 RID: 14468
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x04003885 RID: 14469
		public const float LabelGap = 2f;

		// Token: 0x04003886 RID: 14470
		public const float ButtonExtraSpace = 16f;
	}
}
