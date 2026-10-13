using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200009F RID: 159
	public abstract class Window_DevListing : Window_Dev
	{
		// Token: 0x06000354 RID: 852 RVA: 0x00012CB4 File Offset: 0x00010EB4
		protected Window_DevListing()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000355 RID: 853 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00012D06 File Offset: 0x00010F06
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(275f, 400f);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000358 RID: 856 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AutoUpdate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00012D17 File Offset: 0x00010F17
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00012D1F File Offset: 0x00010F1F
		protected bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = (value || this.dirty);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012D34 File Offset: 0x00010F34
		public sealed override void DoWindowContents(Rect inRect)
		{
			this.y = 4f;
			this.dirty = false;
			this.container = inRect;
			using (new TextBlock(GameFont.Tiny))
			{
				this.DoWindowListing();
			}
			this.windowRect.height = this.y + 4f;
			if (this.dirty && this.AutoUpdate)
			{
				this.OnChanged();
			}
		}

		// Token: 0x0600035C RID: 860
		protected abstract void DoWindowListing();

		// Token: 0x0600035D RID: 861 RVA: 0x00012DB8 File Offset: 0x00010FB8
		protected virtual void OnChanged()
		{
			this.dirty = false;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00012DC4 File Offset: 0x00010FC4
		protected void PrintLabel(string text, GameFont font = GameFont.Tiny)
		{
			using (new TextBlock(font))
			{
				DevGUI.Label(new Rect(this.container.x, this.y, this.container.width, 20f), text);
				this.y += 20f;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00012E38 File Offset: 0x00011038
		protected bool SliderFieldInt(string label, ref float sliderValue, ref int value, int min = 0, int max = 1, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			rect3.yMin += rect3.height / 4f;
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			sliderValue = DevGUI.HorizontalSlider(rect3, sliderValue, (float)min, (float)max);
			int num = Mathf.RoundToInt(sliderValue);
			if (num != value)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012EBC File Offset: 0x000110BC
		protected bool SliderFieldFloat(string label, ref float value, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			rect3.yMin += rect3.height / 4f;
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float b = value;
			value = DevGUI.HorizontalSlider(rect3, value, min, max);
			if (!Mathf.Approximately(value, b))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00012F38 File Offset: 0x00011138
		protected bool SliderFieldFloat(string label, ref float value, ref bool enabled, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			Rect rect4 = rect3.RightPartPixels(rect.height);
			rect3.yMin += rect3.height / 4f;
			rect3.xMax = rect4.x;
			DevGUI.Label(rect2, label);
			bool flag = enabled;
			DevGUI.Checkbox(rect4, ref enabled);
			if (enabled != flag)
			{
				this.dirty = true;
			}
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float b = value;
			value = DevGUI.HorizontalSlider(rect3, value, min, max);
			if (!Mathf.Approximately(value, b))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012FF0 File Offset: 0x000111F0
		protected bool SliderFieldDouble(string label, ref double value, ref bool fine, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			Rect rect4 = rect3.RightPartPixels(rect.height);
			rect3.yMin += rect3.height / 4f;
			rect3.xMax = rect4.x;
			bool flag = fine;
			DevGUI.Checkbox(rect4, ref fine);
			TooltipHandler.TipRegion(rect4, "Toggle fine value adjustment");
			if (fine)
			{
				max /= 10f;
			}
			if ((fine != flag & fine) && value > (double)max)
			{
				value = (double)max;
				this.dirty = true;
			}
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float num = (float)value;
			float num2 = DevGUI.HorizontalSlider(rect3, num, min, max);
			if (!Mathf.Approximately(num, num2))
			{
				value = (double)num2;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000130E0 File Offset: 0x000112E0
		protected bool TextFieldInt(string label, ref string source, ref int value, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			DevGUI.Label(rect2, label);
			source = DevGUI.TextField(rect3, source);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			int num;
			if (int.TryParse(source, out num) && num != value)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00013144 File Offset: 0x00011344
		protected bool TextFieldDouble(string label, ref string source, ref double value, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			DevGUI.Label(rect2, label);
			source = DevGUI.TextField(rect3, source);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			double num;
			if (double.TryParse(source, out num) && Math.Abs(num - value) > 9.999999747378752E-05)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000131B8 File Offset: 0x000113B8
		protected bool Button(string label)
		{
			if (DevGUI.ButtonText(this.TakeRow(), label, null))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000131E8 File Offset: 0x000113E8
		protected bool Toggle(string label, ref bool value)
		{
			this.y += 4f;
			if (DevGUI.ButtonText(this.TakeRow(), label, null))
			{
				value = !value;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001322F File Offset: 0x0001142F
		protected void Gap(float height)
		{
			this.y += height;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0001323F File Offset: 0x0001143F
		protected Rect TakeRow()
		{
			Rect result = new Rect(this.container.x, this.y, this.container.width, 20f);
			this.y += 24f;
			return result;
		}

		// Token: 0x04000210 RID: 528
		private Rect container;

		// Token: 0x04000211 RID: 529
		private float y;

		// Token: 0x04000212 RID: 530
		private bool dirty;
	}
}
