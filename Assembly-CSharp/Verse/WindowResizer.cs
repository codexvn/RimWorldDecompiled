using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C47 RID: 3143
	public class WindowResizer
	{
		// Token: 0x06005066 RID: 20582 RVA: 0x0019DC3C File Offset: 0x0019BE3C
		public Rect DoResizeControl(Rect winRect)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.isResizing = true;
				this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
			}
			if (this.isResizing)
			{
				winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
				winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
				if (winRect.width < this.minWindowSize.x)
				{
					winRect.width = this.minWindowSize.x;
				}
				if (winRect.height < this.minWindowSize.y)
				{
					winRect.height = this.minWindowSize.y;
				}
				winRect.xMax = Mathf.Min((float)UI.screenWidth, winRect.xMax);
				winRect.yMax = Mathf.Min((float)UI.screenHeight, winRect.yMax);
				if (Event.current.type == EventType.MouseUp)
				{
					this.isResizing = false;
				}
			}
			Widgets.ButtonImage(rect, TexUI.WinExpandWidget, true, null);
			return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
		}

		// Token: 0x04003A03 RID: 14851
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04003A04 RID: 14852
		private bool isResizing;

		// Token: 0x04003A05 RID: 14853
		private Rect resizeStart;

		// Token: 0x04003A06 RID: 14854
		private const float ResizeButtonSize = 24f;
	}
}
