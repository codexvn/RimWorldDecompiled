using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000400 RID: 1024
	public struct FleckAttachLink
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x000831A8 File Offset: 0x000813A8
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000831B5 File Offset: 0x000813B5
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x000831BD File Offset: 0x000813BD
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000831C5 File Offset: 0x000813C5
		public FleckAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.detachAfterTicks = -1;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000831F0 File Offset: 0x000813F0
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = (this.targetInt.Thing.DrawPosHeld ?? this.lastDrawPosInt);
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x040016F8 RID: 5880
		private TargetInfo targetInt;

		// Token: 0x040016F9 RID: 5881
		private Vector3 lastDrawPosInt;

		// Token: 0x040016FA RID: 5882
		public int detachAfterTicks;

		// Token: 0x040016FB RID: 5883
		public static readonly FleckAttachLink Invalid = new FleckAttachLink(TargetInfo.Invalid);
	}
}
