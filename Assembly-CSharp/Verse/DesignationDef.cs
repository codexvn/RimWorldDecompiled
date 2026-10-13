using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000273 RID: 627
	public class DesignationDef : Def
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004B7B3 File Offset: 0x000499B3
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x04000C39 RID: 3129
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000C3A RID: 3130
		public TargetType targetType;

		// Token: 0x04000C3B RID: 3131
		public bool removeIfBuildingDespawned;

		// Token: 0x04000C3C RID: 3132
		public bool designateCancelable = true;

		// Token: 0x04000C3D RID: 3133
		public bool shouldBatchDraw = true;

		// Token: 0x04000C3E RID: 3134
		[Unsaved(false)]
		public Material iconMat;
	}
}
