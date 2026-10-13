using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008C1 RID: 2241
	public class MoteText : MoteThrown
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x0012DEDD File Offset: 0x0012C0DD
		protected float TimeBeforeStartFadeout
		{
			get
			{
				if (this.overrideTimeBeforeStartFadeout < 0f)
				{
					return base.SolidTime;
				}
				return this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x0012DEF9 File Offset: 0x0012C0F9
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0012DF20 File Offset: 0x0012C120
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x040029F0 RID: 10736
		public string text;

		// Token: 0x040029F1 RID: 10737
		public Color textColor = Color.white;

		// Token: 0x040029F2 RID: 10738
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
