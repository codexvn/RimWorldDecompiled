using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008BC RID: 2236
	public struct MoteAttachLink
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003890 RID: 14480 RVA: 0x0012D579 File Offset: 0x0012B779
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003891 RID: 14481 RVA: 0x0012D586 File Offset: 0x0012B786
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06003892 RID: 14482 RVA: 0x0012D58E File Offset: 0x0012B78E
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0012D596 File Offset: 0x0012B796
		public MoteAttachLink(TargetInfo target, Vector3 offset, bool rotateWithTarget = false)
		{
			this.targetInt = target;
			this.offsetInt = offset;
			this.rotateWithTarget = rotateWithTarget;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0012D5C7 File Offset: 0x0012B7C7
		public void UpdateTarget(TargetInfo target, Vector3 offset)
		{
			this.targetInt = target;
			this.offsetInt = offset;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0012D5D8 File Offset: 0x0012B7D8
		public Vector3 GetOffset()
		{
			if (!this.rotateWithTarget || !this.targetInt.HasThing || !this.targetInt.Thing.SpawnedOrAnyParentSpawned)
			{
				return this.offsetInt;
			}
			return this.offsetInt.RotatedBy(this.targetInt.Thing.Rotation);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0012D630 File Offset: 0x0012B830
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing && this.targetInt.Thing.SpawnedOrAnyParentSpawned)
			{
				this.lastDrawPosInt = this.targetInt.Thing.SpawnedParentOrMe.DrawPos + this.GetOffset();
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted() + this.GetOffset();
		}

		// Token: 0x040029E3 RID: 10723
		private TargetInfo targetInt;

		// Token: 0x040029E4 RID: 10724
		private Vector3 offsetInt;

		// Token: 0x040029E5 RID: 10725
		private Vector3 lastDrawPosInt;

		// Token: 0x040029E6 RID: 10726
		public bool rotateWithTarget;

		// Token: 0x040029E7 RID: 10727
		public static readonly MoteAttachLink Invalid = new MoteAttachLink(TargetInfo.Invalid, Vector3.zero, false);
	}
}
