using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000216 RID: 534
	public class ColorForStuff
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x00042E3D File Offset: 0x0004103D
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x00042E45 File Offset: 0x00041045
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x04000886 RID: 2182
		private ThingDef stuff;

		// Token: 0x04000887 RID: 2183
		private Color color = Color.white;
	}
}
