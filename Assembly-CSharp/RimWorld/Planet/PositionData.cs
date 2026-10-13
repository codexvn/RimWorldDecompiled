using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C46 RID: 15430
	public class PositionData : IExposable
	{
		// Token: 0x06014F38 RID: 85816 RVA: 0x000024DF File Offset: 0x000006DF
		public PositionData()
		{
		}

		// Token: 0x06014F39 RID: 85817 RVA: 0x00645864 File Offset: 0x00643A64
		public PositionData(IntVec3 position, Rot4 rotation, bool drafted = false)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (rotation == Rot4.East)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			else if (rotation == Rot4.South)
			{
				rotationDirection = RotationDirection.Opposite;
			}
			else if (rotation == Rot4.West)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			this.relativeRotation = rotationDirection;
			this.position = position;
			this.drafted = drafted;
		}

		// Token: 0x06014F3A RID: 85818 RVA: 0x006458C0 File Offset: 0x00643AC0
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<RotationDirection>(ref this.relativeRotation, "relativeRotation", RotationDirection.None, false);
			Scribe_Values.Look<bool>(ref this.drafted, "drafted", false, false);
		}

		// Token: 0x0400E970 RID: 59760
		public IntVec3 position;

		// Token: 0x0400E971 RID: 59761
		public RotationDirection relativeRotation;

		// Token: 0x0400E972 RID: 59762
		public bool drafted;

		// Token: 0x02003C47 RID: 15431
		public struct Data
		{
			// Token: 0x0400E973 RID: 59763
			public IntVec3 local;

			// Token: 0x0400E974 RID: 59764
			public Rot4 rotation;

			// Token: 0x0400E975 RID: 59765
			public bool drafted;
		}
	}
}
