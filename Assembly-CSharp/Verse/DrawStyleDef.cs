using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000277 RID: 631
	public class DrawStyleDef : Def
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0004B845 File Offset: 0x00049A45
		public DrawStyle DrawStyleWorker
		{
			get
			{
				return GenWorker<DrawStyle>.Get(this.drawStyleType);
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0004B852 File Offset: 0x00049A52
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
			});
		}

		// Token: 0x04000C46 RID: 3142
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x04000C47 RID: 3143
		public bool drawOutline;

		// Token: 0x04000C48 RID: 3144
		public bool drawArea = true;

		// Token: 0x04000C49 RID: 3145
		public bool drawShortSideMeasurement = true;

		// Token: 0x04000C4A RID: 3146
		public bool canSnap = true;

		// Token: 0x04000C4B RID: 3147
		public Type drawStyleType;

		// Token: 0x04000C4C RID: 3148
		public Texture2D uiIcon = BaseContent.BadTex;
	}
}
