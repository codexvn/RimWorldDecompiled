using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000A2 RID: 162
	public abstract class EditWindow : Window_Dev
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0001330D File Offset: 0x0001150D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0001331E File Offset: 0x0001151E
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00013328 File Offset: 0x00011528
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00013378 File Offset: 0x00011578
		protected void DoRowButton(ref float x, float y, string text, string tooltip, Action action)
		{
			Vector2 vector = Text.CalcSize(text);
			Rect rect = new Rect(x, y, vector.x + 10f, 24f);
			if (DevGUI.ButtonText(rect, text, null))
			{
				action();
			}
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			x += rect.width + 4f;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000133E8 File Offset: 0x000115E8
		protected void DoImageToggle(ref float x, float y, Texture2D texture, string tooltip, ref bool toggle)
		{
			Rect rect = new Rect(x, y, 24f, 24f);
			DevGUI.CheckboxImage(rect, texture, ref toggle);
			TooltipHandler.TipRegion(rect, tooltip);
			x += 28f;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001341C File Offset: 0x0001161C
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this && Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x04000214 RID: 532
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04000215 RID: 533
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04000216 RID: 534
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
