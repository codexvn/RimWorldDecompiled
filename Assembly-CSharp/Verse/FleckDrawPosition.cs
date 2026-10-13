using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040A RID: 1034
	public struct FleckDrawPosition
	{
		// Token: 0x06001A33 RID: 6707 RVA: 0x000842E2 File Offset: 0x000824E2
		public FleckDrawPosition(Vector3 worldPos, float height, Vector3 anchorOffset, Vector3 unattachedOffset)
		{
			this.worldPosition = worldPos;
			this.height = height;
			this.anchorOffset = anchorOffset;
			this.unattachedDrawOffset = unattachedOffset;
			this.attachedOffset = Vector3.zero;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x0008430C File Offset: 0x0008250C
		public FleckDrawPosition(Vector3 worldPos, float height, Vector3 anchorOffset, Vector3 unattachedOffset, Vector3 attachedOffset)
		{
			this.worldPosition = worldPos;
			this.height = height;
			this.anchorOffset = anchorOffset;
			this.unattachedDrawOffset = unattachedOffset;
			this.attachedOffset = attachedOffset;
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x00084333 File Offset: 0x00082533
		public Vector3 ExactPosition
		{
			get
			{
				return this.worldPosition + this.unattachedDrawOffset + this.attachedOffset + Vector3.forward * this.height + this.anchorOffset;
			}
		}

		// Token: 0x04001728 RID: 5928
		public Vector3 worldPosition;

		// Token: 0x04001729 RID: 5929
		public float height;

		// Token: 0x0400172A RID: 5930
		public Vector3 anchorOffset;

		// Token: 0x0400172B RID: 5931
		public Vector3 unattachedDrawOffset;

		// Token: 0x0400172C RID: 5932
		public Vector3 attachedOffset;
	}
}
