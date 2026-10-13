using System;

namespace Verse
{
	// Token: 0x02000217 RID: 535
	public class IconForStuffAppearance
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00042E60 File Offset: 0x00041060
		public StuffAppearanceDef Appearance
		{
			get
			{
				return this.appearance;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x00042E68 File Offset: 0x00041068
		public string IconPath
		{
			get
			{
				return this.iconPath;
			}
		}

		// Token: 0x04000888 RID: 2184
		private StuffAppearanceDef appearance;

		// Token: 0x04000889 RID: 2185
		[NoTranslate]
		private string iconPath;
	}
}
