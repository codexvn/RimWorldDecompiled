using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DB RID: 987
	internal sealed class DebugCell
	{
		// Token: 0x0600194B RID: 6475 RVA: 0x0007FC3D File Offset: 0x0007DE3D
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0007FC70 File Offset: 0x0007DE70
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04001655 RID: 5717
		public IntVec3 c;

		// Token: 0x04001656 RID: 5718
		public string displayString;

		// Token: 0x04001657 RID: 5719
		public float colorPct;

		// Token: 0x04001658 RID: 5720
		public int ticksLeft;

		// Token: 0x04001659 RID: 5721
		public Material customMat;
	}
}
